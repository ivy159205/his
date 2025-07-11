using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChiTietDonThuocController : ControllerBase
    {
        private readonly DBCHIS _context;

        public ChiTietDonThuocController(DBCHIS context)
        {
            _context = context;
        }

        // GET: api/ChiTietDonThuoc
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChiTietDonThuoc>>> GetChiTietDonThuocs()
        {
            return await _context.ChiTietDonThuocs
                .Include(c => c.MaDonThuocNavigation)
                .Include(c => c.MaThuocNavigation)
                .ToListAsync();
        }

        // GET: api/ChiTietDonThuoc/5/ABC123
        [HttpGet("{maDonThuoc}/{maThuoc}")]
        public async Task<ActionResult<ChiTietDonThuoc>> GetChiTietDonThuoc(string maDonThuoc, string maThuoc)
        {
            var chiTietDonThuoc = await _context.ChiTietDonThuocs
                .Include(c => c.MaDonThuocNavigation)
                .Include(c => c.MaThuocNavigation)
                .FirstOrDefaultAsync(c => c.MaDonThuoc == maDonThuoc && c.MaThuoc == maThuoc);

            if (chiTietDonThuoc == null)
            {
                return NotFound();
            }

            return chiTietDonThuoc;
        }

        // PUT: api/ChiTietDonThuoc/5/ABC123
        [HttpPut("{maDonThuoc}/{maThuoc}")]
        public async Task<IActionResult> PutChiTietDonThuoc(string maDonThuoc, string maThuoc, ChiTietDonThuoc chiTietDonThuoc)
        {
            if (maDonThuoc != chiTietDonThuoc.MaDonThuoc || maThuoc != chiTietDonThuoc.MaThuoc)
            {
                return BadRequest();
            }

            // Tự động tính thành tiền
            if (chiTietDonThuoc.DonGia.HasValue)
            {
                chiTietDonThuoc.ThanhTien = chiTietDonThuoc.DonGia.Value * chiTietDonThuoc.SoLuong;
            }

            _context.Entry(chiTietDonThuoc).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChiTietDonThuocExists(maDonThuoc, maThuoc))
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

        // POST: api/ChiTietDonThuoc
        [HttpPost]
        public async Task<ActionResult<ChiTietDonThuoc>> PostChiTietDonThuoc(ChiTietDonThuoc chiTietDonThuoc)
        {
            // Tự động tính thành tiền
            if (chiTietDonThuoc.DonGia.HasValue)
            {
                chiTietDonThuoc.ThanhTien = chiTietDonThuoc.DonGia.Value * chiTietDonThuoc.SoLuong;
            }

            _context.ChiTietDonThuocs.Add(chiTietDonThuoc);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ChiTietDonThuocExists(chiTietDonThuoc.MaDonThuoc, chiTietDonThuoc.MaThuoc))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetChiTietDonThuoc",
                new { maDonThuoc = chiTietDonThuoc.MaDonThuoc, maThuoc = chiTietDonThuoc.MaThuoc },
                chiTietDonThuoc);
        }

        // DELETE: api/ChiTietDonThuoc/5/ABC123
        [HttpDelete("{maDonThuoc}/{maThuoc}")]
        public async Task<IActionResult> DeleteChiTietDonThuoc(string maDonThuoc, string maThuoc)
        {
            var chiTietDonThuoc = await _context.ChiTietDonThuocs
                .FirstOrDefaultAsync(c => c.MaDonThuoc == maDonThuoc && c.MaThuoc == maThuoc);

            if (chiTietDonThuoc == null)
            {
                return NotFound();
            }

            _context.ChiTietDonThuocs.Remove(chiTietDonThuoc);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/ChiTietDonThuoc/prescription/5
        [HttpGet("prescription/{maDonThuoc}")]
        public async Task<ActionResult<IEnumerable<ChiTietDonThuoc>>> GetChiTietByPrescription(string maDonThuoc)
        {
            return await _context.ChiTietDonThuocs
                .Where(c => c.MaDonThuoc == maDonThuoc)
                .Include(c => c.MaDonThuocNavigation)
                .Include(c => c.MaThuocNavigation)
                .ToListAsync();
        }

        // GET: api/ChiTietDonThuoc/medicine/ABC123
        [HttpGet("medicine/{maThuoc}")]
        public async Task<ActionResult<IEnumerable<ChiTietDonThuoc>>> GetChiTietByMedicine(string maThuoc)
        {
            return await _context.ChiTietDonThuocs
                .Where(c => c.MaThuoc == maThuoc)
                .Include(c => c.MaDonThuocNavigation)
                .Include(c => c.MaThuocNavigation)
                .OrderByDescending(c => c.MaDonThuocNavigation.NgayKeDon)
                .ToListAsync();
        }

        // POST: api/ChiTietDonThuoc/prescription/5/medicines
        [HttpPost("prescription/{maDonThuoc}/medicines")]
        public async Task<ActionResult<IEnumerable<ChiTietDonThuoc>>> AddMultipleMedicines(
            string maDonThuoc,
            List<ChiTietDonThuoc> chiTietDonThuocs)
        {
            // Kiểm tra đơn thuốc có tồn tại không
            var donThuocExists = await _context.DonThuocs.AnyAsync(d => d.MaDonThuoc == maDonThuoc);
            if (!donThuocExists)
            {
                return NotFound("Đơn thuốc không tồn tại");
            }

            // Thiết lập mã đơn thuốc cho tất cả chi tiết
            foreach (var chiTiet in chiTietDonThuocs)
            {
                chiTiet.MaDonThuoc = maDonThuoc;

                // Tự động tính thành tiền
                if (chiTiet.DonGia.HasValue)
                {
                    chiTiet.ThanhTien = chiTiet.DonGia.Value * chiTiet.SoLuong;
                }
            }

            _context.ChiTietDonThuocs.AddRange(chiTietDonThuocs);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return Conflict("Một hoặc nhiều thuốc đã tồn tại trong đơn thuốc");
            }

            return Ok(chiTietDonThuocs);
        }

        // PUT: api/ChiTietDonThuoc/prescription/5/recalculate
        [HttpPut("prescription/{maDonThuoc}/recalculate")]
        public async Task<IActionResult> RecalculatePrescriptionTotal(string maDonThuoc)
        {
            var chiTietDonThuocs = await _context.ChiTietDonThuocs
                .Where(c => c.MaDonThuoc == maDonThuoc)
                .ToListAsync();

            if (!chiTietDonThuocs.Any())
            {
                return NotFound();
            }

            // Tính lại thành tiền cho từng chi tiết
            foreach (var chiTiet in chiTietDonThuocs)
            {
                if (chiTiet.DonGia.HasValue)
                {
                    chiTiet.ThanhTien = chiTiet.DonGia.Value * chiTiet.SoLuong;
                }
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/ChiTietDonThuoc/prescription/5/total
        [HttpGet("prescription/{maDonThuoc}/total")]
        public async Task<ActionResult<object>> GetPrescriptionTotal(string maDonThuoc)
        {
            var chiTietDonThuocs = await _context.ChiTietDonThuocs
                .Where(c => c.MaDonThuoc == maDonThuoc)
                .ToListAsync();

            if (!chiTietDonThuocs.Any())
            {
                return NotFound();
            }

            var total = chiTietDonThuocs.Sum(c => c.ThanhTien ?? 0);
            var totalQuantity = chiTietDonThuocs.Sum(c => c.SoLuong);

            return Ok(new
            {
                MaDonThuoc = maDonThuoc,
                TongSoLuong = totalQuantity,
                TongTien = total,
                SoLoaiThuoc = chiTietDonThuocs.Count
            });
        }

        // GET: api/ChiTietDonThuoc/search
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ChiTietDonThuoc>>> SearchChiTietDonThuoc(
            [FromQuery] string? maDonThuoc = null,
            [FromQuery] string? maThuoc = null,
            [FromQuery] decimal? donGiaMin = null,
            [FromQuery] decimal? donGiaMax = null,
            [FromQuery] int? soLuongMin = null,
            [FromQuery] int? soLuongMax = null)
        {
            var query = _context.ChiTietDonThuocs
                .Include(c => c.MaDonThuocNavigation)
                .Include(c => c.MaThuocNavigation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(maDonThuoc))
            {
                query = query.Where(c => c.MaDonThuoc == maDonThuoc);
            }

            if (!string.IsNullOrEmpty(maThuoc))
            {
                query = query.Where(c => c.MaThuoc == maThuoc);
            }

            if (donGiaMin.HasValue)
            {
                query = query.Where(c => c.DonGia >= donGiaMin.Value);
            }

            if (donGiaMax.HasValue)
            {
                query = query.Where(c => c.DonGia <= donGiaMax.Value);
            }

            if (soLuongMin.HasValue)
            {
                query = query.Where(c => c.SoLuong >= soLuongMin.Value);
            }

            if (soLuongMax.HasValue)
            {
                query = query.Where(c => c.SoLuong <= soLuongMax.Value);
            }

            return await query.ToListAsync();
        }

        // GET: api/ChiTietDonThuoc/statistics/medicine
        [HttpGet("statistics/medicine")]
        public async Task<ActionResult<IEnumerable<object>>> GetMedicineStatistics()
        {
            var statistics = await _context.ChiTietDonThuocs
                .Include(c => c.MaThuocNavigation)
                .GroupBy(c => new { c.MaThuoc, c.MaThuocNavigation.TenThuoc })
                .Select(g => new
                {
                    MaThuoc = g.Key.MaThuoc,
                    TenThuoc = g.Key.TenThuoc,
                    TongSoLuong = g.Sum(c => c.SoLuong),
                    TongTien = g.Sum(c => c.ThanhTien ?? 0),
                    SoLanKe = g.Count()
                })
                .OrderByDescending(s => s.TongSoLuong)
                .ToListAsync();

            return Ok(statistics);
        }

        // GET: api/ChiTietDonThuoc/count
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetChiTietCount()
        {
            return await _context.ChiTietDonThuocs.CountAsync();
        }

        // DELETE: api/ChiTietDonThuoc/prescription/5
        [HttpDelete("prescription/{maDonThuoc}")]
        public async Task<IActionResult> DeleteAllByPrescription(string maDonThuoc)
        {
            var chiTietDonThuocs = await _context.ChiTietDonThuocs
                .Where(c => c.MaDonThuoc == maDonThuoc)
                .ToListAsync();

            if (!chiTietDonThuocs.Any())
            {
                return NotFound();
            }

            _context.ChiTietDonThuocs.RemoveRange(chiTietDonThuocs);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ChiTietDonThuocExists(string maDonThuoc, string maThuoc)
        {
            return _context.ChiTietDonThuocs.Any(e => e.MaDonThuoc == maDonThuoc && e.MaThuoc == maThuoc);
        }
    }
}