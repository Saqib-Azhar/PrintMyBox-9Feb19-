using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Practicing_OAuth.Models
{
    public class Article
    {
        public string ArticleID { get; set; }

        public string Title { get; set; }
        public string Heading { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public string ImageID { get; set; }
        public string Author1 { get; set; }

        [DisplayFormat(DataFormatString = "{0:MMMM dd, yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DatePublished { get; set; }

        [DisplayFormat(DataFormatString = "{0:MMMM dd, yyyy}", ApplyFormatInEditMode = true)]
        public DateTime LastUpdated { get; set; }

        public string Category1 { get; set; }
        public string Category2 { get; set; }

        public string Body { get; set; }
        public string ArticleType { get; set; }
        public string Topic { get; set; }
        public List<string> SubHeadings { get; set; }
    }
}