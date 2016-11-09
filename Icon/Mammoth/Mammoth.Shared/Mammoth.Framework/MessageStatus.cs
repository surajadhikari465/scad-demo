namespace Mammoth.Framework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("esb.MessageStatus")]
    public partial class MessageStatus
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MessageStatus()
        {
            MessageHistories = new HashSet<MessageHistory>();
            MessageQueueItemLocales = new HashSet<MessageQueueItemLocale>();
        }

        [Key]
        public int MessageStatusId { get; set; }

        [Required]
        [StringLength(255)]
        public string MessageStatusName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MessageHistory> MessageHistories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MessageQueueItemLocale> MessageQueueItemLocales { get; set; }
    }
}
