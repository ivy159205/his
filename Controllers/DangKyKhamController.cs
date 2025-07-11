using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DangKyKhamController : ControllerBase
    {
        private readonly DBCHIS _context;

        public DangKyKhamController(DBCHIS context)
        {
            _context = context;
        }

        // GET: api/DangKyKham
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DangKyKham>>> GetDangKyKhams()
        {
            return await _context.DangKyKhams
                .Include(d => d.MaBnNavigation)
                .Include(d => d.MaBacSiNavigation)
                .Include(d => d.MaKhoaNavigation)
                .ToListAsync();
        }

        // GET: api/DangKyKham/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DangKyKham>> GetDangKyKham(string id)
        {
            var dangKyKham = await _context.DangKyKhams
                .Include(d => d.MaBnNavigation)
                .Include(d => d.MaBacSiNavigation)
                .Include(d => d.MaKhoaNavigation)
                .FirstOrDefaultAsync(d => d.MaDangKy == id);

            if (dangKyKham == null)
            {
                return NotFound();
            }

            return dangKyKham;
        }

        // PUT: api/DangKyKham/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDangKyKham(string id, DangKyKham dangKyKham)
        {
            if (id != dangKyKham.MaDangKy)
            {
                return BadRequest();
            }

            _context.Entry(dangKyKham).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DangKyKhamExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/DangKyKham
        [HttpPost]
        public async Task<ActionResult<DangKyKham>> PostDangKyKham(DangKyKham dangKyKham)
        {
            // Tự động tạo mã đăng ký nếu chưa có
            if (string.IsNullOrEmpty(dangKyKham.MaDangKy))
            {
                dangKyKham.MaDangKy = await GenerateMaDangKy();
            }

            // Tự động set ngày tạo
            dangKyKham.NgayTao = DateTime.Now;

            _context.DangKyKhams.Add(dangKyKham);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DangKyKhamExists(dangKyKham.MaDangKy))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetDangKyKham", new { id = dangKyKham.MaDangKy }, dangKyKham);
        }

        // DELETE: api/DangKyKham/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDangKyKham(string id)
        {
            var dangKyKham = await _context.DangKyKhams.FindAsync(id);
            if (dangKyKham == null)
            {
                return NotFound();
            }

            _context.DangKyKhams.Remove(dangKyKham);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/DangKyKham/ByBenhNhan/5
        [HttpGet("ByBenhNhan/{maBenhNhan}")]
        public async Task<ActionResult<IEnumerable<DangKyKham>>> GetDangKyKhamByBenhNhan(string maBenhNhan)
        {
            return await _context.DangKyKhams
                .Include(d => d.MaBnNavigation)
                .Include(d => d.MaBacSiNavigation)
                .Include(d => d.MaKhoaNavigation)
                .Where(d => d.MaBn == maBenhNhan)
                .OrderByDescending(d => d.NgayKham)
                .ToListAsync();
        }

        // GET: api/DangKyKham/ByNgayKham/2024-01-01
        [HttpGet("ByNgayKham/{ngayKham}")]
        public async Task<ActionResult<IEnumerable<DangKyKham>>> GetDangKyKhamByNgayKham(DateOnly ngayKham)
        {
            return await _context.DangKyKhams
                .Include(d => d.MaBnNavigation)
                .Include(d => d.MaBacSiNavigation)
                .Include(d => d.MaKhoaNavigation)
                .Where(d => d.NgayKham == ngayKham)
                .OrderBy(d => d.Stt)
                .ToListAsync();
        }

        // GET: api/DangKyKham/ByBacSi/5
        [HttpGet("ByBacSi/{maBacSi}")]
        public async Task<ActionResult<IEnumerable<DangKyKham>>> GetDangKyKhamByBacSi(string maBacSi)
        {
            return await _context.DangKyKhams
                .Include(d => d.MaBnNavigation)
                .Include(d => d.MaBacSiNavigation)
                .Include(d => d.MaKhoaNavigation)
                .Where(d => d.MaBacSi == maBacSi)
                .OrderByDescending(d => d.NgayKham)
                .ThenBy(d => d.Stt)
                .ToListAsync();
        }

        // PUT: api/DangKyKham/UpdateTrangThai/5
        [HttpPut("UpdateTrangThai/{id}")]
        public async Task<IActionResult> UpdateTrangThai(string id, [FromBody] string trangThai)
        {
            var dangKyKham = await _context.DangKyKhams.FindAsync(id);
            if (dangKyKham == null)
            {
                return NotFound();
            }

            dangKyKham.TrangThai = trangThai;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DangKyKhamExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // GET: api/DangKyKham/Statistics
        [HttpGet("Statistics")]
        public async Task<ActionResult<object>> GetStatistics()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            var statistics = new
            {
                TongSoDangKy = await _context.DangKyKhams.CountAsync(),
                DangKyHomNay = await _context.DangKyKhams.CountAsync(d => d.NgayKham == today),
                DangKyChoKham = await _context.DangKyKhams.CountAsync(d => d.TrangThai == "Chờ khám"),
                DangKyHoanThanh = await _context.DangKyKhams.CountAsync(d => d.TrangThai == "Hoàn thành"),
                DangKyHuy = await _context.DangKyKhams.CountAsync(d => d.TrangThai == "Hủy")
            };

            return statistics;
        }

        private bool DangKyKhamExists(string id)
        {
            return _context.DangKyKhams.Any(e => e.MaDangKy == id);
        }

        private async Task<string> GenerateMaDangKy()
        {
            var today = DateTime.Today;
            var prefix = $"DK{today:yyyyMMdd}";

            var lastRecord = await _context.DangKyKhams
                .Where(d => d.MaDangKy.StartsWith(prefix))
                .OrderByDescending(d => d.MaDangKy)
                .FirstOrDefaultAsync();

            if (lastRecord != null)
            {
                var lastNumber = int.Parse(lastRecord.MaDangKy.Substring(prefix.Length));
                return $"{prefix}{(lastNumber + 1):D3}";
            }
            else
            {
                return $"{prefix}001";
            }
        }
    }
}