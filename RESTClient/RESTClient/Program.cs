using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using SmartHome.Repository;
using SmartHome.ViewModel;
using Newtonsoft.Json;


//http://codebetter.com/howarddierking/2012/11/09/versioning-restful-services/

namespace RESTClient
{
    class Program
    {
        static void Main(string[] args)
        {
            RunAsync().Wait();
        }

        static async Task RunAsync()
        {
            using (var client = new HttpClient())
            {
                
                  /*    client.BaseAddress = new Uri("http://localhost:9000/");
                      client.DefaultRequestHeaders.Accept.Clear();
                      client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                      // HTTP GET
                      HttpResponseMessage response = await client.GetAsync("api/products/1");
                      if (response.IsSuccessStatusCode)
                      {
                          IndexViewModel product = await response.Content.ReadAsStringAsync<string>();
                          --Console.WriteLine("{0}\t${1}\t{2}", product.Name, product.Price, product.Category);
                      }

                      // HTTP POST
                      var gizmo = new Product() { Name = "Gizmo", Price = 100, Category = "Widget" };
                      response = await client.PostAsync("api/products", gizmo);
                      if (response.IsSuccessStatusCode)
                      {
                          Uri gizmoUrl = response.Headers.Location;

                          // HTTP PUT
                          gizmo.Price = 80;   // Update price
                          response = await client.PutAsJsonAsync(gizmoUrl, gizmo);

                          // HTTP DELETE
                          response = await client.DeleteAsync(gizmoUrl);
                     }
                  
                */

                client.BaseAddress = new Uri("http://localhost:3474");
                client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //HttpContent requestContent = new StringContent(string.Format("userid={0}&pin={1}", "userid", "1234"), Encoding.UTF8, "application/x-www-form-urlencoded");

//                var content = new FormUrlEncodedContent(new[]
//                {
//                    new KeyValuePair<string, string>("userid","userid"),
//                    new KeyValuePair<string, string>("pin", "1234")
//                });

                HttpResponseMessage responseMessage = await client.GetAsync(string.Format("api/GarageDoorREST?userid={0}&pin={1}", "userid", "1234"));

                // var requestSession = new RequestSession() { UserID = "userid", Pin="pin"};
                //HttpResponseMessage responseMessage = await client.PutAsync("/api/GarageDoorREST", content);

                var response = new RequestSession();
                if (responseMessage.IsSuccessStatusCode)
                {
                    string jsonMessage;
                    using (Stream responseStream = await responseMessage.Content.ReadAsStreamAsync())
                    {
                        jsonMessage = new StreamReader(responseStream).ReadToEnd();
                    }

                    response = (RequestSession)JsonConvert.DeserializeObject(jsonMessage, typeof(RequestSession));

                }

                // HTTP POST
                //var gizmo = new RequestSession() {};

                var pinAndKey = "1234" + response.SecretKey;
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("userid","userid"),
                    new KeyValuePair<string, string>("sessionid","23232455654"),
                    new KeyValuePair<string, string>("hash", HashMD5.GetMd5Hash(pinAndKey))
                });
                responseMessage = await client.PostAsync("api/GarageDoorREST", content);  //method not allow
                if (responseMessage.IsSuccessStatusCode)
                {
                }
            }
        }
    }
}

