using System;
using System.Collections.Generic;

namespace AnalyticsWebapps.Database
{
    public partial class DrugInformation
    {
        public string FullText { get; set; }
        public string DrugName { get; set; }

        public SyndromicEvents SyndromicEvents { get; set; }
    }
}
