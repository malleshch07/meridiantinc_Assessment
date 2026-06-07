using System;
using System.Collections.Generic;
using System.Text;

namespace Meridianitinc_Assessment.Models
{
    public class DatasetResponse
    {
        public int Count { get; set; }

        public List<string> Data { get; set; } = new();
    }
}
