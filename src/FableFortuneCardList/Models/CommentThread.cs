using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FableFortuneCardList.Models
{
    public class CommentThread
    {
        public int ID { get; set; }
        public string PageUrl { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
