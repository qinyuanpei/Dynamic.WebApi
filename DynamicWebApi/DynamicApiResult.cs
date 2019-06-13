using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicWebApi
{
    [Serializable]
    public class DynamicApiResult
    {
        [JsonProperty("flag")]
        public bool Flag { get; set; }

        [JsonProperty("mag")]
        public string Msg { get; set; }

        [JsonProperty("result")]
        public object Result { get; set; }


    }
}