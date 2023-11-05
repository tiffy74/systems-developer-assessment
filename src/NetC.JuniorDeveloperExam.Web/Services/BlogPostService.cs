using Microsoft.Extensions.Options;
using NetC.JuniorDeveloperExam.Web.Models;
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
        //public string JsonFilePath { get; }

        //public BlogPostService(string jsonFilePath)
        //{
        //    JsonFilePath = jsonFilePath;
        //}
        
        public List<Rootobject> GetAllBlogPosts()
        {
            var jsonFilePath = HttpContext.Current.Server.MapPath("~/App_Data/Blog-Posts.json");
            string jsonContent = File.ReadAllText(jsonFilePath);
            Rootobject rootObject = JsonConvert.DeserializeObject<Rootobject>(jsonContent);
            List<Rootobject> blogPosts = new List<Rootobject> { rootObject };

            return blogPosts;
        }
        public Blogpost GetBlogPostById(int id)
        {
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
            return blogPost.FirstOrDefault(x => x.id == id);
        }
    }
}