%
% Day 14 Solution 
% 

:- working_directory(_, 'c:/src/github/aoc2019/prolog/day14/').

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

% Transform required list of products into required list of reactants.
shopping_list([], []).
shopping_list([Product | Tail], Reactants) :-
	how_to_make(Product, Requirements),
	merge_lists(Requirements, Tail, Reactants).

how_to_make(Product, Reqts) :-
	Product =.. [ProdName, ProdQtyNeeded],
	Head =.. [ProdName, ProdBasis],
	produce_from(Head, )


day14_part1 :-
	read_input_data("input/input14.txt").
