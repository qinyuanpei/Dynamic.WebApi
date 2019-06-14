using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace DynamicWebApi
{
    [Serializable]
    [DataContract]
    public class DynamicApiResult
    {
        [JsonProperty("flag")]
        [DataMember]
        public bool Flag { get; set; }

        [JsonProperty("mag")]
        [DataMember]
        public string Msg { get; set; }

        [JsonProperty("result")]
        [DataMember]
        public object Result { get; set; }


    }
}