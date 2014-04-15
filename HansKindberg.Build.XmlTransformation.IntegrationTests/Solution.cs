using System.IO;

namespace HansKindberg.Build.XmlTransformation.IntegrationTests
{
	public static class Solution
	{
		#region Fields

		private static readonly string _directory = new DirectoryInfo(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName + @"\";

		#endregion

		#region Properties

		public static string Directory
		{
			get { return _directory; }
		}

		#endregion
	}
}