using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheFoodApp.Areas.Identity.Data;
using TheFoodApp.Models;

namespace TheFoodApp.Controllers;


[Authorize]
public class RecipeController : Controller
{
    
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _hostEnvironment;
    private readonly UserManager<User> _userManager;
    
    public RecipeController(ApplicationDbContext context , IWebHostEnvironment hostEnvironment, UserManager<User> userManager)
    {
        _context = context;
        _hostEnvironment = hostEnvironment;
        _userManager = userManager;
    }
    // GET
    public IActionResult Index()
    {
        List<Recipe> recipes = _context.Recipes.ToList();
        
        return View(recipes);
    }
    
    public IActionResult Create()
    {
        return View();
    }

    public IActionResult Details(int id)
    {
        var recipe = _context.Recipes
            .Include(r => r.Comments)
            .ThenInclude(c => c.User) // Include user data for each comment
            .FirstOrDefault(r => r.RecipeId == id);

        if (recipe == null)
        {
            return NotFound();
        }

        var viewModel = new RecipeDetailsViewModel
        {
            Recipe = recipe,
            NewComment = new Comment { RecipeId = recipe.RecipeId }
        };

        return View(viewModel);
    }

    
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("Name,Description")] Recipe recipe, IFormFile imageFile)
    {
       // if (ModelState.IsValid)
       // {


            var userId = _userManager.GetUserId(User);
            if (imageFile != null)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
                string extension = Path.GetExtension(imageFile.FileName);
                recipe.ImageUrl = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/images/", fileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    imageFile.CopyTo(fileStream);
                }
            }

            recipe.UserId = userId;
            recipe.Owner = _userManager.Users.FirstOrDefault(u => u.Id == userId);
            _context.Recipes.Add(recipe);
            _context.SaveChanges();
            return RedirectToAction("Index");
       // }
         return View(recipe);
        
    }

    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var recipe = _context.Recipes.Find(id);
        if (recipe == null)
        {
            return NotFound();
        }

        return View(recipe);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id,[Bind("RecipeId,Name,Description")] Recipe recipe, IFormFile imageFile)
    {
        Recipe recipefromDb = _context.Recipes.Find(id);
        //recipe.RecipeId = _context.Recipes.Find(id).RecipeId;
        
        if (id != recipefromDb.RecipeId)
        {
            return NotFound();
        }
        try
        {
            if (imageFile != null)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
                string extension = Path.GetExtension(imageFile.FileName);
                recipe.ImageUrl = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/images/", fileName);
                recipefromDb.ImageUrl = fileName;
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    imageFile.CopyTo(fileStream);
                }
            }

            else if (imageFile == null)
            {
                recipe.ImageUrl = recipefromDb.ImageUrl;
            }
            
            
            recipefromDb.Name = recipe.Name;
            recipefromDb.Description = recipe.Description;
           
            _context.SaveChanges();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!RecipeExists(recipe.RecipeId))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
        return RedirectToAction(nameof(Index));
    }
 
    public IActionResult Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var recipe = _context.Recipes
            .FirstOrDefault(m => m.RecipeId == id);
        if (recipe == null)
        {
            return NotFound();
        }

        return View(recipe);
    }


   

    private bool RecipeExists(int id)
    {
        return _context.Recipes.Any(e => e.RecipeId == id);
    }
    
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var recipe = _context.Recipes.Find(id);
        if (recipe != null)
        {
            _context.Recipes.Remove(recipe);
            _context.SaveChanges();
        }

        return RedirectToAction(nameof(Index)); //
    }

   
}