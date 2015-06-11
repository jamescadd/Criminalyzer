using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Criminalyzer
{
    public class Jail
    {
        public string city { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string state { get; set; }
    }

    public class Record
    {
        public string name { get; set; }
        public List<string> charges { get; set; }
        public string id { get; set; }
        public string book_date_formatted { get; set; }
        public List<List<object>> details { get; set; }
        public string mugshot { get; set; }
        public string book_date { get; set; }
        public Jail jail { get; set; }
        public string more_info_url { get; set; }
    }

    public class Jailbase
    {
        public int status { get; set; }
        public int next_page { get; set; }
        public List<Record> records { get; set; }
        public int current_page { get; set; }
        public int total_records { get; set; }
        public string msg { get; set; }
    }

}
