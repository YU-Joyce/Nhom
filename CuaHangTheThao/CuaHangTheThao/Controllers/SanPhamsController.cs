using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CuaHangTheThao.Data;
using CuaHangTheThao.Models;

namespace CuaHangTheThao.Controllers
{
    public class SanPhamsController : Controller
    {
        private readonly CuaHangTheThaoContext _context;

        public SanPhamsController(CuaHangTheThaoContext context)
        {
            _context = context;
        }

        // GET: SanPhams
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var sanPhams = _context.SanPham.Include(s => s.DanhMuc).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                sanPhams = sanPhams.Where(s => s.TenSanPham.Contains(searchString) || s.MoTa.Contains(searchString));
            }

            return View(await sanPhams.ToListAsync());
        }


        // GET: SanPhams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPham
                .Include(s => s.DanhMuc)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }

        // GET: SanPhams/Create
        public IActionResult Create()
        {
            ViewBag.DanhMucId = new SelectList(_context.DanhMuc, "Id", "TenDanhMuc");
            return View();
        }

        // POST: SanPhams/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SanPham sanPham, IFormFile HinhAnh)
        {
            if (ModelState.IsValid)
            {
                if (HinhAnh != null && HinhAnh.Length > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var fileExtension = Path.GetExtension(HinhAnh.FileName).ToLower();
                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("HinhAnh", "Loại tệp không hợp lệ. Chỉ hỗ trợ .jpg, .jpeg, .png, .gif.");
                        ViewBag.DanhMucId = new SelectList(_context.DanhMuc, "Id", "TenDanhMuc", sanPham.DanhMucId);
                        return View(sanPham);
                    }

                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", HinhAnh.FileName);

                    if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await HinhAnh.CopyToAsync(stream);
                    }

                    sanPham.HinhAnh = HinhAnh.FileName;
                }

                _context.Add(sanPham);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Lưu lại tên file hình ảnh khi có lỗi xảy ra
            if (HinhAnh != null)
            {
                sanPham.HinhAnh = HinhAnh.FileName;
            }

            ViewBag.DanhMucId = new SelectList(_context.DanhMuc, "Id", "TenDanhMuc", sanPham.DanhMucId);
            return View(sanPham);
        }



        // GET: SanPhams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPham.FindAsync(id);
            if (sanPham == null)
            {
                return NotFound();
            }
            ViewData["DanhMucId"] = new SelectList(_context.DanhMuc, "Id", "TenDanhMuc", sanPham.DanhMucId);
            return View(sanPham);
        }

        // POST: SanPhams/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenSanPham,MoTa,DanhMucId,Gia,SoLuong,HinhAnh")] SanPham sanPham, IFormFile HinhAnh)
        {
            if (id != sanPham.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra và cập nhật hình ảnh mới nếu có
                    if (HinhAnh != null && HinhAnh.Length > 0)
                    {
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                        var fileExtension = Path.GetExtension(HinhAnh.FileName).ToLower();
                        if (!allowedExtensions.Contains(fileExtension))
                        {
                            ModelState.AddModelError("HinhAnh", "Loại tệp không hợp lệ. Chỉ hỗ trợ .jpg, .jpeg, .png, .gif.");
                            ViewData["DanhMucId"] = new SelectList(_context.DanhMuc, "Id", "TenDanhMuc", sanPham.DanhMucId);
                            return View(sanPham);
                        }

                        // Xóa hình ảnh cũ nếu có
                        var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", sanPham.HinhAnh);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }

                        // Tạo đường dẫn lưu hình ảnh mới
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", HinhAnh.FileName);

                        // Kiểm tra thư mục và tạo nếu cần
                        if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                        }

                        // Lưu hình ảnh mới vào thư mục
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await HinhAnh.CopyToAsync(stream);
                        }

                        // Cập nhật tên hình ảnh trong SanPham
                        sanPham.HinhAnh = HinhAnh.FileName;
                    }

                    _context.Update(sanPham);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SanPhamExists(sanPham.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DanhMucId"] = new SelectList(_context.DanhMuc, "Id", "TenDanhMuc", sanPham.DanhMucId);
            return View(sanPham);
        }

        // GET: SanPhams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPham
                .Include(s => s.DanhMuc)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }

        // POST: SanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sanPham = await _context.SanPham.FindAsync(id);
            if (sanPham != null)
            {
                // Xóa hình ảnh khỏi thư mục khi sản phẩm bị xóa
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", sanPham.HinhAnh);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                _context.SanPham.Remove(sanPham);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SanPhamExists(int id)
        {
            return _context.SanPham.Any(e => e.Id == id);
        }
    }
}
