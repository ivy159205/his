using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using System.Text.RegularExpressions;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NhaCungCapController : ControllerBase
    {
        private readonly DBCHIS _context;

        public NhaCungCapController(DBCHIS context)
        {
            _context = context;
        }

        // GET: api/NhaCungCap
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DmNhaCungCap>>> GetNhaCungCaps()
        {
            try
            {
                return await _context.DmNhaCungCaps.ToListAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        // GET: api/NhaCungCap/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DmNhaCungCap>> GetNhaCungCap(string id)
        {
            try
            {
                var nhaCungCap = await _context.DmNhaCungCaps.FindAsync(id);

                if (nhaCungCap == null)
                {
                    return NotFound($"Không tìm thấy nhà cung cấp với mã: {id}");
                }

                return nhaCungCap;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        // GET: api/NhaCungCap/active
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<DmNhaCungCap>>> GetActiveNhaCungCaps()
        {
            try
            {
                return await _context.DmNhaCungCaps
                    .Where(x => x.TrangThai == true)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        // POST: api/NhaCungCap
        [HttpPost]
        public async Task<ActionResult<DmNhaCungCap>> CreateNhaCungCap(DmNhaCungCap nhaCungCap)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Validation tùy chỉnh
                var validationResult = ValidateNhaCungCap(nhaCungCap);
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.ErrorMessage);
                }

                // Kiểm tra trùng mã
                if (await _context.DmNhaCungCaps.AnyAsync(x => x.MaNcc == nhaCungCap.MaNcc))
                {
                    return Conflict($"Mã nhà cung cấp '{nhaCungCap.MaNcc}' đã tồn tại");
                }

                // Kiểm tra trùng mã số thuế
                if (!string.IsNullOrEmpty(nhaCungCap.MaSoThue) &&
                    await _context.DmNhaCungCaps.AnyAsync(x => x.MaSoThue == nhaCungCap.MaSoThue))
                {
                    return Conflict($"Mã số thuế '{nhaCungCap.MaSoThue}' đã tồn tại");
                }

                // Thiết lập giá trị mặc định
                nhaCungCap.NgayTao = DateTime.Now;
                nhaCungCap.TrangThai = nhaCungCap.TrangThai ?? true;

                _context.DmNhaCungCaps.Add(nhaCungCap);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetNhaCungCap), new { id = nhaCungCap.MaNcc }, nhaCungCap);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        // PUT: api/NhaCungCap/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNhaCungCap(string id, DmNhaCungCap nhaCungCap)
        {
            try
            {
                if (id != nhaCungCap.MaNcc)
                {
                    return BadRequest("Mã nhà cung cấp không khớp");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Validation tùy chỉnh
                var validationResult = ValidateNhaCungCap(nhaCungCap);
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.ErrorMessage);
                }

                var existingNhaCungCap = await _context.DmNhaCungCaps.FindAsync(id);
                if (existingNhaCungCap == null)
                {
                    return NotFound($"Không tìm thấy nhà cung cấp với mã: {id}");
                }

                // Kiểm tra trùng mã số thuế với nhà cung cấp khác
                if (!string.IsNullOrEmpty(nhaCungCap.MaSoThue) &&
                    await _context.DmNhaCungCaps.AnyAsync(x => x.MaSoThue == nhaCungCap.MaSoThue && x.MaNcc != id))
                {
                    return Conflict($"Mã số thuế '{nhaCungCap.MaSoThue}' đã tồn tại");
                }

                // Cập nhật các trường
                existingNhaCungCap.TenNcc = nhaCungCap.TenNcc;
                existingNhaCungCap.DiaChi = nhaCungCap.DiaChi;
                existingNhaCungCap.DienThoai = nhaCungCap.DienThoai;
                existingNhaCungCap.Email = nhaCungCap.Email;
                existingNhaCungCap.MaSoThue = nhaCungCap.MaSoThue;
                existingNhaCungCap.NguoiLienHe = nhaCungCap.NguoiLienHe;
                existingNhaCungCap.TrangThai = nhaCungCap.TrangThai;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await NhaCungCapExists(id))
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

        // DELETE: api/NhaCungCap/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNhaCungCap(string id)
        {
            try
            {
                var nhaCungCap = await _context.DmNhaCungCaps.FindAsync(id);
                if (nhaCungCap == null)
                {
                    return NotFound($"Không tìm thấy nhà cung cấp với mã: {id}");
                }

                // Kiểm tra có phiếu nhập kho nào đang sử dụng không
                var hasPhieuNhapKho = await _context.PhieuNhapKhos.AnyAsync(x => x.MaNcc == id);
                if (hasPhieuNhapKho)
                {
                    return BadRequest("Không thể xóa nhà cung cấp này vì đang có phiếu nhập kho");
                }

                _context.DmNhaCungCaps.Remove(nhaCungCap);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        // PATCH: api/NhaCungCap/5/status
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(string id, [FromBody] bool trangThai)
        {
            try
            {
                var nhaCungCap = await _context.DmNhaCungCaps.FindAsync(id);
                if (nhaCungCap == null)
                {
                    return NotFound($"Không tìm thấy nhà cung cấp với mã: {id}");
                }

                nhaCungCap.TrangThai = trangThai;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        // GET: api/NhaCungCap/search
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<DmNhaCungCap>>> SearchNhaCungCap([FromQuery] string? keyword)
        {
            try
            {
                if (string.IsNullOrEmpty(keyword))
                {
                    return await GetNhaCungCaps();
                }

                var results = await _context.DmNhaCungCaps
                    .Where(x => x.MaNcc.Contains(keyword) ||
                               x.TenNcc.Contains(keyword) ||
                               (x.DiaChi != null && x.DiaChi.Contains(keyword)) ||
                               (x.DienThoai != null && x.DienThoai.Contains(keyword)) ||
                               (x.Email != null && x.Email.Contains(keyword)) ||
                               (x.MaSoThue != null && x.MaSoThue.Contains(keyword)) ||
                               (x.NguoiLienHe != null && x.NguoiLienHe.Contains(keyword)))
                    .ToListAsync();

                return results;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        // GET: api/NhaCungCap/5/statistics
        [HttpGet("{id}/statistics")]
        public async Task<ActionResult<object>> GetNhaCungCapStatistics(string id)
        {
            try
            {
                var nhaCungCap = await _context.DmNhaCungCaps.FindAsync(id);
                if (nhaCungCap == null)
                {
                    return NotFound($"Không tìm thấy nhà cung cấp với mã: {id}");
                }

                var statistics = new
                {
                    SoPhieuNhapKho = await _context.PhieuNhapKhos.CountAsync(x => x.MaNcc == id),
                    TongGiaTriNhapKho = await _context.PhieuNhapKhos
                        .Where(x => x.MaNcc == id)
                        .SumAsync(x => x.TongTien ?? 0),
                    PhieuNhapKhoGanNhat = await _context.PhieuNhapKhos
                        .Where(x => x.MaNcc == id)
                        .OrderByDescending(x => x.NgayNhap)
                        .Select(x => new { x.MaPhieuNhap, x.NgayNhap, x.TongTien })
                        .FirstOrDefaultAsync()
                };

                return statistics;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        // GET: api/NhaCungCap/5/phieu-nhap-kho
        [HttpGet("{id}/phieu-nhap-kho")]
        public async Task<ActionResult<IEnumerable<PhieuNhapKho>>> GetPhieuNhapKhoByNhaCungCap(string id)
        {
            try
            {
                var nhaCungCap = await _context.DmNhaCungCaps.FindAsync(id);
                if (nhaCungCap == null)
                {
                    return NotFound($"Không tìm thấy nhà cung cấp với mã: {id}");
                }

                var phieuNhapKhos = await _context.PhieuNhapKhos
                    .Where(x => x.MaNcc == id)
                    .OrderByDescending(x => x.NgayNhap)
                    .ToListAsync();

                return phieuNhapKhos;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        // GET: api/NhaCungCap/validate-email/{email}
        [HttpGet("validate-email/{email}")]
        public async Task<ActionResult<bool>> ValidateEmail(string email)
        {
            try
            {
                var exists = await _context.DmNhaCungCaps.AnyAsync(x => x.Email == email);
                return !exists;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        // GET: api/NhaCungCap/validate-ma-so-thue/{maSoThue}
        [HttpGet("validate-ma-so-thue/{maSoThue}")]
        public async Task<ActionResult<bool>> ValidateMaSoThue(string maSoThue)
        {
            try
            {
                var exists = await _context.DmNhaCungCaps.AnyAsync(x => x.MaSoThue == maSoThue);
                return !exists;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        private async Task<bool> NhaCungCapExists(string id)
        {
            return await _context.DmNhaCungCaps.AnyAsync(e => e.MaNcc == id);
        }

        private (bool IsValid, string ErrorMessage) ValidateNhaCungCap(DmNhaCungCap nhaCungCap)
        {
            // Validate email format
            if (!string.IsNullOrEmpty(nhaCungCap.Email) && !IsValidEmail(nhaCungCap.Email))
            {
                return (false, "Email không đúng định dạng");
            }

            // Validate phone format
            if (!string.IsNullOrEmpty(nhaCungCap.DienThoai) && !IsValidPhone(nhaCungCap.DienThoai))
            {
                return (false, "Số điện thoại không đúng định dạng");
            }

            // Validate tax code format (Vietnamese tax code: 10 or 13 digits)
            if (!string.IsNullOrEmpty(nhaCungCap.MaSoThue) && !IsValidTaxCode(nhaCungCap.MaSoThue))
            {
                return (false, "Mã số thuế không đúng định dạng (10 hoặc 13 chữ số)");
            }

            return (true, string.Empty);
        }

        private bool IsValidEmail(string email)
        {
            var emailRegex = new Regex(@"^[^\s@]+@[^\s@]+\.[^\s@]+$");
            return emailRegex.IsMatch(email);
        }

        private bool IsValidPhone(string phone)
        {
            var phoneRegex = new Regex(@"^[\d\s\-\+\(\)]{10,15}$");
            return phoneRegex.IsMatch(phone);
        }

        private bool IsValidTaxCode(string taxCode)
        {
            var taxCodeRegex = new Regex(@"^\d{10}$|^\d{13}$");
            return taxCodeRegex.IsMatch(taxCode);
        }
    }
}