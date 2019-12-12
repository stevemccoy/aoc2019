using System.Collections.Generic;

namespace aoc2019
{
    public class Path
    {
        public Path(string pathString, int id)
        {
            Id = id;
            start = (0, 0);
            Seen = new Dictionary<(int, int), int>();
            ReadMoves(pathString);
        }

        private void ReadMoves(string pathString)
        {
            moves = new List<Move>();
            foreach (var s in pathString.Split(','))
            {
                var m = new Move(s);
                moves.Add(m);
            }
        }

        public void TraceMoves(int id)
        {
            var intersections = new HashSet<(int,int)>();
            int x, y;
            (x, y) = start;
            int length = 0;
            foreach (var move in moves)
            {
                var count = move.length;
                while (count > 0)
                {
                    length++;
                    x += move.dx;
                    y += move.dy;
                    if (Seen.ContainsKey((x, y)))
                    {
                        intersections.Add((x, y));
                    }
                    else
                    {
                        Seen.Add((x, y), length);
                    }
                    count--;
                }
            }
//            return intersections;
        }

        public void Offset(int x, int y)
        {
            start = (x, y);
        }

        public int Id;

        private (int, int) start;
        private List<Move> moves;

        public Dictionary<(int, int), int> Seen;
    }
}