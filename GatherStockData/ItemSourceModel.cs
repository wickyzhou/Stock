using System;
using System.Collections.Generic;
using System.Text;

namespace GatherStockData
{
    public class ItemSourceModel
    {
        public int Id { get; set; }

        public string SourceName { get; set; }

        public string SourceUrl { get; set; }

        public string GatherUrl { get; set; }

        public string HttpRequestMethod { get; set; }

        public int GroupId { get; set; }

        public string TargetTableName { get; set; }

        public string ContentBegin { get; set; }

        public string ContentEnd { get; set; }

        public string FieldPattern { get; set; }

        public string FieldRef{ get; set; }

        public string ExtraFieldPattern { get; set; }

        public string ExtraFieldRef { get; set; }

    }
}
