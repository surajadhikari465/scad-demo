using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Icon.Dvs.Model;

namespace Icon.Dvs.Serializer
{
    /// <summary>
    /// Converts SQS message to S3Pointer
    /// The SQS message should be raw message (Raw message delivery needs to be enabled in SNS subscription)
    /// </summary>
    public class DvsSqsMessageConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(DvsSqsMessage));
        }

        public override object ReadJson(
            JsonReader reader, 
            Type objectType, 
            object existingValue, 
            JsonSerializer serializer)
        {
            DvsSqsMessage message = new DvsSqsMessage();
            JObject messageJson = JObject.Load(reader);
            message.MessageId = messageJson.GetValue("MessageId").ToString();
            Dictionary<string, JObject> messageAttributesJson = messageJson.GetValue("MessageAttributes").ToObject<Dictionary<string, JObject>>();

            message.MessageAttributes = new Dictionary<string, string>();

            foreach(string key in messageAttributesJson.Keys)
            {
                JObject valueJson;
                JToken valueToken;
                messageAttributesJson.TryGetValue(key, out valueJson);
                // valueJson has keys (Type, Value). Since In DVS every attribute is string, converting to string by default
                valueJson.TryGetValue("Value", out valueToken);
                message.MessageAttributes.Add(key, valueToken.ToString());
            }

            JArray s3PointerArray = JsonConvert.DeserializeObject<JArray>(messageJson.GetValue("Message").ToString());

            // s3PointerArray[0] contains not so useful string "software.amazon.payloadoffloading.PayloadS3Pointer"
            S3Pointer s3Pointer = s3PointerArray[1].ToObject<S3Pointer>();
            message.S3BucketName = s3Pointer.S3BucketName;
            message.S3Key = s3Pointer.S3Key;
            return message;
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
