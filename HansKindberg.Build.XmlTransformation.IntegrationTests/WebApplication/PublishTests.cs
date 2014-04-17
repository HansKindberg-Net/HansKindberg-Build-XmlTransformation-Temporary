using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HansKindberg.Build.XmlTransformation.IntegrationTests.WebApplication
{
	[TestClass]
	public class PublishTests
	{
		#region Fields

		private static readonly string _expectedPublishDirectory = Path.Combine(Solution.Directory, "HansKindberg.Build.XmlTransformation.IntegrationTests", @"WebApplication\Expected\Publish");
		private static readonly string _resetDirectory = Path.Combine(Solution.Directory, "HansKindberg.Build.XmlTransformation.IntegrationTests", @"WebApplication\Reset");

		#endregion

		#region Methods

		private static void PublishShouldTransformCorrectly(IEnumerable<string> files, PublishProfile publishProfile)
		{
			if(files == null)
				throw new ArgumentNullException("files");

			var webApplicationProject = new WebApplicationProject();

			webApplicationProject.Publish(publishProfile, LoggerVerbosity.Quiet);

			foreach(var file in files)
			{
				var relativeExpectedDirectory = Path.GetDirectoryName(file) ?? string.Empty;
				var exprectedFileName = Path.GetFileName(file);

				var expected = File.ReadAllText(Path.Combine(_expectedPublishDirectory, relativeExpectedDirectory, string.Format(CultureInfo.InvariantCulture, "Publish-{0}-{1}", publishProfile, exprectedFileName)));
				var actual = File.ReadAllText(Path.Combine(webApplicationProject.GetPublishDirectory(webApplicationProject.GetConfiguration(publishProfile)), file));

				Assert.AreEqual(expected, actual);
			}
		}

		[TestMethod]
		public void Publish_ShouldTransformCorrectly()
		{
			var files = new[] {@"Xml\Another-File.xml", @"Xml\File.xml", "Web.config"};

			PublishShouldTransformCorrectly(files, PublishProfile.Production);

			PublishShouldTransformCorrectly(files, PublishProfile.Test);
		}

		[TestMethod]
		public void Publish_ShouldNotTransformOnBuild()
		{
			var files = new[] {"Web.config" };
			
			PublishShouldNotTransformOnBuild(files, PublishProfile.Production);

			PublishShouldNotTransformOnBuild(files, PublishProfile.Test);
		}

		private static void PublishShouldNotTransformOnBuild(IEnumerable<string> files, PublishProfile publishProfile)
		{
			if(files == null)
				throw new ArgumentNullException("files");

			// ReSharper disable PossibleMultipleEnumeration

			var resetFiles = new List<string>();
			// ReSharper disable LoopCanBeConvertedToQuery
			foreach (var file in files)
			{
				var relativeResetDirectory = Path.GetDirectoryName(file) ?? string.Empty;
				var resetFileName = Path.GetFileName(file);

				resetFiles.Add(Path.Combine(_resetDirectory, relativeResetDirectory, string.Format(CultureInfo.InvariantCulture, "{0}-{1}", "Reset", resetFileName)));
			}
			// ReSharper restore LoopCanBeConvertedToQuery
			
			var webApplicationProject = new WebApplicationProject();

			// Reset
			for(int i = 0; i < files.Count(); i++)
			{
				File.Copy(resetFiles.ElementAt(i), Path.Combine(webApplicationProject.ProjectDirectory, files.ElementAt(i)), true);
			}

			webApplicationProject.Publish(publishProfile, LoggerVerbosity.Quiet);

			for(int i = 0; i < files.Count(); i++)
			{
				var expected = File.ReadAllText(resetFiles.ElementAt(i));
				var actual = File.ReadAllText(Path.Combine(webApplicationProject.ProjectDirectory, files.ElementAt(i)));

				Assert.AreEqual(expected, actual);
			}
			// ReSharper restore PossibleMultipleEnumeration
		}

		#endregion
	}
}