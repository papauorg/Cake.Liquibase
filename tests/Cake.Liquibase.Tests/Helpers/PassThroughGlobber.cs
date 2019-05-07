using System;
using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.Liquibase.Tests.Helpers
{
    public class PassThroughGlobber : IGlobber
    {
        public IEnumerable<Path> Match(string pattern, GlobberSettings settings)
        {
            return new FilePath[] { new FilePath(pattern) };
        }

        IEnumerable<Path> IGlobber.Match(string pattern, Func<IDirectory, bool> predicate)
        {
           return new FilePath[] { new FilePath(pattern) }; 
        }
    }
}