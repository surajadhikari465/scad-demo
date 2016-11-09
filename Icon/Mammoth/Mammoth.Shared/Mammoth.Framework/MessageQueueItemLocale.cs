namespace Mammoth.Framework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("esb.MessageQueueItemLocale")]
    public partial class MessageQueueItemLocale : IMessageQueue
    {
        [Key]
        public int MessageQueueId { get; set; }

        public int MessageTypeId { get; set; }

        public int MessageStatusId { get; set; }

        public int? MessageHistoryId { get; set; }

        public int MessageActionId { get; set; }

        public DateTime InsertDate { get; set; }

        public string RegionCode { get; set; }

        public int BusinessUnitId { get; set; }

        public int ItemId { get; set; }

        public string ItemTypeCode { get; set; }

        public string ItemTypeDesc { get; set; }

        public string LocaleName { get; set; }

        public string ScanCode { get; set; }

        public bool CaseDiscount { get; set; }

        public bool TmDiscount { get; set; }

        public int? AgeRestriction { get; set; }

        public bool RestrictedHours { get; set; }

        public bool Authorized { get; set; }

        public bool Discontinued { get; set; }

        public string LabelTypeDescription { get; set; }

        public bool LocalItem { get; set; }

        public string ProductCode { get; set; }

        public string RetailUnit { get; set; }

        public string SignDescription { get; set; }

        public string Locality { get; set; }

        public string SignRomanceLong { get; set; }

        public string SignRomanceShort { get; set; }

        public bool? ColorAdded { get; set; }

        public string CountryOfProcessing { get; set; }

        public string Origin { get; set; }

        public bool? ElectronicShelfTag { get; set; }
        
        public DateTime? Exclusive { get; set; }

        public int? NumberOfDigitsSentToScale { get; set; }

        public string ChicagoBaby { get; set; }

        public string TagUom { get; set; }

        public string LinkedItem { get; set; }

        public string ScaleExtraText { get; set; }

        public decimal? Msrp { get; set; }

        public int? InProcessBy { get; set; }

        public DateTime? ProcessedDate { get; set; }

        public virtual MessageAction MessageAction { get; set; }

        public virtual MessageHistory MessageHistory { get; set; }

        public virtual MessageStatus MessageStatu { get; set; }

        public virtual MessageType MessageType { get; set; }
    }
}
