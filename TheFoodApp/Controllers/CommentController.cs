using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheFoodApp.Areas.Identity.Data;
using TheFoodApp.Models;


namespace TheFoodApp.Controllers;

public class CommentController : Controller
{
    private readonly ApplicationDbContext _context;
  
    private readonly UserManager<User> _userManager;
    
    public CommentController(ApplicationDbContext context , UserManager<User> userManager)
    {
        _context = context;
       
        _userManager = userManager;
    }
    // GET
    
    
   
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(RecipeDetailsViewModel model)
    {
        
        var comment = model.NewComment;
        
        comment.UserId = _userManager.GetUserId(User);
            _context.Comments.Add(comment);
            _context.SaveChanges();

            return RedirectToAction("Details", "Recipe", new { id = comment.RecipeId });
        
        
    }
}