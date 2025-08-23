using WebApi.Application;
using WebApi.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(
    builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new ArgumentNullException("DefaultConnection は必須です")
);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontEnd",
        policy => policy.WithOrigins()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

var app = builder.Build();

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
