using System;
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
		private static readonly string _expectedPublishDirectory = Path.Combine(Solution.Directory, "HansKindberg.Build.XmlTransformation.IntegrationTests", @"WebApplication\Expected\Publish");

		#region Methods

		[TestMethod]
		public void Publish_ShouldTransformCorrectly()
		{
			// Production-profile
			var publishProfile = PublishProfile.Production;
			var expected = File.ReadAllText(Path.Combine(_expectedPublishDirectory, string.Format(CultureInfo.InvariantCulture, "Publish-{0}-Web.config", publishProfile)));
			
			var webApplicationProject = new WebApplicationProject();

			webApplicationProject.Publish(publishProfile, LoggerVerbosity.Quiet);

			var actual = File.ReadAllText(Path.Combine(webApplicationProject.GetPublishDirectory(webApplicationProject.GetConfiguration(publishProfile)), "Web.config"));

			Assert.AreEqual(expected, actual);

			// Test-profile
			publishProfile = PublishProfile.Test;
			expected = File.ReadAllText(Path.Combine(_expectedPublishDirectory, string.Format(CultureInfo.InvariantCulture, "Publish-{0}-Web.config", publishProfile)));

			webApplicationProject.Publish(publishProfile, LoggerVerbosity.Quiet);

			actual = File.ReadAllText(Path.Combine(webApplicationProject.GetPublishDirectory(webApplicationProject.GetConfiguration(publishProfile)), "Web.config"));

			Assert.AreEqual(expected, actual);
		}

		#endregion
	}
}