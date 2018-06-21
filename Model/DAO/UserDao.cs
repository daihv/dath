using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
namespace Model.DAO
{
    public class UserDao
    {
        TDCD_DbContext db = null;
        public UserDao()
        {
            db = new TDCD_DbContext();
        }
        public int Insert(User entity)
        {
            db.Users.Add(entity);
            db.SaveChanges();
            return entity.uId;
        }
        public User GetByName(string userName)
        {
            return db.Users.SingleOrDefault(x => x.username == userName);
        }
        public bool Login(string username, string password)
        {
            var result = db.Users.Count(x => x.username == username && x.password == password);
            if(result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
