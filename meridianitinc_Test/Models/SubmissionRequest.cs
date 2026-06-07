using System;
using System.Collections.Generic;
using System.Text;

namespace Meridianitinc_Assessment.Models
{
    public class SubmissionRequest
    {
        public string Type { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }
}
