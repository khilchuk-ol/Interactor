namespace Interactor.Application.Common.Interfaces;

public interface IUserService
{
    public Task RegisterRandomUser();
    
    public Task BanRandomUser();
}