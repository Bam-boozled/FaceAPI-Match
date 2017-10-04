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
    public class HoloFotoJson
    {
        [JsonProperty("faceId")]
        public string FaceId { get; set; }

        [JsonProperty("faceAttributes")]
        public FaceAttributes FaceAttributes { get; set; }

        [JsonProperty("faceRectangle")]
        public FaceRectangle FaceRectangle { get; set; }
    }

    public class FaceAttributes
    {
        [JsonProperty("facialHair")]
        public FacialHair FacialHair { get; set; }

        [JsonProperty("glasses")]
        public string Glasses { get; set; }

        [JsonProperty("age")]
        public double Age { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("smile")]
        public double Smile { get; set; }
    }

    public class FacialHair
    {
        [JsonProperty("moustache")]
        public double Moustache { get; set; }

        [JsonProperty("beard")]
        public long Beard { get; set; }

        [JsonProperty("sideburns")]
        public double Sideburns { get; set; }
    }

    public class FaceRectangle
    {
        [JsonProperty("left")]
        public long Left { get; set; }

        [JsonProperty("height")]
        public long Height { get; set; }

        [JsonProperty("top")]
        public long Top { get; set; }

        [JsonProperty("width")]
        public long Width { get; set; }
    }
}
