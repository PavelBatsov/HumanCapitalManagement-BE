namespace HCM.Domain.Interfaces.Services
{
    public interface IAccountService
    {
        Task Login();

        Task Register();
    }
}
