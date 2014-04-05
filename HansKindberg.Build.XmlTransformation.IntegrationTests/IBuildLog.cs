using System.Collections.Generic;

namespace HansKindberg.Build.XmlTransformation.IntegrationTests
{
	public interface IBuildLog
	{
		#region Properties

		IEnumerable<string> Messages { get; }

		#endregion

		#region Methods

		string ToString();

		#endregion
	}
}