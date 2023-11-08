using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;



namespace NetC.JuniorDeveloperExam.Web.Models
{

    public class Rootobject
    {
        public Blogpost[] blogPosts { get; set; }
    }

    public class Blogpost
    {
        public int id { get; set; }
        public DateTime date { get; set; }
        public string title { get; set; }
        public string image { get; set; }
        public string htmlContent { get; set; }
        public List<Comment> comments { get; set; }
        public Comment comment { get; set; }
    }

    public class Comment
    {
        public int BlogPostid { get; set; }
        public int CommentId { get; set; }
        public string name { get; set; }
        public DateTime date { get; set; }
        public string emailAddress { get; set; }
        public string message { get; set; }
        public List<Reply> replies { get; set; }
    }
    public class Reply
    {
        public int BlogPostid { get; set; }
        public int CommentId { get; set; }
        public int ReplyId { get; set; }
        public string name { get; set; }
        public DateTime date { get; set; }
        public string emailAddress { get; set; }
        public string message { get; set; }
    }
}