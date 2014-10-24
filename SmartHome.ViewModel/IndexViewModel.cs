using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.ViewModel
{
    public class IndexViewModel
    {
        public string SessionID { get; set; }
        public string SecretKey{get;set;}
        public string UserID {get;set;}
        public string Pin { get; set; }
        public string Hash { get; set;}
    }

}
