using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TDCD_SharingManga.Models;
using Model.DAO;
using Model.EF;
using TDCD_SharingManga.Models;

namespace TDCD_SharingManga.Controllers
{
    public class MangaController : Controller
    {
        TDCD_DbContext db = new TDCD_DbContext();
        // GET: Manga
        public ActionResult Index(int id)
        {
            var dao = new MangaDao();

            var mangaId = id.ToString();
            if( String.IsNullOrEmpty((string)Session[mangaId]))
            {
                dao.AddView(id);
                Session[mangaId] = "Đã xem";
            }
            var manga = db.Mangas.Where(i => i.mId == id).FirstOrDefault();
            var item = new MangaDetail();
            item.mId = manga.mId;
            var chapter = db.Chapters.OrderByDescending(i => i.cId)
                .Where(i => i.mId == manga.mId).FirstOrDefault();
            item.displayName = chapter == null ? manga.mName : chapter.cName;
            item.createdBy = db.Users.Where(i => i.uId == manga.CreatedBy).FirstOrDefault().displayName;
            item.updatedAt = manga.updatedAt;
            item.chapters = manga.mChapter;
            item.totalComment = manga.totalComment;
            item.image = manga.image;
            item.description = manga.mDescription;
            item.author = manga.mAuthor;
            item.totalView = manga.totalView;
            var categories = db.Categories.Where(i => i.mId == item.mId).Join(db.CategoryDetails, d => d.categoryId, f => f.categoryId, (d, f) => new { Category = d, CategoryDetail = f });
            item.listCategories = categories.Select(i => new CategoryModel { id = i.Category.categoryId, displayName = i.CategoryDetail.categoryName}).ToList();
            return View(item);
        }

        public JsonResult GetChapters(int mangaId)
        {
            var listChapter = db.Chapters.Where(i => i.mId == mangaId).OrderBy(i => i.cPostOn).Select(i => new ChapterInfo {
                    chapterId = i.cId,
                    displayName = i.cName,
                    postTime = i.cPostOn.ToString()
                }).ToList();
            return Json(listChapter, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetManga(string name)
        {
            var listChapter = db.Mangas.Where(i => i.mName.ToLower().Contains(name.ToLower())).OrderBy(i => i.mName).Select(i => new MangaDetail
            {
                displayName = i.mName,
                mId = i.mId
            }).ToList();
            return Json(listChapter, JsonRequestBehavior.AllowGet);
        }

        public ActionResult C(int id)
        {
            var chapter = db.Chapters.Where(i => i.cId == id).FirstOrDefault();
            var chapterDetail = new ChapterDetail();
            chapterDetail.Images = db.Images.Where(i => i.cId == id).Select(i => i.source).ToList();
            var nextChapter = db.Chapters.Where(i => i.mId == chapter.mId && i.cId > id).OrderBy(i => i.cId).FirstOrDefault();
            var prevChapter = db.Chapters.Where(i => i.mId == chapter.mId && i.cId < id).OrderByDescending(i => i.cId).FirstOrDefault();
            chapterDetail.nextChapter = (nextChapter != null) ? nextChapter.cId : 0;
            chapterDetail.prevChapter = (prevChapter != null) ? prevChapter.cId : 0;
            chapterDetail.displayName = chapter.cName;
            chapterDetail.mId = chapter.mId;
            return View(chapterDetail);
        }

        public JsonResult GetTopMonth()
        {
            var now = DateTime.Now;
            var item = db.Views.Where(i => i.vMonth == now.Month).GroupBy(i => i.mId).Select(i => new MangaDetail { mId = i.Key, totalView = i.Count() }).OrderByDescending(i => i.totalView).Take(15).ToList();
            var listManga = item.Select(i => GetFullInfoManga(i)).ToList();
            return Json(listManga, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTopAll()
        {
            var listManga = db.Mangas.OrderByDescending(i => i.totalView).Select(i => new MangaDetail { mId = i.mId, totalView = i.totalView, displayName = i.mName });
            return Json(listManga, JsonRequestBehavior.AllowGet);
        }
        public MangaDetail GetFullInfoManga(MangaDetail manga)
        {
            var item = db.Mangas.Where(i => i.mId == manga.mId).FirstOrDefault();
            manga.displayName = item.mName;
            return manga;
        }
    }
}