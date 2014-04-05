using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Build.Framework;

namespace HansKindberg.Build.XmlTransformation.IntegrationTests
{
	public class Logger : ILogger, IBuildLog
	{
		#region Fields

		private readonly IList<string> _messages = new List<string>();

		#endregion

		#region Properties

		public virtual IEnumerable<string> Messages
		{
			get { return this._messages.ToArray(); }
		}

		public virtual string Parameters { get; set; }
		public virtual LoggerVerbosity Verbosity { get; set; }

		#endregion

		#region Methods

		public virtual void Initialize(IEventSource eventSource)
		{
			if(eventSource == null)
				throw new ArgumentNullException("eventSource");

			eventSource.AnyEventRaised += (sender, buildEventArgs) => this._messages.Add(buildEventArgs.Message);
			eventSource.ErrorRaised += (sender, buildErrorEventArgs) =>
			{
				this._messages.Add("Build error: " + buildErrorEventArgs.Message);
				//buildErrorEventArgs.
			};
		}

		public virtual void Shutdown() {}

		public override string ToString()
		{
			return string.Join(Environment.NewLine, this.Messages);
		}

		#endregion
	}
}