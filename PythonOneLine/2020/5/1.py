print(max([int(''.join([str(int(c in 'BR')) for c in b]), 2) for b in open('_').read().split('\n')]))