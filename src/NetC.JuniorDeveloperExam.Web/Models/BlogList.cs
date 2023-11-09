using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetC.JuniorDeveloperExam.Web.Models
{
    public class BlogList
    {
        public List<Blogpost> FullBlogList { get; internal set; }
        public int SelectedBlogPostID { get; internal set; }
    }
}