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
    class Groups
    {

        public static string APIkey = ConfigurationManager.AppSettings["APIkey"];
        public static string GroupID { get; private set; }
        public static List<string> GroupIdList = new List<string>();
        public static List<int> GroupIdListNumber = new List<int>();
        /// <summary>
        /// Set GroupID
        /// </summary>
        public static void InsertGroupID()
        {
            Console.WriteLine("\nKies de group  waar je wil gaan matchen via het nummer dat achter de groep staat \n");
            PutIdsInlist();
            int intTemp = Convert.ToInt32(Console.ReadLine());
            GroupID = GroupIdList[intTemp];
            Console.WriteLine(GroupID);

        }
    
        /// <summary>
        ///  http request to get al the groups connected listed in your APIkey
        /// </summary>
        /// <returns> All groups and info connected to the APIkey</returns>
        public static async Task<string> GetGroupInfo()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", APIkey);
            string uri = "https://westcentralus.api.cognitive.microsoft.com/face/v1.0/persongroups/";
            HttpResponseMessage response = await client.GetAsync(uri);
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Put all the PersonIDs of a selected group in a list.
        /// </summary>
        public static async void PutIdsInlist()
        {

            string personList = await GetGroupInfo();
             var result = JsonConvert.DeserializeObject<List<GroupsJson>>(personList);

            for (int x = 0; x < result.Count; x++)

            {
                GroupIdList.Add(result[x].PersonGroupId);
                GroupIdListNumber.Add(x);
                Console.WriteLine("\nGroup: " + GroupIdList[x] + "  number: [" + GroupIdListNumber[x] + "]");

            }


        }
    }
}
