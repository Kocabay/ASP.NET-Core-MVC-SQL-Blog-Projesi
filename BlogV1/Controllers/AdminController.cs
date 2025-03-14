using BlogV1.Context;
using BlogV1.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlogV1.Controllers
{
    public class AdminController : Controller
    {
        private readonly BlogDbContext _context;

        public AdminController(BlogDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Blogs()
        {
            var values = _context.Blogs.ToList();
            return View(values);
        }

        public IActionResult EditBlog(int id)
        {
            var blog = _context.Blogs.Where(x => x.Id == id).FirstOrDefault();
            return View(blog);
        }
        //edit blog işlemleri
        [HttpPost]
        public IActionResult EditBlog(Blog model)
        {

            var blogs = _context.Blogs.Where(x => x.Id == model.Id).FirstOrDefault();
            blogs.Name = model.Name;
            blogs.Description = model.Description;
            blogs.Tags = model.Tags;
            blogs.ImageUrl = model.ImageUrl;
            blogs.PublishDate = DateTime.Now;

            _context.SaveChanges();
            return RedirectToAction("Blogs");
        }
        //toggle ac kapat
        public IActionResult ToggleStatus(int id)
        {
            var blog = _context.Blogs.Where(x => x.Id == id).FirstOrDefault();
            if (blog.Status == 1)
            {
                blog.Status = 0;
            }
            else
            {
                blog.Status = 1;
            }
            _context.SaveChanges();
            return RedirectToAction("Blogs");
        }


        //blog silme işlemi
        public IActionResult DeleteBlog(int id)
        {
            var blogs = _context.Blogs.Where(x => x.Id == id).FirstOrDefault();
            _context.Blogs.Remove(blogs);
            _context.SaveChanges();
            return RedirectToAction("Blogs");
        }


        public IActionResult CreateBlog()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateBlog(Blog model)
        {
            model.PublishDate = DateTime.Now;
            model.Status = 1;

            _context.Blogs.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Blogs");
        }
    }
}
