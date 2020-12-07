using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LoggerMiddleware.Extensibility
{
    public class LogParams
    {
        public string Ip { get; set; }
        public string CorrelationID { get; set; }
        public string DateTime_Now { get; set; }

        public LogParams(string ip, string correlationId, string dateTime_Now)
        {
            Ip = ip;
            CorrelationID = correlationId;
            DateTime_Now = dateTime_Now;
        }

        public virtual IEnumerable<string> Values() => new [] { Ip, CorrelationID, DateTime_Now };
    }

    public class RequestLogParams : LogParams
    {
        public string Request_Path { get; set; }

        public RequestLogParams(string ip, string correlationId, string dateTime_Now, string request_path) : base(ip, correlationId, dateTime_Now)
        {
            Request_Path = request_path;
        }

        public override IEnumerable<string> Values() => base.Values().Concat(new [] { Request_Path });
    }

    public class ResponseLogParams : LogParams
    {
        public string Response_StatusCode { get; set; }
        public string ElapsedMilliseconds { get; set; }

        public string Request_Path { get; set; }

        public ResponseLogParams(string ip, string correlationId, string dateTime_Now, string request_path, string response_statusCode, string elapsedMilliseconds) : base(ip, correlationId, dateTime_Now)
        {
            Request_Path = request_path;
            Response_StatusCode = response_statusCode;
            ElapsedMilliseconds = elapsedMilliseconds;
        }

        public override IEnumerable<string> Values() => base.Values().Skip(1).Concat(new [] { Request_Path, Response_StatusCode, ElapsedMilliseconds });
    }
}


