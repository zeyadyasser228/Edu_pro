// Services/IErrorLogService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EduPro.Models;

namespace EduPro.Services
{
    public interface IErrorLogService
    {
        Task LogErrorAsync(Exception ex, string controllerName, string actionName,
            string userId = null, string userEmail = null, string additionalInfo = null);

        Task<List<ErrorLog>> GetRecentErrorsAsync(int count = 100);
    }
}