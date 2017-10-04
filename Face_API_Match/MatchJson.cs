using System;
using System.Collections.Generic;
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
    class MatchJson
    {

        [JsonProperty("isIdentical")]
        public bool IsIdentical { get; set; }

        [JsonProperty("confidence")]
        public double Confidence { get; set; }
    }
}
