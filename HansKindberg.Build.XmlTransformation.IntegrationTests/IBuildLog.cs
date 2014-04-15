namespace HansKindberg.Build.XmlTransformation.IntegrationTests
{
	public interface IBuildLog
	{
		#region Properties

		string Messages { get; }

		#endregion

		#region Methods

		string ToString();

		#endregion
	}
}