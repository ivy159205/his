using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BenhAnNoiTruController : ControllerBase
    {
        private readonly DBCHIS _context;

        public BenhAnNoiTruController(DBCHIS context)
        {
            _context = context;
        }

        // GET: api/BenhAnNoiTru
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BenhAnNoiTru>>> GetBenhAnNoiTru()
        {
            return await _context.BenhAnNoiTrus
                .Include(b => b.MaBnNavigation)
                .Include(b => b.MaNhapVienNavigation)
                .ToListAsync();
        }

        // GET: api/BenhAnNoiTru/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BenhAnNoiTru>> GetBenhAnNoiTru(string id)
        {
            var benhAnNoiTru = await _context.BenhAnNoiTrus
                .Include(b => b.MaBnNavigation)
                .Include(b => b.MaNhapVienNavigation)
                .FirstOrDefaultAsync(b => b.MaBanoiTru == id);

            if (benhAnNoiTru == null)
            {
                return NotFound();
            }

            return benhAnNoiTru;
        }

        // GET: api/BenhAnNoiTru/benhnhan/{maBn}
        [HttpGet("benhnhan/{maBn}")]
        public async Task<ActionResult<IEnumerable<BenhAnNoiTru>>> GetBenhAnByBenhNhan(string maBn)
        {
            var benhAnList = await _context.BenhAnNoiTrus
                .Include(b => b.MaBnNavigation)
                .Include(b => b.MaNhapVienNavigation)
                .Where(b => b.MaBn == maBn)
                .OrderByDescending(b => b.NgayNhapVien)
                .ToListAsync();

            return benhAnList;
        }

        // GET: api/BenhAnNoiTru/nhapvien/{maNhapVien}
        [HttpGet("nhapvien/{maNhapVien}")]
        public async Task<ActionResult<IEnumerable<BenhAnNoiTru>>> GetBenhAnByNhapVien(string maNhapVien)
        {
            var benhAnList = await _context.BenhAnNoiTrus
                .Include(b => b.MaBnNavigation)
                .Include(b => b.MaNhapVienNavigation)
                .Where(b => b.MaNhapVien == maNhapVien)
                .OrderByDescending(b => b.NgayNhapVien)
                .ToListAsync();

            return benhAnList;
        }

        // GET: api/BenhAnNoiTru/theongay
        [HttpGet("theongay")]
        public async Task<ActionResult<IEnumerable<BenhAnNoiTru>>> GetBenhAnByNgayNhapVien([FromQuery] DateTime tuNgay, [FromQuery] DateTime denNgay)
        {
            var benhAnList = await _context.BenhAnNoiTrus
                .Include(b => b.MaBnNavigation)
                .Include(b => b.MaNhapVienNavigation)
                .Where(b => b.NgayNhapVien.HasValue &&
                           b.NgayNhapVien.Value.Date >= tuNgay.Date &&
                           b.NgayNhapVien.Value.Date <= denNgay.Date)
                .OrderBy(b => b.NgayNhapVien)
                .ToListAsync();

            return benhAnList;
        }

        // GET: api/BenhAnNoiTru/dangdieutri
        [HttpGet("dangdieutri")]
        public async Task<ActionResult<IEnumerable<BenhAnNoiTru>>> GetBenhAnDangDieuTri()
        {
            var benhAnList = await _context.BenhAnNoiTrus
                .Include(b => b.MaBnNavigation)
                .Include(b => b.MaNhapVienNavigation)
                .Where(b => b.NgayXuatVien == null || b.TrangThai == "Đang điều trị")
                .OrderBy(b => b.NgayNhapVien)
                .ToListAsync();

            return benhAnList;
        }

        // GET: api/BenhAnNoiTru/daxuatvien
        [HttpGet("daxuatvien")]
        public async Task<ActionResult<IEnumerable<BenhAnNoiTru>>> GetBenhAnDaXuatVien()
        {
            var benhAnList = await _context.BenhAnNoiTrus
                .Include(b => b.MaBnNavigation)
                .Include(b => b.MaNhapVienNavigation)
                .Where(b => b.NgayXuatVien.HasValue)
                .OrderByDescending(b => b.NgayXuatVien)
                .ToListAsync();

            return benhAnList;
        }

        // POST: api/BenhAnNoiTru
        [HttpPost]
        public async Task<ActionResult<BenhAnNoiTru>> PostBenhAnNoiTru(BenhAnNoiTru benhAnNoiTru)
        {
            // Kiểm tra dữ liệu đầu vào
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Kiểm tra xem MaBanoiTru đã tồn tại chưa
            if (await _context.BenhAnNoiTrus.AnyAsync(b => b.MaBanoiTru == benhAnNoiTru.MaBanoiTru))
            {
                return Conflict("Mã bệnh án nội trú đã tồn tại");
            }

            // Kiểm tra sự tồn tại của các khóa ngoại
            if (!await _context.BenhNhans.AnyAsync(b => b.MaBn == benhAnNoiTru.MaBn))
            {
                return BadRequest("Mã bệnh nhân không tồn tại");
            }

            if (!await _context.NhapViens.AnyAsync(n => n.MaNhapVien == benhAnNoiTru.MaNhapVien))
            {
                return BadRequest("Mã nhập viện không tồn tại");
            }

            // Tính số ngày điều trị nếu có ngày xuất viện
            if (benhAnNoiTru.NgayNhapVien.HasValue && benhAnNoiTru.NgayXuatVien.HasValue)
            {
                benhAnNoiTru.SoNgayDieuTri = (int)(benhAnNoiTru.NgayXuatVien.Value - benhAnNoiTru.NgayNhapVien.Value).TotalDays;
            }

            benhAnNoiTru.NgayTao = DateTime.Now;
            _context.BenhAnNoiTrus.Add(benhAnNoiTru);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBenhAnNoiTru), new { id = benhAnNoiTru.MaBanoiTru }, benhAnNoiTru);
        }

        // PUT: api/BenhAnNoiTru/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBenhAnNoiTru(string id, BenhAnNoiTru benhAnNoiTru)
        {
            if (id != benhAnNoiTru.MaBanoiTru)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Kiểm tra sự tồn tại của bệnh án
            if (!await _context.BenhAnNoiTrus.AnyAsync(b => b.MaBanoiTru == id))
            {
                return NotFound();
            }

            // Kiểm tra sự tồn tại của các khóa ngoại
            if (!await _context.BenhNhans.AnyAsync(b => b.MaBn == benhAnNoiTru.MaBn))
            {
                return BadRequest("Mã bệnh nhân không tồn tại");
            }

            if (!await _context.NhapViens.AnyAsync(n => n.MaNhapVien == benhAnNoiTru.MaNhapVien))
            {
                return BadRequest("Mã nhập viện không tồn tại");
            }

            // Tính lại số ngày điều trị nếu có thay đổi
            if (benhAnNoiTru.NgayNhapVien.HasValue && benhAnNoiTru.NgayXuatVien.HasValue)
            {
                benhAnNoiTru.SoNgayDieuTri = (int)(benhAnNoiTru.NgayXuatVien.Value - benhAnNoiTru.NgayNhapVien.Value).TotalDays;
            }

            _context.Entry(benhAnNoiTru).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await BenhAnNoiTruExists(id))
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

        // PUT: api/BenhAnNoiTru/xuatvien/5
        [HttpPut("xuatvien/{id}")]
        public async Task<IActionResult> XuatVien(string id, [FromBody] XuatVienRequest request)
        {
            var benhAn = await _context.BenhAnNoiTrus.FindAsync(id);
            if (benhAn == null)
            {
                return NotFound();
            }

            if (benhAn.NgayXuatVien.HasValue)
            {
                return BadRequest("Bệnh nhân đã được xuất viện");
            }

            benhAn.NgayXuatVien = request.NgayXuatVien;
            benhAn.ChanDoanXuatVien = request.ChanDoanXuatVien;
            benhAn.TinhTrangXuatVien = request.TinhTrangXuatVien;
            benhAn.HuongDieuTri = request.HuongDieuTri;
            benhAn.KetQuaDieuTri = request.KetQuaDieuTri;
            benhAn.TrangThai = "Đã xuất viện";

            // Tính số ngày điều trị
            if (benhAn.NgayNhapVien.HasValue)
            {
                benhAn.SoNgayDieuTri = (int)(benhAn.NgayXuatVien.Value - benhAn.NgayNhapVien.Value).TotalDays;
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/BenhAnNoiTru/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBenhAnNoiTru(string id)
        {
            var benhAnNoiTru = await _context.BenhAnNoiTrus.FindAsync(id);
            if (benhAnNoiTru == null)
            {
                return NotFound();
            }

            _context.BenhAnNoiTrus.Remove(benhAnNoiTru);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/BenhAnNoiTru/thongke/theongay
        [HttpGet("thongke/theongay")]
        public async Task<ActionResult> GetThongKeTheoNgay([FromQuery] DateTime tuNgay, [FromQuery] DateTime denNgay)
        {
            var thongKeNhapVien = await _context.BenhAnNoiTrus
                .Where(b => b.NgayNhapVien.HasValue &&
                           b.NgayNhapVien.Value.Date >= tuNgay.Date &&
                           b.NgayNhapVien.Value.Date <= denNgay.Date)
                .GroupBy(b => b.NgayNhapVien.Value.Date)
                .Select(g => new
                {
                    NgayNhapVien = g.Key,
                    SoLuongNhapVien = g.Count()
                })
                .OrderBy(x => x.NgayNhapVien)
                .ToListAsync();

            var thongKeXuatVien = await _context.BenhAnNoiTrus
                .Where(b => b.NgayXuatVien.HasValue &&
                           b.NgayXuatVien.Value.Date >= tuNgay.Date &&
                           b.NgayXuatVien.Value.Date <= denNgay.Date)
                .GroupBy(b => b.NgayXuatVien.Value.Date)
                .Select(g => new
                {
                    NgayXuatVien = g.Key,
                    SoLuongXuatVien = g.Count()
                })
                .OrderBy(x => x.NgayXuatVien)
                .ToListAsync();

            return Ok(new { ThongKeNhapVien = thongKeNhapVien, ThongKeXuatVien = thongKeXuatVien });
        }

        // GET: api/BenhAnNoiTru/thongke/ketqua
        [HttpGet("thongke/ketqua")]
        public async Task<ActionResult> GetThongKeKetQuaDieuTri([FromQuery] DateTime tuNgay, [FromQuery] DateTime denNgay)
        {
            var thongKe = await _context.BenhAnNoiTrus
                .Where(b => b.NgayXuatVien.HasValue &&
                           b.NgayXuatVien.Value.Date >= tuNgay.Date &&
                           b.NgayXuatVien.Value.Date <= denNgay.Date)
                .GroupBy(b => b.KetQuaDieuTri)
                .Select(g => new
                {
                    KetQuaDieuTri = g.Key ?? "Chưa xác định",
                    SoLuong = g.Count()
                })
                .OrderByDescending(x => x.SoLuong)
                .ToListAsync();

            return Ok(thongKe);
        }

        // GET: api/BenhAnNoiTru/thongke/songaydieutri
        [HttpGet("thongke/songaydieutri")]
        public async Task<ActionResult> GetThongKeSoNgayDieuTri([FromQuery] DateTime tuNgay, [FromQuery] DateTime denNgay)
        {
            var thongKe = await _context.BenhAnNoiTrus
                .Where(b => b.NgayXuatVien.HasValue &&
                           b.NgayXuatVien.Value.Date >= tuNgay.Date &&
                           b.NgayXuatVien.Value.Date <= denNgay.Date &&
                           b.SoNgayDieuTri.HasValue)
                .GroupBy(b => b.SoNgayDieuTri.Value)
                .Select(g => new
                {
                    SoNgayDieuTri = g.Key,
                    SoLuong = g.Count()
                })
                .OrderBy(x => x.SoNgayDieuTri)
                .ToListAsync();

            var trungBinhSoNgay = await _context.BenhAnNoiTrus
                .Where(b => b.NgayXuatVien.HasValue &&
                           b.NgayXuatVien.Value.Date >= tuNgay.Date &&
                           b.NgayXuatVien.Value.Date <= denNgay.Date &&
                           b.SoNgayDieuTri.HasValue)
                .AverageAsync(b => b.SoNgayDieuTri.Value);

            return Ok(new { ThongKe = thongKe, TrungBinhSoNgay = Math.Round(trungBinhSoNgay, 2) });
        }

        private async Task<bool> BenhAnNoiTruExists(string id)
        {
            return await _context.BenhAnNoiTrus.AnyAsync(e => e.MaBanoiTru == id);
        }
    }

    // DTO cho xuất viện
    public class XuatVienRequest
    {
        public DateTime NgayXuatVien { get; set; }
        public string? ChanDoanXuatVien { get; set; }
        public string? TinhTrangXuatVien { get; set; }
        public string? HuongDieuTri { get; set; }
        public string? KetQuaDieuTri { get; set; }
    }
}