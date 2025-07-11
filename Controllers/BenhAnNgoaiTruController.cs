using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BenhAnNgoaiTruController : ControllerBase
    {
        private readonly DBCHIS _context;

        public BenhAnNgoaiTruController(DBCHIS context)
        {
            _context = context;
        }

        // GET: api/BenhAnNgoaiTru
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BenhAnNgoaiTru>>> GetBenhAnNgoaiTru()
        {
            return await _context.BenhAnNgoaiTrus
                .Include(b => b.MaBacSiNavigation)
                .Include(b => b.MaBnNavigation)
                .Include(b => b.MaDangKyNavigation)
                .ToListAsync();
        }

        // GET: api/BenhAnNgoaiTru/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BenhAnNgoaiTru>> GetBenhAnNgoaiTru(string id)
        {
            var benhAnNgoaiTru = await _context.BenhAnNgoaiTrus
                .Include(b => b.MaBacSiNavigation)
                .Include(b => b.MaBnNavigation)
                .Include(b => b.MaDangKyNavigation)
                .FirstOrDefaultAsync(b => b.MaBa == id);

            if (benhAnNgoaiTru == null)
            {
                return NotFound();
            }

            return benhAnNgoaiTru;
        }

        // GET: api/BenhAnNgoaiTru/benhnhan/{maBn}
        [HttpGet("benhnhan/{maBn}")]
        public async Task<ActionResult<IEnumerable<BenhAnNgoaiTru>>> GetBenhAnByBenhNhan(string maBn)
        {
            var benhAnList = await _context.BenhAnNgoaiTrus
                .Include(b => b.MaBacSiNavigation)
                .Include(b => b.MaBnNavigation)
                .Include(b => b.MaDangKyNavigation)
                .Where(b => b.MaBn == maBn)
                .OrderByDescending(b => b.NgayKham)
                .ToListAsync();

            return benhAnList;
        }

        // GET: api/BenhAnNgoaiTru/ngaykham/{ngay}
        [HttpGet("ngaykham/{ngay}")]
        public async Task<ActionResult<IEnumerable<BenhAnNgoaiTru>>> GetBenhAnByNgayKham(DateTime ngay)
        {
            var benhAnList = await _context.BenhAnNgoaiTrus
                .Include(b => b.MaBacSiNavigation)
                .Include(b => b.MaBnNavigation)
                .Include(b => b.MaDangKyNavigation)
                .Where(b => b.NgayKham.Date == ngay.Date)
                .OrderBy(b => b.NgayKham)
                .ToListAsync();

            return benhAnList;
        }

        // GET: api/BenhAnNgoaiTru/bacsi/{maBacSi}
        [HttpGet("bacsi/{maBacSi}")]
        public async Task<ActionResult<IEnumerable<BenhAnNgoaiTru>>> GetBenhAnByBacSi(string maBacSi)
        {
            var benhAnList = await _context.BenhAnNgoaiTrus
                .Include(b => b.MaBacSiNavigation)
                .Include(b => b.MaBnNavigation)
                .Include(b => b.MaDangKyNavigation)
                .Where(b => b.MaBacSi == maBacSi)
                .OrderByDescending(b => b.NgayKham)
                .ToListAsync();

            return benhAnList;
        }

        // POST: api/BenhAnNgoaiTru
        [HttpPost]
        public async Task<ActionResult<BenhAnNgoaiTru>> PostBenhAnNgoaiTru(BenhAnNgoaiTru benhAnNgoaiTru)
        {
            // Kiểm tra dữ liệu đầu vào
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Kiểm tra xem MaBa đã tồn tại chưa
            if (await _context.BenhAnNgoaiTrus.AnyAsync(b => b.MaBa == benhAnNgoaiTru.MaBa))
            {
                return Conflict("Mã bệnh án đã tồn tại");
            }

            // Kiểm tra sự tồn tại của các khóa ngoại
            if (!await _context.BenhNhans.AnyAsync(b => b.MaBn == benhAnNgoaiTru.MaBn))
            {
                return BadRequest("Mã bệnh nhân không tồn tại");
            }

            if (!await _context.DangKyKhams.AnyAsync(d => d.MaDangKy == benhAnNgoaiTru.MaDangKy))
            {
                return BadRequest("Mã đăng ký không tồn tại");
            }

            if (!string.IsNullOrEmpty(benhAnNgoaiTru.MaBacSi) &&
                !await _context.DmNhanViens.AnyAsync(n => n.MaNv == benhAnNgoaiTru.MaBacSi))
            {
                return BadRequest("Mã bác sĩ không tồn tại");
            }

            benhAnNgoaiTru.NgayTao = DateTime.Now;
            _context.BenhAnNgoaiTrus.Add(benhAnNgoaiTru);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBenhAnNgoaiTru), new { id = benhAnNgoaiTru.MaBa }, benhAnNgoaiTru);
        }

        // PUT: api/BenhAnNgoaiTru/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBenhAnNgoaiTru(string id, BenhAnNgoaiTru benhAnNgoaiTru)
        {
            if (id != benhAnNgoaiTru.MaBa)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Kiểm tra sự tồn tại của bệnh án
            if (!await _context.BenhAnNgoaiTrus.AnyAsync(b => b.MaBa == id))
            {
                return NotFound();
            }

            // Kiểm tra sự tồn tại của các khóa ngoại
            if (!await _context.BenhNhans.AnyAsync(b => b.MaBn == benhAnNgoaiTru.MaBn))
            {
                return BadRequest("Mã bệnh nhân không tồn tại");
            }

            if (!await _context.DangKyKhams.AnyAsync(d => d.MaDangKy == benhAnNgoaiTru.MaDangKy))
            {
                return BadRequest("Mã đăng ký không tồn tại");
            }

            if (!string.IsNullOrEmpty(benhAnNgoaiTru.MaBacSi) &&
                !await _context.DmNhanViens.AnyAsync(n => n.MaNv == benhAnNgoaiTru.MaBacSi))
            {
                return BadRequest("Mã bác sĩ không tồn tại");
            }

            _context.Entry(benhAnNgoaiTru).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await BenhAnNgoaiTruExists(id))
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

        // DELETE: api/BenhAnNgoaiTru/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBenhAnNgoaiTru(string id)
        {
            var benhAnNgoaiTru = await _context.BenhAnNgoaiTrus.FindAsync(id);
            if (benhAnNgoaiTru == null)
            {
                return NotFound();
            }

            _context.BenhAnNgoaiTrus.Remove(benhAnNgoaiTru);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/BenhAnNgoaiTru/thongke/theongay
        [HttpGet("thongke/theongay")]
        public async Task<ActionResult> GetThongKeTheoNgay([FromQuery] DateTime tuNgay, [FromQuery] DateTime denNgay)
        {
            var thongKe = await _context.BenhAnNgoaiTrus
                .Where(b => b.NgayKham.Date >= tuNgay.Date && b.NgayKham.Date <= denNgay.Date)
                .GroupBy(b => b.NgayKham.Date)
                .Select(g => new
                {
                    NgayKham = g.Key,
                    SoLuongKham = g.Count()
                })
                .OrderBy(x => x.NgayKham)
                .ToListAsync();

            return Ok(thongKe);
        }

        // GET: api/BenhAnNgoaiTru/thongke/theobacsi
        [HttpGet("thongke/theobacsi")]
        public async Task<ActionResult> GetThongKeTheoBacSi([FromQuery] DateTime tuNgay, [FromQuery] DateTime denNgay)
        {
            var thongKe = await _context.BenhAnNgoaiTrus
                .Include(b => b.MaBacSiNavigation)
                .Where(b => b.NgayKham.Date >= tuNgay.Date && b.NgayKham.Date <= denNgay.Date)
                .GroupBy(b => new { b.MaBacSi, b.MaBacSiNavigation.HoTen })
                .Select(g => new
                {
                    MaBacSi = g.Key.MaBacSi,
                    TenBacSi = g.Key.HoTen,
                    SoLuongKham = g.Count()
                })
                .OrderByDescending(x => x.SoLuongKham)
                .ToListAsync();

            return Ok(thongKe);
        }

        private async Task<bool> BenhAnNgoaiTruExists(string id)
        {
            return await _context.BenhAnNgoaiTrus.AnyAsync(e => e.MaBa == id);
        }
    }
}