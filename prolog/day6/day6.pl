%
% Day 6 Solution 
% 

day6_part1() :-
	open("C:\\src\\github\\aoc2019\\cs\\aoc2019\\aoc2019\\input\\input6.txt", read, Stream),
	read_line_to_string(Stream, String),
	string_chars(String, CharList),
	identifier(C1, C2, CharList, Rest),
	writeln(String),
	writeln([C1, C2, CharList, Rest]),
	close(Stream).


list([])     --> [].
list([L|Ls]) --> [L], list(Ls).

% DCG Parser for input file - read in "XYZ)ABC" syntax to construct arcs for graph.
%
% orbits(B, A) == "B orbits around A"
%
orbits(B, A, InList, Tail) -->
	thing(A), ")", thing(B).
%	{ assertz(direct_orbit(BName, AName))
%	}.

% ID of a body - sequence of alphanumeric characters.
thing(AtomName, InList) -->
	identifier_chars(InList, IDChars),
	{ atom_codes(AtomName, IDChars)
	}.

identifier_chars([]).
identifier_chars([HChar | TChars]) -->
	identifier_char(HChar),
	identifier_chars(TChars).

identifier_char(Cout) -->
	[Cin],
	{	code_type(Cin, alnum),
		downcase_atom(Cin, Cout)
	}.


% Use the input data to assert link clauses.

% Analyse the links to count direct and indirect orbit relationships.

