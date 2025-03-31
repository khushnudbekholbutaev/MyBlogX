using BlogPostify.Domain.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogPostify.Service.DTOs.Posts
{
    public class LanguageResultDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string CoverImage { get; set; }
        public bool IsPublished { get; set; }
        public int UserId { get; set; }
        public List<string> TagNames { get; set; } = [];
        public DateTimeOffset CreatedAt { get; set; }
    }
}
