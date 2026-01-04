namespace WebApi.IntegrationTests.Helpers;

public static class DbCleaner
{
    public static async Task CleanAsync(AppDbContext context)
    {
        await context.Database.ExecuteSqlRawAsync(Sql);
    }

    private const string Sql = """
    DO
    $$
    DECLARE
        r RECORD;
    BEGIN
        FOR r IN
            SELECT tablename
            FROM pg_tables
            WHERE schemaname = 'public'
        LOOP
            EXECUTE 'TRUNCATE TABLE '
                || quote_ident(r.tablename)
                || ' RESTART IDENTITY CASCADE';
        END LOOP;
    END
    $$;
    """;
}
