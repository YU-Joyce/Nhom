using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CuaHangTheThao.Data;
using CuaHangTheThao.Models;

namespace CuaHangTheThao.Controllers
{
    public class NguoiDungsController : Controller
    {
        private readonly CuaHangTheThaoContext _context;

        public NguoiDungsController(CuaHangTheThaoContext context)
        {
            _context = context;
        }

        // GET: NguoiDungs
        public async Task<IActionResult> Index(string searchString)
        {
            var nguoiDungs = from n in _context.NguoiDung
                             select n;

            if (!String.IsNullOrEmpty(searchString))
            {
                nguoiDungs = nguoiDungs.Where(n => n.TenDangNhap.Contains(searchString) || n.Email.Contains(searchString));
            }

            return View(await nguoiDungs.ToListAsync());
        }


        // GET: NguoiDungs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nguoiDung = await _context.NguoiDung
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nguoiDung == null)
            {
                return NotFound();
            }

            return View(nguoiDung);
        }

        // GET: NguoiDungs/Create
        public IActionResult Create()
        {
            ViewBag.RoleList = new SelectList(new List<string> { "User", "Admin" });
            return View();
        }

        // POST: NguoiDungs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenDangNhap,MatKhau,Email,DiaChi,SoDienThoai,VaiTro")] NguoiDung nguoiDung)
        {
            ViewBag.RoleList = new SelectList(new List<string> { "User", "Admin" });

            if (ModelState.IsValid)
            {
                _context.Add(nguoiDung);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nguoiDung);
        }


        // GET: NguoiDungs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nguoiDung = await _context.NguoiDung.FindAsync(id);
            if (nguoiDung == null)
            {
                return NotFound();
            }

            ViewBag.RoleList = new SelectList(new List<string> { "User", "Admin" });
            return View(nguoiDung);
        }

        // POST: NguoiDungs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenDangNhap,MatKhau,Email,DiaChi,SoDienThoai,VaiTro")] NguoiDung nguoiDung)
        {
            ViewBag.RoleList = new SelectList(new List<string> { "User", "Admin" });

            if (id != nguoiDung.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nguoiDung);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NguoiDungExists(nguoiDung.Id))
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
            return View(nguoiDung);
        }


        // GET: NguoiDungs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nguoiDung = await _context.NguoiDung
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nguoiDung == null)
            {
                return NotFound();
            }

            return View(nguoiDung);
        }

        // POST: NguoiDungs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nguoiDung = await _context.NguoiDung.FindAsync(id);
            if (nguoiDung != null)
            {
                _context.NguoiDung.Remove(nguoiDung);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NguoiDungExists(int id)
        {
            return _context.NguoiDung.Any(e => e.Id == id);
        }
    }
}
