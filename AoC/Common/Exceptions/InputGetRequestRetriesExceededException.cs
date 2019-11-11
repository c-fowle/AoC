using System;

namespace AoC.Common.Exceptions
{
    public class InputGetRequestRetriesExceededException : Exception
    {
        public InputGetRequestRetriesExceededException()
            : base ("Failed to successfully download input data within the allowed number of retries")
        {
        }
    }
}
