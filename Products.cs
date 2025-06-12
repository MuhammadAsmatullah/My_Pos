using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace My_Pos.Models
{
    public class Products
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductID { get; set; }

        public string? Descript { get; set; }
        public int? ReportCatId { get; set; }

        //public string? Category { get; set; }

        public int PriceA { get; set; }

        //public string? ImagePath { get; set; }
        [Column("Image")]
        public byte[]? ImageData { get; set; }

        public int ReportCatID { get; set; }

      

    }
}
