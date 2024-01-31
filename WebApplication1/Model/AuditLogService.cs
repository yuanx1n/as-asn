// In AuditLogService.cs
using System;

namespace WebApplication1.Model
{
	public class AuditLogService
	{
		private readonly AuthDbContext _dbContext;

		public AuditLogService(AuthDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public void LogFailedLogin(string userId, string reason)
		{
			var logEntry = new Auditlog
			{
				Userid = userId,
				Action = $"Failed Login: {reason}",
				Time = DateTime.UtcNow
			};

			_dbContext.Auditlogs.Add(logEntry);
			_dbContext.SaveChanges();
		}

		public void LogSuccessfulLogin(string userId)
		{
			var logEntry = new Auditlog
			{
				Userid = userId,
				Action = "Successful Login",
				Time = DateTime.UtcNow
			};

			_dbContext.Auditlogs.Add(logEntry);
			_dbContext.SaveChanges();
		}
		public void LogLogout(string userId)
		{
			var logEntry = new Auditlog
			{
				Userid = userId,
				Action = "User Logout",
				Time = DateTime.UtcNow
			};

			_dbContext.Auditlogs.Add(logEntry);
			_dbContext.SaveChanges();
		}

        internal void LogMultipleLogin(string id)
        {
			var logEntry = new Auditlog
			{
				Userid = id,
				Action = "Multiple Logins",
				Time = DateTime.UtcNow
			};

            _dbContext.Auditlogs.Add(logEntry);
            _dbContext.SaveChanges();
        }
    }
}
