﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace TDCD_SharingManga.Models
{
    public class ChapterDetail
    {
        public IEnumerable<string> Images { get; set; }
        public int nextChapter { get; set; }
        public int prevChapter { get; set; }
        public int chapterId { get; set; }
        public string displayName { get; set; }
        public int mId { get; set; }
    }
}