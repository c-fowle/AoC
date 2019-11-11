using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AoC.Common.Exceptions
{
    public class NoAoCKeyException : Exception
    {
        public NoAoCKeyException(string path="Data/AoCKey.txt")
            : base(String.Format("Could not locate AoC key file at '{0}'", path))
        {
        }
    }
}
