using System.Diagnostics;

namespace OzonContest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var dirProblemPath = @"Data\4";
            Problem problem = new Problem4();

            if (Directory.Exists(dirProblemPath))
            {
                foreach (string file in Directory.GetFiles(dirProblemPath))
                {
                    if (int.TryParse(Path.GetFileName(file), out _))
                    {
                        problem.SolveFile(file);
                    }
                }
            }
            else
            {
                problem.Solve();
            }
        }
    }

    public abstract class Problem
    {
        public abstract void Solver(StreamReader input, StreamWriter output);

        public void Solve()
        {
            using var input = new StreamReader(Console.OpenStandardInput());
            using var output = new StreamWriter(Console.OpenStandardOutput());

            Solver(input, output);
        }

        public void SolveFile(string fileName)
        {
            var sw = Stopwatch.StartNew();
            using var inputFile = new FileStream(fileName, FileMode.Open);
            using var outputFile = new FileStream(fileName + "aa", FileMode.OpenOrCreate);
            using var input = new StreamReader(inputFile);
            using var output = new StreamWriter(outputFile);

            Solver(input, output);
            Console.WriteLine($"{fileName} {sw.Elapsed}");
        }

        public int ReadLineAsInt(StreamReader input) => Convert.ToInt32(input.ReadLine());
        public string[] ReadLineAsStringArr(StreamReader input) => input.ReadLine()!.Split(' ');

        public int[] ReadLineAsIntArr(StreamReader input) => ReadLineAsStringArr(input)
            .Select(v => Convert.ToInt32(v))
            .ToArray();

        public double[] ReadLineAsDoubleArr(StreamReader input) => ReadLineAsStringArr(input)
            .Select(Convert.ToDouble)
            .ToArray();
    }

    public class Problem1 : Problem
    {
        public override void Solver(StreamReader input, StreamWriter output)
        {
            var lineCount = ReadLineAsDoubleArr(input).First();
            for (int i = 0; i < lineCount; i++)
            {
                output.WriteLine(ReadLineAsDoubleArr(input).Sum());
            }
        }
    }

    public class Problem2 : Problem
    {
        public override void Solver(StreamReader input, StreamWriter output)
        {
            var originalSticker = input.ReadLine()!.ToCharArray();
            var stickerCount = ReadLineAsDoubleArr(input).First();

            for (int i = 0; i < stickerCount; i++)
            {
                var line = ReadLineAsStringArr(input);
                var start = Convert.ToInt32(line[0]);
                var end = Convert.ToInt32(line[1]);
                var sticker = line[2].ToCharArray();

                for (int j = start; j <= end; j++)
                {
                    originalSticker[j - 1] = sticker[j - start];
                }
            }

            output.WriteLine(originalSticker);
        }
    }


    public class Problem3 : Problem
    {
        public int GlobalMessageId { get; set; } = 0;
        public int MessageCount { get; set; } = 0;
        public int UserCount { get; set; } = 0;
        public Dictionary<int, int> DataBase { get; set; }

        public override void Solver(StreamReader input, StreamWriter output)
        {
            var problemInfo = ReadLineAsDoubleArr(input);
            UserCount = (int)problemInfo[0];
            DataBase = Enumerable.Range(1, UserCount).ToDictionary(v => v, _ => MessageCount);

            var messageCount = problemInfo[1];
            for (int i = 0; i < messageCount; i++)
            {
                var line = ReadLineAsStringArr(input);
                switch (line[0])
                {
                    case "1":
                        SendMessage(Convert.ToInt32(line[1]));
                        break;
                    case "2":
                        var messageId = ReceiveMessage(Convert.ToInt32(line[1]));
                        output.WriteLine(messageId);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        private int ReceiveMessage(int userId)
        {
            return Math.Max(DataBase[userId], GlobalMessageId);
        }

        public void SendMessage(int userId)
        {
            MessageCount++;
            if (userId > 0)
            {
                DataBase[userId] = MessageCount;
            }
            else
            {
                GlobalMessageId = MessageCount;
            }
        }
    }

    public class Problem4 : Problem
    {
        public class Player
        {
            public int Place { get; set; }
            public int Result { get; set; }
            public override string ToString() => $"{Place} {Result}";
        }

        public override void Solver(StreamReader input, StreamWriter output)
        {
            var tournamentCount = ReadLineAsInt(input);
            for (int i = 0; i < tournamentCount; i++)
            {
                _ = ReadLineAsInt(input);
                var players = ReadLineAsIntArr(input).Select(v => new Player
                {
                    Result = v,
                    Place = 0
                }).ToArray();

                var bestResult = -1;
                var bestPlace = 1;

                var currentPlace = 0;
                foreach (var player in players.OrderBy(v => v.Result))
                {
                    currentPlace++;
                    if (player.Result - bestResult > 1)
                    {
                        bestPlace = currentPlace;
                    }
                    
                    bestResult = player.Result;
                    player.Place = bestPlace;
                }

                output.WriteLine(string.Join(" ", players.Select(p => p.Place)));
            }
        }
    }
}