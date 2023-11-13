using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TheFoodApp.Areas.Identity.Data;

namespace TheFoodApp.Models;

public class Rating
{
    [Key]
    public int RatingId { get; set; }
    public int Value { get; set; } // Assuming 1-5 or similar scale
    [ForeignKey("User")]
    public string UserId { get; set; }
    public User User { get; set; }
    [ForeignKey("Recipe")]
    public int RecipeId { get; set; }
    public Recipe Recipe { get; set; }
}