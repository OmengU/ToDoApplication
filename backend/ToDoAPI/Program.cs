using Microsoft.EntityFrameworkCore;
using ToDoAPI.Models;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureServices((hostContext, services) =>
                {
                    services.AddControllers();
                    services.AddEndpointsApiExplorer();
                    services.AddSwaggerGen();
                    services.AddHealthChecks();

                    services.AddScoped<IToDoRepository, ToDoRepository>();
                    services.AddDbContext<ToDoManagementDbContext>(options =>
                    {
                        string? connectionString = Environment.GetEnvironmentVariable("db_host_string") ?? hostContext.Configuration["ConnectionStrings:ToDoManagementDbContextConnection"];
                        options.UseNpgsql(connectionString);
                    });

                    // Configure CORS
                    services.AddCors(options =>
                    {
                        options.AddDefaultPolicy(builder =>
                        {
                            builder.WithOrigins("http://localhost:1234")
                                   .AllowAnyMethod()
                                   .AllowAnyHeader();
                        });
                    });
                });

                webBuilder.Configure((hostContext, app) =>
                {
                    if (hostContext.HostingEnvironment.IsDevelopment())
                    {
                        app.UseSwagger();
                        app.UseSwaggerUI();
                    }

                    app.UseRouting();

                    app.UseCors();

                    app.UseAuthorization();

                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllers();
                        endpoints.MapHealthChecks("/healthz");
                    });
                });
            });
}
