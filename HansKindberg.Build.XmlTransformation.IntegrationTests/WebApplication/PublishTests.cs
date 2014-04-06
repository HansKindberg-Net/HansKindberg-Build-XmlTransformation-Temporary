using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HansKindberg.Build.XmlTransformation.IntegrationTests.WebApplication
{
	[TestClass]
	public class PublishTests
	{
		//private static readonly string _projectFullPath = Path.Combine(Solution.Path, @"HansKindberg.Build.XmlTransformation.WebApplication\HansKindberg.Build.XmlTransformation.WebApplication.csproj");
		//private const string _toolsVersion = "12.0";

		//[TestMethod]
		//public void Test()
		//{
		//	var logger = new Logger
		//	{
		//		Verbosity = LoggerVerbosity.Diagnostic
		//	};

		//	var buildParameters = new BuildParameters
		//	{
		//		DetailedSummary = true,
		//		Loggers = new[] { logger }
		//	};

		//	var globalProperties = new Dictionary<string, string>
		//	{
		//		{"Configuration", "Release"},
		//		{"DeployOnBuild", "true"},
		//		{"PublishProfileName", "Production"}
		//	};

		//	//globalProperties.Add("FilesToIncludeForPublish", "OnlyFilesToRunTheApp");

		//	//globalProperties.Add("DeployTarget", "Package");
		//	//globalProperties.Add("PackageAsSingleFile", "True");

		//	globalProperties.Add("UseMsdeployExe", "True");

		//	var projectInstance = new ProjectInstance(_projectFullPath, globalProperties, _toolsVersion);

		//	var targetsToBuild = new string[0];

		//	var buildRequestData = new BuildRequestData(projectInstance, targetsToBuild);

		//	var buildResult = BuildManager.DefaultBuildManager.Build(buildParameters, buildRequestData);

		//	List<string> failures = new List<string>();

		//	foreach(var message in logger.Messages)
		//	{
		//		if(message.ToUpperInvariant().Contains("FAIL"))
		//			failures.Add(message);
		//	}

		//	Assert.IsNotNull(buildResult);

		//	Assert.Inconclusive("Message count: " + logger.Messages.Count());
		//}

		#region Methods

		[TestMethod]
		public void Test2()
		{
			//Assert.IsNotNull(new WebApplicationProject());
			//new WebApplicationProject().DeployForTest();

			var buildLog = new WebApplicationProject().Publish(PublishProfile.Production);

			Assert.IsNotNull(buildLog);

			//Assert.Inconclusive("Message count: " + buildLog.Messages.Count());
		}

		#endregion
	}
}