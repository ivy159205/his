using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChiDinhXetNghiemController : ControllerBase
    {
        private readonly DBCHIS _context;

        public ChiDinhXetNghiemController(DBCHIS context)
        {
            _context = context;
        }

        // GET: api/ChiDinhXetNghiem
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChiDinhXetNghiem>>> GetChiDinhXetNghiems()
        {
            return await _context.ChiDinhXetNghiems
                .Include(c => c.MaBnNavigation)
                .Include(c => c.MaBacSiNavigation)
                .Include(c => c.MaDichVuNavigation)
                .ToListAsync();
        }

        // GET: api/ChiDinhXetNghiem/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ChiDinhXetNghiem>> GetChiDinhXetNghiem(string id)
        {
            var chiDinhXetNghiem = await _context.ChiDinhXetNghiems
                .Include(c => c.MaBnNavigation)
                .Include(c => c.MaBacSiNavigation)
                .Include(c => c.MaDichVuNavigation)
                .FirstOrDefaultAsync(c => c.MaChiDinh == id);

            if (chiDinhXetNghiem == null)
            {
                return NotFound();
            }

            return chiDinhXetNghiem;
        }

        // PUT: api/ChiDinhXetNghiem/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChiDinhXetNghiem(string id, ChiDinhXetNghiem chiDinhXetNghiem)
        {
            if (id != chiDinhXetNghiem.MaChiDinh)
            {
                return BadRequest();
            }

            _context.Entry(chiDinhXetNghiem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChiDinhXetNghiemExists(id))
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

        // POST: api/ChiDinhXetNghiem
        [HttpPost]
        public async Task<ActionResult<ChiDinhXetNghiem>> PostChiDinhXetNghiem(ChiDinhXetNghiem chiDinhXetNghiem)
        {
            // Thiết lập ngày tạo và ngày chỉ định
            chiDinhXetNghiem.NgayTao = DateTime.Now;

            if (chiDinhXetNghiem.NgayChiDinh == default(DateTime))
            {
                chiDinhXetNghiem.NgayChiDinh = DateTime.Now;
            }

            // Thiết lập trạng thái mặc định
            if (string.IsNullOrEmpty(chiDinhXetNghiem.TrangThai))
            {
                chiDinhXetNghiem.TrangThai = "CHO_THUC_HIEN";
            }

            _context.ChiDinhXetNghiems.Add(chiDinhXetNghiem);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ChiDinhXetNghiemExists(chiDinhXetNghiem.MaChiDinh))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetChiDinhXetNghiem", new { id = chiDinhXetNghiem.MaChiDinh }, chiDinhXetNghiem);
        }

        // DELETE: api/ChiDinhXetNghiem/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChiDinhXetNghiem(string id)
        {
            var chiDinhXetNghiem = await _context.ChiDinhXetNghiems.FindAsync(id);
            if (chiDinhXetNghiem == null)
            {
                return NotFound();
            }

            _context.ChiDinhXetNghiems.Remove(chiDinhXetNghiem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/ChiDinhXetNghiem/patient/5
        [HttpGet("patient/{maBn}")]
        public async Task<ActionResult<IEnumerable<ChiDinhXetNghiem>>> GetChiDinhByPatient(string maBn)
        {
            return await _context.ChiDinhXetNghiems
                .Where(c => c.MaBn == maBn)
                .Include(c => c.MaBnNavigation)
                .Include(c => c.MaBacSiNavigation)
                .Include(c => c.MaDichVuNavigation)
                .OrderByDescending(c => c.NgayChiDinh)
                .ToListAsync();
        }

        // GET: api/ChiDinhXetNghiem/doctor/5
        [HttpGet("doctor/{maBacSi}")]
        public async Task<ActionResult<IEnumerable<ChiDinhXetNghiem>>> GetChiDinhByDoctor(string maBacSi)
        {
            return await _context.ChiDinhXetNghiems
                .Where(c => c.MaBacSi == maBacSi)
                .Include(c => c.MaBnNavigation)
                .Include(c => c.MaBacSiNavigation)
                .Include(c => c.MaDichVuNavigation)
                .OrderByDescending(c => c.NgayChiDinh)
                .ToListAsync();
        }

        // GET: api/ChiDinhXetNghiem/service/5
        [HttpGet("service/{maDichVu}")]
        public async Task<ActionResult<IEnumerable<ChiDinhXetNghiem>>> GetChiDinhByService(string maDichVu)
        {
            return await _context.ChiDinhXetNghiems
                .Where(c => c.MaDichVu == maDichVu)
                .Include(c => c.MaBnNavigation)
                .Include(c => c.MaBacSiNavigation)
                .Include(c => c.MaDichVuNavigation)
                .OrderByDescending(c => c.NgayChiDinh)
                .ToListAsync();
        }

        // GET: api/ChiDinhXetNghiem/status/CHO_THUC_HIEN
        [HttpGet("status/{trangThai}")]
        public async Task<ActionResult<IEnumerable<ChiDinhXetNghiem>>> GetChiDinhByStatus(string trangThai)
        {
            return await _context.ChiDinhXetNghiems
                .Where(c => c.TrangThai == trangThai)
                .Include(c => c.MaBnNavigation)
                .Include(c => c.MaBacSiNavigation)
                .Include(c => c.MaDichVuNavigation)
                .OrderByDescending(c => c.NgayChiDinh)
                .ToListAsync();
        }

        // GET: api/ChiDinhXetNghiem/search
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ChiDinhXetNghiem>>> SearchChiDinhXetNghiem(
            [FromQuery] string? maBn = null,
            [FromQuery] string? maBacSi = null,
            [FromQuery] string? maDichVu = null,
            [FromQuery] string? trangThai = null,
            [FromQuery] DateTime? tuNgay = null,
            [FromQuery] DateTime? denNgay = null)
        {
            var query = _context.ChiDinhXetNghiems
                .Include(c => c.MaBnNavigation)
                .Include(c => c.MaBacSiNavigation)
                .Include(c => c.MaDichVuNavigation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(maBn))
            {
                query = query.Where(c => c.MaBn == maBn);
            }

            if (!string.IsNullOrEmpty(maBacSi))
            {
                query = query.Where(c => c.MaBacSi == maBacSi);
            }

            if (!string.IsNullOrEmpty(maDichVu))
            {
                query = query.Where(c => c.MaDichVu == maDichVu);
            }

            if (!string.IsNullOrEmpty(trangThai))
            {
                query = query.Where(c => c.TrangThai == trangThai);
            }

            if (tuNgay.HasValue)
            {
                query = query.Where(c => c.NgayChiDinh >= tuNgay.Value);
            }

            if (denNgay.HasValue)
            {
                query = query.Where(c => c.NgayChiDinh <= denNgay.Value);
            }

            return await query.OrderByDescending(c => c.NgayChiDinh).ToListAsync();
        }

        // PUT: api/ChiDinhXetNghiem/5/status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateChiDinhStatus(string id, [FromBody] string trangThai)
        {
            var chiDinhXetNghiem = await _context.ChiDinhXetNghiems.FindAsync(id);
            if (chiDinhXetNghiem == null)
            {
                return NotFound();
            }

            chiDinhXetNghiem.TrangThai = trangThai;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/ChiDinhXetNghiem/5/results
        [HttpGet("{id}/results")]
        public async Task<ActionResult<ChiDinhXetNghiem>> GetChiDinhWithResults(string id)
        {
            var chiDinhXetNghiem = await _context.ChiDinhXetNghiems
                .Include(c => c.MaBnNavigation)
                .Include(c => c.MaBacSiNavigation)
                .Include(c => c.MaDichVuNavigation)
                .Include(c => c.KetQuaXetNghiems)
                .FirstOrDefaultAsync(c => c.MaChiDinh == id);

            if (chiDinhXetNghiem == null)
            {
                return NotFound();
            }

            return chiDinhXetNghiem;
        }

        // GET: api/ChiDinhXetNghiem/today
        [HttpGet("today")]
        public async Task<ActionResult<IEnumerable<ChiDinhXetNghiem>>> GetChiDinhToday()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            return await _context.ChiDinhXetNghiems
                .Where(c => c.NgayChiDinh >= today && c.NgayChiDinh < tomorrow)
                .Include(c => c.MaBnNavigation)
                .Include(c => c.MaBacSiNavigation)
                .Include(c => c.MaDichVuNavigation)
                .OrderByDescending(c => c.NgayChiDinh)
                .ToListAsync();
        }

        // GET: api/ChiDinhXetNghiem/count
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetChiDinhCount()
        {
            return await _context.ChiDinhXetNghiems.CountAsync();
        }

        // GET: api/ChiDinhXetNghiem/statistics
        [HttpGet("statistics")]
        public async Task<ActionResult<object>> GetChiDinhStatistics()
        {
            var statistics = await _context.ChiDinhXetNghiems
                .GroupBy(c => c.TrangThai)
                .Select(g => new
                {
                    TrangThai = g.Key,
                    SoLuong = g.Count()
                })
                .ToListAsync();

            return Ok(statistics);
        }

        private bool ChiDinhXetNghiemExists(string id)
        {
            return _context.ChiDinhXetNghiems.Any(e => e.MaChiDinh == id);
        }
    }
}