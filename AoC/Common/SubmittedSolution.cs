using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC.Common
{
    public class SubmittedSolution
    {
        public int Part { get; }
        public string Solution { get; }
        public SolutionResponse Response { get; }
        public SubmittedSolution(params string[] stringData)
        {
            Part = int.Parse(stringData[0]);
            Solution = stringData[1];
            Response = (SolutionResponse)int.Parse(stringData[2]);
        }
        public SubmittedSolution(int part, string solution, SolutionResponse response)
        {
            Part = part;
            Solution = solution;
            Response = response;
        }
        public override string ToString()
        {
            return Part + ";" + Solution + ";" + ((int)Response).ToString();
        }
    }
}
