namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    [Table("Manga")]
    public partial class Manga
    {
        [Key]
        public int mId { get; set; }

        [Required]
        [StringLength(100)]
        public string mName { get; set; }

        public int mChapter { get; set; }

        [Column(TypeName = "text")]
        public string mDescription { get; set; }

        [StringLength(100)]
        public string mAuthor { get; set; }

        public int CreatedBy { get; set; }

        public int totalView { get; set; }

        [StringLength(300)]
        public string image { get; set; }

        public DateTime? updatedAt { get; set; }

        public DateTime? publishedAt { get; set; }

        public int totalComment { get; set; }

        public int status { get; set; }
    }
}
