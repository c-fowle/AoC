print(sum([len(__import__('functools').reduce(lambda a, b: a & b, s)) if len(s) > 1 else len(s[0]) for s in [[set(a) for a in x.split('\n')] for x in open('_').read().split('\n\n')]]))