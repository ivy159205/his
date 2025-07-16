using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DmVatTuYteController : ControllerBase
    {
        private readonly DBCHIS _context;

        public DmVatTuYteController(DBCHIS context)
        {
            _context = context;
        }

        // GET: api/DmVatTuYte
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DmVatTuYte>>> GetDmVatTuYtes()
        {
            return await _context.DmVatTuYtes.ToListAsync();
        }

        // GET: api/DmVatTuYte/search?keyword=...
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<DmVatTuYte>>> SearchDmVatTuYtes(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return await _context.DmVatTuYtes.ToListAsync();
            }

            return await _context.DmVatTuYtes
                .Where(vt => vt.TenVatTu.Contains(keyword) ||
                            vt.MaVatTu.Contains(keyword) ||
                            (vt.QuyGach != null && vt.QuyGach.Contains(keyword)))
                .ToListAsync();
        }

        // GET: api/DmVatTuYte/active
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<DmVatTuYte>>> GetActiveDmVatTuYtes()
        {
            return await _context.DmVatTuYtes
                .Where(vt => vt.TrangThai == true)
                .ToListAsync();
        }

        // GET: api/DmVatTuYte/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DmVatTuYte>> GetDmVatTuYte(string id)
        {
            var dmVatTuYte = await _context.DmVatTuYtes.FindAsync(id);

            if (dmVatTuYte == null)
            {
                return NotFound();
            }

            return dmVatTuYte;
        }

        // GET: api/DmVatTuYte/5/details
        [HttpGet("{id}/details")]
        public async Task<ActionResult<DmVatTuYte>> GetDmVatTuYteWithDetails(string id)
        {
            var dmVatTuYte = await _context.DmVatTuYtes
                .Include(vt => vt.ChiTietNhapKhoVatTus)
                .Include(vt => vt.TonKhoVatTus)
                .FirstOrDefaultAsync(vt => vt.MaVatTu == id);

            if (dmVatTuYte == null)
            {
                return NotFound();
            }

            return dmVatTuYte;
        }

        // PUT: api/DmVatTuYte/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDmVatTuYte(string id, DmVatTuYte dmVatTuYte)
        {
            if (id != dmVatTuYte.MaVatTu)
            {
                return BadRequest();
            }

            _context.Entry(dmVatTuYte).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DmVatTuYteExists(id))
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

        // POST: api/DmVatTuYte
        [HttpPost]
        public async Task<ActionResult<DmVatTuYte>> PostDmVatTuYte(DmVatTuYte dmVatTuYte)
        {
            // Tự động set ngày tạo
            dmVatTuYte.NgayTao = DateTime.Now;

            // Mặc định trạng thái là true nếu không được set
            if (dmVatTuYte.TrangThai == null)
            {
                dmVatTuYte.TrangThai = true;
            }

            _context.DmVatTuYtes.Add(dmVatTuYte);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DmVatTuYteExists(dmVatTuYte.MaVatTu))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetDmVatTuYte", new { id = dmVatTuYte.MaVatTu }, dmVatTuYte);
        }

        // DELETE: api/DmVatTuYte/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDmVatTuYte(string id)
        {
            var dmVatTuYte = await _context.DmVatTuYtes.FindAsync(id);
            if (dmVatTuYte == null)
            {
                return NotFound();
            }

            // Kiểm tra xem vật tư có đang được sử dụng không
            var hasRelatedData = await _context.ChiTietNhapKhoVatTus.AnyAsync(ct => ct.MaVatTu == id) ||
                                await _context.TonKhoVatTus.AnyAsync(tk => tk.MaVatTu == id);

            if (hasRelatedData)
            {
                return BadRequest("Không thể xóa vật tư này vì đang có dữ liệu liên quan");
            }

            _context.DmVatTuYtes.Remove(dmVatTuYte);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/DmVatTuYte/5/toggle-status
        [HttpPut("{id}/toggle-status")]
        public async Task<IActionResult> ToggleStatus(string id)
        {
            var dmVatTuYte = await _context.DmVatTuYtes.FindAsync(id);
            if (dmVatTuYte == null)
            {
                return NotFound();
            }

            dmVatTuYte.TrangThai = !dmVatTuYte.TrangThai;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đã cập nhật trạng thái", trangThai = dmVatTuYte.TrangThai });
        }

        // GET: api/DmVatTuYte/by-loai/vat-tu-tieu-hao
        [HttpGet("by-loai/{loaiVatTu}")]
        public async Task<ActionResult<IEnumerable<DmVatTuYte>>> GetDmVatTuYteByLoai(string loaiVatTu)
        {
            return await _context.DmVatTuYtes
                .Where(vt => vt.LoaiVatTu == loaiVatTu)
                .ToListAsync();
        }

        // GET: api/DmVatTuYte/by-nha-san-xuat/abbott
        [HttpGet("by-nha-san-xuat/{nhaSanXuat}")]
        public async Task<ActionResult<IEnumerable<DmVatTuYte>>> GetDmVatTuYteByNhaSanXuat(string nhaSanXuat)
        {
            return await _context.DmVatTuYtes
                .Where(vt => vt.NhaSanXuat == nhaSanXuat)
                .ToListAsync();
        }

        // GET: api/DmVatTuYte/by-nuoc-san-xuat/viet-nam
        [HttpGet("by-nuoc-san-xuat/{nuocSanXuat}")]
        public async Task<ActionResult<IEnumerable<DmVatTuYte>>> GetDmVatTuYteByNuocSanXuat(string nuocSanXuat)
        {
            return await _context.DmVatTuYtes
                .Where(vt => vt.NuocSanXuat == nuocSanXuat)
                .ToListAsync();
        }

        // GET: api/DmVatTuYte/by-don-vi-tinh/hop
        [HttpGet("by-don-vi-tinh/{donViTinh}")]
        public async Task<ActionResult<IEnumerable<DmVatTuYte>>> GetDmVatTuYteByDonViTinh(string donViTinh)
        {
            return await _context.DmVatTuYtes
                .Where(vt => vt.DonViTinh == donViTinh)
                .ToListAsync();
        }

        // GET: api/DmVatTuYte/statistics
        [HttpGet("statistics")]
        public async Task<ActionResult<object>> GetStatistics()
        {
            var total = await _context.DmVatTuYtes.CountAsync();
            var active = await _context.DmVatTuYtes.CountAsync(vt => vt.TrangThai == true);
            var inactive = await _context.DmVatTuYtes.CountAsync(vt => vt.TrangThai == false);

            var byLoaiVatTu = await _context.DmVatTuYtes
                .GroupBy(vt => vt.LoaiVatTu)
                .Select(g => new { LoaiVatTu = g.Key, SoLuong = g.Count() })
                .ToListAsync();

            var byNhaSanXuat = await _context.DmVatTuYtes
                .GroupBy(vt => vt.NhaSanXuat)
                .Select(g => new { NhaSanXuat = g.Key, SoLuong = g.Count() })
                .OrderByDescending(x => x.SoLuong)
                .Take(10)
                .ToListAsync();

            return Ok(new
            {
                TongSo = total,
                DangHoatDong = active,
                NgungHoatDong = inactive,
                TheoLoaiVatTu = byLoaiVatTu,
                TopNhaSanXuat = byNhaSanXuat
            });
        }

        private bool DmVatTuYteExists(string id)
        {
            return _context.DmVatTuYtes.Any(e => e.MaVatTu == id);
        }
    }
}