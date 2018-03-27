using Mammoth.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebSupport.DataAccess.TransferObjects;

namespace WebSupport.ViewModels
{
    public class RegionGpmStatusViewModel
    {
        public RegionGpmStatusViewModel() { }

        public RegionGpmStatusViewModel(RegionGpmStatus entity) : this()
        {
            Region = entity.Region;
            IsGpmEnabled = entity.IsGpmEnabled;
        }

        [Required]
        [StringLength(2)]
        public string Region { get; set; }

        [DisplayName("GPM Status")]
        public bool IsGpmEnabled { get; set; }

        public static RegionGpmStatus ToEntityModel(RegionGpmStatusViewModel viewModel)
        {
            return new RegionGpmStatus
            {
                Region = viewModel.Region,
                IsGpmEnabled = viewModel.IsGpmEnabled
            };
        }
    }
}