using System.ComponentModel.DataAnnotations;

namespace FoodApplication.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        public string? UserId { get;set; }

        [Required]
        public string? Image_Url { get; set; }

        [Required]
        public string? Title { get; set; }

        [Required]
        public string? Publisher { get; set; }

        public string? RecipeId { get; set; }

    }
}
