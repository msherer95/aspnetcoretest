using System;
using System.Collections.Generic;

namespace AnalyticsWebapps.Database
{
    public partial class SyndromicEvents
    {
        public long? SyndromicId { get; set; }
        public string EspEvent { get; set; }
        public string ChiefComplaint { get; set; }
        public long? Age { get; set; }
        public string Race { get; set; }
        public string Gender { get; set; }
        public string Site { get; set; }
        public DateTime? VisitDate { get; set; }
        public int? ZipCode { get; set; }
        public bool? IsForeign { get; set; }
        public bool? Red { get; set; }
        public int Id { get; set; }
    }
}
