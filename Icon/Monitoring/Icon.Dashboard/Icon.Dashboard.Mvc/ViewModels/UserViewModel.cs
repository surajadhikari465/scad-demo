using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Icon.Dashboard.Mvc.ViewModels
{
    public class UserViewModel
    {
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
        public IList<MovieViewModel> FavouriteMovies { get; set; }
    }
}