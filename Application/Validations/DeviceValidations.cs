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
        if (current.State == State.InUse &&
           ((patch.Name != null && patch.Name != current.Name) ||
            (patch.Brand != null && patch.Brand != current.Brand)))
        {
            throw new InvalidStateForUpdateException(current.Id);
        }

        if (!patch.State.HasValue ||
            (patch.State.HasValue && !Enum.IsDefined(typeof(State), patch.State)))
        {
            throw new InvalidStateException(current.Id);
        }

        if (string.IsNullOrWhiteSpace(patch.Name))
        {
            throw new ArgumentException("\"Name\" must be provided.", nameof(patch.Name));
        }

        if (string.IsNullOrWhiteSpace(patch.Brand))
        {
            throw new ArgumentException("\"Brand\" must be provided.", nameof(patch.Brand));
        }
    }

    public static void IsValidForUpdatePartial(Device current, DevicePatch patch)
    {
        if (current.State == State.InUse &&
           ((patch.Name != null && patch.Name != current.Name) ||
            (patch.Brand != null && patch.Brand != current.Brand)))
        {
            throw new InvalidStateForUpdateException(current.Id);
        }

        if (patch.State.HasValue && !Enum.IsDefined(typeof(State), patch.State))
        {
            throw new InvalidStateException(current.Id);
        }

        if (string.IsNullOrWhiteSpace(patch.Name))
        {
            throw new ArgumentException("\"Name\" must be provided.", nameof(patch.Name));
        }

        if (string.IsNullOrWhiteSpace(patch.Brand))
        {
            throw new ArgumentException("\"Brand\" must be provided.", nameof(patch.Brand));
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
