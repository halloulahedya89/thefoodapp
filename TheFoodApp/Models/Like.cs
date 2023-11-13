using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TheFoodApp.Areas.Identity.Data;

namespace TheFoodApp.Models;

public class Like
{
    [Key]
    public int LikeId { get; set; }
    [ForeignKey("User")]
    public string UserId { get; set; }
    public User User { get; set; }
    [ForeignKey("Recipe")]
    public int RecipeId { get; set; }
    public Recipe Recipe { get; set; }
}