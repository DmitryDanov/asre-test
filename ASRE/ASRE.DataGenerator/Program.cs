using ASRE.DataGenerator;
using ASRE.DataGenerator.Services.DataGenerator;
using ASRE.DataGenerator.Services.HttpSender;
using Bogus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

IHost host =
    Host
        .CreateDefaultBuilder(args)
        .ConfigureServices((context, services) =>
        {
            services.Configure<ApiServiceOptions>(context.Configuration.GetSection(ApiServiceOptions.Options));

            services.AddTransient<Application>();
            services.AddHttpClient();
            services.AddTransient<Faker>();
            services.AddTransient<IDataGenerator, DataGenerator>();
            services.AddTransient<IHttpSenderService, HttpSenderService>();
        })
        .Build();

using (var scope = host.Services.CreateScope())
{
    IServiceProvider serviceProvider = scope.ServiceProvider;

    var application = serviceProvider.GetRequiredService<Application>();

    await application.Run(args).ConfigureAwait(false);
}