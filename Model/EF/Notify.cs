namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Notify")]
    public partial class Notify
    {
        [Key]
        public int nId { get; set; }

        public int? uId { get; set; }

        [Column(TypeName = "text")]
        public string nContent { get; set; }

        [StringLength(300)]
        public string link { get; set; }
    }
}
