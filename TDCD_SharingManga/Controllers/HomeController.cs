using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TDCD_SharingManga.Common;
using TDCD_SharingManga.Models;

namespace TDCD_SharingManga.Controllers
{
    public class HomeController : Controller
    {
        TDCD_DbContext db = new TDCD_DbContext();
        public ActionResult Index(int page = 1)
        {
            var listManga = new List<MangaDetail>();
            var mangas = db.Mangas.Where(i => i.status == (int)Constants.STATUS.Published).OrderByDescending(i => i.updatedAt).Skip((page - 1)*Constants.PAGE_SIZE).Take(Constants.PAGE_SIZE).ToList();
            foreach (var manga in mangas)
            {
                var item = new MangaDetail();
                item.mId = manga.mId;
                var chapter = db.Chapters.OrderByDescending(i => i.cId)
                    .Where(i => i.mId == manga.mId).FirstOrDefault();
                item.displayName = chapter == null ? manga.mName : chapter.cName;
                item.image = manga.image;
                item.createdBy = db.Users.Where(i => i.uId == manga.CreatedBy).FirstOrDefault().displayName;
                item.updatedAt = manga.updatedAt;
                item.chapters = manga.mChapter;
                item.totalComment = manga.totalComment;
                item.image = manga.image;
                item.description = manga.mDescription;
                item.author = manga.mAuthor;
                item.totalView = manga.totalView;
                item.status = manga.status;
                var categories = db.Categories.Where(i => i.mId == item.mId).Join(db.CategoryDetails, d => d.categoryId, f => f.categoryId, (d, f) => new { Category = d, CategoryDetail = f });
                item.listCategories = categories.Select(i => new CategoryModel { id = i.Category.categoryId, displayName = i.CategoryDetail.categoryName }).ToList();
                listManga.Add(item);
            }
            return View(new PageListModel { listManga = listManga, page = page, total = db.Mangas.Count(), pageSize = Constants.PAGE_SIZE});
        }

        public ActionResult Sort(int categoryId, int page = 1)
        {
            var listManga = new List<MangaDetail>();
            var ids = db.Categories.Where(i => i.categoryId == categoryId).Select(i => i.mId).ToList();
            var mangas = db.Mangas.Where(i => i.status == (int)Constants.STATUS.Published && ids.Contains(i.mId)).OrderByDescending(i => i.updatedAt).Skip((page - 1) * Constants.PAGE_SIZE).Take(Constants.PAGE_SIZE).ToList();
            foreach (var manga in mangas)
            {
                var item = new MangaDetail();
                item.mId = manga.mId;
                var chapter = db.Chapters.OrderByDescending(i => i.cId)
                    .Where(i => i.mId == manga.mId).FirstOrDefault();
                item.displayName = chapter == null ? manga.mName : chapter.cName;
                item.image = manga.image;
                item.createdBy = db.Users.Where(i => i.uId == manga.CreatedBy).FirstOrDefault().displayName;
                item.updatedAt = manga.updatedAt;
                item.chapters = manga.mChapter;
                item.totalComment = manga.totalComment;
                item.image = manga.image;
                item.description = manga.mDescription;  
                item.author = manga.mAuthor;
                item.totalView = manga.totalView;
                item.status = manga.status;
                var categories = db.Categories.Where(i => i.mId == item.mId).Join(db.CategoryDetails, d => d.categoryId, f => f.categoryId, (d, f) => new { Category = d, CategoryDetail = f });
                item.listCategories = categories.Select(i => new CategoryModel { id = i.Category.categoryId, displayName = i.CategoryDetail.categoryName }).ToList();
                listManga.Add(item);
            }
            return View("Index", new PageListModel { listManga = listManga, page = page, total = db.Mangas.Count(), pageSize = Constants.PAGE_SIZE });
        }

    }
}