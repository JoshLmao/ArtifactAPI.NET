using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtifactAPI.Models
{
    public class Image
    {
        [JsonProperty("default")]
        public string Default { get; set; }
    }
}
