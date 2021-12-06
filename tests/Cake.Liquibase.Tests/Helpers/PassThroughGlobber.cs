using Cake.Core.IO;
using System.Collections.Generic;

namespace Cake.Liquibase.Tests.Helpers
{
	public class PassThroughGlobber : IGlobber
	{
		public IEnumerable<Path> Match(GlobPattern pattern, GlobberSettings settings)
		{
			return new FilePath[] { new FilePath(pattern) };
		}
	}
}