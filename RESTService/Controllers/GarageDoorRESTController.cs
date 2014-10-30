using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SmartHome.ViewModel;
using SmartHome.Repository;
using System.Diagnostics;


namespace RESTService.Controllers
{
    public class GarageDoorRESTController : ApiController
    {
        //C:\Source\Repos\RESTService\RESTService\App_Data\session.txt
        //private string filename = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\" + "session.txt";
        private string filename = AppDomain.CurrentDomain.BaseDirectory + "session.txt";

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public Security Get(string userid, string pin)
        {
            if (userid == "userid" && pin == "1234")
            {
                var secretkey = Guid.NewGuid().ToString();
                WriteToFile(pin + secretkey);
                return new Security { SessionID = "23232455654", SecretKey = secretkey };
            }

            return new Security();
        }

        public string Get(string userid, string sessionid, string hash)
        {
            var pin="1234";
            var pinAndKey = ReadFromFile();
            var currentHash = HashMD5.GetMd5Hash(pinAndKey);
            if (hash == currentHash)
            { 
                var newkey = Guid.NewGuid().ToString();
                WriteToFile(pin + newkey);
                return newkey;
            }
            return string.Empty;
        }

        private void WriteToFile(string fileContents)
        {
            var sw = new System.IO.StreamWriter(filename, false);
            sw.Write(fileContents);
            sw.Close();
        }

        private string ReadFromFile()
        {
            var fileContents = System.IO.File.ReadAllText(filename);
            return fileContents;
        }
        
        // POST api/<controller>
        public Security Post(string userid, string pin)
        {
            if (userid == "userid" && pin == "1234")
            {
                var secretkey = Guid.NewGuid().ToString();
                WriteToFile(pin + secretkey);
                return new Security { SessionID = "23232455654", SecretKey = secretkey };
            }

            return new Security();
        }

      // PUT api/<controller>/5
        public Security Put(string userid, string pin)
        {
            if (userid == "userid" && pin == "1234")
            {
                var secretkey = Guid.NewGuid().ToString();
                WriteToFile(pin + secretkey);
                return new Security { SessionID = "23232455654", SecretKey = secretkey };
            }

            return new Security();
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

    }
}