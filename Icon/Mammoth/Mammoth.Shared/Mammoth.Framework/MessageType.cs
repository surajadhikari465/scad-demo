namespace Mammoth.Framework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("esb.MessageType")]
    public partial class MessageType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MessageType()
        {
            MessageHistories = new HashSet<MessageHistory>();
            MessageQueueItemLocales = new HashSet<MessageQueueItemLocale>();
        }

        public int MessageTypeId { get; set; }

        [Required]
        [StringLength(255)]
        public string MessageTypeName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MessageHistory> MessageHistories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MessageQueueItemLocale> MessageQueueItemLocales { get; set; }
    }
}
