using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BenhNhanController : ControllerBase
    {
        private readonly DBCHIS _context;

        public BenhNhanController(DBCHIS context)
        {
            _context = context;
        }

        // GET: api/BenhNhan
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BenhNhan>>> GetBenhNhans()
        {
            return await _context.BenhNhans
                .Include(b => b.MaDoiTuongBhytNavigation)
                .ToListAsync();
        }

        // GET: api/BenhNhan/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BenhNhan>> GetBenhNhan(string id)
        {
            var benhNhan = await _context.BenhNhans
                .Include(b => b.MaDoiTuongBhytNavigation)
                .FirstOrDefaultAsync(b => b.MaBn == id);

            if (benhNhan == null)
            {
                return NotFound();
            }

            return benhNhan;
        }

        // PUT: api/BenhNhan/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBenhNhan(string id, BenhNhan benhNhan)
        {
            if (id != benhNhan.MaBn)
            {
                return BadRequest();
            }

            _context.Entry(benhNhan).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BenhNhanExists(id))
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

        // POST: api/BenhNhan
        [HttpPost]
        public async Task<ActionResult<BenhNhan>> PostBenhNhan(BenhNhan benhNhan)
        {
            // Thiết lập ngày tạo
            benhNhan.NgayTao = DateTime.Now;

            // Thiết lập trạng thái mặc định
            if (benhNhan.TrangThai == null)
            {
                benhNhan.TrangThai = true;
            }

            _context.BenhNhans.Add(benhNhan);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BenhNhanExists(benhNhan.MaBn))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetBenhNhan", new { id = benhNhan.MaBn }, benhNhan);
        }

        // DELETE: api/BenhNhan/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBenhNhan(string id)
        {
            var benhNhan = await _context.BenhNhans.FindAsync(id);
            if (benhNhan == null)
            {
                return NotFound();
            }

            _context.BenhNhans.Remove(benhNhan);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/BenhNhan/search
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<BenhNhan>>> SearchBenhNhan(
            [FromQuery] string? hoTen = null,
            [FromQuery] string? cccd = null,
            [FromQuery] string? sdt = null,
            [FromQuery] string? soTheBhyt = null)
        {
            var query = _context.BenhNhans
                .Include(b => b.MaDoiTuongBhytNavigation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(hoTen))
            {
                query = query.Where(b => b.HoTen.Contains(hoTen));
            }

            if (!string.IsNullOrEmpty(cccd))
            {
                query = query.Where(b => b.Cccd == cccd);
            }

            if (!string.IsNullOrEmpty(sdt))
            {
                query = query.Where(b => b.Sdt == sdt);
            }

            if (!string.IsNullOrEmpty(soTheBhyt))
            {
                query = query.Where(b => b.SoTheBhyt == soTheBhyt);
            }

            return await query.ToListAsync();
        }

        // GET: api/BenhNhan/active
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<BenhNhan>>> GetActiveBenhNhans()
        {
            return await _context.BenhNhans
                .Where(b => b.TrangThai == true)
                .Include(b => b.MaDoiTuongBhytNavigation)
                .ToListAsync();
        }

        // PUT: api/BenhNhan/5/status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateBenhNhanStatus(string id, [FromBody] bool trangThai)
        {
            var benhNhan = await _context.BenhNhans.FindAsync(id);
            if (benhNhan == null)
            {
                return NotFound();
            }

            benhNhan.TrangThai = trangThai;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/BenhNhan/5/full
        [HttpGet("{id}/full")]
        public async Task<ActionResult<BenhNhan>> GetBenhNhanFull(string id)
        {
            var benhNhan = await _context.BenhNhans
                .Include(b => b.MaDoiTuongBhytNavigation)
                .Include(b => b.BenhAnNgoaiTrus)
                .Include(b => b.BenhAnNoiTrus)
                .Include(b => b.DangKyKhams)
                .Include(b => b.HoaDons)
                .FirstOrDefaultAsync(b => b.MaBn == id);

            if (benhNhan == null)
            {
                return NotFound();
            }

            return benhNhan;
        }

        // GET: api/BenhNhan/count
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetBenhNhanCount()
        {
            return await _context.BenhNhans.CountAsync();
        }

        private bool BenhNhanExists(string id)
        {
            return _context.BenhNhans.Any(e => e.MaBn == id);
        }
    }
}