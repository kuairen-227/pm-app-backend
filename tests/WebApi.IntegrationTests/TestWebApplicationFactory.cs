namespace WebApi.IntegrationTests;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.Single(
                d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

            services.Remove(descriptor);

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(
                    "Host=localhost;Database=test;Username=postgres;Password=postgres");
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = TestAuthHandler.Scheme;
                options.DefaultChallengeScheme = TestAuthHandler.Scheme;
            })
            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                TestAuthHandler.Scheme, _ => { });
        });
    }
}
