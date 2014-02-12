using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LINQtoCSV;

namespace WindowsFormsApplication1 {
    public class songdata {
        [CsvColumn(FieldIndex = 0)]
        public string BatchNo { get; set; }
        [CsvColumn(FieldIndex = 1)]
        public string BatchSeqNo { get; set; }
        [CsvColumn(FieldIndex = 2)]
        public string AdamId { get; set; }
        [CsvColumn(FieldIndex = 3)]
        public string Title { get; set; }
        [CsvColumn(FieldIndex = 4)]
        public string CIp { get; set; }
        [CsvColumn(FieldIndex = 5)]
        public string AIp { get; set; }
        [CsvColumn(FieldIndex = 6)]
        public string Arts { get; set; }
        public string Category { get; set; }
        public string Language { get; set; }
        public string performer { get; set; }
        public string DP { get; set; }
        public string subTitle { get; set; }
        public string Title1 { get; set; }
        public string Arts1 { get; set; }
        public string Arts2 { get; set; }
        public string OriginalTitle { get; set; }
        public string OriginalArts { get; set; }
        public string isExist { get; set; }
    }

    public class existSong {
        [CsvColumn(FieldIndex = 0)]
        public string AdamId { get; set; }
        [CsvColumn(FieldIndex = 2)]
        public string Title { get; set; }
    }
}
