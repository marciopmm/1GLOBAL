using MM.Domain.Entities;
using MM.Domain.Enums;
using MM.Domain.Exceptions;

namespace MM.Application.Validations;

public static class DeviceValidations
{
    public static void IsValidForAdd(Device newDevice)
    {
        if (newDevice == null)
        {
            throw new ArgumentNullException(nameof(newDevice));
        }

        if (string.IsNullOrWhiteSpace(newDevice.Name))
        {
            throw new ArgumentException("\"Name\" must be provided.", nameof(newDevice.Name));
        }

        if (string.IsNullOrWhiteSpace(newDevice.Brand))
        {
            throw new ArgumentException("\"Brand\" must be provided.", nameof(newDevice.Brand));
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

        if (patch.Name != null && string.IsNullOrWhiteSpace(patch.Name))
        {
            throw new ArgumentException("\"Name\" must be provided.", nameof(patch.Name));
        }

        if (patch.Brand != null && string.IsNullOrWhiteSpace(patch.Brand))
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
