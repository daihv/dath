using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace Model.DAO
{
    public class MangaDao
    {
        TDCD_DbContext db = null;
        public MangaDao()
        {
            db = new TDCD_DbContext();
        }

        public int AddManga(Manga entity)
        {
            db.Mangas.Add(entity);
            db.SaveChanges();
            return entity.mId;
        }
        public int AddChapter(Chapter entity)
        {
            db.Chapters.Add(entity);
            var manga = db.Mangas.Where(i => i.mId == entity.mId).FirstOrDefault();
            manga.mChapter++;
            db.SaveChanges();
            return entity.cId;
        }
        public int AddImage(Image entity)
        {
            db.Images.Add(entity);
            db.SaveChanges();
            return entity.iId;
        }
        public int AddCategory(Category entity)
        {
            db.Categories.Add(entity);
            db.SaveChanges();
            return entity.Id;
        }
        public void RemoveCategory(Category entity)
        {
            db.Categories.Remove(entity);
            db.SaveChanges();
        }
        public void RemoveCategory(IEnumerable<Category> a)
        {
            db.Categories.RemoveRange(a);
        }
        public void AddView(int mangaId)
        {
            var manga = db.Mangas.Where(i => i.mId == mangaId).FirstOrDefault();
            manga.totalView++;
            var viewDetail = new View();
            viewDetail.Id = -1;
            var now = DateTime.UtcNow;
            viewDetail.vDate = now;
            viewDetail.vMonth = now.Month;
            viewDetail.vYear = now.Year;
            viewDetail.mId = mangaId;
            db.Views.Add(viewDetail);
            db.SaveChanges();
        }
        public void RemoveManga(Manga entity)
        {
            db.Mangas.Remove(entity);
            db.SaveChanges();
        }
        public void UnPublishedManga(int id)
        {
            var manga = db.Mangas.Where(i => i.mId == id).FirstOrDefault();
            manga.status = 1;
            db.SaveChanges();
        }
        public void PublishedManga(int id)
        {
            var manga = db.Mangas.Where(i => i.mId == id).FirstOrDefault();
            manga.status = 0;
            db.SaveChanges();
        }
        public void RemoveChapter(Chapter entity)
        {
            db.Chapters.Remove(entity);
            db.SaveChanges();
        }
    }
}
