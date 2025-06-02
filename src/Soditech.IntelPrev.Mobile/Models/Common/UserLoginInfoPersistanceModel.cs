using Sensor6ty.Domain;

namespace Soditech.IntelPrev.Mobile.Models.Common;

public class UserLoginInfoPersistanceModel : EntityBase
{
    public string Name { get; set; }

    public string Surname { get; set; }

    public string UserName { get; set; }

    public string EmailAddress { get; set; }

    public string ProfilePictureId { get; set; }
}