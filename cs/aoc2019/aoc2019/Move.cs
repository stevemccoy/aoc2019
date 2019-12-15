namespace aoc2019
{
    public class Move
    {
        public Move(string code)
        {
            string direction = code.ToUpper().Substring(0, 1);
            Length = int.Parse(code.Substring(1));

            switch (direction)
            {
                case "R":
                    Dx = 1;
                    break;
                case "L":
                    Dx = -1;
                    break;
                case "U":
                    Dy = 1;
                    break;
                case "D":
                    Dy = -1;
                    break;
            }
        }

        public int Length;
        public int Dx;
        public int Dy;
    }
}
