import re


def solve(input_):
    rules = {rule_parts[0]: (lambda f, rule_strings, rule_dictionary: f(f, rule_strings, rule_dictionary))(lambda f, rule_strings, rule_dictionary: f(f, rule_strings[1:], (lambda groups: {groups['colour']: int(groups['amount']), **rule_dictionary})(re.match('(?P<amount>\\d*) (?P<colour>.*)', rule_strings[0]).groupdict())) if len(rule_strings) > 0 and rule_strings[0] != 'no other' else rule_dictionary, rule_parts[1:], {}) for rule_parts in [i.split(', ') for i in re.sub(' contain ', ', ', re.sub('( bags*|\n)', '', open('_').read())).split('.') if i != '']}

    all_parents = {}


    print(len((lambda f, colour, all_rules, all_parents: f(f, colour, all_rules, all_parents))(lambda f, colour, all_rules, all_parents: all_parents[colour] if colour in all_parents.keys() else (lambda item: item if all_parents.__setitem__(colour, item) is None else None)(set([p for parent in [k for k, v in all_rules.items() if colour in v.keys()] for p in [parent, *f(f, parent, all_rules, all_parents)]])), 'shiny gold', {rule_parts[0]: (lambda f, rule_strings, rule_dictionary: f(f, rule_strings, rule_dictionary))(lambda f, rule_strings, rule_dictionary: f(f, rule_strings[1:], (lambda groups: {groups['colour']: int(groups['amount']), **rule_dictionary})(re.match('(?P<amount>\\d*) (?P<colour>.*)', rule_strings[0]).groupdict())) if len(rule_strings) > 0 and rule_strings[0] != 'no other' else rule_dictionary, rule_parts[1:], {}) for rule_parts in [i.split(', ') for i in re.sub(' contain ', ', ', re.sub('( bags*|\n)', '', open('_').read())).split('.') if i != '']}, {})))

    def get_parents(colour):
        if colour in all_parents.keys():
            return all_parents[colour]

        all_parents[colour] = []

        colour_parents = [k for k, v in rules.items() if colour in v.keys()]
        parent_parents = [p for k in colour_parents for p in get_parents(k)]

        for i in set(colour_parents + parent_parents):
            all_parents[colour].append(i)

        return all_parents[colour]

    all_child_counts = {}

    def get_child_count(colour):
        if colour in all_child_counts.keys():
            return all_child_counts[colour]

        all_child_counts[colour] = 0

        for child_colour, child_count in rules[colour].items():
            child_children_count = get_child_count(child_colour)
            all_child_counts[colour] += (1 + child_children_count) * child_count

        return all_child_counts[colour]

    return len(get_parents('shiny gold')), get_child_count('shiny gold')


if __name__ == '__main__':
    test_data = 'light red bags contain 1 bright white bag, 2 muted yellow bags.\ndark orange bags contain 3 bright white bags, 4 muted yellow bags.\nbright white bags contain 1 shiny gold bag.\nmuted yellow bags contain 2 shiny gold bags, 9 faded blue bags.\nshiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.\ndark olive bags contain 3 faded blue bags, 4 dotted black bags.\nvibrant plum bags contain 5 faded blue bags, 6 dotted black bags.\nfaded blue bags contain no other bags.\ndotted black bags contain no other bags.'
    test_result = (4, 126)
    test_returned = solve(test_data)

    if test_returned[0] != test_result[0] and test_returned[1] != test_result[1]:
        raise Exception('Test failed Got: {}; Expected: {}'.format(test_returned, test_result))

    print(solve(open('input.txt').read()))
