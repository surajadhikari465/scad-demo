using System.ComponentModel.DataAnnotations;

namespace KitBuilderWebApi.QueryParameters
{
    public class AddInstructionListPrameters
    {
        /// <summary>
        /// Instruction List Type Id
        /// </summary>
        [Required]
        public int TypeId { get; set; }
        /// <summary>
        /// Instruction List Name
        /// </summary>
        [Required, MinLength(3), MaxLength(50)]
        public string Name { get; set; }

    }
}