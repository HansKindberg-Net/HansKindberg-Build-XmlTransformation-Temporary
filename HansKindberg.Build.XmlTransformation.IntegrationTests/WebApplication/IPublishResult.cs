namespace HansKindberg.Build.XmlTransformation.IntegrationTests.WebApplication
{
	public interface IPublishResult
	{
		#region Properties

		IBuildLog BuildLog { get; }
		string PackageDirectory { get; }

		#endregion
	}
}