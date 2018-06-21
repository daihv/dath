namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Comment")]
    public partial class Comment
    {
        [Key]
        public int cmId { get; set; }

        public int? uId { get; set; }

        public int? mId { get; set; }

        [Column("comment")]
        public string comment1 { get; set; }

        public DateTime? commentDate { get; set; }
    }
}
