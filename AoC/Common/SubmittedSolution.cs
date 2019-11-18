using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC.Common
{
    public class SubmittedSolution
    {
        public string Solution { get; }
        public SolutionResponse Response { get; }
        public SubmittedSolution(params string[] stringData)
        {
            Solution = stringData[0];
            Response = (SolutionResponse)int.Parse(stringData[1]);
        }
        public SubmittedSolution(string solution, SolutionResponse response)
        {
            Solution = solution;
            Response = response;
        }
        public override string ToString()
        {
            return Solution + ";" + ((int)Response).ToString();
        }
    }
}
