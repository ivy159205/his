using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhieuXuatKhoController : ControllerBase
    {
        private readonly DBCHIS _context;

        public PhieuXuatKhoController(DBCHIS context)
        {
            _context = context;
        }

        // GET: api/PhieuXuatKho
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PhieuXuatKho>>> GetPhieuXuatKhos()
        {
            return await _context.PhieuXuatKhos
                .Include(p => p.MaKhoaNavigation)
                .Include(p => p.NguoiXuatNavigation)
                .OrderByDescending(p => p.NgayXuat)
                .ToListAsync();
        }

        // GET: api/PhieuXuatKho/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PhieuXuatKho>> GetPhieuXuatKho(string id)
        {
            var phieuXuatKho = await _context.PhieuXuatKhos
                .Include(p => p.MaKhoaNavigation)
                .Include(p => p.NguoiXuatNavigation)
                .FirstOrDefaultAsync(p => p.MaPhieuXuat == id);

            if (phieuXuatKho == null)
            {
                return NotFound();
            }

            return phieuXuatKho;
        }

        // GET: api/PhieuXuatKho/ByKhoa/{maKhoa}
        [HttpGet("ByKhoa/{maKhoa}")]
        public async Task<ActionResult<IEnumerable<PhieuXuatKho>>> GetPhieuXuatKhoByKhoa(string maKhoa)
        {
            var phieuXuatKhos = await _context.PhieuXuatKhos
                .Include(p => p.MaKhoaNavigation)
                .Include(p => p.NguoiXuatNavigation)
                .Where(p => p.MaKhoa == maKhoa)
                .OrderByDescending(p => p.NgayXuat)
                .ToListAsync();

            return phieuXuatKhos;
        }

        // GET: api/PhieuXuatKho/ByNguoiXuat/{nguoiXuat}
        [HttpGet("ByNguoiXuat/{nguoiXuat}")]
        public async Task<ActionResult<IEnumerable<PhieuXuatKho>>> GetPhieuXuatKhoByNguoiXuat(string nguoiXuat)
        {
            var phieuXuatKhos = await _context.PhieuXuatKhos
                .Include(p => p.MaKhoaNavigation)
                .Include(p => p.NguoiXuatNavigation)
                .Where(p => p.NguoiXuat == nguoiXuat)
                .OrderByDescending(p => p.NgayXuat)
                .ToListAsync();

            return phieuXuatKhos;
        }

        // GET: api/PhieuXuatKho/ByNguoiNhan/{nguoiNhan}
        [HttpGet("ByNguoiNhan/{nguoiNhan}")]
        public async Task<ActionResult<IEnumerable<PhieuXuatKho>>> GetPhieuXuatKhoByNguoiNhan(string nguoiNhan)
        {
            var phieuXuatKhos = await _context.PhieuXuatKhos
                .Include(p => p.MaKhoaNavigation)
                .Include(p => p.NguoiXuatNavigation)
                .Where(p => p.NguoiNhan == nguoiNhan)
                .OrderByDescending(p => p.NgayXuat)
                .ToListAsync();

            return phieuXuatKhos;
        }

        // GET: api/PhieuXuatKho/ByLoaiPhieu/{loaiPhieu}
        [HttpGet("ByLoaiPhieu/{loaiPhieu}")]
        public async Task<ActionResult<IEnumerable<PhieuXuatKho>>> GetPhieuXuatKhoByLoaiPhieu(string loaiPhieu)
        {
            var phieuXuatKhos = await _context.PhieuXuatKhos
                .Include(p => p.MaKhoaNavigation)
                .Include(p => p.NguoiXuatNavigation)
                .Where(p => p.LoaiPhieu == loaiPhieu)
                .OrderByDescending(p => p.NgayXuat)
                .ToListAsync();

            return phieuXuatKhos;
        }

        // GET: api/PhieuXuatKho/ByTrangThai/{trangThai}
        [HttpGet("ByTrangThai/{trangThai}")]
        public async Task<ActionResult<IEnumerable<PhieuXuatKho>>> GetPhieuXuatKhoByTrangThai(string trangThai)
        {
            var phieuXuatKhos = await _context.PhieuXuatKhos
                .Include(p => p.MaKhoaNavigation)
                .Include(p => p.NguoiXuatNavigation)
                .Where(p => p.TrangThai == trangThai)
                .OrderByDescending(p => p.NgayXuat)
                .ToListAsync();

            return phieuXuatKhos;
        }

        // GET: api/PhieuXuatKho/ByDateRange
        [HttpGet("ByDateRange")]
        public async Task<ActionResult<IEnumerable<PhieuXuatKho>>> GetPhieuXuatKhoByDateRange(
            DateOnly? fromDate = null,
            DateOnly? toDate = null)
        {
            var query = _context.PhieuXuatKhos
                .Include(p => p.MaKhoaNavigation)
                .Include(p => p.NguoiXuatNavigation)
                .AsQueryable();

            if (fromDate.HasValue)
            {
                query = query.Where(p => p.NgayXuat >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(p => p.NgayXuat <= toDate.Value);
            }

            var phieuXuatKhos = await query
                .OrderByDescending(p => p.NgayXuat)
                .ToListAsync();

            return phieuXuatKhos;
        }

        // GET: api/PhieuXuatKho/ThongKe
        [HttpGet("ThongKe")]
        public async Task<ActionResult<object>> GetThongKePhieuXuatKho(DateOnly? ngayBatDau = null, DateOnly? ngayKetThuc = null)
        {
            var query = _context.PhieuXuatKhos.AsQueryable();

            if (ngayBatDau.HasValue)
            {
                query = query.Where(p => p.NgayXuat >= ngayBatDau.Value);
            }

            if (ngayKetThuc.HasValue)
            {
                query = query.Where(p => p.NgayXuat <= ngayKetThuc.Value);
            }

            var thongKe = new
            {
                TongSoPhieu = await query.CountAsync(),
                TongGiaTriXuat = await query.SumAsync(p => p.TongTien ?? 0),
                TrungBinhGiaTriPhieu = await query.AverageAsync(p => p.TongTien ?? 0),
                TheoTrangThai = await query
                    .GroupBy(p => p.TrangThai)
                    .Select(g => new {
                        TrangThai = g.Key,
                        SoLuong = g.Count(),
                        GiaTri = g.Sum(p => p.TongTien ?? 0)
                    })
                    .ToListAsync(),
                TheoLoaiPhieu = await query
                    .GroupBy(p => p.LoaiPhieu)
                    .Select(g => new {
                        LoaiPhieu = g.Key,
                        SoLuong = g.Count(),
                        GiaTri = g.Sum(p => p.TongTien ?? 0)
                    })
                    .ToListAsync(),
                TheoKhoa = await query
                    .Include(p => p.MaKhoaNavigation)
                    .GroupBy(p => new { p.MaKhoa, TenKhoa = p.MaKhoaNavigation.TenKhoa })
                    .Select(g => new {
                        MaKhoa = g.Key.MaKhoa,
                        TenKhoa = g.Key.TenKhoa,
                        SoLuong = g.Count(),
                        GiaTri = g.Sum(p => p.TongTien ?? 0)
                    })
                    .OrderByDescending(x => x.GiaTri)
                    .ToListAsync(),
                TheoNguoiXuat = await query
                    .Include(p => p.NguoiXuatNavigation)
                    .GroupBy(p => new { p.NguoiXuat, TenNguoiXuat = p.NguoiXuatNavigation.HoTen })
                    .Select(g => new {
                        MaNguoiXuat = g.Key.NguoiXuat,
                        TenNguoiXuat = g.Key.TenNguoiXuat,
                        SoLuong = g.Count(),
                        GiaTri = g.Sum(p => p.TongTien ?? 0)
                    })
                    .OrderByDescending(x => x.SoLuong)
                    .ToListAsync()
            };

            return thongKe;
        }

        // GET: api/PhieuXuatKho/PhieuChuaDuyet
        [HttpGet("PhieuChuaDuyet")]
        public async Task<ActionResult<IEnumerable<PhieuXuatKho>>> GetPhieuChuaDuyet()
        {
            var phieuXuatKhos = await _context.PhieuXuatKhos
                .Include(p => p.MaKhoaNavigation)
                .Include(p => p.NguoiXuatNavigation)
                .Where(p => p.TrangThai == "Chờ duyệt" || p.TrangThai == "Tạm lưu")
                .OrderByDescending(p => p.NgayXuat)
                .ToListAsync();

            return phieuXuatKhos;
        }

        // POST: api/PhieuXuatKho
        [HttpPost]
        public async Task<ActionResult<PhieuXuatKho>> PostPhieuXuatKho(PhieuXuatKho phieuXuatKho)
        {
            // Tự động tạo mã phiếu xuất nếu chưa có
            if (string.IsNullOrEmpty(phieuXuatKho.MaPhieuXuat))
            {
                phieuXuatKho.MaPhieuXuat = await GenerateMaPhieuXuat();
            }

            // Tự động set ngày tạo
            if (phieuXuatKho.NgayTao == null)
            {
                phieuXuatKho.NgayTao = DateTime.Now;
            }

            // Set trạng thái mặc định
            if (string.IsNullOrEmpty(phieuXuatKho.TrangThai))
            {
                phieuXuatKho.TrangThai = "Tạm lưu";
            }

            // Kiểm tra khoa có tồn tại không
            if (!string.IsNullOrEmpty(phieuXuatKho.MaKhoa))
            {
                var khoa = await _context.DmKhoaPhongs.FindAsync(phieuXuatKho.MaKhoa);
                if (khoa == null)
                {
                    return BadRequest("Khoa không tồn tại");
                }
            }

            // Kiểm tra người xuất có tồn tại không
            if (!string.IsNullOrEmpty(phieuXuatKho.NguoiXuat))
            {
                var nguoiXuat = await _context.DmNhanViens.FindAsync(phieuXuatKho.NguoiXuat);
                if (nguoiXuat == null)
                {
                    return BadRequest("Người xuất không tồn tại");
                }
            }

            _context.PhieuXuatKhos.Add(phieuXuatKho);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPhieuXuatKho", new { id = phieuXuatKho.MaPhieuXuat }, phieuXuatKho);
        }

        // PUT: api/PhieuXuatKho/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPhieuXuatKho(string id, PhieuXuatKho phieuXuatKho)
        {
            if (id != phieuXuatKho.MaPhieuXuat)
            {
                return BadRequest();
            }

            _context.Entry(phieuXuatKho).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhieuXuatKhoExists(id))
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

        // PUT: api/PhieuXuatKho/UpdateTrangThai/5
        [HttpPut("UpdateTrangThai/{id}")]
        public async Task<IActionResult> UpdateTrangThai(string id, [FromBody] string trangThai)
        {
            var phieuXuatKho = await _context.PhieuXuatKhos.FindAsync(id);
            if (phieuXuatKho == null)
            {
                return NotFound();
            }

            phieuXuatKho.TrangThai = trangThai;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/PhieuXuatKho/DuyetPhieu/5
        [HttpPut("DuyetPhieu/{id}")]
        public async Task<IActionResult> DuyetPhieu(string id, [FromBody] DuyetPhieuXuatRequest request)
        {
            var phieuXuatKho = await _context.PhieuXuatKhos.FindAsync(id);
            if (phieuXuatKho == null)
            {
                return NotFound();
            }

            phieuXuatKho.TrangThai = "Đã duyệt";

            // Cập nhật thông tin người duyệt nếu có
            if (!string.IsNullOrEmpty(request.NguoiDuyet))
            {
                phieuXuatKho.LyDoXuat = $"{phieuXuatKho.LyDoXuat} - Duyệt bởi: {request.NguoiDuyet} lúc {DateTime.Now}";
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/PhieuXuatKho/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhieuXuatKho(string id)
        {
            var phieuXuatKho = await _context.PhieuXuatKhos.FindAsync(id);
            if (phieuXuatKho == null)
            {
                return NotFound();
            }

            // Kiểm tra xem có thể xóa không (chỉ xóa được phiếu tạm lưu)
            if (phieuXuatKho.TrangThai == "Đã duyệt")
            {
                return BadRequest("Không thể xóa phiếu đã duyệt");
            }

            _context.PhieuXuatKhos.Remove(phieuXuatKho);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Helper method để tạo mã phiếu xuất tự động
        private async Task<string> GenerateMaPhieuXuat()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var prefix = $"PX{today:yyyyMMdd}";

            var lastRecord = await _context.PhieuXuatKhos
                .Where(p => p.MaPhieuXuat.StartsWith(prefix))
                .OrderByDescending(p => p.MaPhieuXuat)
                .FirstOrDefaultAsync();

            if (lastRecord == null)
            {
                return $"{prefix}001";
            }

            var lastNumber = int.Parse(lastRecord.MaPhieuXuat.Substring(prefix.Length));
            var newNumber = lastNumber + 1;

            return $"{prefix}{newNumber:D3}";
        }

        private bool PhieuXuatKhoExists(string id)
        {
            return _context.PhieuXuatKhos.Any(e => e.MaPhieuXuat == id);
        }
    }

    // DTO cho duyệt phiếu xuất
    public class DuyetPhieuXuatRequest
    {
        public string NguoiDuyet { get; set; } = null!;
        public string? GhiChu { get; set; }
    }
}