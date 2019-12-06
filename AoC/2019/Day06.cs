using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AoC.Common;
using AoC.Common.Attributes;
using AoC.Common.ExtensionMethods;

using AoC._2019.Classes;

namespace AoC._2019
{
    [Year(2019)]
    [Day(6)]
    [Test("COM)B\nB)C\nC)D\nD)E\nE)F\nB)G\nG)H\nD)I\nE)J\nJ)K\nK)L", "42", null)]
    [Test("COM)B\nB)C\nC)D\nD)E\nE)F\nB)G\nG)H\nD)I\nE)J\nJ)K\nK)L\nK)YOU\nI)SAN", null, "4")]

    public class Day06 : Puzzle
    {
        private Dictionary<string, string> ParentObjectCache { get; }
        private Dictionary<string, List<string>> OrbitTreeCache { get; }
        
        public Day06() : base()
        {
            ParentObjectCache = new Dictionary<string, string>();
            OrbitTreeCache = new Dictionary<string, List<string>>();
        }
        private List<string[]> ParseInput(string input) => input.Split('\n').ForEach(s => s.Split(')').ToArray()).ToList();
        private void ClearCaches()
        {
            ParentObjectCache.Clear();
            OrbitTreeCache.Clear();
        }
        private string GetParent(string objName, Dictionary<string, List<string>> allOrbits)
        {
            if (!ParentObjectCache.ContainsKey(objName))
            {
                var objOrbits = allOrbits.Where(kvp => kvp.Value.Contains(objName)).Select(kvp => kvp.Key);
                if (objOrbits.Count() == 0) ParentObjectCache.Add(objName, null);           
                else ParentObjectCache.Add(objName, objOrbits.Single());
            }
            return ParentObjectCache[objName];
        }
        private IList<string> GetOrbitTree(string objName, Dictionary<string, List<string>> allOrbits)
        {
            if (!OrbitTreeCache.ContainsKey(objName)) 
            {
                var parent = GetParent(objName, allOrbits);
                if (parent == null) OrbitTreeCache.Add(objName, new List<string>());
                else
                {
                    var tree = GetOrbitTree(parent, allOrbits).CloneAsList().ToList();
                    tree.Add(parent);
                    OrbitTreeCache.Add(objName, tree);
                }
            }
            return OrbitTreeCache[objName];
        }
        private int GetIndirectOrbits(string objName, Dictionary<string, List<string>> allOrbits) => GetOrbitTree(objName, allOrbits).Count();
        private string GetNearestCommonNeighbour (string obj1, string obj2, Dictionary<string, List<string>> allOrbits)
        {
            var obj1Tree = GetOrbitTree(obj1, allOrbits);
            var obj2Tree = GetOrbitTree(obj2, allOrbits);

            var overlaps = obj1Tree.Where(o => obj2Tree.Contains(o));

            return overlaps.OrderByDescending(o => GetIndirectOrbits(o, allOrbits)).First();
        }
        protected override string Part1(string input)
        {
            ClearCaches();

            var orbitMap = ParseInput(input);
            var orbits = new Dictionary<string, List<string>>();
            var objects = new List<string>();

            orbitMap.ForEach(od =>
            {
                if (!orbits.ContainsKey(od[0])) orbits.Add(od[0], new List<string>());
                if (!objects.Contains(od[0])) objects.Add(od[0]);
                if (!objects.Contains(od[1])) objects.Add(od[1]);
                orbits[od[0]].Add(od[1]);
            });

            var totalIndirectOrbits = 0;

            objects.ForEach(obj => totalIndirectOrbits += GetIndirectOrbits(obj, orbits));

            return totalIndirectOrbits.ToString();
        }
        protected override string Part2(string input)
        {
            ClearCaches();

            var orbitMap = ParseInput(input);
            var orbits = new Dictionary<string, List<string>>();
            var objects = new List<string>();

            orbitMap.ForEach(od =>
            {
                if (!orbits.ContainsKey(od[0])) orbits.Add(od[0], new List<string>());
                if (!objects.Contains(od[0])) objects.Add(od[0]);
                if (!objects.Contains(od[1])) objects.Add(od[1]);
                orbits[od[0]].Add(od[1]);
            });

            var yourChildren = GetIndirectOrbits(GetParent("YOU", orbits), orbits);
            var santasChildren = GetIndirectOrbits(GetParent("SAN", orbits), orbits);

            var neighbour = GetNearestCommonNeighbour("YOU", "SAN", orbits);
            var neighboursChildren = GetIndirectOrbits(neighbour, orbits);


            return ((yourChildren - neighboursChildren) + (santasChildren - neighboursChildren)).ToString();
        }
    }
}
