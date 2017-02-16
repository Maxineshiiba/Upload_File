
import csv

# Creates a list containing 6 lists, each of 6 items, all set to 0
arr = [["" for x in range(6)] for x in range(6)]
max_len=0;

with open('matrix.csv') as csvfile:
    reader = csv.DictReader(csvfile)
    for row in reader:
        print(row['name'], row['test'], row['launch'])
        if( len(arr[int(row['test'])][int(row['launch'])]) == 0 ) :
			arr[int(row['test'])][int(row['launch'])] = row['name']
        else:
			arr[int(row['test'])][int(row['launch'])] = arr[int(row['test'])][int(row['launch'])] + "," + row['name']

        print "he"
        current_len = len(arr[int(row['test'])][int(row['launch'])])

        if( current_len > max_len ):
			max_len=len(arr[int(row['test'])][int(row['launch'])])

pad=" " * max_len

for r in range(6):
	row_str = "|"
	for c in range(6):
		size = len(arr[r][c])
		row_str = row_str + pad[0:max_len-size] + arr[r][c] + "|"
	print row_str
