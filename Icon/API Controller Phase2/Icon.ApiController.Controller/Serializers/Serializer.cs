using Icon.ApiController.Common;
using Icon.Common.Email;
using Icon.Logging;
using System;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace Icon.ApiController.Controller.Serializers
{
    public class Serializer<T> : ISerializer<T>
    {
        private XmlWriterSettings settings;
        private XmlSerializerNamespaces namespaces;
        private ILogger<Serializer<T>> logger;
        private IEmailClient emailClient;

        public Serializer(ILogger<Serializer<T>> logger, IEmailClient emailClient)
        {
            this.logger = logger;
            this.emailClient = emailClient;
            
            settings = new XmlWriterSettings();

            // These settings prevent things like tab and newline characters from appearing in the serialized string.
            settings.NewLineHandling = NewLineHandling.None;
            settings.Indent = false;

            // UTF-8 is the desired format for ESB.
            settings.Encoding = System.Text.Encoding.UTF8;

            namespaces = NamespaceHelper.SetupNamespaces(typeof(T));
        }

        public string Serialize(T miniBulk, TextWriter writer)
        {
            XmlWriter xmlWriter = XmlWriter.Create(writer, settings);
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            try
            {
                serializer.Serialize(xmlWriter, miniBulk, namespaces);
            }
            catch (Exception ex)
            {
                logger.Error("Message serialization was unsuccessful.  The queued messages in this mini-bulk will be marked as Failed.");

                ExceptionLogger<Serializer<T>> exceptionLogger = new ExceptionLogger<Serializer<T>>(logger);
                exceptionLogger.LogException(miniBulk.ToString(), ex, this.GetType(), MethodBase.GetCurrentMethod());

                string errorMessage = string.Format(Resource.FailedToSerializeMiniBulkMessage, ControllerType.Type, ControllerType.Instance);
                string emailSubject = Resource.FailedToSerializeMiniBulkEmailSubject;
                string emailBody = EmailHelper.BuildMessageBodyForSerializationFailure(errorMessage, ex.ToString());

                try
                {
                    emailClient.Send(emailBody, emailSubject);
                }
                catch (Exception mailEx)
                {
                    string mailErrorMessage = "A failure occurred while attempting to send the alert email.";
                    exceptionLogger.LogException(mailErrorMessage, mailEx, this.GetType(), MethodBase.GetCurrentMethod());
                }

                return string.Empty;
            }

            string xml = writer.ToString();

            logger.Info(string.Format("Serialization complete.  Message size (bytes): {0}.", System.Text.ASCIIEncoding.Unicode.GetByteCount(xml)));

            return xml;
        }
    }
}
