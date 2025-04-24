
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduPro.Data;
using EduPro.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EduPro.Services
{
    public class ErrorLogService : IErrorLogService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor; //
        private readonly ILogger<ErrorLogService> _logger;

        public ErrorLogService(
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor,
            ILogger<ErrorLogService> logger)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task LogErrorAsync(Exception ex, string controllerName, string actionName,
            string userId = null, string userEmail = null, string additionalInfo = null)
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;

                var errorLog = new ErrorLog
                {
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    ControllerName = controllerName,
                    ActionName = actionName,
                    UserId = userId,
                    UserEmail = userEmail,
                    AdditionalInfo = additionalInfo,
                    Timestamp = DateTime.UtcNow,
                    RequestPath = httpContext?.Request?.Path.Value,
                    RequestMethod = httpContext?.Request?.Method,
                    UserAgent = httpContext?.Request?.Headers["User-Agent"].ToString(),
                    IpAddress = httpContext?.Connection?.RemoteIpAddress?.ToString()
                };

                _context.ErrorLogs.Add(errorLog);
                await _context.SaveChangesAsync();

                _logger.LogError(ex,
                    $"Error in {controllerName}/{actionName}: {ex.Message} | User: {userEmail ?? "Anonymous"}");
            }
            catch (Exception logEx)
            {
               
                _logger.LogError(logEx,
                    $"Failed to log error to database. Original error: {ex.Message}");
            }
        }

        public async Task<List<ErrorLog>> GetRecentErrorsAsync(int count = 100)
        {
            return await _context.ErrorLogs
                .OrderByDescending(e => e.Timestamp)
                .Take(count)
                .ToListAsync();
        }
    }
}