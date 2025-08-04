using ASRE.DataLayer.Models;
using AutoMapper;

namespace ASRE.PatientApi.Models;

public sealed class PatientMapperProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PatientMapperProfile"/> class.
    /// </summary>
    public PatientMapperProfile()
    {
        CreateMap<PatientModel, Patient>()
            .ForMember(d => d.Use, o => o.PreCondition(c => c.Name?.Use != null))
            .ForMember(d => d.Use, o => o.MapFrom(s => s.Name!.Use))
            .ForMember(d => d.FamilyName, o => o.PreCondition(c => c.Name?.Family != null))
            .ForMember(d => d.FamilyName, o => o.MapFrom(s => s.Name!.Family))
            .ForMember(d => d.Gender, o => o.Ignore())
            .ForMember(d => d.BirthDate, o => o.PreCondition(c => c.BirthDate != null))
            .AfterMap((source, destination, context) =>
            {
                if (source.Name?.Given != null)
                {
                    destination.PatientGivenNames = source.Name!.Given?.Select(s => new PatientGivenName { Name = s }).ToList();
                }
            });

        CreateMap<Patient, PatientModel>()
            .ForPath(d => d.Name!.Id, o => o.MapFrom(s => s.Id))
            .ForPath(d => d.Name!.Use, o => o.MapFrom(s => s.Use))
            .ForPath(d => d.Name!.Family, o => o.MapFrom(s => s.FamilyName))
            .ForPath(d => d.Name!.Given, o => o.MapFrom(s => s.PatientGivenNames.Select(i => i.Name)))
            .ForMember(d => d.Gender, o => o.MapFrom(s => s.Gender != null ? s.Gender.Name : null))
            .ForMember(d => d.Active, o => o.MapFrom(s => s.Activation != null ? s.Activation.Name : null));
    }
}