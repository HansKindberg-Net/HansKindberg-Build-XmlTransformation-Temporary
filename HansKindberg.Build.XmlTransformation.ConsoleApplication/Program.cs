using System;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace HansKindberg.Build.XmlTransformation.ConsoleApplication
{
	internal class Program
	{
		#region Methods

		protected internal static string GetTabs(string key)
		{
			if(key == null)
				throw new ArgumentNullException("key");

			int keyLength = key.Length;

			if(keyLength > 20)
				return "\t";

			if(keyLength > 12)
				return "\t\t";

			if(keyLength > 6)
				return "\t\t\t";

			return "\t\t\t\t";
		}

		[SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Console.Write(System.String)")]
		[SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Console.WriteLine(System.String)")]
		[SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Console.WriteLine(System.String,System.Object,System.Object,System.Object)")]
		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "args")]
		[SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "AppSettings")]
		protected internal static void Main(string[] args)
		{
			Console.WriteLine("AppSettings");
			Console.WriteLine("===========");

			foreach(var key in ConfigurationManager.AppSettings.AllKeys)
			{
				Console.WriteLine("{0}:{1}{2}", key, GetTabs(key), ConfigurationManager.AppSettings[key]);
			}

			Console.WriteLine(string.Empty);
			Console.Write("Press any key to exit.");
			Console.Read();
		}

		#endregion
	}
}