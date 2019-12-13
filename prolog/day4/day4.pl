%
% Day 4 Solution - six digit combination.
%
% day4_part1([D1,D2,D3,D4,D5,D6])
%

day4_part1(Count) :-
	setof(Solution, day4_part1_solution(Solution), Bag),
	length(Bag, Count).

day4_part1_solution(Solution) :-
	between(372304, 847060, N),
	integer_to_digits(N, Solution),
	non_decreasing(Solution),
	repeated_digits(Solution).

day4_part2(Count) :-
	setof(Solution, day4_part2_solution(Solution), Bag),
	length(Bag, Count).

day4_part2_solution(Solution) :-
	between(372304, 847060, N),
	integer_to_digits(N, Solution),
	non_decreasing(Solution),
	repeated_digits_part2(Solution).

% Convert integer to decimal digits.
integer_to_digits(Value, Digits) :-
	number_string(Value, S1),
	string_codes(S1, CL1),
	my_map_list(CL1, code_to_digit, Digits).

code_to_digit(Code, Digit) :-
	Digit is Code - 48.

% minimum value 372304
% maximum value 847060


% Map list elements from one list through a named function(X,Y), producing a second list.
my_map_list([], _, []).
my_map_list([X | InTail], Function, [Y | OutTail]) :-
	Goal =.. [Function, X, Y],
	Goal,
	my_map_list(InTail, Function, OutTail).

% Digits in sequence never decrease from left to right.
non_decreasing([_]).
non_decreasing([H1, H2 | Tail]) :- 
	H1 =< H2,
	!,
	non_decreasing([H2 | Tail]).

% Some pair of adjacent digits are repeated.
repeated_digits(Solution) :-
	conc(_, [D, D | _], Solution).

% Some pair of adjacent digits are repeated.
repeated_digits_part2([D1, D1, D2 | _]) :-
	D1 \== D2,
	!.
repeated_digits_part2(Solution) :-
	conc(_, [D0, D1, D1], Solution),
	D0 \== D1,
	!.
repeated_digits_part2(Solution) :-
	conc(_, [D0, D1, D1, D2 | _], Solution),
	D0 \== D1,
	D1 \== D2.

% List concatenation.
conc([], L, L).
conc([H | Tail], L1, [H | L2]) :-
	conc(Tail, L1, L2).
