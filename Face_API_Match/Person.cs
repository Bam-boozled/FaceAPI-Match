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
    class Person
    {
        public static string APIkey = ConfigurationManager.AppSettings["APIkey"];
        public static List<string> PersonIdList = new List<string>();

        /// <summary>
        /// List all persons in a person group, and retrieve person information 
        /// (including personId, name, userData and persistedFaceIds of registered faces of the person).
        /// </summary>
        /// <param name="personGroupId">personGroupId of the target person group.</param>
        /// <returns> A successful call returns an array of person information that belong to the person group.  </returns>
        public static async Task<string> GetPersonInfo(string personGroupId)
        {

            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", APIkey);

            string uri = "https://westcentralus.api.cognitive.microsoft.com/face/v1.0/persongroups/" + personGroupId + "/persons?";

            HttpResponseMessage response = await client.GetAsync(uri);

            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Lists all personIds from group <see cref="Groups"/> 
        /// </summary>
        public static async void PutPersonsInlist()
        {

            string personGroupId = Groups.GroupID;
            string personList = await GetPersonInfo(personGroupId);

            var result = JsonConvert.DeserializeObject<List<PersonJson>>(personList);
            Console.WriteLine("Gegevens ophalen van alle mensen in de groep \n");

            for (int x = 0; x < result.Count; x++)
            {
                if (result[x].PersistedFaceIds.Count != 0)
                {
                    Console.WriteLine("\nnaam van deze persoon: " + result[x].Name);
                    Console.WriteLine("PersonId van deze persoon: " + result[x].PersonId);
                    Console.WriteLine("Beschrijving van deze persoon: " + result[x].UserData);
                    Console.WriteLine("FaceId van deze persoon: " + result[x].PersistedFaceIds.Count);

                    PersonIdList.Add(result[x].PersonId);

                }
            }
        }

        /// <summary>
        /// Retrieve a person's information, including registered persisted faces, name and userData.
        /// </summary>
        /// <param name="personGroupId">Specifying the person group containing the target person.</param>
        /// <param name="personId">Specifying the target person.</param>
        /// <returns> name from target person</returns>
        public static async Task<string> ShowPersonInfo(string personGroupId, string personId)

        {
            string persongroupid = personGroupId;
            string personid = personId;
            string personinfo = await PersonInfo(persongroupid, personid);
            var result = JsonConvert.DeserializeObject<PersonJson>(personinfo);

            return result.Name;

        }

        /// <summary>
        /// Retrieve a person's information, including registered persisted faces, name and userData.
        /// </summary>
        /// <param name="personGroupId">Specifying the person group containing the target person.</param>
        /// <param name="personId">Specifying the target person.</param>
        /// <returns>A successful call returns the person's information. </returns>
        public static async Task<string> PersonInfo(string personGroupId, string personId)
        {

            var client = new HttpClient();
            // Request headers - replace this example key with your valid key.
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", APIkey);

            string uri = "https://westcentralus.api.cognitive.microsoft.com/face/v1.0/persongroups/" + personGroupId + "/persons/" + personId;

            HttpResponseMessage response = await client.GetAsync(uri);

            return await response.Content.ReadAsStringAsync();

        }
    }
}
