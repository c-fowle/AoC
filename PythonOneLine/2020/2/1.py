print(sum([1 for a in [__import__('re').match(r'(?P<min>\d*)-(?P<max>\d*) (?P<char>[a-z]): (?P<pass>[a-z]*)', s).groups() for s in open('_').read().split('\n')] if a[3].count(a[2]) <= int(a[1]) and a[3].count(a[2]) >= int(a[0])]))