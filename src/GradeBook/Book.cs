using System;
using System.Collections.Generic;
using System.IO;

namespace GradeBook
{
    public delegate void GradeAddedDelegate(object sender, EventArgs args);

    public class NamedObject
    {
        public NamedObject(string name)
        {
            Name = name;
        }

        public string Name
        {
            get;
            set;
        }
    }

    public interface IBook
    {
        bool AddGrade(double grade);
        Statistics GetStatistics();
        string Name { get; }
        event GradeAddedDelegate GradeAdded;
    }

    public abstract class Book : NamedObject, IBook
    {
        protected Book(string name) : base(name)
        {
        }

        public abstract event GradeAddedDelegate GradeAdded;

        public abstract bool AddGrade(double grade);

        public abstract Statistics GetStatistics();
    }

    public class DiskBook : Book
    {
        public DiskBook(string name) : base(name)
        {
            
        }

        public override event GradeAddedDelegate GradeAdded;

        public override bool AddGrade(double grade)
        {
            var bookFile = File.AppendText($"{Name}.txt");
            bookFile.WriteLine(grade);

            return true;
        }

        public override Statistics GetStatistics()
        {
            throw new NotImplementedException();
        }
    }

    public class InMemoryBook : Book
    {
        public List<double> Grades;

        public InMemoryBook(string name) : base("")
        {
            Grades = new List<double>();
            Name = name;
        }

        public override bool AddGrade(double grade)
        {
            var idAdded = false;
            if(grade >= 0 && grade <= 100)
            {
                Grades.Add(grade);
                if(GradeAdded != null)
                {
                    // broadcast
                    GradeAdded(this, new EventArgs());
                }

                idAdded = true;
            }

            return idAdded;
        }

        public override event GradeAddedDelegate GradeAdded;

        public override Statistics GetStatistics()
        {
            var result = new Statistics();
            result.Average = 0.0;
            result.High = double.MinValue;
            result.Low = double.MaxValue;

            foreach(var grade in Grades){
                result.High = Math.Max(result.High, grade);
                result.Low = Math.Min(result.Low, grade);
                
                result.Average += grade;
            }

            result.Average = result.Average / Grades.Count;

            switch(result.Average)
            {
                case var d when d >= 90:
                    result.GradeLetter = "A";
                    break;
                case var d when d >= 80:
                    result.GradeLetter = "B";
                    break;
                case var d when d >= 70:
                    result.GradeLetter = "C";
                    break;
                case var d when d >= 60:
                    result.GradeLetter = "D";
                    break;
                default:
                    result.GradeLetter = "F";
                    break;
            }

            return result;
        }
    }
}
