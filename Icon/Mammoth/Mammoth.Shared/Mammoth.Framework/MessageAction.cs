namespace Mammoth.Framework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("esb.MessageAction")]
    public partial class MessageAction
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MessageAction()
        {
            MessageQueueItemLocales = new HashSet<MessageQueueItemLocale>();
        }

        public int MessageActionId { get; set; }

        [Required]
        [StringLength(255)]
        public string MessageActionName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MessageQueueItemLocale> MessageQueueItemLocales { get; set; }
    }
}
