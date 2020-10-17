using System;

namespace GradeBook
{
    class Program
    {
        static void Main(string[] args)
        {
            IBook book = new DiskBook("Aaron's Grade Book");
            book.GradeAdded += OneGradeAdded;
            EnterGrades(book);

            var stats = book.GetStatistics();

            Console.WriteLine($"The lowest grade is {stats.Low}.");
            Console.WriteLine($"The highest grade is {stats.High}.");
            Console.WriteLine($"The average grade is {stats.Average}.");
            Console.WriteLine($"The average letter is {stats.GradeLetter}.");
        }

        private static void EnterGrades(IBook book)
        {
            var input = "";
            double grade;

            do
            {
                try
                {
                    Console.WriteLine("Enter your grade or 'q' to quit: ");
                    input = Console.ReadLine();

                    if (input == "q" || input == "q") break;

                    grade = double.Parse(input);

                    var isAdded = book.AddGrade(grade);
                    if (!isAdded)
                    {
                        Console.WriteLine("Number out of range! (0 - 100)");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid number!");
                }
            } while (true);
        }

        static void OneGradeAdded(object sender, EventArgs e)
        {
            Console.WriteLine("A grade was added");
        }
    }
}
