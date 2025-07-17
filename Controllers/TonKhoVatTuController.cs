using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TonKhoVatTuController : ControllerBase
    {
        private readonly DBCHIS _context;

        public TonKhoVatTuController(DBCHIS context)
        {
            _context = context;
        }

        // GET: api/TonKhoVatTu
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TonKhoVatTu>>> GetTonKhoVatTus()
        {
            return await _context.TonKhoVatTus
                .Include(t => t.MaVatTuNavigation)
                .ToListAsync();
        }

        // GET: api/TonKhoVatTu/5/LOT001
        [HttpGet("{maVatTu}/{soLo}")]
        public async Task<ActionResult<TonKhoVatTu>> GetTonKhoVatTu(string maVatTu, string soLo)
        {
            var tonKhoVatTu = await _context.TonKhoVatTus
                .Include(t => t.MaVatTuNavigation)
                .FirstOrDefaultAsync(t => t.MaVatTu == maVatTu && t.SoLo == soLo);

            if (tonKhoVatTu == null)
            {
                return NotFound();
            }

            return tonKhoVatTu;
        }

        // GET: api/TonKhoVatTu/vattu/5
        [HttpGet("vattu/{maVatTu}")]
        public async Task<ActionResult<IEnumerable<TonKhoVatTu>>> GetTonKhoByMaVatTu(string maVatTu)
        {
            var tonKhoVatTus = await _context.TonKhoVatTus
                .Include(t => t.MaVatTuNavigation)
                .Where(t => t.MaVatTu == maVatTu)
                .ToListAsync();

            return tonKhoVatTus;
        }

        // GET: api/TonKhoVatTu/hethan
        [HttpGet("hethan")]
        public async Task<ActionResult<IEnumerable<TonKhoVatTu>>> GetVatTuHetHan()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var vatTuHetHan = await _context.TonKhoVatTus
                .Include(t => t.MaVatTuNavigation)
                .Where(t => t.HanSuDung <= today)
                .ToListAsync();

            return vatTuHetHan;
        }

        // GET: api/TonKhoVatTu/saphethan/{days}
        [HttpGet("saphethan/{days}")]
        public async Task<ActionResult<IEnumerable<TonKhoVatTu>>> GetVatTuSapHetHan(int days)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var futureDate = today.AddDays(days);

            var vatTuSapHetHan = await _context.TonKhoVatTus
                .Include(t => t.MaVatTuNavigation)
                .Where(t => t.HanSuDung <= futureDate && t.HanSuDung > today)
                .ToListAsync();

            return vatTuSapHetHan;
        }

        // GET: api/TonKhoVatTu/toncao/{minQuantity}
        [HttpGet("toncao/{minQuantity}")]
        public async Task<ActionResult<IEnumerable<TonKhoVatTu>>> GetVatTuTonCao(int minQuantity)
        {
            var vatTuTonCao = await _context.TonKhoVatTus
                .Include(t => t.MaVatTuNavigation)
                .Where(t => t.SoLuongTon >= minQuantity)
                .ToListAsync();

            return vatTuTonCao;
        }

        // GET: api/TonKhoVatTu/tonthap/{maxQuantity}
        [HttpGet("tonthap/{maxQuantity}")]
        public async Task<ActionResult<IEnumerable<TonKhoVatTu>>> GetVatTuTonThap(int maxQuantity)
        {
            var vatTuTonThap = await _context.TonKhoVatTus
                .Include(t => t.MaVatTuNavigation)
                .Where(t => t.SoLuongTon <= maxQuantity && t.SoLuongTon > 0)
                .ToListAsync();

            return vatTuTonThap;
        }

        // GET: api/TonKhoVatTu/vitri/{viTri}
        [HttpGet("vitri/{viTri}")]
        public async Task<ActionResult<IEnumerable<TonKhoVatTu>>> GetVatTuByViTri(string viTri)
        {
            var vatTuByViTri = await _context.TonKhoVatTus
                .Include(t => t.MaVatTuNavigation)
                .Where(t => t.ViTriLuuTru == viTri)
                .ToListAsync();

            return vatTuByViTri;
        }

        // GET: api/TonKhoVatTu/trangthai/{trangThai}
        [HttpGet("trangthai/{trangThai}")]
        public async Task<ActionResult<IEnumerable<TonKhoVatTu>>> GetVatTuByTrangThai(bool trangThai)
        {
            var vatTuByTrangThai = await _context.TonKhoVatTus
                .Include(t => t.MaVatTuNavigation)
                .Where(t => t.TrangThai == trangThai)
                .ToListAsync();

            return vatTuByTrangThai;
        }

        // PUT: api/TonKhoVatTu/5/LOT001
        [HttpPut("{maVatTu}/{soLo}")]
        public async Task<IActionResult> PutTonKhoVatTu(string maVatTu, string soLo, TonKhoVatTu tonKhoVatTu)
        {
            if (maVatTu != tonKhoVatTu.MaVatTu || soLo != tonKhoVatTu.SoLo)
            {
                return BadRequest("Mã vật tư và số lô không khớp");
            }

            tonKhoVatTu.NgayCapNhat = DateTime.Now;
            _context.Entry(tonKhoVatTu).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TonKhoVatTuExists(maVatTu, soLo))
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

        // POST: api/TonKhoVatTu
        [HttpPost]
        public async Task<ActionResult<TonKhoVatTu>> PostTonKhoVatTu(TonKhoVatTu tonKhoVatTu)
        {
            if (TonKhoVatTuExists(tonKhoVatTu.MaVatTu, tonKhoVatTu.SoLo))
            {
                return Conflict("Tồn kho vật tư với mã vật tư và số lô này đã tồn tại");
            }

            tonKhoVatTu.NgayCapNhat = DateTime.Now;
            _context.TonKhoVatTus.Add(tonKhoVatTu);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return Conflict("Không thể thêm tồn kho vật tư");
            }

            return CreatedAtAction("GetTonKhoVatTu",
                new { maVatTu = tonKhoVatTu.MaVatTu, soLo = tonKhoVatTu.SoLo },
                tonKhoVatTu);
        }

        // DELETE: api/TonKhoVatTu/5/LOT001
        [HttpDelete("{maVatTu}/{soLo}")]
        public async Task<IActionResult> DeleteTonKhoVatTu(string maVatTu, string soLo)
        {
            var tonKhoVatTu = await _context.TonKhoVatTus
                .FirstOrDefaultAsync(t => t.MaVatTu == maVatTu && t.SoLo == soLo);

            if (tonKhoVatTu == null)
            {
                return NotFound();
            }

            _context.TonKhoVatTus.Remove(tonKhoVatTu);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/TonKhoVatTu/capnhatton/5/LOT001
        [HttpPut("capnhatton/{maVatTu}/{soLo}")]
        public async Task<IActionResult> CapNhatTonKho(string maVatTu, string soLo, [FromBody] int soLuongMoi)
        {
            var tonKhoVatTu = await _context.TonKhoVatTus
                .FirstOrDefaultAsync(t => t.MaVatTu == maVatTu && t.SoLo == soLo);

            if (tonKhoVatTu == null)
            {
                return NotFound();
            }

            tonKhoVatTu.SoLuongTon = soLuongMoi;
            tonKhoVatTu.NgayCapNhat = DateTime.Now;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // PUT: api/TonKhoVatTu/capnhatgia/5/LOT001
        [HttpPut("capnhatgia/{maVatTu}/{soLo}")]
        public async Task<IActionResult> CapNhatGiaVatTu(string maVatTu, string soLo, [FromBody] UpdateGiaVatTuRequest request)
        {
            var tonKhoVatTu = await _context.TonKhoVatTus
                .FirstOrDefaultAsync(t => t.MaVatTu == maVatTu && t.SoLo == soLo);

            if (tonKhoVatTu == null)
            {
                return NotFound();
            }

            if (request.DonGiaNhap.HasValue)
                tonKhoVatTu.DonGiaNhap = request.DonGiaNhap;

            if (request.DonGiaXuat.HasValue)
                tonKhoVatTu.DonGiaXuat = request.DonGiaXuat;

            tonKhoVatTu.NgayCapNhat = DateTime.Now;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // PUT: api/TonKhoVatTu/capnhatvitri/5/LOT001
        [HttpPut("capnhatvitri/{maVatTu}/{soLo}")]
        public async Task<IActionResult> CapNhatViTriLuuTru(string maVatTu, string soLo, [FromBody] string viTriMoi)
        {
            var tonKhoVatTu = await _context.TonKhoVatTus
                .FirstOrDefaultAsync(t => t.MaVatTu == maVatTu && t.SoLo == soLo);

            if (tonKhoVatTu == null)
            {
                return NotFound();
            }

            tonKhoVatTu.ViTriLuuTru = viTriMoi;
            tonKhoVatTu.NgayCapNhat = DateTime.Now;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // PUT: api/TonKhoVatTu/capnhattrangthai/5/LOT001
        [HttpPut("capnhattrangthai/{maVatTu}/{soLo}")]
        public async Task<IActionResult> CapNhatTrangThai(string maVatTu, string soLo, [FromBody] bool trangThaiMoi)
        {
            var tonKhoVatTu = await _context.TonKhoVatTus
                .FirstOrDefaultAsync(t => t.MaVatTu == maVatTu && t.SoLo == soLo);

            if (tonKhoVatTu == null)
            {
                return NotFound();
            }

            tonKhoVatTu.TrangThai = trangThaiMoi;
            tonKhoVatTu.NgayCapNhat = DateTime.Now;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // GET: api/TonKhoVatTu/thongke
        [HttpGet("thongke")]
        public async Task<ActionResult<object>> GetThongKeTonKho()
        {
            var thongKe = new
            {
                TongSoVatTu = await _context.TonKhoVatTus.CountAsync(),
                TongSoLuongTon = await _context.TonKhoVatTus.SumAsync(t => t.SoLuongTon ?? 0),
                VatTuHetHan = await _context.TonKhoVatTus.CountAsync(t => t.HanSuDung <= DateOnly.FromDateTime(DateTime.Now)),
                VatTuSapHetHan = await _context.TonKhoVatTus.CountAsync(t => t.HanSuDung <= DateOnly.FromDateTime(DateTime.Now.AddDays(30)) && t.HanSuDung > DateOnly.FromDateTime(DateTime.Now)),
                VatTuTonThap = await _context.TonKhoVatTus.CountAsync(t => t.SoLuongTon <= 10 && t.SoLuongTon > 0),
                VatTuHetTon = await _context.TonKhoVatTus.CountAsync(t => t.SoLuongTon == 0),
                VatTuHoatDong = await _context.TonKhoVatTus.CountAsync(t => t.TrangThai == true),
                VatTuNgungHoatDong = await _context.TonKhoVatTus.CountAsync(t => t.TrangThai == false)
            };

            return thongKe;
        }

        private bool TonKhoVatTuExists(string maVatTu, string soLo)
        {
            return _context.TonKhoVatTus.Any(e => e.MaVatTu == maVatTu && e.SoLo == soLo);
        }
    }

    // Model để cập nhật giá vật tư
    public class UpdateGiaVatTuRequest
    {
        public decimal? DonGiaNhap { get; set; }
        public decimal? DonGiaXuat { get; set; }
    }
}