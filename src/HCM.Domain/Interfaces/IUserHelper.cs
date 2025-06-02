namespace HCM.Domain.Interfaces
{
    public interface IUserHelper
    {
        Guid CurrentUserId();

        string CurrentUserRole();
    }
}
