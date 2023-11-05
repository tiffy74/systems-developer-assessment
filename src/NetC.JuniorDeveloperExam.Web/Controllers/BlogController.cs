using System;
using NetC.JuniorDeveloperExam.Web.Services;
using System.Web.Mvc;
using NetC.JuniorDeveloperExam.Web.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.EnterpriseServices.CompensatingResourceManager;
using Serilog;

namespace NetC.JuniorDeveloperExam.Web.Controllers
{
    public class BlogController : Controller
    {
        private readonly BlogPostService _blogPostService;
        public BlogController(BlogPostService blogPostService)
        {
            _blogPostService = blogPostService;
        }
        public ActionResult Index()
        {
            var blogPosts = _blogPostService.GetAllBlogPosts();

            return View(blogPosts);
        }
        public ActionResult BlogPost(int id)
        {
            var blogPost = _blogPostService.GetBlogPostById(id);

            if (blogPost == null)
            {
                return HttpNotFound(); // Return a 404 if the blog post is not found
            }

            return View(blogPost);
        }
    }
}