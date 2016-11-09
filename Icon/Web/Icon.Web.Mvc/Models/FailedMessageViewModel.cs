using System;

namespace Icon.Web.Mvc.Models
{
    public class FailedMessageViewModel
    {
        public int Id { get; set; }
        public string MessageType { get; set; }
        public int MessageTypeId { get; set; }
        public string MessageStatus { get; set; }
        public int MessageStatusId { get; set; }
        public DateTime InsertDate { get; set; }
    }
}