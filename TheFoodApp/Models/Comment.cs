using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TheFoodApp.Areas.Identity.Data;

namespace TheFoodApp.Models;

public class Comment
{
    [Key]
    public int CommentId { get; set; }
    
    [Required]
    [StringLength(500, MinimumLength = 3)]
    public string Text { get; set; }
    [ForeignKey("User")]
    public string UserId { get; set; }
    public User User { get; set; }
    [ForeignKey("Recipe")]
    public int RecipeId { get; set; }
    public Recipe Recipe { get; set; }
}