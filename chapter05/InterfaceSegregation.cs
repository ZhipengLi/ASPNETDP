
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPPatterns.Chap5.LayerSupertypePattern.Model
{
    interface IProduct
    {
        decimal price { get; set; }
        int stock { get; set; }
        double WeightInKg { get; set; }
    }

    interface IMovie
    {
        string certification { get; set; }
        double RunningTime { get; set; }
    }

    class TShirt : IProduct
    {
        public decimal price { get; set; }
        public int stock { get; set; }
        public double WeightInKg { get; set; }
    }

    class BluRayDisc : IProduct, IMovie
    {
        public decimal price { get; set; }
        public int stock { get; set; }
        public double WeightInKg { get; set; }
        public string certification { get; set; }
        public double RunningTime { get; set; }
    }

    class DVD : IProduct, IMovie
    {
        public decimal price { get; set; }
        public int stock { get; set; }
        public double WeightInKg { get; set; }
        public string certification { get; set; }
        public double RunningTime { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            


            Console.ReadLine();
        }
    }
}

