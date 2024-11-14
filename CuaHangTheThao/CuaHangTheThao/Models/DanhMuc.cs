using System.ComponentModel.DataAnnotations;

namespace CuaHangTheThao.Models
{
    public class DanhMuc
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Tên Danh Mục")]
        public string TenDanhMuc { get; set; }

        [StringLength(500)]
        [Display(Name = "Mô Tả")]
        public string MoTa { get; set; }

        [Required]
        [Display(Name = "Trạng Thái")]
        public bool TrangThai { get; set; } // true = Hoạt động, false = Không hoạt động
    }
}
