using Mapster;
using WebApi.Models;
using WebApi.Models.DTO;

namespace WebApi;

public class RegisterMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Device, DeviceDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Sessions, src => src.Sessions);

        config.NewConfig<Session, SessionDto>()
            .Map(dest => dest.SessionId, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.StartTime, src => src.StartTime)
            .Map(dest => dest.EndTime, src => src.EndTime)
            .Map(dest => dest.Version, src => src.Version);
        
        config.NewConfig<SessionForCreationDto, Session>()
            .Map(dest => dest.Id, src => Guid.NewGuid())
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.StartTime, src => src.StartTime)
            .Map(dest => dest.EndTime, src => src.EndTime)
            .Map(dest => dest.Version, src => src.Version);
    }
}