print((lambda r: (len((lambda f, a, b, c: f(f, a, b, c))(lambda f, a, b, c: c[a] if a in c else (lambda d: d if c.__setitem__(a, d) is None else None)(set([g for e in [d for d in b if a in b[d]] for g in [e, *f(f, e, b, c)]])), 'shiny gold', {a[0]: (lambda f, b, c: f(f, b, c))(lambda f, b, c: f(f, b[1:], (lambda d: {d['b']: int(d['a']), **c})(r.match('(?P<a>\\d*) (?P<b>.*)', b[0]).groupdict())) if len(b) > 0 and b[0][0] != 'n' else c, a[1:], {}) for a in [i.split(', ') for i in r.sub(' contain ', ', ', r.sub('( bags*|\n)', '', open('_').read())).split('.') if i != '']}, {}))))(__import__('re')))