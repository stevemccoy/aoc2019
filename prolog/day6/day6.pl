%
% Day 6 Solution 
% 

identifier_char(Cout) -->
	[Cin],
	{	char_type(Cin, alnum),
		downcase_atom(Cin, Cout)
	}.

identifier_chars([HChar | TChars]) -->
	identifier_char(HChar),
	identifier_chars(TChars).
identifier_chars([Char]) -->
	identifier_char(Char).

% ID of a body - sequence of alphanumeric characters.
thing(AtomName) -->
	identifier_chars(IDChars),
	{ atom_chars(AtomName, IDChars)
	}.

% DCG Parser for input file - read in "XYZ)ABC" syntax to construct arcs for graph.
%
% orbits(B, A) == "B orbits around A"
%
orbits(B, A) -->
	thing(A), [')'], thing(B),
	!,
	{ assertz(direct_orbit(B, A))
	}.

read_input_line(Stream) :- 
	read_line_to_string(Stream, String),
	string_chars(String, Chars),
	phrase(orbits(_, _), Chars, []).

read_input_lines(Stream) :-
	read_input_line(Stream),
	!,
	read_input_lines(Stream).
read_input_lines(_).

read_input_data(FileName) :-
	retractall(direct_orbit(_,_)),
	open(FileName, read, Stream),
	read_input_lines(Stream),
	close(Stream).

solve_part1_for(FileName) :-
	read_input_data(FileName),
	setof((B, A), any_orbit(B, A), List),
	writeln(List),
	length(List, N),
	write("Number of orbits = "),
	writeln(N).

day6_test1 :-
	solve_part1_for("input/test1.txt").

day6_part1 :-
	solve_part1_for("input/input6.txt").

% Use the input data to assert link clauses.

% Analyse the links to count direct and indirect orbit relationships.

any_orbit(B, A) :-
	direct_orbit(B, A).
any_orbit(B, A) :-
	direct_orbit(C, A),
	any_orbit(B, C).


% Orbital transfers.

s(A, B) :- direct_orbit(A, B).
s(B, A) :- direct_orbit(A, B).

:- dynamic goal/1.

% Depth first search.

path(Node, Node, [Node]).
path(FirstNode, LastNode, [LastNode | Path]) :-
	path(FirstNode, OneButLast, Path),
	s(OneButLast, LastNode),
	not(member(LastNode, Path)).

depth_first_iterative_deepening(Node, Solution) :-
	path(Node, GoalNode, Solution),
	goal(GoalNode).

solve_part2_for(FileName) :-
	retractall(goal(_)),
	read_input_data(FileName),
	direct_orbit(you, Here),
	direct_orbit(san, There),
	assertz(goal(There)),
	depth_first_iterative_deepening(Here, Solution),
	!,
	write("From "),
	write(Here),
	write(" To "),
	writeln(There),
	writeln("Solution as follows:"),
	writeln(Solution),
	length(Solution, N), 
	M is N - 1,
	write("Length = "),
	writeln(M).

day6_test2 :-
	solve_part2_for("input/test2.txt").

day6_part2 :- 
	solve_part2_for("input/input6.txt").

