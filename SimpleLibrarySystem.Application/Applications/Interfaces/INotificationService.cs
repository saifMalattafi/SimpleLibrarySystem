
using SimpleLibrarySystem.Domain.Common.Results;

namespace SimpleLibrarySystem.Application.Applications.Interfaces
{
    public interface INotificationService
    {
        Result Notify(string to, string message);
    }
}
