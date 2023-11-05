using Microsoft.Extensions.Options;
using NetC.JuniorDeveloperExam.Web.Models;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace NetC.JuniorDeveloperExam.Web.Services
{
    public class BlogPostService
    {
        private readonly IMemoryCache _cache;

        public BlogPostService(IMemoryCache cache)
        {
            _cache = cache;
        }
        
        public List<Rootobject> GetAllBlogPosts()
        {
            if (_cache.TryGetValue("Index", out List<Rootobject> cachedBlogPosts))
            {
                return cachedBlogPosts;
            }
            var jsonFilePath = HttpContext.Current.Server.MapPath("~/App_Data/Blog-Posts.json");
            string jsonContent = File.ReadAllText(jsonFilePath);
            Rootobject rootObject = JsonConvert.DeserializeObject<Rootobject>(jsonContent);
            List<Rootobject> blogPosts = new List<Rootobject> { rootObject };

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            };
            _cache.Set("Index", blogPosts, cacheEntryOptions);
            
            return blogPosts;
        }
        public Blogpost GetBlogPostById(int id)
        {
            string cacheKey = "BlogPost_" + id;

            if (_cache.TryGetValue("BlogPost", out Blogpost cachedBlogPost))
            {
                return cachedBlogPost;
            }

            var jsonFilePath = HttpContext.Current.Server.MapPath("~/App_Data/Blog-Posts.json");
            string jsonContent = File.ReadAllText(jsonFilePath);
            Rootobject rootObject = JsonConvert.DeserializeObject<Rootobject>(jsonContent);
            List<Rootobject> blogPosts = new List<Rootobject> { rootObject };
            List<Blogpost> blogPost = new List<Blogpost>();
            foreach (Rootobject obj in blogPosts)
            {
                var posts = obj.blogPosts;
                foreach (Blogpost post in posts)
                {
                    blogPost.Add(post);
                }
            }

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            };
            _cache.Set(cacheKey, blogPost.FirstOrDefault(x => x.id == id), cacheEntryOptions);

            return blogPost.FirstOrDefault(x => x.id == id);
        }
        public void AddCommentToBlogPost(int blogPostId, Comment comment)
        {
            // Retrieve the Blogpost JSON data from the file
            var jsonFilePath = HttpContext.Current.Server.MapPath("~/App_Data/Blog-Posts.json");
            string jsonContent = File.ReadAllText(jsonFilePath);
            Rootobject rootObject = JsonConvert.DeserializeObject<Rootobject>(jsonContent);

            // Find the specific blog post by ID
            Blogpost blogPost = rootObject.blogPosts.FirstOrDefault(post => post.id == blogPostId);

            if (blogPost != null)
            {
                var date = DateTime.Now;
                // Create a new comment
                Comment newComment = new Comment
                {
                    name = comment.name,
                    emailAddress = comment.emailAddress,
                    message = comment.message,
                    date = date
                };

                // Add the new comment to the blog post's comments
                blogPost.comments.Add(newComment);

                // Update the JSON data with the new comment
                File.WriteAllText(jsonFilePath, JsonConvert.SerializeObject(rootObject, Formatting.Indented));
            }
        }
    }
}