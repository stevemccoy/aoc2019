%
% Day 14 Solution 
% 

:- working_directory(_, 'c:/src/github/aoc2019/prolog/day14/').
:- dynamic produce_from/2.

integer(I) -->
        digit(D0),
        digits(D),
        { number_codes(I, [D0|D])
        }.

digits([D|T]) -->
        digit(D), !,
        digits(T).
digits([]) -->
        [].

digit(D) -->
        [D],
        { code_type(D, digit)
        }.

% ID of a substance - sequence of alphanumeric characters.
identifier(AtomName) -->
	identifier_chars(IDChars),
	{ atom_chars(AtomName, IDChars)
	}.

identifier_chars([HChar | TChars]) -->
	identifier_char(HChar),
	identifier_chars(TChars).
identifier_chars([Char]) -->
	identifier_char(Char).

identifier_char(Cout) -->
	[Cin],
	{	char_type(Cin, alnum),
		downcase_atom(Cin, Cout)
	}.

space(Char) -->
	[Char],
	{ code_type(Char, space)
	}.

spaces([]) -->
	[].
spaces([Char | Rest]) -->
	space(Char),
	spaces(Rest).

reaction(Product, Reactants) -->
	species_list(Reactants), spaces(_), [ '=', '>' ], spaces(_),
	species(Product).

species_list([S, T | Tail]) -->
	species(S), spaces(_), [','], spaces(_),
	species_list([T |Tail]).
species_list([S]) -->
	species(S).

species(S) -->
	integer(I), spaces(_), identifier(AtomName),
	{
		S =.. [AtomName, I]
	}.

read_input_line(Stream, P, R) :- 
	read_line_to_string(Stream, String),
	string_chars(String, Chars),
	phrase(reaction(P, R), Chars, []),
	assertz(produce_from(P, R)).

read_input_lines(Stream) :-
	read_input_line(Stream, _, _),
	!,
	read_input_lines(Stream).
read_input_lines(_).

read_input_data(FileName) :-
	retractall(produce_from(_, _)),
	open(FileName, read, Stream),
	read_input_lines(Stream),
	close(Stream).

read_input_string(String) :-
	retractall(produce_from(_, _)),
	open_string(String, Stream),
	read_input_lines(Stream),
	close(Stream).

% shopping_list(Products, Reactants).
% Transform required list of products into required list of reactants.
shopping_list([], []).
shopping_list([Product | Tail], Reactants) :-
	how_to_make(Product, Requirements),
	!,
	merge_lists(Tail, Requirements, Tail2),
	shopping_list(Tail2, Reactants).
% Product cannot be produced by a reactant.
shopping_list([Product | Tail], Result) :-
	shopping_list(Tail, Reactants),
	merge_lists([Product], Reactants, Result).

% Determine required reactants to produce single product at a given quantity.
how_to_make(Product, Reqts) :-
	Product =.. [ProdName, ProdQtyNeeded],
	% Construct a term to pattern match with reaction list.
	Head =.. [ProdName, ProdBasis],
	produce_from(Head, Tail),
	% How many times over do we need this reaction?
	Factor is ceiling(ProdQtyNeeded / ProdBasis),
	reactants_times(Tail, Factor, Reqts).

% Multiply the quantity of all the given reactants by a factor.
reactants_times([], _, []).
reactants_times([R1 | Rest1], Factor, [R2 | Rest2]) :-
	!,
	R1 =.. [Name, Qty1],
	Qty2 is Qty1 * Factor,
	R2 =.. [Name, Qty2],
	reactants_times(Rest1, Factor, Rest2).

merge_lists(List1, List2, Result) :-
	conc(List1, List2, List3),
	merge_lists_rec(List3, [], Result).

% Merge the items from the first list, into the second, providing
% the final result in the third argument.

merge_lists_rec([], Result, Result).
merge_lists_rec([Reqt1 | Rest1], Into, Result) :-
	Reqt1 =.. [Name, Qty1],
	Reqt2 =.. [Name, Qty2],
	del(Reqt2, Into, Into2),
	!,
	Qty3 is Qty1 + Qty2,
	Reqt3 =.. [Name, Qty3],
	conc(Into2, [Reqt3], Into3),
	merge_lists_rec(Rest1, Into3, Result).
merge_lists_rec([Reqt1 | Rest1], Into, Result) :-
	merge_lists_rec(Rest1, [Reqt1 | Into], Result).

% List concatenation.
conc([], L, L).
conc([H | Tail], L1, [H | L2]) :-
	conc(Tail, L1, L2).

% Delete an item from a list.
del(Item, [Item | List], List).
del(Item, [First | List1], [First | List2]) :-
	del(Item, List1, List2).


day14_part1 :-
	read_input_data("input/input14.txt").


day14_test1 :-
	read_input_string("10 ORE => 10 A
1 ORE => 1 B
7 A, 1 B => 1 C
7 A, 1 C => 1 D
7 A, 1 D => 1 E
7 A, 1 E => 1 FUEL"),
	shopping_list([fuel(1)], L),
	writeln(L).

day14_test2 :-
	read_input_string("9 ORE => 2 A
8 ORE => 3 B
7 ORE => 5 C
3 A, 4 B => 1 AB
5 B, 7 C => 1 BC
4 C, 1 A => 1 CA
2 AB, 3 BC, 4 CA => 1 FUEL"),
	shopping_list([fuel(1)], L),
	writeln(L).

day14_test3 :-
	read_input_string("157 ORE => 5 NZVS
165 ORE => 6 DCFZ
44 XJWVT, 5 KHKGT, 1 QDVJ, 29 NZVS, 9 GPVTF, 48 HKGWZ => 1 FUEL
12 HKGWZ, 1 GPVTF, 8 PSHF => 9 QDVJ
179 ORE => 7 PSHF
177 ORE => 5 HKGWZ
7 DCFZ, 7 PSHF => 2 XJWVT
165 ORE => 2 GPVTF
3 DCFZ, 7 NZVS, 5 HKGWZ, 10 PSHF => 8 KHKGT"),
	shopping_list([fuel(1)], L),
	writeln(L).



