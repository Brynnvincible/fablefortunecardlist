using System;
using System.Collections.Generic;

namespace FableFortuneCardList.Models
{
    public class Comment
    {
        public int ID { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public int UpVotes { get; set; }
        public int DownVotes { get; set; }
        public CommentThread Thread { get; set; }
        public Comment RepliedTo { get; set; }
        public List<Comment> Replies { get; set; }        
    }
}
