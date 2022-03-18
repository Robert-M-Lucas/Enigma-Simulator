_in = input("Enter encoding string: ")

alph = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"

out = "{ "
for i in _in:
    out += str(alph.index(i)) + ", "
out = out[:-2] + " }"
print(out)