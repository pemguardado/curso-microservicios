using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using AddChild;
using AddChild.Data;

IServiceCollection serviceDescriptors = new ServiceCollection();

Host.CreateDefaultBuilder(args)
   .ConfigureHostConfiguration(configHost =>
   {
       configHost.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
       configHost.AddEnvironmentVariables();
   })
   .ConfigureServices((hostContext, services) =>
   {
       IConfiguration configuration = hostContext.Configuration;
       services.AddOptions();
       services.AddHostedService<Worker>();
       var connectionString = configuration["SQL_CONNECTION_STRING"] 
                   ?? configuration.GetConnectionString("DefaultConnection");
       services.AddDbContext<DataContext>(options =>
           options.UseSqlServer(connectionString));
   }).Build().Run();