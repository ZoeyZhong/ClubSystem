using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClubSystem.Models
{
    public class ParkCom
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime? EditTime { get; set; }
        public bool isDel { get; set; }
        public string attachment { get; set; }
        public string option1 { get; set; }
        public string option2 { get; set; }
        public string option3 { get; set; }
        public string option4 { get; set; }
        public string option5 { get; set; }
    }
}