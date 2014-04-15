using System.Globalization;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HansKindberg.Build.XmlTransformation.IntegrationTests.WebApplication
{
	[TestClass]
	public class BuildTests
	{
		#region Fields

		private static readonly string _expectedBuildDirectory = Path.Combine(Solution.Directory, "HansKindberg.Build.XmlTransformation.IntegrationTests", @"WebApplication\Expected\Build");

		#endregion

		#region Methods

		[TestMethod]
		public void Build_ShouldTransformCorrectly()
		{
			// Debug
			var configuration = Configuration.Debug;
			var expected = File.ReadAllText(Path.Combine(_expectedBuildDirectory, string.Format(CultureInfo.InvariantCulture, "Build-{0}-Web.config", configuration)));

			var webApplicationProject = new WebApplicationProject();

			webApplicationProject.Build(configuration, LoggerVerbosity.Quiet);

			var actual = File.ReadAllText(Path.Combine(webApplicationProject.ProjectDirectory, "Web.config"));

			Assert.AreEqual(expected, actual);

			// Release
			configuration = Configuration.Release;
			expected = File.ReadAllText(Path.Combine(_expectedBuildDirectory, string.Format(CultureInfo.InvariantCulture, "Build-{0}-Web.config", configuration)));

			webApplicationProject.Build(configuration, LoggerVerbosity.Quiet);

			actual = File.ReadAllText(Path.Combine(webApplicationProject.ProjectDirectory, "Web.config"));

			Assert.AreEqual(expected, actual);
		}

		#endregion
	}
}