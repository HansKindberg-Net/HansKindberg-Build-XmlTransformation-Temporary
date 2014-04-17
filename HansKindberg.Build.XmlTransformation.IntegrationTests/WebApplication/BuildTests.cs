using System;
using System.Collections.Generic;
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

		private static void BuildShouldTransformCorrectly(IEnumerable<string> files, Configuration configuration)
		{
			if(files == null)
				throw new ArgumentNullException("files");

			var webApplicationProject = new WebApplicationProject();

			webApplicationProject.Build(configuration, LoggerVerbosity.Quiet);

			foreach(var file in files)
			{
				var relativeExpectedDirectory = Path.GetDirectoryName(file) ?? string.Empty;
				var exprectedFileName = Path.GetFileName(file);

				var expected = File.ReadAllText(Path.Combine(_expectedBuildDirectory, relativeExpectedDirectory, string.Format(CultureInfo.InvariantCulture, "Build-{0}-{1}", configuration, exprectedFileName)));
				var actual = File.ReadAllText(Path.Combine(webApplicationProject.ProjectDirectory, file));

				Assert.AreEqual(expected, actual);
			}
		}

		[TestMethod]
		public void Build_ShouldTransformCorrectly()
		{
			var files = new[] {@"Xml\Another-File.xml", @"Xml\File.xml", "Web.config"};

			BuildShouldTransformCorrectly(files, Configuration.Debug);

			BuildShouldTransformCorrectly(files, Configuration.Release);
		}

		#endregion
	}
}