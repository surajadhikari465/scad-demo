using WebSupport.DataAccess.Models;
using WebSupport.ViewModels;

namespace WebSupport.Models
{
    public class CheckPointRequestBuilderModel
    {
        public CheckPointMessageModel getCurrentPriceInfo { get; set; }
        public CheckPointRequestViewModel CheckPointRequestViewModel { get; set; }
    }
}