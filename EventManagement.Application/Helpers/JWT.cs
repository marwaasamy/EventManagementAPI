using System;
using System.Collections.Generic;
using System.Text;

namespace EventManagement.Infrastructure.Helpers
{
    public class JWT
    {
        public string SecretKey { get; set; }
        public string AudienceIP { get; set; }
        public string IssuerIP { get; set; }
        public double DurationDays { get; set; }
    }
}
