using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.Framework
{
    [Table("gpm.PriceResetMessageHistory")]
    public class PriceResetMessageHistory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PriceResetMessageHistory() { }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PriceResetMessageHistoryId { get; set; }

        [Required]
        public Guid MessageId { get; set; }

        public int MessageTypeId { get; set; }

        public int MessageStatusId { get; set; }

        [Column(TypeName = "xml")]
        [Required]
        public string Message { get; set; }

        [Column(TypeName = "datetime2")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime InsertDate { get; set; }

        public string MessageProperties { get; set; }

        public virtual MessageStatus MessageStatus { get; set; }

        public virtual MessageType MessageType { get; set; }
    }
}
