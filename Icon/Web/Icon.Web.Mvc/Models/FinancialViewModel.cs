using System.ComponentModel.DataAnnotations;

namespace Icon.Web.Mvc.Models
{
    public class FinancialViewModel
    {
        [Required]
        [Display(Name="PeopleSoft Number")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter only numbers")]
        [MinLength(4, ErrorMessage="PeopleSoft number must be at least 4 digits")]
        [MaxLength(4, ErrorMessage="PeopleSoft number must be no more than 4 digits")]
        public string PeopleSoftNumber { get; set; }

        [Required]
        [Display(Name="Sub-Team Name")]
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "Please enter only letters")]
        [MaxLength(248)]
        public string SubTeamName { get; set; }

        [Display(Name = "POS Dept Number")]
        [RegularExpression(@"^[1-9][0-9]{2}$", ErrorMessage = "Please enter a value between 100-999.")]
        [MinLength(3, ErrorMessage = "POS Dept number must be at least 3 digits")]
        [MaxLength(3, ErrorMessage = "POS Dept number must be no more than 3 digits")]
        public string PosDeptNumber { get; set; }

        [Display(Name = "Team Number")]
        [RegularExpression(@"^[1-9][0-9]{2}$", ErrorMessage = "Please enter a value between 100-999.")]
        [MinLength(3, ErrorMessage = "Team number must be at least 3 digits")]
        [MaxLength(3, ErrorMessage = "Team number must be no more than 3 digits")]
        public string TeamNumber { get; set; }

        [Display(Name = "Team Name")]
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "Please enter only letters")]
        [MaxLength(100)]
        public string TeamName { get; set; }

        public int HierarchyId { get; set; }
        public string HierarchyName { get; set; }
        public int HierarchyClassId { get; set; }
        public bool NonAlignedSubteam { get; set; }

    }
}