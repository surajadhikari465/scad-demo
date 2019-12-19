using BulkItemUploadProcessor.Common.Models;
using System.Collections.Generic;

namespace BulkItemUploadProcessor.DataAccess.Commands
{
    public class UpdateItemsCommand
    {
        public List<UpdateItemModel> Items { get; set; } = new List<UpdateItemModel>();
    }
}
