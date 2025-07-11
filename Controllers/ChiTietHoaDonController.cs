using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChiTietHoaDonController : ControllerBase
    {
        private readonly DBCHIS _context;

        public ChiTietHoaDonController(DBCHIS context)
        {
            _context = context;
        }

        // GET: api/ChiTietHoaDon
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChiTietHoaDon>>> GetChiTietHoaDons()
        {
            return await _context.ChiTietHoaDons
                .Include(c => c.MaHoaDonNavigation)
                .Include(c => c.MaDichVuNavigation)
                .ToListAsync();
        }

        // GET: api/ChiTietHoaDon/HD001/DV001
        [HttpGet("{maHoaDon}/{maDichVu}")]
        public async Task<ActionResult<ChiTietHoaDon>> GetChiTietHoaDon(string maHoaDon, string maDichVu)
        {
            var chiTietHoaDon = await _context.ChiTietHoaDons
                .Include(c => c.MaHoaDonNavigation)
                .Include(c => c.MaDichVuNavigation)
                .FirstOrDefaultAsync(c => c.MaHoaDon == maHoaDon && c.MaDichVu == maDichVu);

            if (chiTietHoaDon == null)
            {
                return NotFound();
            }

            return chiTietHoaDon;
        }

        // GET: api/ChiTietHoaDon/hoadon/HD001
        [HttpGet("hoadon/{maHoaDon}")]
        public async Task<ActionResult<IEnumerable<ChiTietHoaDon>>> GetChiTietHoaDonsByMaHoaDon(string maHoaDon)
        {
            var chiTietHoaDons = await _context.ChiTietHoaDons
                .Include(c => c.MaDichVuNavigation)
                .Where(c => c.MaHoaDon == maHoaDon)
                .ToListAsync();

            return chiTietHoaDons;
        }

        // PUT: api/ChiTietHoaDon/HD001/DV001
        [HttpPut("{maHoaDon}/{maDichVu}")]
        public async Task<IActionResult> PutChiTietHoaDon(string maHoaDon, string maDichVu, ChiTietHoaDon chiTietHoaDon)
        {
            if (maHoaDon != chiTietHoaDon.MaHoaDon || maDichVu != chiTietHoaDon.MaDichVu)
            {
                return BadRequest();
            }

            _context.Entry(chiTietHoaDon).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChiTietHoaDonExists(maHoaDon, maDichVu))
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

        // POST: api/ChiTietHoaDon
        [HttpPost]
        public async Task<ActionResult<ChiTietHoaDon>> PostChiTietHoaDon(ChiTietHoaDon chiTietHoaDon)
        {
            // Kiểm tra xem hóa đơn có tồn tại không
            var hoaDonExists = await _context.HoaDons.AnyAsync(h => h.MaHoaDon == chiTietHoaDon.MaHoaDon);
            if (!hoaDonExists)
            {
                return BadRequest("Hóa đơn không tồn tại");
            }

            // Kiểm tra xem dịch vụ có tồn tại không
            var dichVuExists = await _context.DmDichVus.AnyAsync(d => d.MaDichVu == chiTietHoaDon.MaDichVu);
            if (!dichVuExists)
            {
                return BadRequest("Dịch vụ không tồn tại");
            }

            // Kiểm tra xem chi tiết hóa đơn đã tồn tại chưa
            if (ChiTietHoaDonExists(chiTietHoaDon.MaHoaDon, chiTietHoaDon.MaDichVu))
            {
                return BadRequest("Chi tiết hóa đơn đã tồn tại");
            }

            // Tính toán thành tiền nếu chưa có
            if (chiTietHoaDon.ThanhTien == null && chiTietHoaDon.SoLuong.HasValue && chiTietHoaDon.DonGia.HasValue)
            {
                chiTietHoaDon.ThanhTien = chiTietHoaDon.SoLuong * chiTietHoaDon.DonGia;
            }

            // Tính toán tiền bệnh nhân trả
            if (chiTietHoaDon.TienBntra == null && chiTietHoaDon.ThanhTien.HasValue)
            {
                chiTietHoaDon.TienBntra = chiTietHoaDon.ThanhTien - (chiTietHoaDon.TienBhyt ?? 0);
            }

            _context.ChiTietHoaDons.Add(chiTietHoaDon);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ChiTietHoaDonExists(chiTietHoaDon.MaHoaDon, chiTietHoaDon.MaDichVu))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetChiTietHoaDon",
                new { maHoaDon = chiTietHoaDon.MaHoaDon, maDichVu = chiTietHoaDon.MaDichVu },
                chiTietHoaDon);
        }

        // DELETE: api/ChiTietHoaDon/HD001/DV001
        [HttpDelete("{maHoaDon}/{maDichVu}")]
        public async Task<IActionResult> DeleteChiTietHoaDon(string maHoaDon, string maDichVu)
        {
            var chiTietHoaDon = await _context.ChiTietHoaDons
                .FirstOrDefaultAsync(c => c.MaHoaDon == maHoaDon && c.MaDichVu == maDichVu);

            if (chiTietHoaDon == null)
            {
                return NotFound();
            }

            _context.ChiTietHoaDons.Remove(chiTietHoaDon);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/ChiTietHoaDon/tongtienhoadon/HD001
        [HttpGet("tongtienhoadon/{maHoaDon}")]
        public async Task<ActionResult<decimal>> GetTongTienHoaDon(string maHoaDon)
        {
            var tongTien = await _context.ChiTietHoaDons
                .Where(c => c.MaHoaDon == maHoaDon)
                .SumAsync(c => c.ThanhTien ?? 0);

            return tongTien;
        }

        // GET: api/ChiTietHoaDon/tongtienbhyt/HD001
        [HttpGet("tongtienbhyt/{maHoaDon}")]
        public async Task<ActionResult<decimal>> GetTongTienBHYT(string maHoaDon)
        {
            var tongTienBHYT = await _context.ChiTietHoaDons
                .Where(c => c.MaHoaDon == maHoaDon)
                .SumAsync(c => c.TienBhyt ?? 0);

            return tongTienBHYT;
        }

        // GET: api/ChiTietHoaDon/tongtienbenhnantra/HD001
        [HttpGet("tongtienbenhnantra/{maHoaDon}")]
        public async Task<ActionResult<decimal>> GetTongTienBenhNhanTra(string maHoaDon)
        {
            var tongTienBNTra = await _context.ChiTietHoaDons
                .Where(c => c.MaHoaDon == maHoaDon)
                .SumAsync(c => c.TienBntra ?? 0);

            return tongTienBNTra;
        }

        private bool ChiTietHoaDonExists(string maHoaDon, string maDichVu)
        {
            return _context.ChiTietHoaDons.Any(c => c.MaHoaDon == maHoaDon && c.MaDichVu == maDichVu);
        }
    }
}