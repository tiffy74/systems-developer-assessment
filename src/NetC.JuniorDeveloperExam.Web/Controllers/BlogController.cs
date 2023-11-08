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
        [HttpPost]
        public ActionResult AddComment(int Id, Comment comment)
        {
            if (ModelState.IsValid) // You should define validation rules in your CommentModel
            {
                try
                {
                    _blogPostService.AddComment(Id, comment);
                    TempData["CommentSuccess"] = "Your comment has been added!";
                }
                catch (Exception ex)
                {
                    TempData["CommentError"] = "An error occurred while adding the comment: " + ex.Message;
                }
            }
            else
            {
                TempData["CommentError"] = "Please fill in all required fields.";
            }

            return RedirectToAction("BlogPost", new { id = Id });
        }
        [HttpPost]
        public ActionResult AddReply(int Id, int commentId, Reply reply)
        {
            if (ModelState.IsValid) // You should define validation rules in your CommentModel
            {
                try
                {
                    _blogPostService.AddReply(Id, commentId, reply);
                    TempData["CommentSuccess"] = "Your comment has been added!";
                }
                catch (Exception ex)
                {
                    TempData["CommentError"] = "An error occurred while adding the comment: " + ex.Message;
                }
            }
            else
            {
                TempData["CommentError"] = "Please fill in all required fields.";
            }

            return RedirectToAction("BlogPost", new { id = Id });
        }
    }
}