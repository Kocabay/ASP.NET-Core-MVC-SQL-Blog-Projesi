using BlogV1.Context;
using BlogV1.Identity;
using BlogV1.Models;
using BlogV1.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogV1.Controllers
{
    public class BlogsController : Controller
    {
        private readonly BlogDbContext _context;
        private readonly UserManager<BlogIdentityUser> _userManager;
        private readonly SignInManager<BlogIdentityUser> _signInManager;

        public BlogsController(BlogDbContext context, UserManager<BlogIdentityUser> userManager, SignInManager<BlogIdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            var blogs = _context.Blogs.Where(x => x.Status == 1).ToList();
            return View(blogs);
        }

        public IActionResult Details(int id)
        {
            var blog = _context.Blogs.Where(x => x.Id == id).FirstOrDefault();
            blog.ViewCount += 1;
            _context.SaveChanges();
            var comment = _context.Comments.Where(x => x.BlogId == id).ToList();
            ViewBag.Comments = comment.ToList();
            return View(blog);
        }

        [HttpPost]
        public IActionResult CreateComment(Comment model)
        {
            model.PublisDate = DateTime.Now;
            _context.Comments.Add(model);
            var blog = _context.Blogs.Where(x => x.Id == model.BlogId).FirstOrDefault();
            blog.CommentCount += 1;

            _context.SaveChanges();
            return RedirectToAction("Details", new { id = model.BlogId });
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateContact(Contact model)
        {
            model.CreatedAt = DateTime.Now;
            _context.Contacts.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Support()
        {
            return View();
        }

        
    }
}
