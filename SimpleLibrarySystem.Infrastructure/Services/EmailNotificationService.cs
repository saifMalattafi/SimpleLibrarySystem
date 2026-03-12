using SimpleLibrarySystem.Application.Applications.Interfaces;
using SimpleLibrarySystem.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLibrarySystem.Infrastructure.Services
{
    public class EmailNotificationService : INotificationService
    {
        public Result Notify(string to, string message)
        {
            // code to send the message via the email.
            return Result.Success();
        }
    }
}
