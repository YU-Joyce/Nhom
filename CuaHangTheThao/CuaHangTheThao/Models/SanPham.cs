using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CuaHangTheThao.Models
{
    public class SanPham
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Tên Sản Phẩm")]
        public string? TenSanPham { get; set; }

        [StringLength(500)]
        [Display(Name = "Mô Tả")]
        public string? MoTa { get; set; }

        [Required]
        [ForeignKey("DanhMuc")]
        [Display(Name = "Danh Mục")]
        public int DanhMucId { get; set; }
        public DanhMuc DanhMuc { get; set; }

        [Required]
        [Display(Name = "Giá")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải là một số dương.")]
        public decimal Gia { get; set; }

        [Required]
        [Display(Name = "Số Lượng")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng phải là một số nguyên không âm.")]
        public int SoLuong { get; set; }

        [Display(Name = "Hình Ảnh")]
        public string? HinhAnh { get; set; }
    }
}
