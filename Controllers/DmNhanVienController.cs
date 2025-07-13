using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DmNhanVienController : ControllerBase
    {
        private readonly DBCHIS _context;

        public DmNhanVienController(DBCHIS context)
        {
            _context = context;
        }

        // GET: api/DmNhanVien
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DmNhanVien>>> GetDmNhanViens()
        {
            return await _context.DmNhanViens
                .Include(nv => nv.MaKhoaNavigation)
                .ToListAsync();
        }

        // GET: api/DmNhanVien/search
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<DmNhanVien>>> SearchNhanVien(
            [FromQuery] string? keyword = null,
            [FromQuery] string? chucVu = null,
            [FromQuery] string? maKhoa = null,
            [FromQuery] bool? trangThai = null)
        {
            var query = _context.DmNhanViens.Include(nv => nv.MaKhoaNavigation).AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(nv => nv.HoTen.Contains(keyword) ||
                                         nv.MaNv.Contains(keyword) ||
                                         (nv.Email != null && nv.Email.Contains(keyword)));
            }

            if (!string.IsNullOrEmpty(chucVu))
            {
                query = query.Where(nv => nv.ChucVu == chucVu);
            }

            if (!string.IsNullOrEmpty(maKhoa))
            {
                query = query.Where(nv => nv.MaKhoa == maKhoa);
            }

            if (trangThai.HasValue)
            {
                query = query.Where(nv => nv.TrangThai == trangThai);
            }

            return await query.ToListAsync();
        }

        // GET: api/DmNhanVien/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DmNhanVien>> GetDmNhanVien(string id)
        {
            var dmNhanVien = await _context.DmNhanViens
                .Include(nv => nv.MaKhoaNavigation)
                .FirstOrDefaultAsync(nv => nv.MaNv == id);

            if (dmNhanVien == null)
            {
                return NotFound($"Không tìm thấy nhân viên với mã: {id}");
            }

            return dmNhanVien;
        }

        // GET: api/DmNhanVien/5/details
        [HttpGet("{id}/details")]
        public async Task<ActionResult<DmNhanVien>> GetDmNhanVienDetails(string id)
        {
            var dmNhanVien = await _context.DmNhanViens
                .Include(nv => nv.MaKhoaNavigation)
                .Include(nv => nv.BenhAnNgoaiTrus)
                .Include(nv => nv.ChiDinhXetNghiems)
                .Include(nv => nv.DangKyKhams)
                .Include(nv => nv.DonThuocs)
                .Include(nv => nv.HoaDons)
                .Include(nv => nv.KetQuaXetNghiems)
                .Include(nv => nv.LogHoatDongs)
                .Include(nv => nv.NhapViens)
                .Include(nv => nv.PhieuNhapKhos)
                .Include(nv => nv.PhieuXuatKhos)
                .FirstOrDefaultAsync(nv => nv.MaNv == id);

            if (dmNhanVien == null)
            {
                return NotFound($"Không tìm thấy nhân viên với mã: {id}");
            }

            return dmNhanVien;
        }

        // PUT: api/DmNhanVien/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDmNhanVien(string id, DmNhanVien dmNhanVien)
        {
            if (id != dmNhanVien.MaNv)
            {
                return BadRequest("Mã nhân viên không khớp");
            }

            // Kiểm tra xem nhân viên có tồn tại không
            var existingNhanVien = await _context.DmNhanViens.FindAsync(id);
            if (existingNhanVien == null)
            {
                return NotFound($"Không tìm thấy nhân viên với mã: {id}");
            }

            // Kiểm tra Email có trùng không (nếu có)
            if (!string.IsNullOrEmpty(dmNhanVien.Email))
            {
                var duplicateEmail = await _context.DmNhanViens
                    .AnyAsync(nv => nv.Email == dmNhanVien.Email && nv.MaNv != id);
                if (duplicateEmail)
                {
                    return BadRequest("Email đã tồn tại");
                }
            }

            // Kiểm tra mã khoa có tồn tại không
            if (!string.IsNullOrEmpty(dmNhanVien.MaKhoa))
            {
                var khoaExists = await _context.DmKhoaPhongs.AnyAsync(k => k.MaKhoa == dmNhanVien.MaKhoa);
                if (!khoaExists)
                {
                    return BadRequest("Mã khoa không tồn tại");
                }
            }

            _context.Entry(existingNhanVien).CurrentValues.SetValues(dmNhanVien);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DmNhanVienExists(id))
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

        // POST: api/DmNhanVien
        [HttpPost]
        public async Task<ActionResult<DmNhanVien>> PostDmNhanVien(DmNhanVien dmNhanVien)
        {
            // Kiểm tra mã nhân viên đã tồn tại
            if (await _context.DmNhanViens.AnyAsync(nv => nv.MaNv == dmNhanVien.MaNv))
            {
                return BadRequest("Mã nhân viên đã tồn tại");
            }

            // Kiểm tra Email có trùng không (nếu có)
            if (!string.IsNullOrEmpty(dmNhanVien.Email))
            {
                var duplicateEmail = await _context.DmNhanViens
                    .AnyAsync(nv => nv.Email == dmNhanVien.Email);
                if (duplicateEmail)
                {
                    return BadRequest("Email đã tồn tại");
                }
            }

            // Kiểm tra mã khoa có tồn tại không
            if (!string.IsNullOrEmpty(dmNhanVien.MaKhoa))
            {
                var khoaExists = await _context.DmKhoaPhongs.AnyAsync(k => k.MaKhoa == dmNhanVien.MaKhoa);
                if (!khoaExists)
                {
                    return BadRequest("Mã khoa không tồn tại");
                }
            }

            // Thiết lập giá trị mặc định
            dmNhanVien.NgayTao = DateTime.Now;
            dmNhanVien.TrangThai = dmNhanVien.TrangThai ?? true;

            _context.DmNhanViens.Add(dmNhanVien);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DmNhanVienExists(dmNhanVien.MaNv))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetDmNhanVien", new { id = dmNhanVien.MaNv }, dmNhanVien);
        }

        // DELETE: api/DmNhanVien/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDmNhanVien(string id)
        {
            var dmNhanVien = await _context.DmNhanViens.FindAsync(id);
            if (dmNhanVien == null)
            {
                return NotFound($"Không tìm thấy nhân viên với mã: {id}");
            }

            // Kiểm tra xem nhân viên có dữ liệu liên quan không
            var hasRelatedData = await _context.BenhAnNgoaiTrus.AnyAsync(ba => ba.MaBacSi == id) ||
                                await _context.ChiDinhXetNghiems.AnyAsync(cd => cd.MaBacSi == id) ||
                                await _context.DangKyKhams.AnyAsync(dk => dk.MaBacSi == id) ||
                                await _context.DonThuocs.AnyAsync(dt => dt.MaBacSi == id) ||
                                await _context.HoaDons.AnyAsync(hd => hd.NguoiThu == id) ||
                                await _context.KetQuaXetNghiems.AnyAsync(kq => kq.NguoiThucHien == id) ||
                                await _context.LogHoatDongs.AnyAsync(log => log.MaNv == id) ||
                                await _context.NhapViens.AnyAsync(nv => nv.MaBacSi == id) ||
                                await _context.PhieuNhapKhos.AnyAsync(pn => pn.NguoiNhap == id) ||
                                await _context.PhieuXuatKhos.AnyAsync(px => px.NguoiXuat == id);

            if (hasRelatedData)
            {
                return BadRequest("Không thể xóa nhân viên này vì có dữ liệu liên quan");
            }

            _context.DmNhanViens.Remove(dmNhanVien);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PATCH: api/DmNhanVien/5/status
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(string id, [FromBody] bool trangThai)
        {
            var dmNhanVien = await _context.DmNhanViens.FindAsync(id);
            if (dmNhanVien == null)
            {
                return NotFound($"Không tìm thấy nhân viên với mã: {id}");
            }

            dmNhanVien.TrangThai = trangThai;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/DmNhanVien/doctors
        [HttpGet("doctors")]
        public async Task<ActionResult<IEnumerable<DmNhanVien>>> GetDoctors()
        {
            return await _context.DmNhanViens
                .Include(nv => nv.MaKhoaNavigation)
                .Where(nv => nv.ChucVu == "Bác sĩ" || nv.ChucVu == "Bác sĩ chuyên khoa")
                .Where(nv => nv.TrangThai == true)
                .ToListAsync();
        }

        // GET: api/DmNhanVien/by-khoa/5
        [HttpGet("by-khoa/{maKhoa}")]
        public async Task<ActionResult<IEnumerable<DmNhanVien>>> GetNhanVienByKhoa(string maKhoa)
        {
            return await _context.DmNhanViens
                .Include(nv => nv.MaKhoaNavigation)
                .Where(nv => nv.MaKhoa == maKhoa && nv.TrangThai == true)
                .ToListAsync();
        }

        // GET: api/DmNhanVien/statistics
        [HttpGet("statistics")]
        public async Task<ActionResult<object>> GetStatistics()
        {
            var totalNhanVien = await _context.DmNhanViens.CountAsync();
            var activeNhanVien = await _context.DmNhanViens.CountAsync(nv => nv.TrangThai == true);
            var inactiveNhanVien = totalNhanVien - activeNhanVien;

            var nhanVienByChucVu = await _context.DmNhanViens
                .Where(nv => nv.TrangThai == true)
                .GroupBy(nv => nv.ChucVu)
                .Select(g => new { ChucVu = g.Key, SoLuong = g.Count() })
                .ToListAsync();

            var nhanVienByKhoa = await _context.DmNhanViens
                .Where(nv => nv.TrangThai == true)
                .Include(nv => nv.MaKhoaNavigation)
                .GroupBy(nv => new { nv.MaKhoa, nv.MaKhoaNavigation.TenKhoa })
                .Select(g => new {
                    MaKhoa = g.Key.MaKhoa,
                    TenKhoa = g.Key.TenKhoa,
                    SoLuong = g.Count()
                })
                .ToListAsync();

            return new
            {
                TongNhanVien = totalNhanVien,
                NhanVienHoatDong = activeNhanVien,
                NhanVienKhongHoatDong = inactiveNhanVien,
                TheoChucVu = nhanVienByChucVu,
                TheoKhoa = nhanVienByKhoa
            };
        }

        private bool DmNhanVienExists(string id)
        {
            return _context.DmNhanViens.Any(e => e.MaNv == id);
        }
    }
}