using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml.Linq;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;

namespace HansKindberg.Build.XmlTransformation.IntegrationTests
{
	public abstract class Project
	{
		#region Fields

		private readonly string _buildPropertiesFile;
		private readonly string _buildTargetsFile;
		private readonly IEnumerable<string> _outputDirectories = new[] {"bin", "obj"};
		private readonly string _projectDirectory;
		private readonly string _projectFile;
		private const string _testApplicationsDirectoryName = "Test-Applications";
		private const string _toolsVersion = "12.0";

		#endregion

		#region Constructors

		protected Project(string testApplicationRelativeProjectFile) : this(testApplicationRelativeProjectFile, null, null) {}

		protected Project(string testApplicationRelativeProjectFile, string buildPropertiesFile, string buildTargetsFile)
		{
			if(testApplicationRelativeProjectFile == null)
				throw new ArgumentNullException("testApplicationRelativeProjectFile");

			var testApplicationsDirectory = Path.Combine(Solution.Directory, _testApplicationsDirectoryName);

			var projectFile = new FileInfo(Path.Combine(testApplicationsDirectory, testApplicationRelativeProjectFile));

			if(!projectFile.Exists)
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The project-file \"{0}\" does not exist.", projectFile.FullName), "testApplicationRelativeProjectFile");

			if(buildPropertiesFile != null)
			{
				try
				{
					var xmlDocument = XDocument.Load(buildPropertiesFile);

					if(xmlDocument.Root == null || !xmlDocument.Root.Name.LocalName.Equals("Project", StringComparison.OrdinalIgnoreCase))
						throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The build-properties file \"{0}\" is invalid.", buildPropertiesFile), "buildPropertiesFile");
				}
				catch(Exception exception)
				{
					throw new ArgumentException("The build-properties file is invalid.", "buildPropertiesFile", exception);
				}
			}

			if(buildTargetsFile != null)
			{
				try
				{
					var xmlDocument = XDocument.Load(buildTargetsFile);

					if(xmlDocument.Root == null || !xmlDocument.Root.Name.LocalName.Equals("Project", StringComparison.OrdinalIgnoreCase))
						throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The build-targets file \"{0}\" is invalid.", buildTargetsFile), "buildTargetsFile");
				}
				catch(Exception exception)
				{
					throw new ArgumentException("The build-targets file is invalid.", "buildTargetsFile", exception);
				}
			}

			this._buildPropertiesFile = buildPropertiesFile;
			this._buildTargetsFile = buildTargetsFile;
			this._projectDirectory = projectFile.DirectoryName;
			this._projectFile = projectFile.FullName;
		}

		#endregion

		#region Properties

		protected virtual string BuildPropertiesFile
		{
			get { return this._buildPropertiesFile; }
		}

		protected virtual string BuildTargetsFile
		{
			get { return this._buildTargetsFile; }
		}

		protected virtual IEnumerable<string> OutputDirectories
		{
			get { return this._outputDirectories; }
		}

		protected virtual string ProjectDirectory
		{
			get { return this._projectDirectory; }
		}

		protected virtual string ProjectFile
		{
			get { return this._projectFile; }
		}

		protected virtual string ToolsVersion
		{
			get { return _toolsVersion; }
		}

		#endregion

		#region Methods

		public virtual IBuildLog Build(Configuration configuration)
		{
			return this.Build(configuration, new Dictionary<string, string>());
			//var globalProperties = new Dictionary<string, string>
			//{
			//	//{"DeleteExistingFiles", "True"},
			//	{"Configuration", "Release"},
			//	{"DeployOnBuild", "true"},
			//	{"PublishProfileName", "Production"}
			//	//{"WebPublishMethod", "FileSystem"},
			//	//{"LastUsedBuildConfiguration", "Release"}
			//	//{"LastUsedPlatform", "Any CPU"},
			//	//{"FilesToIncludeForPublish", "OnlyFilesToRunTheApp"},
			//	//{"DeployTarget", "Package"},
			//	//{"PackageAsSingleFile", "True"},
			//	//{"publishUrl", @"C:\Data\Projects\HansKindberg-Build-XmlTransformation-Temporary\Publish\Production"}
			//};
		}

		protected virtual IBuildLog Build(IDictionary<string, string> globalProperties)
		{
			return this.Build(null, globalProperties);
		}

		protected virtual IBuildLog Build(Configuration? configuration, IDictionary<string, string> globalProperties)
		{
			return this.Build(configuration, globalProperties, LoggerVerbosity.Detailed);
		}

		protected virtual IBuildLog Build(Configuration? configuration, IDictionary<string, string> globalProperties, LoggerVerbosity loggerVerbosity)
		{
			if(globalProperties == null)
				throw new ArgumentNullException("globalProperties");

			this.ClearOutput();

			if(configuration.HasValue)
				globalProperties["Configuration"] = configuration.ToString();

			var logger = new Logger
			{
				Verbosity = loggerVerbosity
			};

			var buildParameters = new BuildParameters
			{
				DetailedSummary = true,
				Loggers = new[] {logger}
			};

			var projectInstance = new ProjectInstance(this.ProjectFile, globalProperties, this.ToolsVersion);

			//var targetsToBuild = new string[]{"Build"};
			var targetsToBuild = new string[0];

			var buildRequestData = new BuildRequestData(projectInstance, targetsToBuild);

			BuildManager.DefaultBuildManager.Build(buildParameters, buildRequestData);

			return logger;
		}

		protected virtual void ClearOutput()
		{
			foreach(var outputDirectory in this.OutputDirectories)
			{
				this.RemoveDirectory(Path.Combine(this.ProjectDirectory, outputDirectory));
			}
		}

		protected virtual void RemoveDirectory(string path)
		{
			if(!Directory.Exists(path))
				return;

			Directory.Delete(path, true);
		}

		#endregion
	}
}