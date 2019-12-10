using System;
using System.Collections.Generic;
using System.Text;

namespace projectApp.Model
{
    class Image
    {
        public string Name { get; set; }
        public string TimeStamp { get; set; }
        public string Directory { get; set; }
        public List<double> Coordinates { get; set; }
        public int Rating { get; set; }
        public List<string> Category { get; set; }

        public Image(string path = "")
        {
            Name = "";
            TimeStamp = "";
            Directory = path;
            Coordinates = new List<double>();
            Rating = 0;
            Category = new List<string>();
        }
    }
}
