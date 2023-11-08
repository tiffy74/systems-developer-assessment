﻿using Microsoft.Extensions.Options;
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
        private readonly IJsonService _jsonService;
        private int commentIdCounter = 1;
        private string path = HttpContext.Current.Server.MapPath("~/App_Data/Blog-Posts.json");
        public BlogPostService(IMemoryCache cache, IJsonService jsonService)
        {
            _cache = cache;
            _jsonService = jsonService;
        }

        public List<Rootobject> GetAllBlogPosts()
        {
            if (_cache.TryGetValue("Index", out List<Rootobject> cachedBlogPosts))
            {
                return cachedBlogPosts;
            }

            Rootobject rootObject = _jsonService.ReadJson<Rootobject>(path);
            List<Rootobject> blogPosts = new List<Rootobject> { rootObject };

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMilliseconds(30)
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

            Rootobject rootObject = _jsonService.ReadJson<Rootobject>(path);
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
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMilliseconds(30)
            };
            _cache.Set(cacheKey, blogPost.FirstOrDefault(x => x.id == id), cacheEntryOptions);

            return blogPost.FirstOrDefault(x => x.id == id);
        }

        public void AddComment(int blogPostId, Comment commentForm)
        {
            Rootobject rootObject = _jsonService.ReadJson<Rootobject>(path);
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
                // Add the comment to the blog post's comments list
                blogPost.comments.Add(comment);

                _jsonService.WriteJson<Rootobject>(path, rootObject);
            }
            else
            {
                // Handle the case where the blog post is not found (e.g., return an error or throw an exception).
                throw new Exception("Blog post not found.");
            }
        }
        public void AddReply(int blogPostId, int commentId, Reply replyForm)
        {
            Rootobject rootObject = _jsonService.ReadJson<Rootobject>(path);

            // Find the blog post
            var blogPost = rootObject.blogPosts.FirstOrDefault(post => post.id == blogPostId);

            if (blogPost == null)
            {
                // Handle the case where the blog post doesn't exist
                throw new InvalidOperationException("Blog post not found.");
            }

            // Find the comment
            var comment = blogPost.comments.FirstOrDefault(c => c.CommentId == commentId);

            if (comment == null)
            {
                // Handle the case where the comment doesn't exist
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
    }
}