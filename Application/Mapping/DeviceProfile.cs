using AutoMapper;
using MM.Application.DTOs;
using MM.Domain.Entities;
using MM.Domain.Enums;

namespace MM.Application.Mapping;

public class DeviceProfile : Profile
{
    public DeviceProfile()
    {
        CreateMap<Device, DeviceDTO>();
        CreateMap<DeviceDTO, Device>()
            .ForCtorParam("state", opt => opt.MapFrom(src => src.State))
            .ForCtorParam("creationTime", opt => opt.MapFrom(src => src.CreationTime == default ? DateTime.UtcNow : src.CreationTime))
            .ForMember(d => d.Id, opt => opt.Ignore());

        CreateMap<AddDeviceDtoRequest, Device>()
            .ConstructUsing(src =>
                new Device(src.Name, src.Brand, src.State, DateTime.UtcNow));
        CreateMap<UpdateDeviceDtoRequest, DevicePatch>()
            .ConstructUsing(src =>
                new DevicePatch(src.Name, src.Brand, src.State));
    }
}