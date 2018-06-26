using Model.DAO;
using Model.EF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TDCD_SharingManga.Common;
using TDCD_SharingManga.Models;

namespace TDCD_SharingManga.Controllers
{
    public class ManageController : Controller
    {
        TDCD_DbContext db = new TDCD_DbContext();
        // GET: Manage
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadImage(MangaDetail item)
        {
            var path = "";
            var fileName = "";
            var user = (UserLogin)Session[Constants.USER_SESSION];
            if (item.imgFile != null)
            {
                fileName = item.imgFile.FileName;
                if (item.imgFile.ContentLength > 0)
                {
                    if (Path.GetExtension(fileName).ToLower() == ".jpg"
                        || Path.GetExtension(fileName).ToLower() == ".png"
                        || Path.GetExtension(fileName).ToLower() == ".gif"
                        || Path.GetExtension(fileName).ToLower() == ".jpeg")
                    {
                        string currentDir = HttpRuntime.AppDomainAppPath;
                        string dir = Path.Combine(HttpRuntime.AppDomainAppPath, "Image");
                        path = Path.Combine(dir, fileName);
                        item.imgFile.SaveAs(path);
                        var manga = new Manga();
                        manga.mName = item.displayName;
                        manga.publishedAt = DateTime.Now;
                        manga.updatedAt = DateTime.Now;
                        manga.mAuthor = item.author;
                        manga.CreatedBy = user.UserId;
                        manga.image = "/Image/" + item.imgFile.FileName;
                        manga.mDescription = item.description;
                        MangaDao dao = new MangaDao();
                        dao.AddManga(manga);
                    }

                }
            }
            return RedirectToAction("Index", "Manage");
        }

        public ActionResult UploadedManga(int page = 1)
        {
            var user = (UserLogin)Session[Constants.USER_SESSION];
            var mangaList = new List<MangaDetail>();
            int total = 0;
            if(user == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                if(user.UserName == "admin")
                {
                    var list = db.Mangas.OrderBy(i => i.mName);
                    mangaList = list.Skip(Constants.PAGE_SIZE * (page - 1)).Take(Constants.PAGE_SIZE).Select(i => new MangaDetail {
                        displayName = i.mName,
                        mId = i.mId,
                        author = i.mAuthor,
                        updatedAt = i.updatedAt,
                        chapters = i.mChapter,
                        status = i.status
                    }).ToList();
                    total = list.Count();
                }
                else
                {
                    var list = db.Mangas.Where(i => i.CreatedBy == user.UserId).OrderBy(i => i.mName);
                    mangaList = list.Skip(Constants.PAGE_SIZE * (page - 1)).Take(Constants.PAGE_SIZE).Select(i => new MangaDetail
                    {
                        displayName = i.mName,
                        mId = i.mId,
                        author = i.mAuthor,
                        updatedAt = i.updatedAt,
                        chapters = i.mChapter
                    }).ToList();
                    total = list.Count();
                }
            }
            return View(new PageListModel { listManga = mangaList, page = page, pageSize = Constants.PAGE_SIZE, total = total});
        }
        public ActionResult DeleteManga(int id)
        {
            MangaDao dao = new MangaDao();
            dao.RemoveManga(id);
            return RedirectToAction("Index", "Manage");
        }
        public ActionResult Published(int id)
        {
            MangaDao dao = new MangaDao();
            dao.RemoveManga(id);
            return RedirectToAction("Index", "Manage");
        }
        public ActionResult UnPublished(int id)
        {
            MangaDao dao = new MangaDao();
            dao.RemoveManga(id);
            return RedirectToAction("Index", "Manage");
        }

        [HttpGet]
        public ActionResult Manga(int id = 0)
        {
            var manga = db.Mangas.Where(i => i.mId == id).FirstOrDefault();
            if (manga == null)
                return RedirectToAction("UploadedManga");
            var user = (UserLogin)Session[Constants.USER_SESSION];
            if (user != null)
            {
                if (user.UserName == "admin" || user.UserId == manga.CreatedBy )
                {
                    var item = new MangaDetail();
                    item.mId = manga.mId;
                    var chapter = db.Chapters.OrderByDescending(i => i.cId)
                        .Where(i => i.mId == manga.mId).FirstOrDefault();
                    item.displayName = manga.mName;
                    item.createdBy = db.Users.Where(i => i.uId == manga.CreatedBy).FirstOrDefault().displayName;
                    item.updatedAt = manga.updatedAt;
                    item.chapters = manga.mChapter;
                    item.totalComment = manga.totalComment;
                    item.image = manga.image;
                    item.description = manga.mDescription;
                    item.author = manga.mAuthor;
                    item.totalView = manga.totalView;
                    return View(item);
                }
                else
                {
                    return View("Error");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public ActionResult Manga(MangaDetail mangaDetail)
        {
            var manga = db.Mangas.Where(i => i.mId == mangaDetail.mId).FirstOrDefault();
            if(manga != null)
            {
                var path = "";
                var fileName = "";
                var user = (UserLogin)Session[Constants.USER_SESSION];
                if (mangaDetail.imgFile != null)
                {
                    fileName = mangaDetail.imgFile.FileName;
                    if (mangaDetail.imgFile.ContentLength > 0)
                    {
                        if (Path.GetExtension(fileName).ToLower() == ".jpg"
                            || Path.GetExtension(fileName).ToLower() == ".png"
                            || Path.GetExtension(fileName).ToLower() == ".gif"
                            || Path.GetExtension(fileName).ToLower() == ".jpeg")
                        {
                            string currentDir = HttpRuntime.AppDomainAppPath;
                            string dir = Path.Combine(HttpRuntime.AppDomainAppPath, "Image");
                            path = Path.Combine(dir, fileName);
                            mangaDetail.imgFile.SaveAs(path);
                        }
                        manga.image = "/Image/" + mangaDetail.imgFile.FileName;
                    }
                }
                manga.mName = mangaDetail.displayName;
                manga.updatedAt = DateTime.Now;
                manga.mAuthor = mangaDetail.author;
                manga.mDescription = mangaDetail.description;
                db.SaveChanges();
            }
            return RedirectToAction("UploadedManga");
        }
    }
}