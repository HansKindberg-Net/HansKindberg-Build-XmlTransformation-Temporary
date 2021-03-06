﻿using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Microsoft.Build.Framework;

namespace HansKindberg.Build.XmlTransformation.IntegrationTests.WebApplication
{
	public class WebApplicationProject : Project
	{
		#region Fields

		private const string _testApplicationRelativeProjectFile = @"WebApplication\HansKindberg.Build.XmlTransformation.WebApplication.csproj";

		#endregion

		#region Constructors

		public WebApplicationProject() : base(_testApplicationRelativeProjectFile) {}

		#endregion

		#region Methods

		public virtual Configuration GetConfiguration(PublishProfile publishProfile)
		{
			return Configuration.Release;
		}

		public virtual string GetPublishDirectory(Configuration configuration)
		{
			return Path.Combine(this.ProjectDirectory, string.Format(CultureInfo.InvariantCulture, @"obj\{0}\Package\PackageTmp\", configuration));
		}

		public virtual IBuildLog Publish(PublishProfile publishProfile)
		{
			return this.Publish(publishProfile, null);
		}

		public virtual IBuildLog Publish(PublishProfile publishProfile, LoggerVerbosity? loggerVerbosity)
		{
			var globalProperties = new Dictionary<string, string>
			{
				{"AutoParameterizationWebConfigConnectionStrings", "False"},
				{"DeployOnBuild", "True"},
				{"PublishProfileName", publishProfile.ToString()},
				{"UseMsdeployExe", "True"}
			};

			return this.Build(this.GetConfiguration(publishProfile), globalProperties, loggerVerbosity);
		}

		#endregion
	}
}