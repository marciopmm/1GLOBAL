using OneGlobal.Domain.Entities;
using OneGlobal.Domain.Enums;

namespace OneGlobal.Application.Validations;

public static class DeviceValidations
{
    public static bool IsValidForUpdate(Device current, DevicePatch patch)
    {
        if (patch.State != State.InUse)
        {
            return true;
        }

        return current.Name == (patch.Name ?? current.Name) &&
               current.Brand == (patch.Brand ?? current.Brand);
    }

    public static bool IsValidForDelete(Device current)
    {
        return current.State != State.InUse;
    }
}
