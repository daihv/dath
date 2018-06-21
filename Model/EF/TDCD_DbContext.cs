namespace Model.EF
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class TDCD_DbContext : DbContext
    {
        public TDCD_DbContext()
            : base("name=TDCD_DbContext")
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<CategoryDetail> CategoryDetails { get; set; }
        public virtual DbSet<Chapter> Chapters { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Follow> Follows { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Manga> Mangas { get; set; }
        public virtual DbSet<Notify> Notifies { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<View> Views { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Manga>()
                .Property(e => e.mDescription)
                .IsUnicode(false);

            modelBuilder.Entity<Notify>()
                .Property(e => e.nContent)
                .IsUnicode(false);

            modelBuilder.Entity<View>()
                .Property(e => e.mId);
        }
    }
}
