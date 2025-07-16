using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DonThuocController : ControllerBase
    {
        private readonly DBCHIS _context;

        public DonThuocController(DBCHIS context)
        {
            _context = context;
        }

        // GET: api/DonThuoc
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DonThuoc>>> GetDonThuocs()
        {
            return await _context.DonThuocs
                .Include(d => d.MaBnNavigation)
                .Include(d => d.MaBacSiNavigation)
                .ToListAsync();
        }

        // GET: api/DonThuoc/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DonThuoc>> GetDonThuoc(string id)
        {
            var donThuoc = await _context.DonThuocs
                .Include(d => d.MaBnNavigation)
                .Include(d => d.MaBacSiNavigation)
                .FirstOrDefaultAsync(d => d.MaDonThuoc == id);

            if (donThuoc == null)
            {
                return NotFound();
            }

            return donThuoc;
        }

        // GET: api/DonThuoc/5/details
        [HttpGet("{id}/details")]
        public async Task<ActionResult<DonThuoc>> GetDonThuocWithDetails(string id)
        {
            var donThuoc = await _context.DonThuocs
                .Include(d => d.MaBnNavigation)
                .Include(d => d.MaBacSiNavigation)
                .Include(d => d.ChiTietDonThuocs)
                    .ThenInclude(ct => ct.MaThuocNavigation)
                .FirstOrDefaultAsync(d => d.MaDonThuoc == id);

            if (donThuoc == null)
            {
                return NotFound();
            }

            return donThuoc;
        }

        // GET: api/DonThuoc/benh-nhan/BN001
        [HttpGet("benh-nhan/{maBn}")]
        public async Task<ActionResult<IEnumerable<DonThuoc>>> GetDonThuocByBenhNhan(string maBn)
        {
            return await _context.DonThuocs
                .Include(d => d.MaBnNavigation)
                .Include(d => d.MaBacSiNavigation)
                .Where(d => d.MaBn == maBn)
                .OrderByDescending(d => d.NgayKeDon)
                .ToListAsync();
        }

        // GET: api/DonThuoc/bac-si/BS001
        [HttpGet("bac-si/{maBacSi}")]
        public async Task<ActionResult<IEnumerable<DonThuoc>>> GetDonThuocByBacSi(string maBacSi)
        {
            return await _context.DonThuocs
                .Include(d => d.MaBnNavigation)
                .Include(d => d.MaBacSiNavigation)
                .Where(d => d.MaBacSi == maBacSi)
                .OrderByDescending(d => d.NgayKeDon)
                .ToListAsync();
        }

        // GET: api/DonThuoc/by-date?tuNgay=2024-01-01&denNgay=2024-12-31
        [HttpGet("by-date")]
        public async Task<ActionResult<IEnumerable<DonThuoc>>> GetDonThuocByDateRange(DateTime tuNgay, DateTime denNgay)
        {
            return await _context.DonThuocs
                .Include(d => d.MaBnNavigation)
                .Include(d => d.MaBacSiNavigation)
                .Where(d => d.NgayKeDon >= tuNgay && d.NgayKeDon <= denNgay)
                .OrderByDescending(d => d.NgayKeDon)
                .ToListAsync();
        }

        // GET: api/DonThuoc/by-trang-thai/dang-xu-ly
        [HttpGet("by-trang-thai/{trangThai}")]
        public async Task<ActionResult<IEnumerable<DonThuoc>>> GetDonThuocByTrangThai(string trangThai)
        {
            return await _context.DonThuocs
                .Include(d => d.MaBnNavigation)
                .Include(d => d.MaBacSiNavigation)
                .Where(d => d.TrangThai == trangThai)
                .OrderByDescending(d => d.NgayKeDon)
                .ToListAsync();
        }

        // GET: api/DonThuoc/by-loai-don/noi-tru
        [HttpGet("by-loai-don/{loaiDon}")]
        public async Task<ActionResult<IEnumerable<DonThuoc>>> GetDonThuocByLoaiDon(string loaiDon)
        {
            return await _context.DonThuocs
                .Include(d => d.MaBnNavigation)
                .Include(d => d.MaBacSiNavigation)
                .Where(d => d.LoaiDon == loaiDon)
                .OrderByDescending(d => d.NgayKeDon)
                .ToListAsync();
        }

        // GET: api/DonThuoc/search?keyword=...
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<DonThuoc>>> SearchDonThuocs(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return await _context.DonThuocs
                    .Include(d => d.MaBnNavigation)
                    .Include(d => d.MaBacSiNavigation)
                    .ToListAsync();
            }

            return await _context.DonThuocs
                .Include(d => d.MaBnNavigation)
                .Include(d => d.MaBacSiNavigation)
                .Where(d => d.MaDonThuoc.Contains(keyword) ||
                           d.MaBn.Contains(keyword) ||
                           (d.ChanDoan != null && d.ChanDoan.Contains(keyword)) ||
                           (d.MaBnNavigation != null && d.MaBnNavigation.HoTen.Contains(keyword)))
                .OrderByDescending(d => d.NgayKeDon)
                .ToListAsync();
        }

        // PUT: api/DonThuoc/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDonThuoc(string id, DonThuoc donThuoc)
        {
            if (id != donThuoc.MaDonThuoc)
            {
                return BadRequest();
            }

            _context.Entry(donThuoc).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DonThuocExists(id))
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

        // POST: api/DonThuoc
        [HttpPost]
        public async Task<ActionResult<DonThuoc>> PostDonThuoc(DonThuoc donThuoc)
        {
            // Tự động set ngày tạo
            donThuoc.NgayTao = DateTime.Now;

            // Mặc định trạng thái nếu không được set
            if (string.IsNullOrEmpty(donThuoc.TrangThai))
            {
                donThuoc.TrangThai = "Chờ xử lý";
            }

            // Mặc định tổng tiền = 0 nếu không được set
            if (donThuoc.TongTien == null)
            {
                donThuoc.TongTien = 0;
            }

            _context.DonThuocs.Add(donThuoc);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DonThuocExists(donThuoc.MaDonThuoc))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetDonThuoc", new { id = donThuoc.MaDonThuoc }, donThuoc);
        }

        // DELETE: api/DonThuoc/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDonThuoc(string id)
        {
            var donThuoc = await _context.DonThuocs.FindAsync(id);
            if (donThuoc == null)
            {
                return NotFound();
            }

            // Kiểm tra xem đơn thuốc có chi tiết không
            var hasChiTiet = await _context.ChiTietDonThuocs.AnyAsync(ct => ct.MaDonThuoc == id);
            if (hasChiTiet)
            {
                return BadRequest("Không thể xóa đơn thuốc này vì đã có chi tiết thuốc");
            }

            _context.DonThuocs.Remove(donThuoc);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/DonThuoc/5/cap-nhat-trang-thai
        [HttpPut("{id}/cap-nhat-trang-thai")]
        public async Task<IActionResult> CapNhatTrangThai(string id, [FromBody] string trangThaiMoi)
        {
            var donThuoc = await _context.DonThuocs.FindAsync(id);
            if (donThuoc == null)
            {
                return NotFound();
            }

            donThuoc.TrangThai = trangThaiMoi;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đã cập nhật trạng thái", trangThai = trangThaiMoi });
        }

        // PUT: api/DonThuoc/5/cap-nhat-tong-tien
        [HttpPut("{id}/cap-nhat-tong-tien")]
        public async Task<IActionResult> CapNhatTongTien(string id)
        {
            var donThuoc = await _context.DonThuocs
                .Include(d => d.ChiTietDonThuocs)
                .FirstOrDefaultAsync(d => d.MaDonThuoc == id);

            if (donThuoc == null)
            {
                return NotFound();
            }

            // Tính lại tổng tiền từ chi tiết
            donThuoc.TongTien = donThuoc.ChiTietDonThuocs.Sum(ct => ct.SoLuong * ct.DonGia);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đã cập nhật tổng tiền", tongTien = donThuoc.TongTien });
        }

        // GET: api/DonThuoc/statistics
        [HttpGet("statistics")]
        public async Task<ActionResult<object>> GetStatistics()
        {
            var today = DateTime.Today;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            var startOfYear = new DateTime(today.Year, 1, 1);

            var totalToday = await _context.DonThuocs.CountAsync(d => d.NgayKeDon.Date == today);
            var totalMonth = await _context.DonThuocs.CountAsync(d => d.NgayKeDon >= startOfMonth);
            var totalYear = await _context.DonThuocs.CountAsync(d => d.NgayKeDon >= startOfYear);

            var byTrangThai = await _context.DonThuocs
                .GroupBy(d => d.TrangThai)
                .Select(g => new { TrangThai = g.Key, SoLuong = g.Count() })
                .ToListAsync();

            var byLoaiDon = await _context.DonThuocs
                .GroupBy(d => d.LoaiDon)
                .Select(g => new { LoaiDon = g.Key, SoLuong = g.Count() })
                .ToListAsync();

            var tongTienThang = await _context.DonThuocs
                .Where(d => d.NgayKeDon >= startOfMonth)
                .SumAsync(d => d.TongTien ?? 0);

            return Ok(new
            {
                DonThuocHomNay = totalToday,
                DonThuocThang = totalMonth,
                DonThuocNam = totalYear,
                TheoTrangThai = byTrangThai,
                TheoLoaiDon = byLoaiDon,
                TongTienThang = tongTienThang
            });
        }

        // GET: api/DonThuoc/bao-cao-theo-thang?nam=2024&thang=12
        [HttpGet("bao-cao-theo-thang")]
        public async Task<ActionResult<object>> GetBaoCaoTheoThang(int nam, int thang)
        {
            var startDate = new DateTime(nam, thang, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var donThuocs = await _context.DonThuocs
                .Where(d => d.NgayKeDon >= startDate && d.NgayKeDon <= endDate)
                .ToListAsync();

            var baoCao = donThuocs
                .GroupBy(d => d.NgayKeDon.Date)
                .Select(g => new
                {
                    Ngay = g.Key,
                    SoLuongDon = g.Count(),
                    TongTien = g.Sum(d => d.TongTien ?? 0)
                })
                .OrderBy(x => x.Ngay)
                .ToList();

            return Ok(new
            {
                Thang = thang,
                Nam = nam,
                TongDon = donThuocs.Count,
                TongTien = donThuocs.Sum(d => d.TongTien ?? 0),
                ChiTietTheoNgay = baoCao
            });
        }

        private bool DonThuocExists(string id)
        {
            return _context.DonThuocs.Any(e => e.MaDonThuoc == id);
        }
    }
}