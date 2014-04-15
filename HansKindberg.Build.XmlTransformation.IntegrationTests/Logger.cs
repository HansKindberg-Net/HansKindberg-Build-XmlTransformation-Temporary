using System;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;

namespace HansKindberg.Build.XmlTransformation.IntegrationTests
{
	public class Logger : ConsoleLogger, IBuildLog
	{
		#region Fields

		private string _messages = string.Empty;

		#endregion

		#region Constructors

		public Logger() : base(LoggerVerbosity.Minimal)
		{
			this.WriteHandler = this.Write;
		}

		#endregion

		#region Properties

		public virtual string Messages
		{
			get { return this._messages; }
		}

		#endregion

		#region Methods

		protected virtual string RemoveMultipleWhiteSpaces(string message)
		{
			while(true)
			{
				if(!message.Contains("  "))
					break;

				message = message.Replace("  ", " ");
			}

			return message;
		}

		public override string ToString()
		{
			return string.Join(Environment.NewLine, this.Messages);
		}

		public virtual void Write(string message)
		{
			if(message != null && message.EndsWith(" = ", StringComparison.OrdinalIgnoreCase))
				message = this.RemoveMultipleWhiteSpaces(message);

			this._messages += message;
		}

		#endregion
	}
}