namespace NZWalks;


public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureService()
    {
        var server = Configuration["DBServer"] ?? "localhost";
        var port = Configuration["DBPort"] ?? "1443";
        var user = Configuration["DBUser"] ?? "SA";
        var password = Configuration["DBPassword"] ?? "H809901@m1401";


    }
}