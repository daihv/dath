namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        [Key]
        public int uId { get; set; }

        [StringLength(50)]
        public string username { get; set; }

        [StringLength(50)]
        public string password { get; set; }

        [StringLength(100)]
        public string email { get; set; }
        
        public int? role { get; set; }

        [StringLength(100)]
        public string displayName { get; set; }

        public DateTime? createDate { get; set; }
    }
}
