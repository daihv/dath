namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Follow")]
    public partial class Follow
    {
        [Key]
        public int fId { get; set; }

        public int? uId { get; set; }

        public int? mId { get; set; }

        public DateTime? fDate { get; set; }
    }
}
