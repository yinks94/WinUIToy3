using System.Threading.Tasks;

namespace WinUIToy3.Contracts.Services;

public interface IActivationService
{
    Task ActivateAsync(object activationArgs);
}