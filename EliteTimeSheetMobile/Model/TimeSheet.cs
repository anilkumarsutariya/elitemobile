using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace EliteTimeSheetMobile.Model
{
    public class TimeSheet
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [MaxLength(255)]
        public string Name { get; set; }
        [MaxLength(255)]
        public string Facility { get; set; }
        [MaxLength(255)]
        public string Date { get; set; }
        [MaxLength(255)]
        public string Lunch { get; set; }
        [MaxLength(255)]
        public string SupervisiorName { get; set;}
        [MaxLength(255)]
        public string EmpSignature { get; set; }
        [MaxLength(255)]
        public string SupSignature { get; set; }
        [MaxLength(255)]
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public string Comments { get; set; }

    }
}
