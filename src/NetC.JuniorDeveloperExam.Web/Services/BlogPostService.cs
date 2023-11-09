using Microsoft.Extensions.Options;
using NetC.JuniorDeveloperExam.Web.Models;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Serilog;

namespace NetC.JuniorDeveloperExam.Web.Services
{
    public class BlogPostService
    {
        private readonly IMemoryCache _cache;
        private readonly IJsonService _jsonService;
        private string path = HttpContext.Current.Server.MapPath("~/App_Data/Blog-Posts.json");
        public BlogPostService(IMemoryCache cache, IJsonService jsonService)
        {
            _cache = cache;
            _jsonService = jsonService;
        }

        public List<Rootobject> GetAllBlogPosts()
        {
            Rootobject rootObject = _jsonService.ReadJson<Rootobject>(path);
            List<Rootobject> blogPosts = new List<Rootobject> { rootObject };
            try
            {
                // Check cache and see if cached posts
                if (_cache.TryGetValue("Index", out List<Rootobject> cachedBlogPosts))
                {
                    return cachedBlogPosts;
                }
                // Set cache timer
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                };
                _cache.Set("Index", blogPosts, cacheEntryOptions);
            }
            catch (Exception ex)
            {
                // Log exceptions
                Log.Information($"Exception: {ex.Message}");
            }

            return blogPosts;
        }

        public Blogpost GetBlogPostById(int id)
        {
            Rootobject rootObject = _jsonService.ReadJson<Rootobject>(path);
            List<Rootobject> blogPosts = new List<Rootobject> { rootObject };
            List<Blogpost> blogPost = new List<Blogpost>();
            try
            {
                // Set a unique key for cached item
                string cacheKey = "BlogPost_" + id;

                if (_cache.TryGetValue("BlogPost", out Blogpost cachedBlogPost))
                {
                    return cachedBlogPost;
                }

                foreach (Rootobject obj in blogPosts)
                {
                    var posts = obj.blogPosts;
                    foreach (Blogpost post in posts)
                    {
                        blogPost.Add(post);
                    }
                }
                // Set Cache Timer
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                };
                _cache.Set(cacheKey, blogPost.FirstOrDefault(x => x.id == id), cacheEntryOptions);
            }
            catch (Exception ex)
            {
                // Log exceptions
                Log.Information($"Exception: {ex.Message}");
            }
            return blogPost.FirstOrDefault(x => x.id == id);
        }

        public void AddComment(int blogPostId, Comment commentForm)
        {
            Rootobject rootObject = _jsonService.ReadJson<Rootobject>(path);
            try
            {
                var blogPost = rootObject.blogPosts.FirstOrDefault(bp => bp.id == blogPostId);

                if (blogPost != null)
                {
                    if (blogPost.comments == null)
                    {
                        blogPost.comments = new List<Comment>();
                    }

                    int commentCount = blogPost.comments.Count;

                    Comment comment = new Comment
                    {
                        BlogPostid = blogPostId,
                        CommentId = commentCount + 1,
                        name = commentForm.name,
                        emailAddress = commentForm.emailAddress,
                        date = DateTime.Now,
                        message = commentForm.message
                    };
                    blogPost.comments.Add(comment);

                    _jsonService.WriteJson<Rootobject>(path, rootObject);
                }
                else
                {
                    // Handle exceptions
                    throw new Exception("Blog post not found.");
                }
            }
            catch (Exception ex)
            {
                // Log exceptions
                Log.Information($"Exception: {ex.Message}");
            }
        }

        public void AddReply(int blogPostId, int commentId, Reply replyForm)
        {
            try
            {
                Rootobject rootObject = _jsonService.ReadJson<Rootobject>(path);

                // Find post
                var blogPost = rootObject.blogPosts.FirstOrDefault(post => post.id == blogPostId);

                if (blogPost == null)
                {

                    throw new InvalidOperationException("Blog post not found.");
                }

                // Find comment
                var comment = blogPost.comments.FirstOrDefault(c => c.CommentId == commentId);

                if (comment == null)
                {
                    throw new InvalidOperationException("Comment not found.");
                }

                // If replies is null, initialize it
                if (comment.replies == null)
                {
                    comment.replies = new List<Reply>();
                }

                // Calculate the next reply ID
                int replyCount = comment.replies.Count;

                // Add the reply
                comment.replies.Add(new Reply
                {
                    BlogPostid = blogPostId,
                    CommentId = commentId,
                    ReplyId = replyCount + 1,
                    name = replyForm.name,
                    emailAddress = replyForm.emailAddress,
                    date = DateTime.Now,
                    message = replyForm.message
                });

                // Save changes
                _jsonService.WriteJson<Rootobject>(path, rootObject);
            }
            catch (Exception ex)
            {
                // Log exceptions
                Log.Information($"Exception: {ex.Message}");
            }
        }
    }
}