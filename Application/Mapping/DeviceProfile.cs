using AutoMapper;
using MM.Application.DTOs;
using MM.Domain.Entities;
using MM.Domain.Enums;
using System.Collections.Generic;

namespace MM.Application.Mapping;

public class DeviceProfile : Profile
{
    private static readonly IReadOnlyDictionary<State, string> StateToText = new Dictionary<State, string>
    {
        [State.Available] = "Available",
        [State.InUse] = "InUse",
        [State.Inactive] = "Inactive"
    };

    private static readonly IReadOnlyDictionary<string, State> TextToState =
        new Dictionary<string, State>(StringComparer.OrdinalIgnoreCase)
        {
            ["Available"] = State.Available,
            ["InUse"] = State.InUse,
            ["Inactive"] = State.Inactive
        };

    public DeviceProfile()
    {
        CreateMap<Device, DeviceDTO>()
            .ForMember(dest => dest.State, opt => opt.MapFrom(src => MapStateToText(src.State)));
        CreateMap<DeviceDTO, Device>()
            .ForCtorParam("state", opt => opt.MapFrom(src => MapTextToState(src.State)))
            .ForCtorParam("creationTime", opt => opt.MapFrom(src => src.CreationTime == default ? DateTime.UtcNow : src.CreationTime))
            .ForMember(d => d.Id, opt => opt.Ignore());

        CreateMap<AddDeviceDtoRequest, Device>()
            .ConstructUsing(src =>
                new Device(src.Name, src.Brand, MapTextToState(src.State), DateTime.UtcNow));
        CreateMap<UpdateDeviceDtoRequest, DevicePatch>()
            .ConstructUsing(src =>
                new DevicePatch(src.Name, src.Brand, MapNullableTextToState(src.State)));
    }

    private static string MapStateToText(State state)
    {
        return StateToText.TryGetValue(state, out var text)
            ? text
            : state.ToString();
    }

    private static State MapTextToState(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("State value must be provided.", nameof(value));
        }

        var trimmed = value.Trim();
        if (TextToState.TryGetValue(trimmed, out var state))
        {
            return state;
        }

        if (Enum.TryParse<State>(trimmed, ignoreCase: true, out var parsed))
        {
            return parsed;
        }

        if (int.TryParse(trimmed, out var numeric) && Enum.IsDefined(typeof(State), numeric))
        {
            return (State)numeric;
        }

        throw new ArgumentOutOfRangeException(nameof(value), value, "Unknown state value. Expected: Available, InUse, Inactive, or corresponding numeric values.");
    }

    private static State? MapNullableTextToState(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : MapTextToState(value);
    }
}