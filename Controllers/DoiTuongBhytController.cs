using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoiTuongBhytController : ControllerBase
    {
        private readonly DBCHIS _context;

        public DoiTuongBhytController(DBCHIS context)
        {
            _context = context;
        }

        // GET: api/DoiTuongBhyt
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DmDoiTuongBhyt>>> GetDoiTuongBhyts()
        {
            try
            {
                return await _context.DmDoiTuongBhyts.ToListAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        // GET: api/DoiTuongBhyt/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DmDoiTuongBhyt>> GetDoiTuongBhyt(string id)
        {
            try
            {
                var doiTuongBhyt = await _context.DmDoiTuongBhyts.FindAsync(id);

                if (doiTuongBhyt == null)
                {
                    return NotFound($"Không tìm thấy đối tượng BHYT với mã: {id}");
                }

                return doiTuongBhyt;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        // GET: api/DoiTuongBhyt/active
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<DmDoiTuongBhyt>>> GetActiveDoiTuongBhyts()
        {
            try
            {
                return await _context.DmDoiTuongBhyts
                    .Where(x => x.TrangThai == true)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        // POST: api/DoiTuongBhyt
        [HttpPost]
        public async Task<ActionResult<DmDoiTuongBhyt>> CreateDoiTuongBhyt(DmDoiTuongBhyt doiTuongBhyt)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Kiểm tra trùng mã
                if (await _context.DmDoiTuongBhyts.AnyAsync(x => x.MaDoiTuong == doiTuongBhyt.MaDoiTuong))
                {
                    return Conflict($"Mã đối tượng BHYT '{doiTuongBhyt.MaDoiTuong}' đã tồn tại");
                }

                // Thiết lập giá trị mặc định
                doiTuongBhyt.NgayTao = DateTime.Now;
                doiTuongBhyt.TrangThai = doiTuongBhyt.TrangThai ?? true;

                _context.DmDoiTuongBhyts.Add(doiTuongBhyt);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetDoiTuongBhyt), new { id = doiTuongBhyt.MaDoiTuong }, doiTuongBhyt);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        // PUT: api/DoiTuongBhyt/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDoiTuongBhyt(string id, DmDoiTuongBhyt doiTuongBhyt)
        {
            try
            {
                if (id != doiTuongBhyt.MaDoiTuong)
                {
                    return BadRequest("Mã đối tượng không khớp");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingDoiTuong = await _context.DmDoiTuongBhyts.FindAsync(id);
                if (existingDoiTuong == null)
                {
                    return NotFound($"Không tìm thấy đối tượng BHYT với mã: {id}");
                }

                // Cập nhật các trường
                existingDoiTuong.TenDoiTuong = doiTuongBhyt.TenDoiTuong;
                existingDoiTuong.MucHuong = doiTuongBhyt.MucHuong;
                existingDoiTuong.MoTa = doiTuongBhyt.MoTa;
                existingDoiTuong.TrangThai = doiTuongBhyt.TrangThai;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await DoiTuongBhytExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        // DELETE: api/DoiTuongBhyt/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoiTuongBhyt(string id)
        {
            try
            {
                var doiTuongBhyt = await _context.DmDoiTuongBhyts.FindAsync(id);
                if (doiTuongBhyt == null)
                {
                    return NotFound($"Không tìm thấy đối tượng BHYT với mã: {id}");
                }

                // Kiểm tra có bệnh nhân nào đang sử dụng không
                var hasBenhNhan = await _context.BenhNhans.AnyAsync(x => x.MaDoiTuongBhyt == id);
                if (hasBenhNhan)
                {
                    return BadRequest("Không thể xóa đối tượng BHYT này vì đang có bệnh nhân sử dụng");
                }

                _context.DmDoiTuongBhyts.Remove(doiTuongBhyt);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        // PATCH: api/DoiTuongBhyt/5/status
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(string id, [FromBody] bool trangThai)
        {
            try
            {
                var doiTuongBhyt = await _context.DmDoiTuongBhyts.FindAsync(id);
                if (doiTuongBhyt == null)
                {
                    return NotFound($"Không tìm thấy đối tượng BHYT với mã: {id}");
                }

                doiTuongBhyt.TrangThai = trangThai;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        // GET: api/DoiTuongBhyt/search
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<DmDoiTuongBhyt>>> SearchDoiTuongBhyt([FromQuery] string? keyword)
        {
            try
            {
                if (string.IsNullOrEmpty(keyword))
                {
                    return await GetDoiTuongBhyts();
                }

                var results = await _context.DmDoiTuongBhyts
                    .Where(x => x.MaDoiTuong.Contains(keyword) ||
                               x.TenDoiTuong.Contains(keyword) ||
                               (x.MoTa != null && x.MoTa.Contains(keyword)))
                    .ToListAsync();

                return results;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        private async Task<bool> DoiTuongBhytExists(string id)
        {
            return await _context.DmDoiTuongBhyts.AnyAsync(e => e.MaDoiTuong == id);
        }
    }
}