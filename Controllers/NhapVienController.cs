using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NhapVienController : ControllerBase
    {
        private readonly DBCHIS _context;

        public NhapVienController(DBCHIS context)
        {
            _context = context;
        }

        // GET: api/NhapVien
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NhapVien>>> GetNhapViens()
        {
            return await _context.NhapViens
                .Include(n => n.MaBnNavigation)
                .Include(n => n.MaBacSiNavigation)
                .Include(n => n.MaKhoaNavigation)
                .OrderByDescending(n => n.NgayNhapVien)
                .ToListAsync();
        }

        // GET: api/NhapVien/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NhapVien>> GetNhapVien(string id)
        {
            var nhapVien = await _context.NhapViens
                .Include(n => n.MaBnNavigation)
                .Include(n => n.MaBacSiNavigation)
                .Include(n => n.MaKhoaNavigation)
                .Include(n => n.BenhAnNoiTrus)
                .FirstOrDefaultAsync(n => n.MaNhapVien == id);

            if (nhapVien == null)
            {
                return NotFound();
            }

            return nhapVien;
        }

        // GET: api/NhapVien/ByBenhNhan/{maBn}
        [HttpGet("ByBenhNhan/{maBn}")]
        public async Task<ActionResult<IEnumerable<NhapVien>>> GetNhapVienByBenhNhan(string maBn)
        {
            var nhapViens = await _context.NhapViens
                .Include(n => n.MaBnNavigation)
                .Include(n => n.MaBacSiNavigation)
                .Include(n => n.MaKhoaNavigation)
                .Where(n => n.MaBn == maBn)
                .OrderByDescending(n => n.NgayNhapVien)
                .ToListAsync();

            return nhapViens;
        }

        // GET: api/NhapVien/ByKhoa/{maKhoa}
        [HttpGet("ByKhoa/{maKhoa}")]
        public async Task<ActionResult<IEnumerable<NhapVien>>> GetNhapVienByKhoa(string maKhoa)
        {
            var nhapViens = await _context.NhapViens
                .Include(n => n.MaBnNavigation)
                .Include(n => n.MaBacSiNavigation)
                .Include(n => n.MaKhoaNavigation)
                .Where(n => n.MaKhoa == maKhoa)
                .OrderByDescending(n => n.NgayNhapVien)
                .ToListAsync();

            return nhapViens;
        }

        // GET: api/NhapVien/ByBacSi/{maBacSi}
        [HttpGet("ByBacSi/{maBacSi}")]
        public async Task<ActionResult<IEnumerable<NhapVien>>> GetNhapVienByBacSi(string maBacSi)
        {
            var nhapViens = await _context.NhapViens
                .Include(n => n.MaBnNavigation)
                .Include(n => n.MaBacSiNavigation)
                .Include(n => n.MaKhoaNavigation)
                .Where(n => n.MaBacSi == maBacSi)
                .OrderByDescending(n => n.NgayNhapVien)
                .ToListAsync();

            return nhapViens;
        }

        // GET: api/NhapVien/ByTrangThai/{trangThai}
        [HttpGet("ByTrangThai/{trangThai}")]
        public async Task<ActionResult<IEnumerable<NhapVien>>> GetNhapVienByTrangThai(string trangThai)
        {
            var nhapViens = await _context.NhapViens
                .Include(n => n.MaBnNavigation)
                .Include(n => n.MaBacSiNavigation)
                .Include(n => n.MaKhoaNavigation)
                .Where(n => n.TrangThai == trangThai)
                .OrderByDescending(n => n.NgayNhapVien)
                .ToListAsync();

            return nhapViens;
        }

        // GET: api/NhapVien/ByDateRange
        [HttpGet("ByDateRange")]
        public async Task<ActionResult<IEnumerable<NhapVien>>> GetNhapVienByDateRange(
            DateTime? fromDate = null,
            DateTime? toDate = null)
        {
            var query = _context.NhapViens
                .Include(n => n.MaBnNavigation)
                .Include(n => n.MaBacSiNavigation)
                .Include(n => n.MaKhoaNavigation)
                .AsQueryable();

            if (fromDate.HasValue)
            {
                query = query.Where(n => n.NgayNhapVien >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(n => n.NgayNhapVien <= toDate.Value);
            }

            var nhapViens = await query
                .OrderByDescending(n => n.NgayNhapVien)
                .ToListAsync();

            return nhapViens;
        }

        // GET: api/NhapVien/DangNhapVien
        [HttpGet("DangNhapVien")]
        public async Task<ActionResult<IEnumerable<NhapVien>>> GetBenhNhanDangNhapVien()
        {
            var nhapViens = await _context.NhapViens
                .Include(n => n.MaBnNavigation)
                .Include(n => n.MaBacSiNavigation)
                .Include(n => n.MaKhoaNavigation)
                .Where(n => n.TrangThai == "Đang điều trị" || n.TrangThai == "Nhập viện")
                .OrderByDescending(n => n.NgayNhapVien)
                .ToListAsync();

            return nhapViens;
        }

        // GET: api/NhapVien/ThongKe
        [HttpGet("ThongKe")]
        public async Task<ActionResult<object>> GetThongKeNhapVien(DateTime? ngayBatDau = null, DateTime? ngayKetThuc = null)
        {
            var query = _context.NhapViens.AsQueryable();

            if (ngayBatDau.HasValue)
            {
                query = query.Where(n => n.NgayNhapVien >= ngayBatDau.Value);
            }

            if (ngayKetThuc.HasValue)
            {
                query = query.Where(n => n.NgayNhapVien <= ngayKetThuc.Value);
            }

            var thongKe = new
            {
                TongSoNhapVien = await query.CountAsync(),
                TheoTrangThai = await query
                    .GroupBy(n => n.TrangThai)
                    .Select(g => new { TrangThai = g.Key, SoLuong = g.Count() })
                    .ToListAsync(),
                TheoKhoa = await query
                    .Include(n => n.MaKhoaNavigation)
                    .GroupBy(n => new { n.MaKhoa, TenKhoa = n.MaKhoaNavigation.TenKhoa })
                    .Select(g => new {
                        MaKhoa = g.Key.MaKhoa,
                        TenKhoa = g.Key.TenKhoa,
                        SoLuong = g.Count()
                    })
                    .ToListAsync(),
                TheoLoaiNhapVien = await query
                    .GroupBy(n => n.LoaiNhapVien)
                    .Select(g => new { LoaiNhapVien = g.Key, SoLuong = g.Count() })
                    .ToListAsync()
            };

            return thongKe;
        }

        // POST: api/NhapVien
        [HttpPost]
        public async Task<ActionResult<NhapVien>> PostNhapVien(NhapVien nhapVien)
        {
            // Tự động tạo mã nhập viện nếu chưa có
            if (string.IsNullOrEmpty(nhapVien.MaNhapVien))
            {
                nhapVien.MaNhapVien = await GenerateMaNhapVien();
            }

            // Tự động set ngày tạo
            if (nhapVien.NgayTao == null)
            {
                nhapVien.NgayTao = DateTime.Now;
            }

            // Kiểm tra bệnh nhân có tồn tại không
            var benhNhan = await _context.BenhNhans.FindAsync(nhapVien.MaBn);
            if (benhNhan == null)
            {
                return BadRequest("Bệnh nhân không tồn tại");
            }

            // Kiểm tra khoa có tồn tại không
            if (!string.IsNullOrEmpty(nhapVien.MaKhoa))
            {
                var khoa = await _context.DmKhoaPhongs.FindAsync(nhapVien.MaKhoa);
                if (khoa == null)
                {
                    return BadRequest("Khoa không tồn tại");
                }
            }

            // Kiểm tra bác sĩ có tồn tại không
            if (!string.IsNullOrEmpty(nhapVien.MaBacSi))
            {
                var bacSi = await _context.DmNhanViens.FindAsync(nhapVien.MaBacSi);
                if (bacSi == null)
                {
                    return BadRequest("Bác sĩ không tồn tại");
                }
            }

            _context.NhapViens.Add(nhapVien);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNhapVien", new { id = nhapVien.MaNhapVien }, nhapVien);
        }

        // PUT: api/NhapVien/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNhapVien(string id, NhapVien nhapVien)
        {
            if (id != nhapVien.MaNhapVien)
            {
                return BadRequest();
            }

            _context.Entry(nhapVien).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NhapVienExists(id))
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

        // PUT: api/NhapVien/UpdateTrangThai/5
        [HttpPut("UpdateTrangThai/{id}")]
        public async Task<IActionResult> UpdateTrangThai(string id, [FromBody] string trangThai)
        {
            var nhapVien = await _context.NhapViens.FindAsync(id);
            if (nhapVien == null)
            {
                return NotFound();
            }

            nhapVien.TrangThai = trangThai;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/NhapVien/ChuyenKhoa/5
        [HttpPut("ChuyenKhoa/{id}")]
        public async Task<IActionResult> ChuyenKhoa(string id, [FromBody] ChuyenKhoaRequest request)
        {
            var nhapVien = await _context.NhapViens.FindAsync(id);
            if (nhapVien == null)
            {
                return NotFound();
            }

            nhapVien.MaKhoa = request.MaKhoaMoi;
            nhapVien.MaBacSi = request.MaBacSiMoi;
            nhapVien.SoGiuong = request.SoGiuongMoi;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/NhapVien/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNhapVien(string id)
        {
            var nhapVien = await _context.NhapViens.FindAsync(id);
            if (nhapVien == null)
            {
                return NotFound();
            }

            _context.NhapViens.Remove(nhapVien);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Helper method để tạo mã nhập viện tự động
        private async Task<string> GenerateMaNhapVien()
        {
            var today = DateTime.Now;
            var prefix = $"NV{today:yyyyMMdd}";

            var lastRecord = await _context.NhapViens
                .Where(n => n.MaNhapVien.StartsWith(prefix))
                .OrderByDescending(n => n.MaNhapVien)
                .FirstOrDefaultAsync();

            if (lastRecord == null)
            {
                return $"{prefix}001";
            }

            var lastNumber = int.Parse(lastRecord.MaNhapVien.Substring(prefix.Length));
            var newNumber = lastNumber + 1;

            return $"{prefix}{newNumber:D3}";
        }

        private bool NhapVienExists(string id)
        {
            return _context.NhapViens.Any(e => e.MaNhapVien == id);
        }
    }

    // DTO cho chuyển khoa
    public class ChuyenKhoaRequest
    {
        public string MaKhoaMoi { get; set; } = null!;
        public string? MaBacSiMoi { get; set; }
        public string? SoGiuongMoi { get; set; }
    }
}