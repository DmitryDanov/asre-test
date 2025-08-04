using ASRE.PatientApi.Services.Activation;
using ASRE.PatientApi.Services.Gender;
using ASRE.PatientApi.Services.Patient;

namespace ASRE.PatientApi;

public static class ServiceRegistrations
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddTransient<IPatientService, PatientService>();
        services.AddTransient<IGenderService, GenderService>();
        services.AddTransient<IActivationService, ActivationService>();

        return services;
    }
}