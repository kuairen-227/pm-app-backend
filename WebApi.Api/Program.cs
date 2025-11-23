using WebApi.Application;
using WebApi.Domain;
using WebApi.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// ASP.NET Core API 用の DI
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontEnd",
        policy => policy.WithOrigins()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});
builder.Services.AddHttpContextAccessor();

// DDD プロジェクトごとの DI
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
}
app.UseHttpsRedirection();
app.UseCors("AllowFrontEnd");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
