using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Face_API_Match
{
    class Match
    {
        public static string APIkey = ConfigurationManager.AppSettings["APIkey"];

        public static List<string> personIds = new List<string>();
        public static double BestMatch { get; private set; }
        public static string BestMatchId { get; private set; }
        public static bool IsIdentical { get; private set; }
        public static string BestMatchName { get; private set; }

        public static async void ShowBestMatch()
        {
            List<double> confidence = new List<double>();
            List<bool> isidentical = new List<bool>();

            String faceId = HoloFoto.FaceId;
            String personGroupId = Groups.GroupID;
            List<string> personId = new List<string>();
            personId = Person.PersonIdList;



            for (int x = 0; x < personId.Count; x++)

            {
                string match = await VerifyFaces(faceId, personGroupId, personId[x]);

                var result = JsonConvert.DeserializeObject<MatchJson>(match);

                Console.WriteLine(result.Confidence);
                Console.WriteLine(result.IsIdentical);

                confidence.Add(result.Confidence);
                isidentical.Add(result.IsIdentical);


                if (x == 0)
                {

                    BestMatch = confidence[0];
                    BestMatchId = personId[0];
                    IsIdentical = isidentical[0];
                    BestMatchName = await Person.ShowPersonInfo(personGroupId, personId[x]);


                }
                else if (x > 0 && confidence[x] >= confidence[x - 1])
                {
                    BestMatch = result.Confidence;
                    IsIdentical = result.IsIdentical;
                    BestMatchId = personId[x];
                    BestMatchName = await Person.ShowPersonInfo(personGroupId, personId[x]);


                }

                else
                {
                    BestMatch = BestMatch;
                    BestMatchId = BestMatchId;
                    IsIdentical = IsIdentical;
                    BestMatchName = await Person.ShowPersonInfo(personGroupId, personId[x]);

                }


            }

            Console.WriteLine("Is het een match?" + IsIdentical);
            Console.WriteLine("Hoe groot is de kans dat deze personen overeenkomen : " + BestMatch);
            Console.WriteLine("Wat is de personid van deze match: " + BestMatchId);
            Console.WriteLine("Naam van deze persoon? : " + BestMatchName);








        }
        public static async Task<string> VerifyFaces(string faceId, string personGroupId, string personId)
        {

            const string subscriptionKey = "e57a566e8fed4377b4ef178c8feba526";

            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            string uri = "https://westcentralus.api.cognitive.microsoft.com/face/v1.0/verify";

            string json = "{\"faceId\":\"" + faceId + "\", \"personId\":\"" + personId + "\", \"personGroupId\":\"" + personGroupId + "\"}";
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = await client.PostAsync(uri, content);


            Console.WriteLine("Response status: " + response.StatusCode);

            return await response.Content.ReadAsStringAsync();
        }
    }
}
