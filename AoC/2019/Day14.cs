using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AoC.Common;
using AoC.Common.Attributes;
using AoC.Common.ExtensionMethods;

using AoC._2019.Classes;
using AoC._2019.Enums;

namespace AoC._2019
{
    class ReactionComponent
    {
        public string Element { get; }
        public int Stoichiometry { get; }

        public ReactionComponent(string element, int stoichiometry)
        {
            Element = element;
            Stoichiometry = stoichiometry;
        }
    }

    class Reaction
    {
        public List<ReactionComponent> Reactants { get; }
        public ReactionComponent Product { get; }

        public Reaction(ReactionComponent product)
        {
            Product = product;
            Reactants = new List<ReactionComponent>();
        }
    }

    [Year(2019)]
    [Day(14)]
    [Test(
        "10 ORE => 10 A\n" +
        "1 ORE => 1 B\n" +
        "7 A, 1 B => 1 C\n" +
        "7 A, 1 C => 1 D\n" +
        "7 A, 1 D => 1 E\n" +
        "7 A, 1 E => 1 FUEL",
        "31", null)]
    [Test(
        "157 ORE => 5 NZVS\n" +
        "165 ORE => 6 DCFZ\n" +
        "44 XJWVT, 5 KHKGT, 1 QDVJ, 29 NZVS, 9 GPVTF, 48 HKGWZ => 1 FUEL\n" +
        "12 HKGWZ, 1 GPVTF, 8 PSHF => 9 QDVJ\n179 ORE => 7 PSHF\n" +
        "177 ORE => 5 HKGWZ\n" +
        "7 DCFZ, 7 PSHF => 2 XJWVT\n" +
        "165 ORE => 2 GPVTF\n" +
        "3 DCFZ, 7 NZVS, 5 HKGWZ, 10 PSHF => 8 KHKGT",
        "13312", "82892753")]
    [Test(
        "2 VPVL, 7 FWMGM, 2 CXFTF, 11 MNCFX => 1 STKFG\n" +
        "17 NVRVD, 3 JNWZP => 8 VPVL\n" +
        "53 STKFG, 6 MNCFX, 46 VJHF, 81 HVMC, 68 CXFTF, 25 GNMV => 1 FUEL\n" +
        "22 VJHF, 37 MNCFX => 5 FWMGM\n" +
        "139 ORE => 4 NVRVD\n" +
        "144 ORE => 7 JNWZP\n" +
        "5 MNCFX, 7 RFSQX, 2 FWMGM, 2 VPVL, 19 CXFTF => 3 HVMC\n" +
        "5 VJHF, 7 MNCFX, 9 VPVL, 37 CXFTF => 6 GNMV\n" +
        "145 ORE => 6 MNCFX\n" +
        "1 NVRVD => 8 CXFTF\n" +
        "1 VJHF, 6 MNCFX => 4 RFSQX\n" +
        "176 ORE => 6 VJHF",
        "180697", "5586022")]
    [Test(
        "171 ORE => 8 CNZTR\n" +
        "7 ZLQW, 3 BMBT, 9 XCVML, 26 XMNCP, 1 WPTQ, 2 MZWV, 1 RJRHP => 4 PLWSL\n" +
        "114 ORE => 4 BHXH\n" +
        "14 VRPVC => 6 BMBT\n" +
        "6 BHXH, 18 KTJDG, 12 WPTQ, 7 PLWSL, 31 FHTLT, 37 ZDVW => 1 FUEL\n" +
        "6 WPTQ, 2 BMBT, 8 ZLQW, 18 KTJDG, 1 XMNCP, 6 MZWV, 1 RJRHP => 6 FHTLT\n" +
        "15 XDBXC, 2 LTCX, 1 VRPVC => 6 ZLQW\n" +
        "13 WPTQ, 10 LTCX, 3 RJRHP, 14 XMNCP, 2 MZWV, 1 ZLQW => 1 ZDVW\n" +
        "5 BMBT => 4 WPTQ\n" +
        "189 ORE => 9 KTJDG\n" +
        "1 MZWV, 17 XDBXC, 3 XCVML => 2 XMNCP\n" +
        "12 VRPVC, 27 CNZTR => 2 XDBXC\n" +
        "15 KTJDG, 12 BHXH => 5 XCVML\n" +
        "3 BHXH, 2 VRPVC => 7 MZWV\n" +
        "121 ORE => 7 VRPVC\n" +
        "7 XCVML => 6 RJRHP\n" +
        "5 BHXH, 4 VRPVC => 5 LTCX",
        "2210736", "460664")]
    public class Day14 : _2019Puzzle
    {
        private List<Reaction> ParseInput(string input)
        {
            var parsedReactions = new List<Reaction>();
            var inputLines = input.Split('\n').ToArray();

            inputLines.ForEach(s =>
            {
                var splitReaction = s.Split(new[] { " => " }, StringSplitOptions.RemoveEmptyEntries);
                var productDetails = splitReaction[1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var reactantDetails = splitReaction[0].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(r => r.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)).ToList();

                var product = new ReactionComponent(productDetails[1], int.Parse(productDetails[0]));
                var thisReaction = new Reaction(product);

                reactantDetails.ForEach(r =>
                {
                    var reactant = new ReactionComponent(r[1], int.Parse(r[0]));
                    thisReaction.Reactants.Add(reactant);
                });

                parsedReactions.Add(thisReaction);
            });

            return parsedReactions;
        }

        private List<string> GetDerivatives(string element, IDictionary<string, List<string>> derivatives, IList<Reaction> reactions)
        {
            if (derivatives.ContainsKey(element)) return derivatives[element];

            derivatives.Add(element, new List<string>());

            reactions.Where(reaction => reaction.Reactants.Any(r => r.Element == element)).ForEach(reaction =>
            {
                if (!derivatives[element].Contains(reaction.Product.Element)) derivatives[element].Add(reaction.Product.Element);
                GetDerivatives(reaction.Product.Element, derivatives, reactions).ForEach(childDerivative =>
                {
                    if (!derivatives[element].Contains(childDerivative)) derivatives[element].Add(childDerivative);
                });
            });

            return derivatives[element];
        }

        private void MakeElement(string element, long amount, IDictionary<string, long> resources, IList<Reaction> reactions)
        {
            var synthesis = reactions.SingleOrDefault(r => r.Product.Element == element);
            var repetitions = (long)Math.Ceiling((double)amount / synthesis.Product.Stoichiometry);

            synthesis.Reactants.ForEach(r =>
            {
                if (resources[r.Element] < (r.Stoichiometry * repetitions)) MakeElement(r.Element, (r.Stoichiometry * repetitions) - resources[r.Element], resources, reactions);
                resources[r.Element] -= (r.Stoichiometry * repetitions);
            });

            resources[element] += (synthesis.Product.Stoichiometry * repetitions);
        }

        protected override string Part1(string input)
        {
            var reactionList = ParseInput(input);
            var elementList = reactionList.Select(r => r.Product.Element).ToList();
            elementList.Add("ORE");

            var derivatives = new Dictionary<string, List<string>>();
            elementList.ForEach(e => GetDerivatives(e, derivatives, reactionList));

            var resources = elementList.ToDictionary(e => e, e => 0);
            resources["FUEL"] = 1;

            while (resources.Any(kvp => kvp.Key != "ORE" && kvp.Value != 0))
            {
                elementList.ForEach(e =>
                {
                    if (resources[e] == 0) return;
                    if (derivatives[e].Any(d => resources[d] != 0)) return;

                    var synthesis = reactionList.SingleOrDefault(r => r.Product.Element == e);
                    if (synthesis == null) return;

                    var scale = (int)Math.Ceiling((float)resources[e] / synthesis.Product.Stoichiometry);
                    synthesis.Reactants.ForEach(r => resources[r.Element] += scale * r.Stoichiometry);
                    resources[e] = 0;
                });
            }

            return resources["ORE"].ToString();
        }

        protected override string Part2(string input)
        {
            var reactionList = ParseInput(input);
            var elementList = reactionList.Select(r => r.Product.Element).ToList();
            elementList.Add("ORE");

            var derivatives = new Dictionary<string, List<string>>();
            elementList.ForEach(e => GetDerivatives(e, derivatives, reactionList));

            var minimumOreRequired = int.Parse(Part1(input));

            var resources = elementList.ToDictionary(e => e, e => 0L);
            resources["ORE"] = 1000000000000;

            while(resources["ORE"] > minimumOreRequired) MakeElement("FUEL", (long)Math.Floor(resources["ORE"] / (double)minimumOreRequired), resources, reactionList);

            return resources["FUEL"].ToString();
        }
    }
}
