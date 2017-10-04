using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Face_API_Match
{
    class HoloFoto
    {

        public static string APIkey = ConfigurationManager.AppSettings["APIkey"];
        public static string FaceId { get; private set; }

        public static async void HoloFaceId(string imageFilePath)
        {

            Console.WriteLine("Foto naar API sturen en wachten op de biometrische waarden..\n");

            string faceid = await MakeRequest(imageFilePath);

            var result = JsonConvert.DeserializeObject<List<HoloFotoJson>>(faceid);

            FaceId = result[0].FaceId;
            Console.WriteLine("Goed gegaan, FaceId van deze persoon is :\n" + FaceId);

        }

        
        
        public static async Task<string> MakeRequest(string imageFilePath)
        {

            HttpClient client = new HttpClient();
            // Request headers.
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", APIkey);

            // Assemble the URI for the REST API Call.
            string uri = "https://westcentralus.api.cognitive.microsoft.com/face/v1.0/detect?" + "returnFaceId=true&returnFaceLandmarks=false&returnFaceAttributes=age,gender,smile,facialHair,glasses";
            ;

            // Request body. Posts a JPEG image.
            byte[] byteData = GetImageAsByteArray(imageFilePath);

            ByteArrayContent content = new ByteArrayContent(byteData);

            // Content type "application/octet-stream".
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            // Execute the REST API call.
            HttpResponseMessage response = await client.PostAsync(uri, content);
            return await response.Content.ReadAsStringAsync();

        }


        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }


    }
}
