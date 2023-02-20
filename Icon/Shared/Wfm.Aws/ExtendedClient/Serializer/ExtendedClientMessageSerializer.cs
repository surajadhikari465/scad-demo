using System.Collections.Generic;
using Wfm.Aws.ExtendedClient.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Diagnostics.Tracing;
using System;

namespace Wfm.Aws.ExtendedClient.Serializer
{
    public class ExtendedClientMessageSerializer : IExtendedClientMessageSerializer
    {
        public ExtendedClientMessageModel Deserialize(string message)
        {
            JArray s3PointerArray = null;
            string eventSource = Constants.EventSources.SQS;
            IDictionary<string, string> messageAttributes = new Dictionary<string, string>();
            var jsonObj = JsonConvert.DeserializeObject(message);
            if (((JToken)jsonObj).Type.Equals(JTokenType.Array))
            {
                s3PointerArray = JsonConvert.DeserializeObject<JArray>(message);
            }
            else if (((JToken)jsonObj).Type.Equals(JTokenType.Object))
            {
                JsonReader jsonReader = new JsonTextReader(new StringReader(message));
                JObject messageJson = JObject.Load(jsonReader);
                eventSource = Constants.EventSources.SNS;
                s3PointerArray = JsonConvert.DeserializeObject<JArray>(messageJson.GetValue("Message").ToString());
                IDictionary<string, JObject> messageAttributesJson = messageJson.GetValue("MessageAttributes").ToObject<IDictionary<string, JObject>>();
                foreach (string key in messageAttributesJson.Keys)
                {
                    messageAttributesJson.TryGetValue(key, out JObject valueJson);
                    // valueJson has keys (Type, Value). Converting to string by default
                    valueJson.TryGetValue("Value", out JToken valueToken);
                    messageAttributes[key] = valueToken.ToString();
                }
            }
            ExtendedClientMessageSerializerS3Pointer s3PointerMap = s3PointerArray[1].ToObject<ExtendedClientMessageSerializerS3Pointer>();
            return new ExtendedClientMessageModel()
            {
                EventSource = eventSource,
                MessageAttributes = messageAttributes,
                S3Details = new List<ExtendedClientMessageModelS3Detail>()
                    {
                        new ExtendedClientMessageModelS3Detail()
                        {
                            S3BucketName = s3PointerMap.S3BucketName,
                            S3Key = s3PointerMap.S3Key
                        }
                    }
            };
        }

        public string Serialize(string s3BucketName, string s3Key, IDictionary<string, string> MessageAttributes)
        {
            JArray s3PointerArray = new JArray();
            JValue s3PointerText = new JValue("software.amazon.payloadoffloading.PayloadS3Pointer");
            JObject s3PointerObject = JObject.FromObject(
                new ExtendedClientMessageSerializerS3Pointer()
                {
                    S3BucketName = s3BucketName,
                    S3Key = s3Key
                });
            s3PointerArray.Add(s3PointerText);
            s3PointerArray.Add(s3PointerObject);
            return s3PointerArray.ToString(Formatting.None);
        }
    }

    public class ExtendedClientMessageSerializerS3Pointer
    {
        [JsonProperty(propertyName: "s3BucketName", Required = Required.Always)]
        public string S3BucketName { get; set; }
        [JsonProperty(propertyName: "s3Key", Required = Required.Always)]
        public string S3Key { get; set; }
    }
}
