using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TheFoodApp.Areas.Identity.Data;

namespace TheFoodApp.Models;

public class Recipe
{
    [Key]
    public int RecipeId { get; set; }
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
    [Required]
    
    public string Description { get; set; }
    public string? ImageUrl { get; set; }
    [ForeignKey("User")]
    public string UserId { get; set; }
    public User Owner { get; set; }
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Like> Likes { get; set; } = new List<Like>();
    public ICollection<Rating> Ratings { get; set; } =  new List<Rating>();
}