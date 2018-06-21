using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TDCD_SharingManga.Models
{
    public class MangaDetail
    {
        public int mId { get; set; }
        public string displayName { get; set; }
        public string image { get; set; }
        public string createdBy { get; set; }
        public DateTime? updatedAt { get; set; }
        public int chapters { get; set; }
        public int totalComment { get; set; }
        public string category { get; set; }
        public string description { get; set; }
        public string author { get; set; }
        public string lastestChapter { get; set; }
        public int totalView { get; set; }
        public HttpPostedFileBase imgFile { get; set; }
    }
}