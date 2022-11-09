using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Icon.Dvs.Model
{
    public class S3Pointer
    {
        [JsonProperty(propertyName:"s3BucketName", Required = Required.Always)]
        public string S3BucketName { get; set; }
        [JsonProperty(propertyName: "s3Key", Required = Required.Always)]
        public string S3Key { get; set; }
    }
}
