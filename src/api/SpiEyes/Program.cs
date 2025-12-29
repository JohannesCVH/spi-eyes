using SpiEyes.Models;
using SpiEyes.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();

        builder.Services.AddCors(options =>
        {
           options.AddPolicy(name: "AllowAll", policy =>
           {
                policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
           });
        });

        builder.Configuration.AddJsonFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "SpiEyes/config.json"), optional: false, reloadOnChange: true);
        builder.Services.Configure<Config>(builder.Configuration.GetSection("Configuration"));

        builder.Services.AddSingleton<ISharedDataService, SharedDataService>();
        builder.Services.AddHostedService<FFmpegRtspReaderService>();

        var app = builder.Build();

        app.MapControllers();
        app.UseCors("AllowAll");
        app.Run();
    }
}