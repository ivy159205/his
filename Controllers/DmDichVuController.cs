using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DTOs;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DmDichVuController : ControllerBase
    {
        private readonly DBCHIS _context;

        public DmDichVuController(DBCHIS context)
        {
            _context = context;
        }

        // GET: api/DmDichVu
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DmDichVuDto>>> GetDmDichVus()
        {
            var result = await _context.DmDichVus
                .Include(d => d.MaKhoaNavigation)
                .Select(d => new DmDichVuDto
                {
                    MaDichVu = d.MaDichVu,
                    TenDichVu = d.TenDichVu,
                    LoaiDichVu = d.LoaiDichVu,
                    DonGia = d.DonGia,
                    MaKhoa = d.MaKhoa,
                    TrangThai = d.TrangThai,
                    NgayTao = d.NgayTao,
                    TenKhoa = d.MaKhoaNavigation != null ? d.MaKhoaNavigation.TenKhoa : null
                })
                .ToListAsync();

            return result;
        }

        // GET: api/DmDichVu/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DmDichVuDto>> GetDmDichVu(string id)
        {
            var dmDichVu = await _context.DmDichVus
                .Include(d => d.MaKhoaNavigation)
                .Where(d => d.MaDichVu == id)
                .Select(d => new DmDichVuDto
                {
                    MaDichVu = d.MaDichVu,
                    TenDichVu = d.TenDichVu,
                    LoaiDichVu = d.LoaiDichVu,
                    DonGia = d.DonGia,
                    MaKhoa = d.MaKhoa,
                    TrangThai = d.TrangThai,
                    NgayTao = d.NgayTao,
                    TenKhoa = d.MaKhoaNavigation != null ? d.MaKhoaNavigation.TenKhoa : null
                })
                .FirstOrDefaultAsync();

            if (dmDichVu == null)
            {
                return NotFound();
            }

            return dmDichVu;
        }

        // PUT: api/DmDichVu/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDmDichVu(string id, DmDichVu dmDichVu)
        {
            if (id != dmDichVu.MaDichVu)
            {
                return BadRequest();
            }

            _context.Entry(dmDichVu).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DmDichVuExists(id))
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

        // POST: api/DmDichVu
        [HttpPost]
        public async Task<ActionResult<DmDichVu>> PostDmDichVu(DmDichVu dmDichVu)
        {
            // Tự động tạo mã dịch vụ nếu chưa có
            if (string.IsNullOrEmpty(dmDichVu.MaDichVu))
            {
                dmDichVu.MaDichVu = await GenerateMaDichVu();
            }

            // Tự động set ngày tạo
            dmDichVu.NgayTao = DateTime.Now;

            // Mặc định trạng thái là true (hoạt động)
            if (dmDichVu.TrangThai == null)
            {
                dmDichVu.TrangThai = true;
            }

            _context.DmDichVus.Add(dmDichVu);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DmDichVuExists(dmDichVu.MaDichVu))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetDmDichVu", new { id = dmDichVu.MaDichVu }, dmDichVu);
        }

        // DELETE: api/DmDichVu/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDmDichVu(string id)
        {
            var dmDichVu = await _context.DmDichVus.FindAsync(id);
            if (dmDichVu == null)
            {
                return NotFound();
            }

            // Kiểm tra xem dịch vụ có đang được sử dụng không
            var isUsed = await _context.ChiDinhXetNghiems.AnyAsync(c => c.MaDichVu == id) ||
                        await _context.ChiTietHoaDons.AnyAsync(c => c.MaDichVu == id);

            if (isUsed)
            {
                return BadRequest("Không thể xóa dịch vụ này vì đang được sử dụng trong hệ thống.");
            }

            _context.DmDichVus.Remove(dmDichVu);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/DmDichVu/ByLoaiDichVu/XN
        [HttpGet("ByLoaiDichVu/{loaiDichVu}")]
        public async Task<ActionResult<IEnumerable<DmDichVuDto>>> GetDmDichVuByLoaiDichVu(string loaiDichVu)
        {
            var result = await _context.DmDichVus
                .Include(d => d.MaKhoaNavigation)
                .Where(d => d.LoaiDichVu == loaiDichVu)
                .Select(d => new DmDichVuDto
                {
                    MaDichVu = d.MaDichVu,
                    TenDichVu = d.TenDichVu,
                    LoaiDichVu = d.LoaiDichVu,
                    DonGia = d.DonGia,
                    MaKhoa = d.MaKhoa,
                    TrangThai = d.TrangThai,
                    NgayTao = d.NgayTao,
                    TenKhoa = d.MaKhoaNavigation != null ? d.MaKhoaNavigation.TenKhoa : null
                })
                .OrderBy(d => d.TenDichVu)
                .ToListAsync();

            return result;
        }

        // GET: api/DmDichVu/ByKhoa/K01
        [HttpGet("ByKhoa/{maKhoa}")]
        public async Task<ActionResult<IEnumerable<DmDichVu>>> GetDmDichVuByKhoa(string maKhoa)
        {
            return await _context.DmDichVus
                .Include(d => d.MaKhoaNavigation)
                .Where(d => d.MaKhoa == maKhoa)
                .OrderBy(d => d.TenDichVu)
                .ToListAsync();
        }

        // GET: api/DmDichVu/Active
        [HttpGet("Active")]
        public async Task<ActionResult<IEnumerable<DmDichVuDto>>> GetActiveDmDichVu()
        {
            var result = await _context.DmDichVus
                .Include(d => d.MaKhoaNavigation)
                .Where(d => d.TrangThai == true)
                .Select(d => new DmDichVuDto
                {
                    MaDichVu = d.MaDichVu,
                    TenDichVu = d.TenDichVu,
                    LoaiDichVu = d.LoaiDichVu,
                    DonGia = d.DonGia,
                    MaKhoa = d.MaKhoa,
                    TrangThai = d.TrangThai,
                    NgayTao = d.NgayTao,
                    TenKhoa = d.MaKhoaNavigation != null ? d.MaKhoaNavigation.TenKhoa : null
                })
                .OrderBy(d => d.TenDichVu)
                .ToListAsync();

            return result;
        }

        // GET: api/DmDichVu/Search/{keyword}
        [HttpGet("Search/{keyword}")]
        public async Task<ActionResult<IEnumerable<DmDichVu>>> SearchDmDichVu(string keyword)
        {
            return await _context.DmDichVus
                .Include(d => d.MaKhoaNavigation)
                .Where(d => d.TenDichVu.Contains(keyword) || d.MaDichVu.Contains(keyword))
                .OrderBy(d => d.TenDichVu)
                .ToListAsync();
        }

        // PUT: api/DmDichVu/UpdateTrangThai/5
        [HttpPut("UpdateTrangThai/{id}")]
        public async Task<IActionResult> UpdateTrangThai(string id, [FromBody] bool trangThai)
        {
            var dmDichVu = await _context.DmDichVus.FindAsync(id);
            if (dmDichVu == null)
            {
                return NotFound();
            }

            dmDichVu.TrangThai = trangThai;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DmDichVuExists(id))
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

        // PUT: api/DmDichVu/UpdateDonGia/5
        [HttpPut("UpdateDonGia/{id}")]
        public async Task<IActionResult> UpdateDonGia(string id, [FromBody] decimal donGia)
        {
            var dmDichVu = await _context.DmDichVus.FindAsync(id);
            if (dmDichVu == null)
            {
                return NotFound();
            }

            if (donGia < 0)
            {
                return BadRequest("Đơn giá không được âm.");
            }

            dmDichVu.DonGia = donGia;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DmDichVuExists(id))
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

        // GET: api/DmDichVu/Statistics
        [HttpGet("Statistics")]
        public async Task<ActionResult<object>> GetStatistics()
        {
            var statistics = new
            {
                TongSoDichVu = await _context.DmDichVus.CountAsync(),
                DichVuHoatDong = await _context.DmDichVus.CountAsync(d => d.TrangThai == true),
                DichVuNgungHoatDong = await _context.DmDichVus.CountAsync(d => d.TrangThai == false),
                ThongKeTheoLoai = await _context.DmDichVus
                    .GroupBy(d => d.LoaiDichVu)
                    .Select(g => new { LoaiDichVu = g.Key, SoLuong = g.Count() })
                    .ToListAsync(),
                ThongKeTheoKhoa = await _context.DmDichVus
                    .Include(d => d.MaKhoaNavigation)
                    .GroupBy(d => d.MaKhoaNavigation.TenKhoa)
                    .Select(g => new { TenKhoa = g.Key, SoLuong = g.Count() })
                    .ToListAsync(),
                DonGiaCaoNhat = await _context.DmDichVus.MaxAsync(d => d.DonGia),
                DonGiaThapNhat = await _context.DmDichVus.MinAsync(d => d.DonGia),
                DonGiaTrungBinh = await _context.DmDichVus.AverageAsync(d => d.DonGia)
            };

            return statistics;
        }

        // GET: api/DmDichVu/ByPriceRange/{minPrice}/{maxPrice}
        [HttpGet("ByPriceRange/{minPrice}/{maxPrice}")]
        public async Task<ActionResult<IEnumerable<DmDichVu>>> GetDmDichVuByPriceRange(decimal minPrice, decimal maxPrice)
        {
            return await _context.DmDichVus
                .Include(d => d.MaKhoaNavigation)
                .Where(d => d.DonGia >= minPrice && d.DonGia <= maxPrice)
                .OrderBy(d => d.DonGia)
                .ToListAsync();
        }

        private bool DmDichVuExists(string id)
        {
            return _context.DmDichVus.Any(e => e.MaDichVu == id);
        }

        private async Task<string> GenerateMaDichVu()
        {
            var prefix = "DV";

            var lastRecord = await _context.DmDichVus
                .Where(d => d.MaDichVu.StartsWith(prefix))
                .OrderByDescending(d => d.MaDichVu)
                .FirstOrDefaultAsync();

            if (lastRecord != null)
            {
                var numberPart = lastRecord.MaDichVu.Substring(prefix.Length);
                if (int.TryParse(numberPart, out int lastNumber))
                {
                    return $"{prefix}{(lastNumber + 1):D6}";
                }
            }

            return $"{prefix}000001";
        }
    }
}