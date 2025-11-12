using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.Liquibase.Tests.Helpers
{
    public class PassThroughGlobber : IGlobber
    {
        public IEnumerable<Path> Match(GlobPattern pattern, GlobberSettings settings)
        {
            return [new FilePath(pattern)];
        }
    }
}