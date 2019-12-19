using BulkItemUploadProcessor.Common;
using BulkItemUploadProcessor.Common.Models;
using System;
using System.Collections.Generic;

namespace BulkItemUploadProcessor.DataAccess.Commands
{
    public class AddItemsCommand
    {
        public List<AddItemModel> Items { get; set; }
        public List<ItemIdAndScanCode> AddedItems { get; set; }
        public List<ErrorItem<AddItemModel>> InvalidItems { get; set; }
    }
}