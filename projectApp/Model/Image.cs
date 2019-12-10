using System;
using System.Collections.Generic;
using System.Text;

namespace projectApp.Model
{
    public class Image
    {
        public string Name { get; set; }
        public string TimeStamp { get; set; }
        public string Directory { get; set; }
        public string Coordinates { get; set; }
        public int Rating { get; set; }
        public List<string> Category { get; set; }
        public Double Distance { get; set; }

        public Image(string path = "")
        {
            Name = "";
            TimeStamp = "";
            Directory = path;
            Coordinates = "";//new List<double>();
            Rating = 0;
            Category = new List<string>();
            Distance = 0;
        }
    }
}
