using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HoaDonController : ControllerBase
    {
        private readonly DBCHIS _context;

        public HoaDonController(DBCHIS context)
        {
            _context = context;
        }

        // GET: api/HoaDon
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HoaDon>>> GetHoaDons()
        {
            return await _context.HoaDons
                .Include(h => h.MaBnNavigation)
                .Include(h => h.NguoiThuNavigation)
                .Include(h => h.ChiTietHoaDons)
                .ToListAsync();
        }

        // GET: api/HoaDon/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HoaDon>> GetHoaDon(string id)
        {
            var hoaDon = await _context.HoaDons
                .Include(h => h.MaBnNavigation)
                .Include(h => h.NguoiThuNavigation)
                .Include(h => h.ChiTietHoaDons)
                .FirstOrDefaultAsync(h => h.MaHoaDon == id);

            if (hoaDon == null)
            {
                return NotFound();
            }

            return hoaDon;
        }

        // GET: api/HoaDon/BenhNhan/5
        [HttpGet("BenhNhan/{maBn}")]
        public async Task<ActionResult<IEnumerable<HoaDon>>> GetHoaDonsByBenhNhan(string maBn)
        {
            var hoaDons = await _context.HoaDons
                .Where(h => h.MaBn == maBn)
                .Include(h => h.MaBnNavigation)
                .Include(h => h.NguoiThuNavigation)
                .Include(h => h.ChiTietHoaDons)
                .OrderByDescending(h => h.NgayHoaDon)
                .ToListAsync();

            return hoaDons;
        }

        // GET: api/HoaDon/NgayHoaDon/2024-01-01/2024-12-31
        [HttpGet("NgayHoaDon/{tuNgay}/{denNgay}")]
        public async Task<ActionResult<IEnumerable<HoaDon>>> GetHoaDonsByDateRange(DateTime tuNgay, DateTime denNgay)
        {
            var hoaDons = await _context.HoaDons
                .Where(h => h.NgayHoaDon >= tuNgay && h.NgayHoaDon <= denNgay)
                .Include(h => h.MaBnNavigation)
                .Include(h => h.NguoiThuNavigation)
                .OrderByDescending(h => h.NgayHoaDon)
                .ToListAsync();

            return hoaDons;
        }

        // GET: api/HoaDon/TrangThai/DaThanhToan
        [HttpGet("TrangThai/{trangThai}")]
        public async Task<ActionResult<IEnumerable<HoaDon>>> GetHoaDonsByTrangThai(string trangThai)
        {
            var hoaDons = await _context.HoaDons
                .Where(h => h.TrangThai == trangThai)
                .Include(h => h.MaBnNavigation)
                .Include(h => h.NguoiThuNavigation)
                .OrderByDescending(h => h.NgayHoaDon)
                .ToListAsync();

            return hoaDons;
        }

        // PUT: api/HoaDon/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHoaDon(string id, HoaDon hoaDon)
        {
            if (id != hoaDon.MaHoaDon)
            {
                return BadRequest();
            }

            _context.Entry(hoaDon).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HoaDonExists(id))
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

        // POST: api/HoaDon
        [HttpPost]
        public async Task<ActionResult<HoaDon>> PostHoaDon(HoaDon hoaDon)
        {
            // Kiểm tra xem mã hóa đơn đã tồn tại chưa
            if (HoaDonExists(hoaDon.MaHoaDon))
            {
                return BadRequest("Mã hóa đơn đã tồn tại");
            }

            // Kiểm tra xem bệnh nhân có tồn tại không
            var benhNhan = await _context.BenhNhans.FindAsync(hoaDon.MaBn);
            if (benhNhan == null)
            {
                return BadRequest("Bệnh nhân không tồn tại");
            }

            // Kiểm tra người thu (nếu có)
            if (!string.IsNullOrEmpty(hoaDon.NguoiThu))
            {
                var nhanVien = await _context.DmNhanViens.FindAsync(hoaDon.NguoiThu);
                if (nhanVien == null)
                {
                    return BadRequest("Nhân viên thu không tồn tại");
                }
            }

            // Tự động gán NgayTao nếu chưa có
            if (hoaDon.NgayTao == null)
            {
                hoaDon.NgayTao = DateTime.Now;
            }

            _context.HoaDons.Add(hoaDon);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHoaDon", new { id = hoaDon.MaHoaDon }, hoaDon);
        }

        // DELETE: api/HoaDon/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHoaDon(string id)
        {
            var hoaDon = await _context.HoaDons.FindAsync(id);
            if (hoaDon == null)
            {
                return NotFound();
            }

            _context.HoaDons.Remove(hoaDon);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/HoaDon/ThanhToan/5
        [HttpPut("ThanhToan/{id}")]
        public async Task<IActionResult> ThanhToanHoaDon(string id, [FromBody] ThanhToanRequest request)
        {
            var hoaDon = await _context.HoaDons.FindAsync(id);
            if (hoaDon == null)
            {
                return NotFound();
            }

            hoaDon.TrangThai = "DaThanhToan";
            hoaDon.NgayThanhToan = DateTime.Now;
            hoaDon.HinhThucThanhToan = request.HinhThucThanhToan;
            hoaDon.NguoiThu = request.NguoiThu;

            _context.Entry(hoaDon).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/HoaDon/ThongKe/TheoNgay/2024-01-01/2024-12-31
        [HttpGet("ThongKe/TheoNgay/{tuNgay}/{denNgay}")]
        public async Task<ActionResult<object>> GetThongKeTheoNgay(DateTime tuNgay, DateTime denNgay)
        {
            var thongKe = await _context.HoaDons
                .Where(h => h.NgayHoaDon >= tuNgay && h.NgayHoaDon <= denNgay)
                .GroupBy(h => h.NgayHoaDon.Date)
                .Select(g => new
                {
                    Ngay = g.Key,
                    SoLuongHoaDon = g.Count(),
                    TongTien = g.Sum(h => h.TongTien ?? 0),
                    TienBHYT = g.Sum(h => h.TienBhyt ?? 0),
                    TienBNTra = g.Sum(h => h.TienBntra ?? 0)
                })
                .OrderBy(x => x.Ngay)
                .ToListAsync();

            return thongKe;
        }

        // GET: api/HoaDon/ThongKe/TheoLoai
        [HttpGet("ThongKe/TheoLoai")]
        public async Task<ActionResult<object>> GetThongKeTheoLoai()
        {
            var thongKe = await _context.HoaDons
                .GroupBy(h => h.LoaiHoaDon)
                .Select(g => new
                {
                    LoaiHoaDon = g.Key,
                    SoLuongHoaDon = g.Count(),
                    TongTien = g.Sum(h => h.TongTien ?? 0),
                    TienBHYT = g.Sum(h => h.TienBhyt ?? 0),
                    TienBNTra = g.Sum(h => h.TienBntra ?? 0)
                })
                .ToListAsync();

            return thongKe;
        }

        private bool HoaDonExists(string id)
        {
            return _context.HoaDons.Any(e => e.MaHoaDon == id);
        }
    }

    // DTO cho request thanh toán
    public class ThanhToanRequest
    {
        public string HinhThucThanhToan { get; set; } = null!;
        public string NguoiThu { get; set; } = null!;
    }
}