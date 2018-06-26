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
        [HttpGet]
        public ActionResult Create()
        {
            var manga = new MangaDetail();
            return View(manga);
        }
        [HttpPost]
        public ActionResult Create(MangaDetail item)
        {
            var path = "";
            var fileName = "";
            var user = (UserLogin)Session[Constants.USER_SESSION];
            if (item.imgFile != null)
            {
                fileName = item.imgFile.FileName;
                if (item.imgFile.ContentLength > 0)
                {
                    var manga = new Manga();
                    manga.mName = item.displayName;
                    manga.publishedAt = DateTime.Now;
                    manga.updatedAt = DateTime.Now;
                    manga.mAuthor = item.author;
                    manga.CreatedBy = user.UserId;
                    manga.mDescription = item.description;
                    MangaDao dao = new MangaDao();
                    int id = dao.AddManga(manga);
                    if (Path.GetExtension(fileName).ToLower() == ".jpg"
                        || Path.GetExtension(fileName).ToLower() == ".png"
                        || Path.GetExtension(fileName).ToLower() == ".gif"
                        || Path.GetExtension(fileName).ToLower() == ".jpeg")
                    {
                        string currentDir = Path.Combine(HttpRuntime.AppDomainAppPath, "Image");
                        if (!Directory.Exists(Path.Combine(currentDir, id.ToString())))
                            Directory.CreateDirectory(Path.Combine(currentDir, id.ToString()));
                        string dir = Path.Combine(currentDir, id.ToString());
                        path = Path.Combine(dir, fileName);
                        item.imgFile.SaveAs(path);
                        var _manga = db.Mangas.Where(i => i.mId == id).FirstOrDefault();
                        _manga.image = "/Image/" + manga.mId.ToString() + "/" + item.imgFile.FileName;
                        db.SaveChanges();
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
                            string currentDir = Path.Combine(HttpRuntime.AppDomainAppPath, "Image");
                            if (!Directory.Exists(Path.Combine(currentDir, manga.mId.ToString())))
                                Directory.CreateDirectory(Path.Combine(currentDir, manga.mId.ToString()));
                            //var s = "/Images" + manga.mId.ToString();
                            string dir = Path.Combine(currentDir, manga.mId.ToString());
                            path = Path.Combine(dir, fileName);
                            mangaDetail.imgFile.SaveAs(path);
                        }
                        manga.image = "/Image/" + manga.mId.ToString() + "/" + mangaDetail.imgFile.FileName;
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

        public JsonResult GetChapters(int mangaId)
        {
            var listChapter = db.Chapters.Where(i => i.mId == mangaId).OrderBy(i => i.cPostOn).Select(i => new ChapterInfo
            {
                chapterId = i.cId,
                displayName = i.cName,
                postTime = i.cPostOn.ToString()
            }).ToList();
            return Json(listChapter, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult AddChapter(int id)
        {
            var user = (UserLogin)Session[Constants.USER_SESSION];
            var manga = db.Mangas.Where(i => i.mId == id).FirstOrDefault();
            if(user.UserId != manga.CreatedBy)
            {
                return RedirectToAction("Error");
            }
            var chapter = new ChapterDetail();
            chapter.mId = id;
            return View(chapter);
        }

        [HttpPost]
        public ActionResult AddChapter(ChapterDetail chapter)
        {
            var dao = new MangaDao();
            var path = "";
            var fileName = "";
            var imgList = new List<ImageViewModel>();
            var user = (UserLogin)Session[Constants.USER_SESSION];
            if (chapter.imgFile.Any())
            {
                var c = new Chapter();
                c.cName = chapter.displayName;
                c.cPostOn = DateTime.Now;
                c.mId = chapter.mId;
                var cId = dao.AddChapter(c);
                var currentDir = Path.Combine(HttpRuntime.AppDomainAppPath, "Image" , chapter.mId.ToString(), cId.ToString());
                if (!Directory.Exists(currentDir))
                    Directory.CreateDirectory(currentDir);
                foreach (var file in chapter.imgFile)
                {
                    fileName = file.FileName;
                    if (file.ContentLength > 0)
                    {
                        if (Path.GetExtension(fileName).ToLower() == ".jpg"
                            || Path.GetExtension(fileName).ToLower() == ".png"
                            || Path.GetExtension(fileName).ToLower() == ".gif"
                            || Path.GetExtension(fileName).ToLower() == ".jpeg")
                        {
                            file.SaveAs(Path.Combine(currentDir, fileName));
                            var image = new ImageViewModel();
                            image.src = "/Image/" + c.mId.ToString() + "/" + c.cId.ToString() + "/" + fileName;
                            imgList.Add(image);
                        }
                    }
                }
                foreach(var img in imgList)
                {
                    var i = new Image();
                    i.cId = cId;
                    i.source = img.src;
                    dao.AddImage(i);
                }
            }
            return RedirectToAction("UploadedManga");
        }
    }
}