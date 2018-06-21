namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Chapter")]
    public partial class Chapter
    {
        [Key]
        public int cId { get; set; }

        public int mId { get; set; }

        [StringLength(100)]
        public string cName { get; set; }

        public DateTime? cPostOn { get; set; }
    }
}
