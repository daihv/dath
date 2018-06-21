namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("View")]
    public partial class View
    {
        public int Id { get; set; }

        public int mId { get; set; }
        
        public DateTime? vDate { get; set; }

        public int? vMonth { get; set; }

        public int? vYear { get; set; }
    }
}
