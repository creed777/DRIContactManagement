using DRIContactManagement;
using DRIContactManagement.Repository;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Configuration;

string connectionString = Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING") ?? string.Empty;
//string connectionString = ConfigurationManager.AppSettings["AZURE_SQL_CONNECTIONSTRING"]??string.Empty;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddDbContext<AppDbContext>(
              options => SqlServerDbContextOptionsExtensions.UseSqlServer(options, connectionString));
        services.AddScoped<IContactRepository, ContactRepository>();
        services.AddScoped<IServiceRepository, ServiceRepository>();
    })
    .Build();

host.Run();
