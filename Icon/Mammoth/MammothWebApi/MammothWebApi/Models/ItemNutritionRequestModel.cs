using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace MammothWebApi.Models
{
    public class ItemNutritionRequestModel
    {
        [Required]
        public IEnumerable<int> ItemIds { get; set; }
    }
}