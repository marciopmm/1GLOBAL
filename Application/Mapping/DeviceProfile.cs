using AutoMapper;
using Global.Application.DTOs;
using Global.Domain.Entities;

namespace Global.Application.Mapping;

public class DeviceProfile : Profile
{
    public DeviceProfile()
    {
        CreateMap<CreateDeviceRequest, Device>()
            .ConstructUsing(src =>
                new Device(src.Name, src.Brand, src.State, DateTime.UtcNow));
        CreateMap<Device, DeviceDTO>()
            .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State.ToString()));
        CreateMap<DeviceDTO, Device>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.CreationTime, opt => opt.Ignore());
    }
}