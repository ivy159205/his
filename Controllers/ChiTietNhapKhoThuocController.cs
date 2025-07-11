using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChiTietNhapKhoThuocController : ControllerBase
    {
        private readonly DBCHIS _context;

        public ChiTietNhapKhoThuocController(DBCHIS context)
        {
            _context = context;
        }

        // GET: api/ChiTietNhapKhoThuoc
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChiTietNhapKhoThuoc>>> GetChiTietNhapKhoThuocs()
        {
            return await _context.ChiTietNhapKhoThuocs
                .Include(c => c.MaPhieuNhapNavigation)
                .Include(c => c.MaThuocNavigation)
                .ToListAsync();
        }

        // GET: api/ChiTietNhapKhoThuoc/PN001/TH001/LO001
        [HttpGet("{maPhieuNhap}/{maThuoc}/{soLo}")]
        public async Task<ActionResult<ChiTietNhapKhoThuoc>> GetChiTietNhapKhoThuoc(string maPhieuNhap, string maThuoc, string soLo)
        {
            var chiTietNhapKhoThuoc = await _context.ChiTietNhapKhoThuocs
                .Include(c => c.MaPhieuNhapNavigation)
                .Include(c => c.MaThuocNavigation)
                .FirstOrDefaultAsync(c => c.MaPhieuNhap == maPhieuNhap && c.MaThuoc == maThuoc && c.SoLo == soLo);

            if (chiTietNhapKhoThuoc == null)
            {
                return NotFound();
            }

            return chiTietNhapKhoThuoc;
        }

        // GET: api/ChiTietNhapKhoThuoc/phieunhap/PN001
        [HttpGet("phieunhap/{maPhieuNhap}")]
        public async Task<ActionResult<IEnumerable<ChiTietNhapKhoThuoc>>> GetChiTietNhapKhoThuocsByMaPhieuNhap(string maPhieuNhap)
        {
            var chiTietNhapKhoThuocs = await _context.ChiTietNhapKhoThuocs
                .Include(c => c.MaThuocNavigation)
                .Where(c => c.MaPhieuNhap == maPhieuNhap)
                .ToListAsync();

            return chiTietNhapKhoThuocs;
        }

        // GET: api/ChiTietNhapKhoThuoc/thuoc/TH001
        [HttpGet("thuoc/{maThuoc}")]
        public async Task<ActionResult<IEnumerable<ChiTietNhapKhoThuoc>>> GetChiTietNhapKhoThuocsByMaThuoc(string maThuoc)
        {
            var chiTietNhapKhoThuocs = await _context.ChiTietNhapKhoThuocs
                .Include(c => c.MaPhieuNhapNavigation)
                .Where(c => c.MaThuoc == maThuoc)
                .OrderBy(c => c.HanSuDung)
                .ToListAsync();

            return chiTietNhapKhoThuocs;
        }

        // GET: api/ChiTietNhapKhoThuoc/thuoc/TH001/solo/LO001
        [HttpGet("thuoc/{maThuoc}/solo/{soLo}")]
        public async Task<ActionResult<IEnumerable<ChiTietNhapKhoThuoc>>> GetChiTietNhapKhoThuocsByMaThuocAndSoLo(string maThuoc, string soLo)
        {
            var chiTietNhapKhoThuocs = await _context.ChiTietNhapKhoThuocs
                .Include(c => c.MaPhieuNhapNavigation)
                .Where(c => c.MaThuoc == maThuoc && c.SoLo == soLo)
                .OrderBy(c => c.HanSuDung)
                .ToListAsync();

            return chiTietNhapKhoThuocs;
        }

        // GET: api/ChiTietNhapKhoThuoc/hethan
        [HttpGet("hethan")]
        public async Task<ActionResult<IEnumerable<ChiTietNhapKhoThuoc>>> GetThuocHetHan([FromQuery] int soNgay = 30)
        {
            var ngayHienTai = DateOnly.FromDateTime(DateTime.Now);
            var ngayKiemTra = ngayHienTai.AddDays(soNgay);

            var thuocHetHan = await _context.ChiTietNhapKhoThuocs
                .Include(c => c.MaThuocNavigation)
                .Include(c => c.MaPhieuNhapNavigation)
                .Where(c => c.HanSuDung <= ngayKiemTra && c.HanSuDung >= ngayHienTai)
                .OrderBy(c => c.HanSuDung)
                .ToListAsync();

            return thuocHetHan;
        }

        // GET: api/ChiTietNhapKhoThuoc/dahhethan
        [HttpGet("dahethan")]
        public async Task<ActionResult<IEnumerable<ChiTietNhapKhoThuoc>>> GetThuocDaHetHan()
        {
            var ngayHienTai = DateOnly.FromDateTime(DateTime.Now);

            var thuocDaHetHan = await _context.ChiTietNhapKhoThuocs
                .Include(c => c.MaThuocNavigation)
                .Include(c => c.MaPhieuNhapNavigation)
                .Where(c => c.HanSuDung < ngayHienTai)
                .OrderBy(c => c.HanSuDung)
                .ToListAsync();

            return thuocDaHetHan;
        }

        // PUT: api/ChiTietNhapKhoThuoc/PN001/TH001/LO001
        [HttpPut("{maPhieuNhap}/{maThuoc}/{soLo}")]
        public async Task<IActionResult> PutChiTietNhapKhoThuoc(string maPhieuNhap, string maThuoc, string soLo, ChiTietNhapKhoThuoc chiTietNhapKhoThuoc)
        {
            if (maPhieuNhap != chiTietNhapKhoThuoc.MaPhieuNhap ||
                maThuoc != chiTietNhapKhoThuoc.MaThuoc ||
                soLo != chiTietNhapKhoThuoc.SoLo)
            {
                return BadRequest();
            }

            _context.Entry(chiTietNhapKhoThuoc).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChiTietNhapKhoThuocExists(maPhieuNhap, maThuoc, soLo))
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

        // POST: api/ChiTietNhapKhoThuoc
        [HttpPost]
        public async Task<ActionResult<ChiTietNhapKhoThuoc>> PostChiTietNhapKhoThuoc(ChiTietNhapKhoThuoc chiTietNhapKhoThuoc)
        {
            // Kiểm tra xem phiếu nhập có tồn tại không
            var phieuNhapExists = await _context.PhieuNhapKhos.AnyAsync(p => p.MaPhieuNhap == chiTietNhapKhoThuoc.MaPhieuNhap);
            if (!phieuNhapExists)
            {
                return BadRequest("Phiếu nhập không tồn tại");
            }

            // Kiểm tra xem thuốc có tồn tại không
            var thuocExists = await _context.DmThuocs.AnyAsync(t => t.MaThuoc == chiTietNhapKhoThuoc.MaThuoc);
            if (!thuocExists)
            {
                return BadRequest("Thuốc không tồn tại");
            }

            // Kiểm tra xem chi tiết nhập kho đã tồn tại chưa
            if (ChiTietNhapKhoThuocExists(chiTietNhapKhoThuoc.MaPhieuNhap, chiTietNhapKhoThuoc.MaThuoc, chiTietNhapKhoThuoc.SoLo))
            {
                return BadRequest("Chi tiết nhập kho thuốc đã tồn tại");
            }

            // Kiểm tra hạn sử dụng
            if (chiTietNhapKhoThuoc.HanSuDung.HasValue)
            {
                var ngayHienTai = DateOnly.FromDateTime(DateTime.Now);
                if (chiTietNhapKhoThuoc.HanSuDung < ngayHienTai)
                {
                    return BadRequest("Hạn sử dụng không thể trong quá khứ");
                }
            }

            // Kiểm tra số lượng nhập
            if (chiTietNhapKhoThuoc.SoLuongNhap <= 0)
            {
                return BadRequest("Số lượng nhập phải lớn hơn 0");
            }

            // Tính toán thành tiền nếu chưa có
            if (chiTietNhapKhoThuoc.ThanhTien == null && chiTietNhapKhoThuoc.DonGiaNhap.HasValue)
            {
                chiTietNhapKhoThuoc.ThanhTien = chiTietNhapKhoThuoc.SoLuongNhap * chiTietNhapKhoThuoc.DonGiaNhap;
            }

            _context.ChiTietNhapKhoThuocs.Add(chiTietNhapKhoThuoc);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ChiTietNhapKhoThuocExists(chiTietNhapKhoThuoc.MaPhieuNhap, chiTietNhapKhoThuoc.MaThuoc, chiTietNhapKhoThuoc.SoLo))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetChiTietNhapKhoThuoc",
                new
                {
                    maPhieuNhap = chiTietNhapKhoThuoc.MaPhieuNhap,
                    maThuoc = chiTietNhapKhoThuoc.MaThuoc,
                    soLo = chiTietNhapKhoThuoc.SoLo
                },
                chiTietNhapKhoThuoc);
        }

        // DELETE: api/ChiTietNhapKhoThuoc/PN001/TH001/LO001
        [HttpDelete("{maPhieuNhap}/{maThuoc}/{soLo}")]
        public async Task<IActionResult> DeleteChiTietNhapKhoThuoc(string maPhieuNhap, string maThuoc, string soLo)
        {
            var chiTietNhapKhoThuoc = await _context.ChiTietNhapKhoThuocs
                .FirstOrDefaultAsync(c => c.MaPhieuNhap == maPhieuNhap && c.MaThuoc == maThuoc && c.SoLo == soLo);

            if (chiTietNhapKhoThuoc == null)
            {
                return NotFound();
            }

            _context.ChiTietNhapKhoThuocs.Remove(chiTietNhapKhoThuoc);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/ChiTietNhapKhoThuoc/tongtien/PN001
        [HttpGet("tongtien/{maPhieuNhap}")]
        public async Task<ActionResult<decimal>> GetTongTienPhieuNhap(string maPhieuNhap)
        {
            var tongTien = await _context.ChiTietNhapKhoThuocs
                .Where(c => c.MaPhieuNhap == maPhieuNhap)
                .SumAsync(c => c.ThanhTien ?? 0);

            return tongTien;
        }

        // GET: api/ChiTietNhapKhoThuoc/tongsoluong/PN001
        [HttpGet("tongsoluong/{maPhieuNhap}")]
        public async Task<ActionResult<int>> GetTongSoLuongPhieuNhap(string maPhieuNhap)
        {
            var tongSoLuong = await _context.ChiTietNhapKhoThuocs
                .Where(c => c.MaPhieuNhap == maPhieuNhap)
                .SumAsync(c => c.SoLuongNhap);

            return tongSoLuong;
        }

        // GET: api/ChiTietNhapKhoThuoc/tonkho/TH001
        [HttpGet("tonkho/{maThuoc}")]
        public async Task<ActionResult<int>> GetTonKhoThuoc(string maThuoc)
        {
            var tonKho = await _context.ChiTietNhapKhoThuocs
                .Where(c => c.MaThuoc == maThuoc)
                .SumAsync(c => c.SoLuongNhap);

            return tonKho;
        }

        // GET: api/ChiTietNhapKhoThuoc/tonkho/TH001/solo/LO001
        [HttpGet("tonkho/{maThuoc}/solo/{soLo}")]
        public async Task<ActionResult<int>> GetTonKhoThuocTheoSoLo(string maThuoc, string soLo)
        {
            var tonKho = await _context.ChiTietNhapKhoThuocs
                .Where(c => c.MaThuoc == maThuoc && c.SoLo == soLo)
                .SumAsync(c => c.SoLuongNhap);

            return tonKho;
        }

        private bool ChiTietNhapKhoThuocExists(string maPhieuNhap, string maThuoc, string soLo)
        {
            return _context.ChiTietNhapKhoThuocs.Any(c =>
                c.MaPhieuNhap == maPhieuNhap &&
                c.MaThuoc == maThuoc &&
                c.SoLo == soLo);
        }
    }
}