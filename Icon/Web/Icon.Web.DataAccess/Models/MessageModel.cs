using System;

namespace Icon.Web.DataAccess.Models
{
    public class MessageModel
    {
        public int Id { get; set; }
        public int MessageStatusId { get; set; }
        public int MessageTypeId { get; set; }
        public DateTime InsertDate { get; set; }
    }
}
