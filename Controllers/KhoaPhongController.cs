using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KhoaPhongController : ControllerBase
    {
        private readonly DBCHIS _context;

        public KhoaPhongController(DBCHIS context)
        {
            _context = context;
        }

        // GET: api/KhoaPhong
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DmKhoaPhong>>> GetKhoaPhongs()
        {
            try
            {
                return await _context.DmKhoaPhongs.ToListAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        // GET: api/KhoaPhong/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DmKhoaPhong>> GetKhoaPhong(string id)
        {
            try
            {
                var khoaPhong = await _context.DmKhoaPhongs.FindAsync(id);

                if (khoaPhong == null)
                {
                    return NotFound($"Không tìm thấy khoa phòng với mã: {id}");
                }

                return khoaPhong;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        // GET: api/KhoaPhong/active
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<DmKhoaPhong>>> GetActiveKhoaPhongs()
        {
            try
            {
                return await _context.DmKhoaPhongs
                    .Where(x => x.TrangThai == true)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        // GET: api/KhoaPhong/by-type/{loaiKhoa}
        [HttpGet("by-type/{loaiKhoa}")]
        public async Task<ActionResult<IEnumerable<DmKhoaPhong>>> GetKhoaPhongsByType(string loaiKhoa)
        {
            try
            {
                return await _context.DmKhoaPhongs
                    .Where(x => x.LoaiKhoa == loaiKhoa)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        // GET: api/KhoaPhong/types
        [HttpGet("types")]
        public async Task<ActionResult<IEnumerable<string>>> GetKhoaPhongTypes()
        {
            try
            {
                return await _context.DmKhoaPhongs
                    .Where(x => !string.IsNullOrEmpty(x.LoaiKhoa))
                    .Select(x => x.LoaiKhoa!)
                    .Distinct()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        // POST: api/KhoaPhong
        [HttpPost]
        public async Task<ActionResult<DmKhoaPhong>> CreateKhoaPhong(DmKhoaPhong khoaPhong)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Kiểm tra trùng mã
                if (await _context.DmKhoaPhongs.AnyAsync(x => x.MaKhoa == khoaPhong.MaKhoa))
                {
                    return Conflict($"Mã khoa phòng '{khoaPhong.MaKhoa}' đã tồn tại");
                }

                // Thiết lập giá trị mặc định
                khoaPhong.NgayTao = DateTime.Now;
                khoaPhong.TrangThai = khoaPhong.TrangThai ?? true;

                _context.DmKhoaPhongs.Add(khoaPhong);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetKhoaPhong), new { id = khoaPhong.MaKhoa }, khoaPhong);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        // PUT: api/KhoaPhong/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateKhoaPhong(string id, DmKhoaPhong khoaPhong)
        {
            try
            {
                if (id != khoaPhong.MaKhoa)
                {
                    return BadRequest("Mã khoa phòng không khớp");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingKhoaPhong = await _context.DmKhoaPhongs.FindAsync(id);
                if (existingKhoaPhong == null)
                {
                    return NotFound($"Không tìm thấy khoa phòng với mã: {id}");
                }

                // Cập nhật các trường
                existingKhoaPhong.TenKhoa = khoaPhong.TenKhoa;
                existingKhoaPhong.LoaiKhoa = khoaPhong.LoaiKhoa;
                existingKhoaPhong.DiaChi = khoaPhong.DiaChi;
                existingKhoaPhong.DienThoai = khoaPhong.DienThoai;
                existingKhoaPhong.TruongKhoa = khoaPhong.TruongKhoa;
                existingKhoaPhong.TrangThai = khoaPhong.TrangThai;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await KhoaPhongExists(id))
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

        // DELETE: api/KhoaPhong/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKhoaPhong(string id)
        {
            try
            {
                var khoaPhong = await _context.DmKhoaPhongs.FindAsync(id);
                if (khoaPhong == null)
                {
                    return NotFound($"Không tìm thấy khoa phòng với mã: {id}");
                }

                // Kiểm tra các ràng buộc liên quan
                var hasRelatedData = await CheckRelatedData(id);
                if (hasRelatedData.HasRelatedData)
                {
                    return BadRequest($"Không thể xóa khoa phòng này vì đang có dữ liệu liên quan: {hasRelatedData.Message}");
                }

                _context.DmKhoaPhongs.Remove(khoaPhong);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        // PATCH: api/KhoaPhong/5/status
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(string id, [FromBody] bool trangThai)
        {
            try
            {
                var khoaPhong = await _context.DmKhoaPhongs.FindAsync(id);
                if (khoaPhong == null)
                {
                    return NotFound($"Không tìm thấy khoa phòng với mã: {id}");
                }

                khoaPhong.TrangThai = trangThai;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        // PATCH: api/KhoaPhong/5/truong-khoa
        [HttpPatch("{id}/truong-khoa")]
        public async Task<IActionResult> UpdateTruongKhoa(string id, [FromBody] string truongKhoa)
        {
            try
            {
                var khoaPhong = await _context.DmKhoaPhongs.FindAsync(id);
                if (khoaPhong == null)
                {
                    return NotFound($"Không tìm thấy khoa phòng với mã: {id}");
                }

                khoaPhong.TruongKhoa = truongKhoa;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        // GET: api/KhoaPhong/search
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<DmKhoaPhong>>> SearchKhoaPhong([FromQuery] string? keyword)
        {
            try
            {
                if (string.IsNullOrEmpty(keyword))
                {
                    return await GetKhoaPhongs();
                }

                var results = await _context.DmKhoaPhongs
                    .Where(x => x.MaKhoa.Contains(keyword) ||
                               x.TenKhoa.Contains(keyword) ||
                               (x.LoaiKhoa != null && x.LoaiKhoa.Contains(keyword)) ||
                               (x.TruongKhoa != null && x.TruongKhoa.Contains(keyword)) ||
                               (x.DiaChi != null && x.DiaChi.Contains(keyword)))
                    .ToListAsync();

                return results;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        // GET: api/KhoaPhong/5/statistics
        [HttpGet("{id}/statistics")]
        public async Task<ActionResult<object>> GetKhoaPhongStatistics(string id)
        {
            try
            {
                var khoaPhong = await _context.DmKhoaPhongs.FindAsync(id);
                if (khoaPhong == null)
                {
                    return NotFound($"Không tìm thấy khoa phòng với mã: {id}");
                }

                var statistics = new
                {
                    SoNhanVien = await _context.DmNhanViens.CountAsync(x => x.MaKhoa == id),
                    SoDichVu = await _context.DmDichVus.CountAsync(x => x.MaKhoa == id),
                    SoDangKyKham = await _context.DangKyKhams.CountAsync(x => x.MaKhoa == id),
                    SoNhapVien = await _context.NhapViens.CountAsync(x => x.MaKhoa == id),
                    SoPhieuXuatKho = await _context.PhieuXuatKhos.CountAsync(x => x.MaKhoa == id)
                };

                return statistics;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        // GET: api/KhoaPhong/5/nhan-vien
        [HttpGet("{id}/nhan-vien")]
        public async Task<ActionResult<IEnumerable<DmNhanVien>>> GetNhanVienByKhoa(string id)
        {
            try
            {
                var khoaPhong = await _context.DmKhoaPhongs.FindAsync(id);
                if (khoaPhong == null)
                {
                    return NotFound($"Không tìm thấy khoa phòng với mã: {id}");
                }

                var nhanViens = await _context.DmNhanViens
                    .Where(x => x.MaKhoa == id)
                    .ToListAsync();

                return nhanViens;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        private async Task<bool> KhoaPhongExists(string id)
        {
            return await _context.DmKhoaPhongs.AnyAsync(e => e.MaKhoa == id);
        }

        private async Task<(bool HasRelatedData, string Message)> CheckRelatedData(string maKhoa)
        {
            var messages = new List<string>();

            var nhanVienCount = await _context.DmNhanViens.CountAsync(x => x.MaKhoa == maKhoa);
            if (nhanVienCount > 0)
                messages.Add($"{nhanVienCount} nhân viên");

            var dichVuCount = await _context.DmDichVus.CountAsync(x => x.MaKhoa == maKhoa);
            if (dichVuCount > 0)
                messages.Add($"{dichVuCount} dịch vụ");

            var dangKyKhamCount = await _context.DangKyKhams.CountAsync(x => x.MaKhoa == maKhoa);
            if (dangKyKhamCount > 0)
                messages.Add($"{dangKyKhamCount} đăng ký khám");

            var nhapVienCount = await _context.NhapViens.CountAsync(x => x.MaKhoa == maKhoa);
            if (nhapVienCount > 0)
                messages.Add($"{nhapVienCount} nhập viện");

            var phieuXuatKhoCount = await _context.PhieuXuatKhos.CountAsync(x => x.MaKhoa == maKhoa);
            if (phieuXuatKhoCount > 0)
                messages.Add($"{phieuXuatKhoCount} phiếu xuất kho");

            return (messages.Any(), string.Join(", ", messages));
        }
    }
}