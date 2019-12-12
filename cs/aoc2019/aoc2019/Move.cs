namespace aoc2019
{
    public class Move
    {
        public Move(string code)
        {
            string direction = code.ToUpper().Substring(0, 1);
            length = int.Parse(code.Substring(1));

            switch (direction)
            {
                case "R":
                    dx = 1;
                    break;
                case "L":
                    dx = -1;
                    break;
                case "U":
                    dy = 1;
                    break;
                case "D":
                    dy = -1;
                    break;
            }
        }

        public int length;
        public int dx;
        public int dy;
    }
}
