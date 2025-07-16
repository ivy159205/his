using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DmThuocController : ControllerBase
    {
        private readonly DBCHIS _context;

        public DmThuocController(DBCHIS context)
        {
            _context = context;
        }

        // GET: api/DmThuoc
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DmThuoc>>> GetDmThuocs()
        {
            return await _context.DmThuocs.ToListAsync();
        }

        // GET: api/DmThuoc/search?keyword=...
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<DmThuoc>>> SearchDmThuocs(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return await _context.DmThuocs.ToListAsync();
            }

            return await _context.DmThuocs
                .Where(t => t.TenThuoc.Contains(keyword) ||
                           t.TenHoatChat.Contains(keyword) ||
                           t.MaThuoc.Contains(keyword))
                .ToListAsync();
        }

        // GET: api/DmThuoc/active
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<DmThuoc>>> GetActiveDmThuocs()
        {
            return await _context.DmThuocs
                .Where(t => t.TrangThai == true)
                .ToListAsync();
        }

        // GET: api/DmThuoc/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DmThuoc>> GetDmThuoc(string id)
        {
            var dmThuoc = await _context.DmThuocs.FindAsync(id);

            if (dmThuoc == null)
            {
                return NotFound();
            }

            return dmThuoc;
        }

        // GET: api/DmThuoc/5/details
        [HttpGet("{id}/details")]
        public async Task<ActionResult<DmThuoc>> GetDmThuocWithDetails(string id)
        {
            var dmThuoc = await _context.DmThuocs
                .Include(t => t.ChiTietDonThuocs)
                .Include(t => t.ChiTietNhapKhoThuocs)
                .Include(t => t.TonKhoThuocs)
                .FirstOrDefaultAsync(t => t.MaThuoc == id);

            if (dmThuoc == null)
            {
                return NotFound();
            }

            return dmThuoc;
        }

        // PUT: api/DmThuoc/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDmThuoc(string id, DmThuoc dmThuoc)
        {
            if (id != dmThuoc.MaThuoc)
            {
                return BadRequest();
            }

            _context.Entry(dmThuoc).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DmThuocExists(id))
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

        // POST: api/DmThuoc
        [HttpPost]
        public async Task<ActionResult<DmThuoc>> PostDmThuoc(DmThuoc dmThuoc)
        {
            // Tự động set ngày tạo
            dmThuoc.NgayTao = DateTime.Now;

            // Mặc định trạng thái là true nếu không được set
            if (dmThuoc.TrangThai == null)
            {
                dmThuoc.TrangThai = true;
            }

            _context.DmThuocs.Add(dmThuoc);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DmThuocExists(dmThuoc.MaThuoc))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetDmThuoc", new { id = dmThuoc.MaThuoc }, dmThuoc);
        }

        // DELETE: api/DmThuoc/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDmThuoc(string id)
        {
            var dmThuoc = await _context.DmThuocs.FindAsync(id);
            if (dmThuoc == null)
            {
                return NotFound();
            }

            // Kiểm tra xem thuốc có đang được sử dụng không
            var hasRelatedData = await _context.ChiTietDonThuocs.AnyAsync(ct => ct.MaThuoc == id) ||
                                await _context.ChiTietNhapKhoThuocs.AnyAsync(ct => ct.MaThuoc == id) ||
                                await _context.TonKhoThuocs.AnyAsync(tk => tk.MaThuoc == id);

            if (hasRelatedData)
            {
                return BadRequest("Không thể xóa thuốc này vì đang có dữ liệu liên quan");
            }

            _context.DmThuocs.Remove(dmThuoc);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/DmThuoc/5/toggle-status
        [HttpPut("{id}/toggle-status")]
        public async Task<IActionResult> ToggleStatus(string id)
        {
            var dmThuoc = await _context.DmThuocs.FindAsync(id);
            if (dmThuoc == null)
            {
                return NotFound();
            }

            dmThuoc.TrangThai = !dmThuoc.TrangThai;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đã cập nhật trạng thái", trangThai = dmThuoc.TrangThai });
        }

        // GET: api/DmThuoc/by-loai/thuoc-kk
        [HttpGet("by-loai/{loaiThuoc}")]
        public async Task<ActionResult<IEnumerable<DmThuoc>>> GetDmThuocByLoai(string loaiThuoc)
        {
            return await _context.DmThuocs
                .Where(t => t.LoaiThuoc == loaiThuoc)
                .ToListAsync();
        }

        // GET: api/DmThuoc/by-nha-san-xuat/pfizer
        [HttpGet("by-nha-san-xuat/{nhaSanXuat}")]
        public async Task<ActionResult<IEnumerable<DmThuoc>>> GetDmThuocByNhaSanXuat(string nhaSanXuat)
        {
            return await _context.DmThuocs
                .Where(t => t.NhaSanXuat == nhaSanXuat)
                .ToListAsync();
        }

        private bool DmThuocExists(string id)
        {
            return _context.DmThuocs.Any(e => e.MaThuoc == id);
        }
    }
}