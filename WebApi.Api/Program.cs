using WebApi.Api;
using WebApi.Api.Middlewares;
using WebApi.Application;
using WebApi.Domain;
using WebApi.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// DDD の各層 DI
builder.Services.AddApi(builder.Configuration.GetSection("Jwt"));
builder.Services.AddApplication();
builder.Services.AddDomain();
builder.Services.AddInfrastructure(
    builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new ArgumentNullException("DefaultConnection は必須です")
);


var app = builder.Build();

// HTTP リクエストパイプラインの構成
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseOpenApi();
    app.UseSwaggerUi();
}
app.UseHttpsRedirection();
app.UseCors("AllowFrontEnd");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseCustomMiddleware();

app.Run();
