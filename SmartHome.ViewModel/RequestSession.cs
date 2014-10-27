using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SmartHome.ViewModel
{
    public class RequestSession
    {
        [JsonProperty("SessionID")]
        public string SessionID { get; set; }

        public string SecretKey{get;set;}

        [JsonProperty("UserID")]
        public string UserID { get; set; }

        public string Pin { get; set; }

        [JsonProperty("Hash")]
        public string Hash { get; set; }
    }

}
