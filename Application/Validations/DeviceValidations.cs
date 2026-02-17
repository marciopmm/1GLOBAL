using MM.Domain.Entities;
using MM.Domain.Enums;
using MM.Domain.Exceptions;

namespace MM.Application.Validations;

public static class DeviceValidations
{
    public static void IsValidForAdd(Device newDevice)
    {
        if (!Enum.IsDefined(typeof(State), newDevice.State))
        {
            throw new InvalidStateException();
        }
    }

    public static void IsValidForUpdate(Device current, DevicePatch patch)
    {
        if (patch.State.HasValue &&
            patch.State == State.InUse &&
            current.Name == (patch.Name ?? current.Name) &&
            current.Brand == (patch.Brand ?? current.Brand))
        {
            throw new InvalidStateForUpdateException(current.Id);
        }

        if (patch.State.HasValue && !Enum.IsDefined(typeof(State), patch.State))
        {
            throw new InvalidStateException(current.Id);
        }
    }

    public static void IsValidForDelete(Device current)
    {
        if (current.State == State.InUse)
        {
            throw new InvalidStateForDeleteException(current.Id);
        }
    }
}
