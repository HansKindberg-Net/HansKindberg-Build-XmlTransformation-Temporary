namespace HansKindberg.Build.XmlTransformation.IntegrationTests.WebApplication
{
	public class PublishResult : IPublishResult
	{
		#region Properties

		public virtual IBuildLog BuildLog { get; set; }
		public virtual string PackageDirectory { get; set; }

		#endregion
	}
}