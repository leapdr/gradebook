using System;
using System.Collections.Generic;

namespace GradeBook
{
    public class Statistics
    {
        public double Average
        {
            get
            {
                return Sum / Count;
            }
        }
        public double High;
        public double Low;
        public string GradeLetter
        {
            get
            {
                switch(Average)
                {
                    case var d when d >= 90:
                        return "A";
                    case var d when d >= 80:
                        return "B";
                    case var d when d >= 70:
                        return "C";
                    case var d when d >= 60:
                        return "D";
                    default:
                        return "F";
                }
            }
        }
        public double Sum;
        public int Count;

        public Statistics()
        {
            Sum = 0;
            High = double.MinValue;
            Low = double.MaxValue;

        }

        internal void AddGrade(double grade)
        {
            Sum += grade;
            Count += 1;
        
            High = Math.Max(High, grade);
            Low = Math.Min(Low, grade);
        }
    }
}