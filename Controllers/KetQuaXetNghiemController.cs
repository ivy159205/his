using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KetQuaXetNghiemController : ControllerBase
    {
        private readonly DBCHIS _context;

        public KetQuaXetNghiemController(DBCHIS context)
        {
            _context = context;
        }

        // GET: api/KetQuaXetNghiem
        [HttpGet]
        public async Task<ActionResult<IEnumerable<KetQuaXetNghiem>>> GetKetQuaXetNghiems()
        {
            return await _context.KetQuaXetNghiems
                .Include(k => k.MaChiDinhNavigation)
                .Include(k => k.NguoiThucHienNavigation)
                .ToListAsync();
        }

        // GET: api/KetQuaXetNghiem/5
        [HttpGet("{id}")]
        public async Task<ActionResult<KetQuaXetNghiem>> GetKetQuaXetNghiem(string id)
        {
            var ketQuaXetNghiem = await _context.KetQuaXetNghiems
                .Include(k => k.MaChiDinhNavigation)
                .ThenInclude(cd => cd.MaBnNavigation)
                .Include(k => k.NguoiThucHienNavigation)
                .FirstOrDefaultAsync(k => k.MaKetQua == id);

            if (ketQuaXetNghiem == null)
            {
                return NotFound();
            }

            return ketQuaXetNghiem;
        }

        // GET: api/KetQuaXetNghiem/ChiDinh/5
        [HttpGet("ChiDinh/{maChiDinh}")]
        public async Task<ActionResult<IEnumerable<KetQuaXetNghiem>>> GetKetQuaXetNghiemsByChiDinh(string maChiDinh)
        {
            var ketQuaXetNghiems = await _context.KetQuaXetNghiems
                .Where(k => k.MaChiDinh == maChiDinh)
                .Include(k => k.MaChiDinhNavigation)
                .Include(k => k.NguoiThucHienNavigation)
                .OrderByDescending(k => k.NgayThucHien)
                .ToListAsync();

            return ketQuaXetNghiems;
        }

        // GET: api/KetQuaXetNghiem/BenhNhan/5
        [HttpGet("BenhNhan/{maBn}")]
        public async Task<ActionResult<IEnumerable<KetQuaXetNghiem>>> GetKetQuaXetNghiemsByBenhNhan(string maBn)
        {
            var ketQuaXetNghiems = await _context.KetQuaXetNghiems
                .Include(k => k.MaChiDinhNavigation)
                .ThenInclude(cd => cd.MaBnNavigation)
                .Include(k => k.NguoiThucHienNavigation)
                .Where(k => k.MaChiDinhNavigation.MaBn == maBn)
                .OrderByDescending(k => k.NgayThucHien)
                .ToListAsync();

            return ketQuaXetNghiems;
        }

        // GET: api/KetQuaXetNghiem/NgayThucHien/2024-01-01/2024-12-31
        [HttpGet("NgayThucHien/{tuNgay}/{denNgay}")]
        public async Task<ActionResult<IEnumerable<KetQuaXetNghiem>>> GetKetQuaXetNghiemsByDateRange(DateTime tuNgay, DateTime denNgay)
        {
            var ketQuaXetNghiems = await _context.KetQuaXetNghiems
                .Where(k => k.NgayThucHien >= tuNgay && k.NgayThucHien <= denNgay)
                .Include(k => k.MaChiDinhNavigation)
                .ThenInclude(cd => cd.MaBnNavigation)
                .Include(k => k.NguoiThucHienNavigation)
                .OrderByDescending(k => k.NgayThucHien)
                .ToListAsync();

            return ketQuaXetNghiems;
        }

        // GET: api/KetQuaXetNghiem/TrangThai/DaThucHien
        [HttpGet("TrangThai/{trangThai}")]
        public async Task<ActionResult<IEnumerable<KetQuaXetNghiem>>> GetKetQuaXetNghiemsByTrangThai(string trangThai)
        {
            var ketQuaXetNghiems = await _context.KetQuaXetNghiems
                .Where(k => k.TrangThai == trangThai)
                .Include(k => k.MaChiDinhNavigation)
                .ThenInclude(cd => cd.MaBnNavigation)
                .Include(k => k.NguoiThucHienNavigation)
                .OrderByDescending(k => k.NgayThucHien)
                .ToListAsync();

            return ketQuaXetNghiems;
        }

        // GET: api/KetQuaXetNghiem/NguoiThucHien/5
        [HttpGet("NguoiThucHien/{nguoiThucHien}")]
        public async Task<ActionResult<IEnumerable<KetQuaXetNghiem>>> GetKetQuaXetNghiemsByNguoiThucHien(string nguoiThucHien)
        {
            var ketQuaXetNghiems = await _context.KetQuaXetNghiems
                .Where(k => k.NguoiThucHien == nguoiThucHien)
                .Include(k => k.MaChiDinhNavigation)
                .ThenInclude(cd => cd.MaBnNavigation)
                .Include(k => k.NguoiThucHienNavigation)
                .OrderByDescending(k => k.NgayThucHien)
                .ToListAsync();

            return ketQuaXetNghiems;
        }

        // GET: api/KetQuaXetNghiem/ChuaDuyet
        [HttpGet("ChuaDuyet")]
        public async Task<ActionResult<IEnumerable<KetQuaXetNghiem>>> GetKetQuaXetNghiemsChuaDuyet()
        {
            var ketQuaXetNghiems = await _context.KetQuaXetNghiems
                .Where(k => k.NgayDuyet == null || k.NguoiDuyet == null)
                .Include(k => k.MaChiDinhNavigation)
                .ThenInclude(cd => cd.MaBnNavigation)
                .Include(k => k.NguoiThucHienNavigation)
                .OrderByDescending(k => k.NgayThucHien)
                .ToListAsync();

            return ketQuaXetNghiems;
        }

        // PUT: api/KetQuaXetNghiem/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKetQuaXetNghiem(string id, KetQuaXetNghiem ketQuaXetNghiem)
        {
            if (id != ketQuaXetNghiem.MaKetQua)
            {
                return BadRequest();
            }

            _context.Entry(ketQuaXetNghiem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KetQuaXetNghiemExists(id))
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

        // POST: api/KetQuaXetNghiem
        [HttpPost]
        public async Task<ActionResult<KetQuaXetNghiem>> PostKetQuaXetNghiem(KetQuaXetNghiem ketQuaXetNghiem)
        {
            // Kiểm tra xem mã kết quả đã tồn tại chưa
            if (KetQuaXetNghiemExists(ketQuaXetNghiem.MaKetQua))
            {
                return BadRequest("Mã kết quả đã tồn tại");
            }

            // Kiểm tra xem chỉ định xét nghiệm có tồn tại không
            var chiDinh = await _context.ChiDinhXetNghiems.FindAsync(ketQuaXetNghiem.MaChiDinh);
            if (chiDinh == null)
            {
                return BadRequest("Chỉ định xét nghiệm không tồn tại");
            }

            // Kiểm tra người thực hiện (nếu có)
            if (!string.IsNullOrEmpty(ketQuaXetNghiem.NguoiThucHien))
            {
                var nhanVien = await _context.DmNhanViens.FindAsync(ketQuaXetNghiem.NguoiThucHien);
                if (nhanVien == null)
                {
                    return BadRequest("Nhân viên thực hiện không tồn tại");
                }
            }

            // Tự động gán NgayTao và NgayThucHien nếu chưa có
            if (ketQuaXetNghiem.NgayTao == null)
            {
                ketQuaXetNghiem.NgayTao = DateTime.Now;
            }

            if (ketQuaXetNghiem.NgayThucHien == null)
            {
                ketQuaXetNghiem.NgayThucHien = DateTime.Now;
            }

            // Tự động gán trạng thái nếu chưa có
            if (string.IsNullOrEmpty(ketQuaXetNghiem.TrangThai))
            {
                ketQuaXetNghiem.TrangThai = "DaThucHien";
            }

            _context.KetQuaXetNghiems.Add(ketQuaXetNghiem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetKetQuaXetNghiem", new { id = ketQuaXetNghiem.MaKetQua }, ketQuaXetNghiem);
        }

        // DELETE: api/KetQuaXetNghiem/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKetQuaXetNghiem(string id)
        {
            var ketQuaXetNghiem = await _context.KetQuaXetNghiems.FindAsync(id);
            if (ketQuaXetNghiem == null)
            {
                return NotFound();
            }

            _context.KetQuaXetNghiems.Remove(ketQuaXetNghiem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/KetQuaXetNghiem/CapNhatKetQua/5
        [HttpPut("CapNhatKetQua/{id}")]
        public async Task<IActionResult> CapNhatKetQua(string id, [FromBody] CapNhatKetQuaRequest request)
        {
            var ketQuaXetNghiem = await _context.KetQuaXetNghiems.FindAsync(id);
            if (ketQuaXetNghiem == null)
            {
                return NotFound();
            }

            ketQuaXetNghiem.KetQua = request.KetQua;
            ketQuaXetNghiem.KetLuan = request.KetLuan;
            ketQuaXetNghiem.NguoiThucHien = request.NguoiThucHien;
            ketQuaXetNghiem.NgayThucHien = DateTime.Now;
            ketQuaXetNghiem.TrangThai = "DaThucHien";

            _context.Entry(ketQuaXetNghiem).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/KetQuaXetNghiem/DuyetKetQua/5
        [HttpPut("DuyetKetQua/{id}")]
        public async Task<IActionResult> DuyetKetQua(string id, [FromBody] DuyetKetQuaRequest request)
        {
            var ketQuaXetNghiem = await _context.KetQuaXetNghiems.FindAsync(id);
            if (ketQuaXetNghiem == null)
            {
                return NotFound();
            }

            // Kiểm tra người duyệt có tồn tại không
            var nguoiDuyet = await _context.DmNhanViens.FindAsync(request.NguoiDuyet);
            if (nguoiDuyet == null)
            {
                return BadRequest("Người duyệt không tồn tại");
            }

            ketQuaXetNghiem.NguoiDuyet = request.NguoiDuyet;
            ketQuaXetNghiem.NgayDuyet = DateTime.Now;
            ketQuaXetNghiem.TrangThai = "DaDuyet";

            _context.Entry(ketQuaXetNghiem).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/KetQuaXetNghiem/ThongKe/TheoNgay/2024-01-01/2024-12-31
        [HttpGet("ThongKe/TheoNgay/{tuNgay}/{denNgay}")]
        public async Task<ActionResult<object>> GetThongKeTheoNgay(DateTime tuNgay, DateTime denNgay)
        {
            var thongKe = await _context.KetQuaXetNghiems
                .Where(k => k.NgayThucHien >= tuNgay && k.NgayThucHien <= denNgay)
                .GroupBy(k => k.NgayThucHien.Value.Date)
                .Select(g => new
                {
                    Ngay = g.Key,
                    SoLuongXetNghiem = g.Count(),
                    SoLuongDaThucHien = g.Count(x => x.TrangThai == "DaThucHien"),
                    SoLuongDaDuyet = g.Count(x => x.TrangThai == "DaDuyet"),
                    SoLuongChuaDuyet = g.Count(x => x.NgayDuyet == null)
                })
                .OrderBy(x => x.Ngay)
                .ToListAsync();

            return thongKe;
        }

        // GET: api/KetQuaXetNghiem/ThongKe/TheoTrangThai
        [HttpGet("ThongKe/TheoTrangThai")]
        public async Task<ActionResult<object>> GetThongKeTheoTrangThai()
        {
            var thongKe = await _context.KetQuaXetNghiems
                .GroupBy(k => k.TrangThai)
                .Select(g => new
                {
                    TrangThai = g.Key,
                    SoLuong = g.Count(),
                    PhanTram = (double)g.Count() / _context.KetQuaXetNghiems.Count() * 100
                })
                .ToListAsync();

            return thongKe;
        }

        // GET: api/KetQuaXetNghiem/ThongKe/TheoNguoiThucHien
        [HttpGet("ThongKe/TheoNguoiThucHien")]
        public async Task<ActionResult<object>> GetThongKeTheoNguoiThucHien()
        {
            var thongKe = await _context.KetQuaXetNghiems
                .Where(k => k.NguoiThucHien != null)
                .GroupBy(k => k.NguoiThucHien)
                .Select(g => new
                {
                    NguoiThucHien = g.Key,
                    TenNguoiThucHien = g.First().NguoiThucHienNavigation.HoTen,
                    SoLuongXetNghiem = g.Count(),
                    SoLuongDaDuyet = g.Count(x => x.TrangThai == "DaDuyet"),
                    TyLeDuyet = g.Count(x => x.TrangThai == "DaDuyet") * 100.0 / g.Count()
                })
                .OrderByDescending(x => x.SoLuongXetNghiem)
                .ToListAsync();

            return thongKe;
        }

        private bool KetQuaXetNghiemExists(string id)
        {
            return _context.KetQuaXetNghiems.Any(e => e.MaKetQua == id);
        }
    }

    // DTOs cho request
    public class CapNhatKetQuaRequest
    {
        public string KetQua { get; set; } = null!;
        public string? KetLuan { get; set; }
        public string NguoiThucHien { get; set; } = null!;
    }

    public class DuyetKetQuaRequest
    {
        public string NguoiDuyet { get; set; } = null!;
    }
}