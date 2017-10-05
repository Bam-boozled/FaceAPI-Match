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


     
        /// <summary>
        /// Post .jpg to the API and deserialize json string for faceId
        /// </summary>
        /// <param name="imageFilePath">location of .jpg file</param>
        public static async Task HoloFaceId(string imageFilePath)
        {

            Console.WriteLine("Foto naar API sturen en wachten op de biometrische waarden..\n");

            string faceid = await MakeRequest(imageFilePath);

            var result = JsonConvert.DeserializeObject<List<HoloFotoJson>>(faceid);

            FaceId = result[0].FaceId;
            Console.WriteLine("Goed gegaan, FaceId van deze persoon is :\n" + FaceId);

        }




        /// <summary>
        ///  Detect human face in an image.
        /// </summary>
        /// <param name="imageFilePath">location of .jpg file</param>
        /// <returns>A successful call returns an array of face entries ranked by face rectangle size in descending order.
        ///  An empty response indicates no faces detected. </returns>
        public static async Task<string> MakeRequest(string imageFilePath)
        {

            // Request headers.
            Groups.client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", APIkey);

            // Assemble the URI for the REST API Call.
            string uri = "https://westcentralus.api.cognitive.microsoft.com/face/v1.0/detect?" + "returnFaceId=true&returnFaceLandmarks=false&returnFaceAttributes=age,gender,smile,facialHair,glasses";
            

            // Request body. Posts a JPEG image.
            byte[] byteData = GetImageAsByteArray(imageFilePath);

            ByteArrayContent content = new ByteArrayContent(byteData);

            // Content type "application/octet-stream".
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            // Execute the REST API call.
            HttpResponseMessage response = await Groups.client.PostAsync(uri, content);
            return await response.Content.ReadAsStringAsync();

        }

        /// <summary>
        /// Returns the contents of the specified file as a byte array.
        /// </summary>
        /// <param name="imageFilePath">location of .jpg file</param>
        /// <returns>The byte array of the image data.</returns>
        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }


    }
}
