This brainfuck program should output the letter "A"

+++++ +++++ // set first cell to 10
[ // enter multiplication loop
> +++++ + // add 6 to the next cell
< - // go back and decrement the cell
] // memory layout: (0|60)
> +++++ // add 5 to the next cell; memory layout: (0|65)
. // print out character