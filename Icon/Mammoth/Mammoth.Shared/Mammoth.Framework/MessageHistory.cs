namespace Mammoth.Framework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("esb.MessageHistory")]
    public partial class MessageHistory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MessageHistory()
        {
            MessageQueueItemLocales = new HashSet<MessageQueueItemLocale>();
            MessageQueuePrices = new HashSet<MessageQueuePrice>();
        }

        public int MessageHistoryId { get; set; }

        public int MessageTypeId { get; set; }

        public int MessageStatusId { get; set; }

        [Column(TypeName = "xml")]
        [Required]
        public string Message { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime InsertDate { get; set; }

        public int? InProcessBy { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ProcessedDate { get; set; }

        public virtual MessageStatus MessageStatus { get; set; }

        public virtual MessageType MessageType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MessageQueueItemLocale> MessageQueueItemLocales { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MessageQueuePrice> MessageQueuePrices { get; set; }
    }
}
