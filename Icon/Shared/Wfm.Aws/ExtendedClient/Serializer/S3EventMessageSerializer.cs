﻿using Amazon.S3.Util;
using System;
using System.Collections.Generic;
using System.Web;
using Wfm.Aws.ExtendedClient.Model;

namespace Wfm.Aws.ExtendedClient.Serializer
{
    public class S3EventMessageSerializer : IExtendedClientMessageSerializer
    {
        public ExtendedClientMessageModel Deserialize(string message)
        {
            S3EventNotification s3EventNotification = S3EventNotification.ParseJson(message);
            IList<ExtendedClientMessageModelS3Detail> s3Details = new List<ExtendedClientMessageModelS3Detail>();
            s3EventNotification.Records.ForEach(record =>
            {
                s3Details.Add(
                    new ExtendedClientMessageModelS3Detail()
                    {
                        S3BucketName = record.S3.Bucket.Name,
                        S3Key = HttpUtility.UrlDecode(record.S3.Object.Key)
                    });
            });
            return new ExtendedClientMessageModel()
            {
                S3Details = s3Details
            };
        }

        public string Serialize(string s3BucketName, string s3Key, IDictionary<string, string> MessageAttributes)
        {
            throw new NotImplementedException();
        }
    }
}
