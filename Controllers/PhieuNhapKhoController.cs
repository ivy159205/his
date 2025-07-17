using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhieuNhapKhoController : ControllerBase
    {
        private readonly DBCHIS _context;

        public PhieuNhapKhoController(DBCHIS context)
        {
            _context = context;
        }

        // GET: api/PhieuNhapKho
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PhieuNhapKho>>> GetPhieuNhapKhos()
        {
            return await _context.PhieuNhapKhos
                .Include(p => p.MaNccNavigation)
                .Include(p => p.NguoiNhapNavigation)
                .OrderByDescending(p => p.NgayNhap)
                .ToListAsync();
        }

        // GET: api/PhieuNhapKho/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PhieuNhapKho>> GetPhieuNhapKho(string id)
        {
            var phieuNhapKho = await _context.PhieuNhapKhos
                .Include(p => p.MaNccNavigation)
                .Include(p => p.NguoiNhapNavigation)
                .Include(p => p.ChiTietNhapKhoThuocs)
                .Include(p => p.ChiTietNhapKhoVatTus)
                .FirstOrDefaultAsync(p => p.MaPhieuNhap == id);

            if (phieuNhapKho == null)
            {
                return NotFound();
            }

            return phieuNhapKho;
        }

        // GET: api/PhieuNhapKho/ByNhaCungCap/{maNcc}
        [HttpGet("ByNhaCungCap/{maNcc}")]
        public async Task<ActionResult<IEnumerable<PhieuNhapKho>>> GetPhieuNhapKhoByNhaCungCap(string maNcc)
        {
            var phieuNhapKhos = await _context.PhieuNhapKhos
                .Include(p => p.MaNccNavigation)
                .Include(p => p.NguoiNhapNavigation)
                .Where(p => p.MaNcc == maNcc)
                .OrderByDescending(p => p.NgayNhap)
                .ToListAsync();

            return phieuNhapKhos;
        }

        // GET: api/PhieuNhapKho/ByNguoiNhap/{nguoiNhap}
        [HttpGet("ByNguoiNhap/{nguoiNhap}")]
        public async Task<ActionResult<IEnumerable<PhieuNhapKho>>> GetPhieuNhapKhoByNguoiNhap(string nguoiNhap)
        {
            var phieuNhapKhos = await _context.PhieuNhapKhos
                .Include(p => p.MaNccNavigation)
                .Include(p => p.NguoiNhapNavigation)
                .Where(p => p.NguoiNhap == nguoiNhap)
                .OrderByDescending(p => p.NgayNhap)
                .ToListAsync();

            return phieuNhapKhos;
        }

        // GET: api/PhieuNhapKho/ByLoaiPhieu/{loaiPhieu}
        [HttpGet("ByLoaiPhieu/{loaiPhieu}")]
        public async Task<ActionResult<IEnumerable<PhieuNhapKho>>> GetPhieuNhapKhoByLoaiPhieu(string loaiPhieu)
        {
            var phieuNhapKhos = await _context.PhieuNhapKhos
                .Include(p => p.MaNccNavigation)
                .Include(p => p.NguoiNhapNavigation)
                .Where(p => p.LoaiPhieu == loaiPhieu)
                .OrderByDescending(p => p.NgayNhap)
                .ToListAsync();

            return phieuNhapKhos;
        }

        // GET: api/PhieuNhapKho/ByTrangThai/{trangThai}
        [HttpGet("ByTrangThai/{trangThai}")]
        public async Task<ActionResult<IEnumerable<PhieuNhapKho>>> GetPhieuNhapKhoByTrangThai(string trangThai)
        {
            var phieuNhapKhos = await _context.PhieuNhapKhos
                .Include(p => p.MaNccNavigation)
                .Include(p => p.NguoiNhapNavigation)
                .Where(p => p.TrangThai == trangThai)
                .OrderByDescending(p => p.NgayNhap)
                .ToListAsync();

            return phieuNhapKhos;
        }

        // GET: api/PhieuNhapKho/ByDateRange
        [HttpGet("ByDateRange")]
        public async Task<ActionResult<IEnumerable<PhieuNhapKho>>> GetPhieuNhapKhoByDateRange(
            DateOnly? fromDate = null,
            DateOnly? toDate = null)
        {
            var query = _context.PhieuNhapKhos
                .Include(p => p.MaNccNavigation)
                .Include(p => p.NguoiNhapNavigation)
                .AsQueryable();

            if (fromDate.HasValue)
            {
                query = query.Where(p => p.NgayNhap >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(p => p.NgayNhap <= toDate.Value);
            }

            var phieuNhapKhos = await query
                .OrderByDescending(p => p.NgayNhap)
                .ToListAsync();

            return phieuNhapKhos;
        }

        // GET: api/PhieuNhapKho/BySoHoaDon/{soHoaDon}
        [HttpGet("BySoHoaDon/{soHoaDon}")]
        public async Task<ActionResult<PhieuNhapKho>> GetPhieuNhapKhoBySoHoaDon(string soHoaDon)
        {
            var phieuNhapKho = await _context.PhieuNhapKhos
                .Include(p => p.MaNccNavigation)
                .Include(p => p.NguoiNhapNavigation)
                .FirstOrDefaultAsync(p => p.SoHoaDon == soHoaDon);

            if (phieuNhapKho == null)
            {
                return NotFound();
            }

            return phieuNhapKho;
        }

        // GET: api/PhieuNhapKho/ThongKe
        [HttpGet("ThongKe")]
        public async Task<ActionResult<object>> GetThongKePhieuNhapKho(DateOnly? ngayBatDau = null, DateOnly? ngayKetThuc = null)
        {
            var query = _context.PhieuNhapKhos.AsQueryable();

            if (ngayBatDau.HasValue)
            {
                query = query.Where(p => p.NgayNhap >= ngayBatDau.Value);
            }

            if (ngayKetThuc.HasValue)
            {
                query = query.Where(p => p.NgayNhap <= ngayKetThuc.Value);
            }

            var thongKe = new
            {
                TongSoPhieu = await query.CountAsync(),
                TongGiaTriNhap = await query.SumAsync(p => p.TongTien ?? 0),
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
                TheoNhaCungCap = await query
                    .Include(p => p.MaNccNavigation)
                    .GroupBy(p => new { p.MaNcc, TenNcc = p.MaNccNavigation.TenNcc })
                    .Select(g => new {
                        MaNcc = g.Key.MaNcc,
                        TenNcc = g.Key.TenNcc,
                        SoLuong = g.Count(),
                        GiaTri = g.Sum(p => p.TongTien ?? 0)
                    })
                    .OrderByDescending(x => x.GiaTri)
                    .ToListAsync(),
                TheoNguoiNhap = await query
                    .Include(p => p.NguoiNhapNavigation)
                    .GroupBy(p => new { p.NguoiNhap, TenNguoiNhap = p.NguoiNhapNavigation.HoTen })
                    .Select(g => new {
                        MaNguoiNhap = g.Key.NguoiNhap,
                        TenNguoiNhap = g.Key.TenNguoiNhap,
                        SoLuong = g.Count(),
                        GiaTri = g.Sum(p => p.TongTien ?? 0)
                    })
                    .OrderByDescending(x => x.SoLuong)
                    .ToListAsync()
            };

            return thongKe;
        }

        // GET: api/PhieuNhapKho/PhieuChuaDuyet
        [HttpGet("PhieuChuaDuyet")]
        public async Task<ActionResult<IEnumerable<PhieuNhapKho>>> GetPhieuChuaDuyet()
        {
            var phieuNhapKhos = await _context.PhieuNhapKhos
                .Include(p => p.MaNccNavigation)
                .Include(p => p.NguoiNhapNavigation)
                .Where(p => p.TrangThai == "Chờ duyệt" || p.TrangThai == "Tạm lưu")
                .OrderByDescending(p => p.NgayNhap)
                .ToListAsync();

            return phieuNhapKhos;
        }

        // POST: api/PhieuNhapKho
        [HttpPost]
        public async Task<ActionResult<PhieuNhapKho>> PostPhieuNhapKho(PhieuNhapKho phieuNhapKho)
        {
            // Tự động tạo mã phiếu nhập nếu chưa có
            if (string.IsNullOrEmpty(phieuNhapKho.MaPhieuNhap))
            {
                phieuNhapKho.MaPhieuNhap = await GenerateMaPhieuNhap();
            }

            // Tự động set ngày tạo
            if (phieuNhapKho.NgayTao == null)
            {
                phieuNhapKho.NgayTao = DateTime.Now;
            }

            // Set trạng thái mặc định
            if (string.IsNullOrEmpty(phieuNhapKho.TrangThai))
            {
                phieuNhapKho.TrangThai = "Tạm lưu";
            }

            // Kiểm tra nhà cung cấp có tồn tại không
            if (!string.IsNullOrEmpty(phieuNhapKho.MaNcc))
            {
                var nhaCungCap = await _context.DmNhaCungCaps.FindAsync(phieuNhapKho.MaNcc);
                if (nhaCungCap == null)
                {
                    return BadRequest("Nhà cung cấp không tồn tại");
                }
            }

            // Kiểm tra người nhập có tồn tại không
            if (!string.IsNullOrEmpty(phieuNhapKho.NguoiNhap))
            {
                var nguoiNhap = await _context.DmNhanViens.FindAsync(phieuNhapKho.NguoiNhap);
                if (nguoiNhap == null)
                {
                    return BadRequest("Người nhập không tồn tại");
                }
            }

            _context.PhieuNhapKhos.Add(phieuNhapKho);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPhieuNhapKho", new { id = phieuNhapKho.MaPhieuNhap }, phieuNhapKho);
        }

        // PUT: api/PhieuNhapKho/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPhieuNhapKho(string id, PhieuNhapKho phieuNhapKho)
        {
            if (id != phieuNhapKho.MaPhieuNhap)
            {
                return BadRequest();
            }

            _context.Entry(phieuNhapKho).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhieuNhapKhoExists(id))
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

        // PUT: api/PhieuNhapKho/UpdateTrangThai/5
        [HttpPut("UpdateTrangThai/{id}")]
        public async Task<IActionResult> UpdateTrangThai(string id, [FromBody] string trangThai)
        {
            var phieuNhapKho = await _context.PhieuNhapKhos.FindAsync(id);
            if (phieuNhapKho == null)
            {
                return NotFound();
            }

            phieuNhapKho.TrangThai = trangThai;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/PhieuNhapKho/DuyetPhieu/5
        [HttpPut("DuyetPhieu/{id}")]
        public async Task<IActionResult> DuyetPhieu(string id, [FromBody] DuyetPhieuRequest request)
        {
            var phieuNhapKho = await _context.PhieuNhapKhos.FindAsync(id);
            if (phieuNhapKho == null)
            {
                return NotFound();
            }

            phieuNhapKho.TrangThai = "Đã duyệt";

            // Cập nhật thông tin người duyệt nếu có
            if (!string.IsNullOrEmpty(request.NguoiDuyet))
            {
                phieuNhapKho.GhiChu = $"{phieuNhapKho.GhiChu} - Duyệt bởi: {request.NguoiDuyet} lúc {DateTime.Now}";
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/PhieuNhapKho/TinhTongTien/5
        [HttpPut("TinhTongTien/{id}")]
        public async Task<IActionResult> TinhTongTien(string id)
        {
            var phieuNhapKho = await _context.PhieuNhapKhos.FindAsync(id);
            if (phieuNhapKho == null)
            {
                return NotFound();
            }

            // Tính tổng tiền từ chi tiết thuốc
            var tongTienThuoc = await _context.ChiTietNhapKhoThuocs
                .Where(ct => ct.MaPhieuNhap == id)
                .SumAsync(ct => ct.SoLuongNhap * (ct.DonGiaNhap ?? 0));

            // Tính tổng tiền từ chi tiết vật tư
            var tongTienVatTu = await _context.ChiTietNhapKhoVatTus
                .Where(ct => ct.MaPhieuNhap == id)
                .SumAsync(ct => ct.SoLuongNhap * (ct.DonGiaNhap ?? 0));

            phieuNhapKho.TongTien = tongTienThuoc + tongTienVatTu;
            await _context.SaveChangesAsync();

            return Ok(new { TongTien = phieuNhapKho.TongTien });
        }

        // DELETE: api/PhieuNhapKho/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhieuNhapKho(string id)
        {
            var phieuNhapKho = await _context.PhieuNhapKhos.FindAsync(id);
            if (phieuNhapKho == null)
            {
                return NotFound();
            }

            // Kiểm tra xem có thể xóa không (chỉ xóa được phiếu tạm lưu)
            if (phieuNhapKho.TrangThai == "Đã duyệt")
            {
                return BadRequest("Không thể xóa phiếu đã duyệt");
            }

            _context.PhieuNhapKhos.Remove(phieuNhapKho);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Helper method để tạo mã phiếu nhập tự động
        private async Task<string> GenerateMaPhieuNhap()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var prefix = $"PN{today:yyyyMMdd}";

            var lastRecord = await _context.PhieuNhapKhos
                .Where(p => p.MaPhieuNhap.StartsWith(prefix))
                .OrderByDescending(p => p.MaPhieuNhap)
                .FirstOrDefaultAsync();

            if (lastRecord == null)
            {
                return $"{prefix}001";
            }

            var lastNumber = int.Parse(lastRecord.MaPhieuNhap.Substring(prefix.Length));
            var newNumber = lastNumber + 1;

            return $"{prefix}{newNumber:D3}";
        }

        private bool PhieuNhapKhoExists(string id)
        {
            return _context.PhieuNhapKhos.Any(e => e.MaPhieuNhap == id);
        }
    }

    // DTO cho duyệt phiếu
    public class DuyetPhieuRequest
    {
        public string NguoiDuyet { get; set; } = null!;
        public string? GhiChu { get; set; }
    }
}