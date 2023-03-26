using System.Net;
using AutoMapper;
using Basket.API.GrpcServices;
using Basket.API.Interfaces;
using Basket.API.Models;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BasketController : ControllerBase
{
    private readonly IBasketRepository _repository;
    private readonly DiscountGrpcService _discountGrpcService;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IMapper _mapper;

    public BasketController(
        IBasketRepository repository,
        DiscountGrpcService discountGrpcService,
        IPublishEndpoint publishEndpoint,
        IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _discountGrpcService = discountGrpcService ?? throw new ArgumentNullException(nameof(discountGrpcService));
        _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet("{userName}", Name = "GetBasket")]
    [ProducesResponseType(typeof(ShoppingCart), (int) HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
    {
        var basket = await _repository.GetBasket(userName);
        return Ok(basket ?? new ShoppingCart(userName));
    }

    [HttpPost(Name = "UpdateBasket")]
    [ProducesResponseType(typeof(ShoppingCart), (int) HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
    {
        // TODO: Communicate with Discount.Grpc and calculate latest prices of products into shoppping cart
        foreach (var item in basket.Items)
        {
            var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
            item.Price -= coupon.Amount;
        }
        return Ok(await _repository.UpdateBasket(basket));
    }

    [HttpDelete("{userName}", Name = "DeleteBasket")]
    [ProducesResponseType(typeof(void), (int) HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteBasket(string userName)
    {
        await _repository.DeleteBasket(userName);
        return Ok();
    }

    [HttpPost("[action]", Name = "CheckoutBasket")]
    [ProducesResponseType((int) HttpStatusCode.Accepted)]
    [ProducesResponseType((int) HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
    {
        // get existing basket with total price
        var basket = await _repository.GetBasket(basketCheckout.UserName);
        if (basket == null)
        {
            return BadRequest();
        }

        // create BasketCheckoutEvent event -- set TotalPrice on basketCheckout eventMessage
        var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
        eventMessage.TotalPrice = basket.TotalPrice;

        // send checkout event to rabbitmq
        await _publishEndpoint.Publish(eventMessage);

        // remove the basket
        await _repository.DeleteBasket(basket.UserName);

        return Accepted();
    }
}
