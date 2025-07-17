using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogHoatDongController : ControllerBase
    {
        private readonly DBCHIS _context;

        public LogHoatDongController(DBCHIS context)
        {
            _context = context;
        }

        // GET: api/LogHoatDong
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LogHoatDong>>> GetLogHoatDongs()
        {
            return await _context.LogHoatDongs
                .Include(l => l.MaNvNavigation)
                .ToListAsync();
        }

        // GET: api/LogHoatDong/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LogHoatDong>> GetLogHoatDong(long id)
        {
            var logHoatDong = await _context.LogHoatDongs
                .Include(l => l.MaNvNavigation)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (logHoatDong == null)
            {
                return NotFound();
            }

            return logHoatDong;
        }

        // GET: api/LogHoatDong/ByMaNV/{maNv}
        [HttpGet("ByMaNV/{maNv}")]
        public async Task<ActionResult<IEnumerable<LogHoatDong>>> GetLogHoatDongByMaNV(string maNv)
        {
            var logs = await _context.LogHoatDongs
                .Include(l => l.MaNvNavigation)
                .Where(l => l.MaNv == maNv)
                .OrderByDescending(l => l.NgayGio)
                .ToListAsync();

            return logs;
        }

        // GET: api/LogHoatDong/ByChucNang/{chucNang}
        [HttpGet("ByChucNang/{chucNang}")]
        public async Task<ActionResult<IEnumerable<LogHoatDong>>> GetLogHoatDongByChucNang(string chucNang)
        {
            var logs = await _context.LogHoatDongs
                .Include(l => l.MaNvNavigation)
                .Where(l => l.ChucNang == chucNang)
                .OrderByDescending(l => l.NgayGio)
                .ToListAsync();

            return logs;
        }

        // GET: api/LogHoatDong/ByDateRange
        [HttpGet("ByDateRange")]
        public async Task<ActionResult<IEnumerable<LogHoatDong>>> GetLogHoatDongByDateRange(
            DateTime? fromDate = null,
            DateTime? toDate = null)
        {
            var query = _context.LogHoatDongs
                .Include(l => l.MaNvNavigation)
                .AsQueryable();

            if (fromDate.HasValue)
            {
                query = query.Where(l => l.NgayGio >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(l => l.NgayGio <= toDate.Value);
            }

            var logs = await query
                .OrderByDescending(l => l.NgayGio)
                .ToListAsync();

            return logs;
        }

        // POST: api/LogHoatDong
        [HttpPost]
        public async Task<ActionResult<LogHoatDong>> PostLogHoatDong(LogHoatDong logHoatDong)
        {
            // Tự động set thời gian hiện tại nếu chưa có
            if (logHoatDong.NgayGio == null)
            {
                logHoatDong.NgayGio = DateTime.Now;
            }

            // Tự động lấy IP address từ request
            if (string.IsNullOrEmpty(logHoatDong.Ipaddress))
            {
                logHoatDong.Ipaddress = GetClientIpAddress();
            }

            _context.LogHoatDongs.Add(logHoatDong);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLogHoatDong", new { id = logHoatDong.Id }, logHoatDong);
        }

        // PUT: api/LogHoatDong/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLogHoatDong(long id, LogHoatDong logHoatDong)
        {
            if (id != logHoatDong.Id)
            {
                return BadRequest();
            }

            _context.Entry(logHoatDong).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LogHoatDongExists(id))
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

        // DELETE: api/LogHoatDong/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLogHoatDong(long id)
        {
            var logHoatDong = await _context.LogHoatDongs.FindAsync(id);
            if (logHoatDong == null)
            {
                return NotFound();
            }

            _context.LogHoatDongs.Remove(logHoatDong);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/LogHoatDong/DeleteByDateRange
        [HttpDelete("DeleteByDateRange")]
        public async Task<IActionResult> DeleteLogHoatDongByDateRange(DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = _context.LogHoatDongs.AsQueryable();

            if (fromDate.HasValue)
            {
                query = query.Where(l => l.NgayGio >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(l => l.NgayGio <= toDate.Value);
            }

            var logsToDelete = await query.ToListAsync();

            if (logsToDelete.Any())
            {
                _context.LogHoatDongs.RemoveRange(logsToDelete);
                await _context.SaveChangesAsync();
            }

            return Ok(new { deletedCount = logsToDelete.Count });
        }

        // Helper method để lấy IP address của client
        private string GetClientIpAddress()
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            // Kiểm tra X-Forwarded-For header (cho trường hợp có proxy)
            if (HttpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                ipAddress = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            }

            // Kiểm tra X-Real-IP header
            if (HttpContext.Request.Headers.ContainsKey("X-Real-IP"))
            {
                ipAddress = HttpContext.Request.Headers["X-Real-IP"].FirstOrDefault();
            }

            return ipAddress ?? "Unknown";
        }

        private bool LogHoatDongExists(long id)
        {
            return _context.LogHoatDongs.Any(e => e.Id == id);
        }
    }
}