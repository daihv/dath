using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TDCD_SharingManga.Models
{
    public class PageListModel
    {
        public IEnumerable<MangaDetail> listManga { get; set; }
        public int total { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
    }
}