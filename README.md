# Micro-MMerce

eCommerce build on microservice. Testing repo 🧑🏻‍🔬

## COMMON RESOLUTION

### Error while adding migration: Unable to create an object of type 'OrderContext'

While creating migrations from Ordering.Infrastructure where it reads connecting string from appsettings.json which locates under Ordering.API it will throw the error saying `Unable to create an object of type 'OrderContext'`. To avoid this, for successful migrations we must pass --project and --startup-project arguments with ef migration command.

Here's the example below

```
dotnet ef migrations add initialMigration --project Ordering.Infrastructure --startup-project Ordering.API
```

Here 'startup-project' is project where connectionString settings are configured and 'project' is project directory where OrderContext is created. It will create migration folder under 'Ordering.Infrastructure'

> **NOTE:** Microsoft.EntityFrameworkCore.Design is also required in .NET 6 for migrations. This should be installed under the project directory where OrderContext is created, in this case 'Ordering.Infrastructure'

### Ocelot API Gw routing error in docker container

Docker is case sensitive environment. Handle any constant filename with sensitive operation.

If the filename is "Ocelot.Development.json" than rather using this line:
`$"ocelot.{builder.Environment.EnvironmentName}.json"`

It should be like this with "O" in caps:
`$"Ocelot.{builder.Environment.EnvironmentName}.json"`

<img width="933" alt="Screenshot 2023-04-02 at 9 58 51 AM" src="https://user-images.githubusercontent.com/59219441/229337790-fe91937a-c3f6-41a7-9de1-2d2ef44343c9.png">

