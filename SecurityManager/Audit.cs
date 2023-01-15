using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SecurityManager
{
     public class Audit
    {
		private static EventLog customLog = null;
		const string SourceName = "SecurityManager.Audit";
		const string LogName = "MyTest";

		static Audit()
		{
			try
			{
				if (!EventLog.SourceExists(SourceName))
				{
					EventLog.CreateEventSource(SourceName, LogName);
				}
				customLog = new EventLog(LogName,
					Environment.MachineName, SourceName);
			}
			catch (Exception e)
			{
				customLog = null;
				Console.WriteLine("Error while trying to create log handle. Error = {0}", e.Message);
			}
		}

		public static void KreirajFolderUspesno(string userName)
		{

			if (customLog != null)
			{
				string UserKreirajFolderUspesno =
					AuditEvents.KreirajFolderUspesno;
				string message = String.Format(UserKreirajFolderUspesno,
					userName);
				customLog.WriteEntry(message);
			}
			else
			{
				throw new ArgumentException(string.Format("Error while trying to create folder (eventid = {0}) to event log.",
					(int)AuditEventTypes.KreirajFolderUspesno));
			}
		}

		public static void KreirajFajlUspesno(string userName)
		{

			if (customLog != null)
			{
				string UserKreirajFajlUspesno =
					AuditEvents.KreirajFajlUspesno;
				string message = String.Format(UserKreirajFajlUspesno,
					userName);
				customLog.WriteEntry(message);
			}
			else
			{
				throw new ArgumentException(string.Format("Error while trying to create file (eventid = {0}) to event log.",
					(int)AuditEventTypes.KreirajFajlUspesno));
			}
		}
		public static void PreimenujFajlUspesno(string userName)
		{

			if (customLog != null)
			{
				string UserPreimenujFajlUspesno =
					AuditEvents.PreimenujFajlUspesno;
				string message = String.Format(UserPreimenujFajlUspesno,
					userName);
				customLog.WriteEntry(message);
			}
			else
			{
				throw new ArgumentException(string.Format("Error while trying to rename file (eventid = {0}) to event log.",
					(int)AuditEventTypes.PreimenujFajlUspesno));
			}
		}
		public static void IzbrisiFajlUspesno(string userName)
		{

			if (customLog != null)
			{
				string UserIzbrisiFajlUspesno =
					AuditEvents.IzbrisiFajlUspesno;
				string message = String.Format(UserIzbrisiFajlUspesno,
					userName);
				customLog.WriteEntry(message);
			}
			else
			{
				throw new ArgumentException(string.Format("Error while trying to delete file (eventid = {0}) to event log.",
					(int)AuditEventTypes.IzbrisiFajlUspesno));
			}
		}
		public static void PremestiFajlUspesno(string userName)
		{

			if (customLog != null)
			{
				string UserPremestiFajlUspesno =
					AuditEvents.PremestiFajlUspesno;
				string message = String.Format(UserPremestiFajlUspesno,
					userName);
				customLog.WriteEntry(message);
			}
			else
			{
				throw new ArgumentException(string.Format("Error while trying to move file (eventid = {0}) to event log.",
					(int)AuditEventTypes.PremestiFajlUspesno));
			}
		}
		public void Dispose()
		{
			if (customLog != null)
			{
				customLog.Dispose();
				customLog = null;
			}
		}
	}
}
