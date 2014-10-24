using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SmartHome.ViewModel;
using SmartHome.Repository;

namespace RESTService.Controllers
{
    public class GarageDoorRESTController : ApiController
    {
        private string filename = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\" + "Runtime\\" + "session.txt";

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public Security Get(string userid, string pin)
        {
            if (userid == "userid" && pin == "1234")
                return new Security { SessionID = "23232455654", SecretKey = Guid.NewGuid().ToString() };

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
                var newpinAndKey =pin + newkey;
                WriteToFile(newpinAndKey);
                return newkey;
            }
            return string.Empty;
        }

        private void WriteToFile(string fileContents)
        {
            var sw = new System.IO.StreamWriter(filename, true);
            sw.WriteLine(fileContents);
            sw.Close();
        }

        private string ReadFromFile()
        {
            var fileContents = System.IO.File.ReadAllText(filename);
            return fileContents;
        }
        
        // POST api/<controller>
        public void Post(Guid id, bool open)
        {
           // var viewModel = new GarageViewModel(){ id = id, Open = open}

        }

      // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

    }
}