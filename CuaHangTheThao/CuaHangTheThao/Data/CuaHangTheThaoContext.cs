using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CuaHangTheThao.Models;

namespace CuaHangTheThao.Data
{
    public class CuaHangTheThaoContext : DbContext
    {
        public CuaHangTheThaoContext (DbContextOptions<CuaHangTheThaoContext> options)
            : base(options)
        {
        }

        public DbSet<CuaHangTheThao.Models.DanhMuc> DanhMuc { get; set; } = default!;
        public DbSet<CuaHangTheThao.Models.SanPham> SanPham { get; set; } = default!;
        public DbSet<CuaHangTheThao.Models.NguoiDung> NguoiDung { get; set; } = default!;
    }
}
