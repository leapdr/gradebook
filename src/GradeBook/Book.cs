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
            using(var writer = File.AppendText($"{Name}.txt")){
                writer.WriteLine(grade);

                if(GradeAdded != null)
                {
                    GradeAdded(this, new EventArgs());
                }
            } // guarantees that Disclose method is called

            return true;
        }

        public override Statistics GetStatistics()
        {
            var stats = new Statistics();
            using(var reader = File.OpenText($"{Name}.txt"))
            {
                string line;
                while( (line = reader.ReadLine()) != null )
                {
                    stats.AddGrade(double.Parse(line));
                }
                // var line = reader.ReadLine();
                // while(line != null)
                // {
                //     stats.AddGrade(double.Parse(line));
                //     line = reader.ReadLine();
                // }
            }

            return stats;
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
            var stats = new Statistics();
            
            foreach(var grade in Grades){
                stats.AddGrade(grade);
            }

            return stats;
        }
    }
}
