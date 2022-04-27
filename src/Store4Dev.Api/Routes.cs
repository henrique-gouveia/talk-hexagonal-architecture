using Store4Dev.Application.Commands;
using Store4Dev.Application.Services;

public static class Routes
{
    public static WebApplication ConfigureRoutes(this WebApplication app)
    {
        app.MapGet("/products", async (IProductAppService productService)
            => Results.Ok(await productService.FindAllAsync()));

        app.MapGet("/products/{id}", async (IProductAppService productService, Guid id) =>
        {
            var product = await productService.FindOneAsync(id);
            return product is not null ? Results.Ok(product) : Results.NotFound();
        });

        app.MapGet("/brands/{id}/products", async (IProductAppService productService, Guid id)
            => Results.Ok(await productService.FindByBrandIdAsync(id)));

        app.MapPost("/products", async (IProductAppService productService, CreateProductCommand command) =>
        {
            var product = await productService.CreateProductAsync(command);
            return Results.Created($"/products/{product.Id}", product);
        });

        app.MapPut("/products/{id}/stock", async (IProductAppService productService, Guid id, decimal quantity, string changeType) =>
            changeType switch
            {
                "inc" => Results.Ok(await productService.IncreaseStockAsync(id, quantity)),
                "dec" => Results.Ok(await productService.DecreaseStockAsync(id, quantity)),
                _ => Results.BadRequest("ChangeType must be 'inc' or 'dec'")
            });

        return app;
    }
}
