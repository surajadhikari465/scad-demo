using WebSupport.DataAccess.Models;
using WebSupport.ViewModels;

namespace WebSupport.Models
{
    public class CheckPointRequestBuilderModel
    {
        public CheckPointRequestBuilderModel() { }

        public CheckPointMessageModel CheckpointMessage { get; set; }
        public CheckPointRequestViewModel CheckPointRequestViewModel { get; set; }
    }
}