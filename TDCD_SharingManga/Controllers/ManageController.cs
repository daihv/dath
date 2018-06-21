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
        public ActionResult Manga(int id = 0)
        {
            var user = (UserLogin)Session[Constants.USER_SESSION];
            if (user != null)
            {
                var manga = db.Mangas.Where(i => i.mId == id).FirstOrDefault();
                if (user.UserId == manga.CreatedBy)
                {
                    return View(manga);
                }
                else
                {
                    return View("Error");
                }
            }
            else
            {
                return RedirectToAction("Index", "User");
            }
        }
        [HttpPost]
        public ActionResult UploadImage(MangaDetail item)
        {
            var path = "";
            var fileName = item.imgFile.FileName;
            var user = (UserLogin)Session[Constants.USER_SESSION];
            if (item.imgFile != null)
            {
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
                        chapters = i.mChapter

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
    }
}