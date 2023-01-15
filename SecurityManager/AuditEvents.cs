using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace SecurityManager
{
    public enum AuditEventTypes
    {
        KreirajFolderUspesno= 0,
        KreirajFajlUspesno= 1,
        PreimenujFajlUspesno= 2,
        IzbrisiFajlUspesno= 3,
        PremestiFajlUspesno= 4
    }

    public class AuditEvents
    {
        private static ResourceManager resourceManager = null;
        private static object resourceLock = new object();

		private static ResourceManager ResourceMgr
		{
			get
			{
				lock (resourceLock)
				{
					if (resourceManager == null)
					{
						resourceManager = new ResourceManager
							(typeof(AuditEventFile).ToString(),
							Assembly.GetExecutingAssembly());
					}
					return resourceManager;
				}
			}
		}

		public static string KreirajFolderUspesno
        {
            get {
				return ResourceMgr.GetString(AuditEventTypes.KreirajFolderUspesno.ToString());
			}
		}
		public static string KreirajFajlUspesno
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.KreirajFajlUspesno.ToString());
			}
		}
		public static string PreimenujFajlUspesno
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.PreimenujFajlUspesno.ToString());
			}
		}
		public static string IzbrisiFajlUspesno
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.IzbrisiFajlUspesno.ToString());
			}
		}
		public static string PremestiFajlUspesno
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.PremestiFajlUspesno.ToString());
			}
		}
	}
}
