using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TonKhoThuocController : ControllerBase
    {
        private readonly DBCHIS _context;

        public TonKhoThuocController(DBCHIS context)
        {
            _context = context;
        }

        // GET: api/TonKhoThuoc
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TonKhoThuoc>>> GetTonKhoThuocs()
        {
            return await _context.TonKhoThuocs
                .Include(t => t.MaThuocNavigation)
                .ToListAsync();
        }

        // GET: api/TonKhoThuoc/5/LOT001
        [HttpGet("{maThuoc}/{soLo}")]
        public async Task<ActionResult<TonKhoThuoc>> GetTonKhoThuoc(string maThuoc, string soLo)
        {
            var tonKhoThuoc = await _context.TonKhoThuocs
                .Include(t => t.MaThuocNavigation)
                .FirstOrDefaultAsync(t => t.MaThuoc == maThuoc && t.SoLo == soLo);

            if (tonKhoThuoc == null)
            {
                return NotFound();
            }

            return tonKhoThuoc;
        }

        // GET: api/TonKhoThuoc/thuoc/5
        [HttpGet("thuoc/{maThuoc}")]
        public async Task<ActionResult<IEnumerable<TonKhoThuoc>>> GetTonKhoByMaThuoc(string maThuoc)
        {
            var tonKhoThuocs = await _context.TonKhoThuocs
                .Include(t => t.MaThuocNavigation)
                .Where(t => t.MaThuoc == maThuoc)
                .ToListAsync();

            return tonKhoThuocs;
        }

        // GET: api/TonKhoThuoc/hethan
        [HttpGet("hethan")]
        public async Task<ActionResult<IEnumerable<TonKhoThuoc>>> GetThuocHetHan()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var thuocHetHan = await _context.TonKhoThuocs
                .Include(t => t.MaThuocNavigation)
                .Where(t => t.HanSuDung <= today)
                .ToListAsync();

            return thuocHetHan;
        }

        // GET: api/TonKhoThuoc/saphethan/{days}
        [HttpGet("saphethan/{days}")]
        public async Task<ActionResult<IEnumerable<TonKhoThuoc>>> GetThuocSapHetHan(int days)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var futureDate = today.AddDays(days);

            var thuocSapHetHan = await _context.TonKhoThuocs
                .Include(t => t.MaThuocNavigation)
                .Where(t => t.HanSuDung <= futureDate && t.HanSuDung > today)
                .ToListAsync();

            return thuocSapHetHan;
        }

        // GET: api/TonKhoThuoc/toncao/{minQuantity}
        [HttpGet("toncao/{minQuantity}")]
        public async Task<ActionResult<IEnumerable<TonKhoThuoc>>> GetThuocTonCao(int minQuantity)
        {
            var thuocTonCao = await _context.TonKhoThuocs
                .Include(t => t.MaThuocNavigation)
                .Where(t => t.SoLuongTon >= minQuantity)
                .ToListAsync();

            return thuocTonCao;
        }

        // GET: api/TonKhoThuoc/tonthap/{maxQuantity}
        [HttpGet("tonthap/{maxQuantity}")]
        public async Task<ActionResult<IEnumerable<TonKhoThuoc>>> GetThuocTonThap(int maxQuantity)
        {
            var thuocTonThap = await _context.TonKhoThuocs
                .Include(t => t.MaThuocNavigation)
                .Where(t => t.SoLuongTon <= maxQuantity && t.SoLuongTon > 0)
                .ToListAsync();

            return thuocTonThap;
        }

        // PUT: api/TonKhoThuoc/5/LOT001
        [HttpPut("{maThuoc}/{soLo}")]
        public async Task<IActionResult> PutTonKhoThuoc(string maThuoc, string soLo, TonKhoThuoc tonKhoThuoc)
        {
            if (maThuoc != tonKhoThuoc.MaThuoc || soLo != tonKhoThuoc.SoLo)
            {
                return BadRequest("Mã thuốc và số lô không khớp");
            }

            tonKhoThuoc.NgayCapNhat = DateTime.Now;
            _context.Entry(tonKhoThuoc).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TonKhoThuocExists(maThuoc, soLo))
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

        // POST: api/TonKhoThuoc
        [HttpPost]
        public async Task<ActionResult<TonKhoThuoc>> PostTonKhoThuoc(TonKhoThuoc tonKhoThuoc)
        {
            if (TonKhoThuocExists(tonKhoThuoc.MaThuoc, tonKhoThuoc.SoLo))
            {
                return Conflict("Tồn kho thuốc với mã thuốc và số lô này đã tồn tại");
            }

            tonKhoThuoc.NgayCapNhat = DateTime.Now;
            _context.TonKhoThuocs.Add(tonKhoThuoc);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return Conflict("Không thể thêm tồn kho thuốc");
            }

            return CreatedAtAction("GetTonKhoThuoc",
                new { maThuoc = tonKhoThuoc.MaThuoc, soLo = tonKhoThuoc.SoLo },
                tonKhoThuoc);
        }

        // DELETE: api/TonKhoThuoc/5/LOT001
        [HttpDelete("{maThuoc}/{soLo}")]
        public async Task<IActionResult> DeleteTonKhoThuoc(string maThuoc, string soLo)
        {
            var tonKhoThuoc = await _context.TonKhoThuocs
                .FirstOrDefaultAsync(t => t.MaThuoc == maThuoc && t.SoLo == soLo);

            if (tonKhoThuoc == null)
            {
                return NotFound();
            }

            _context.TonKhoThuocs.Remove(tonKhoThuoc);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/TonKhoThuoc/capnhatton/5/LOT001
        [HttpPut("capnhatton/{maThuoc}/{soLo}")]
        public async Task<IActionResult> CapNhatTonKho(string maThuoc, string soLo, [FromBody] int soLuongMoi)
        {
            var tonKhoThuoc = await _context.TonKhoThuocs
                .FirstOrDefaultAsync(t => t.MaThuoc == maThuoc && t.SoLo == soLo);

            if (tonKhoThuoc == null)
            {
                return NotFound();
            }

            tonKhoThuoc.SoLuongTon = soLuongMoi;
            tonKhoThuoc.NgayCapNhat = DateTime.Now;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // PUT: api/TonKhoThuoc/capnhatgia/5/LOT001
        [HttpPut("capnhatgia/{maThuoc}/{soLo}")]
        public async Task<IActionResult> CapNhatGiaThuoc(string maThuoc, string soLo, [FromBody] UpdateGiaRequest request)
        {
            var tonKhoThuoc = await _context.TonKhoThuocs
                .FirstOrDefaultAsync(t => t.MaThuoc == maThuoc && t.SoLo == soLo);

            if (tonKhoThuoc == null)
            {
                return NotFound();
            }

            if (request.DonGiaNhap.HasValue)
                tonKhoThuoc.DonGiaNhap = request.DonGiaNhap;

            if (request.DonGiaXuat.HasValue)
                tonKhoThuoc.DonGiaXuat = request.DonGiaXuat;

            tonKhoThuoc.NgayCapNhat = DateTime.Now;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool TonKhoThuocExists(string maThuoc, string soLo)
        {
            return _context.TonKhoThuocs.Any(e => e.MaThuoc == maThuoc && e.SoLo == soLo);
        }
    }

    // Model để cập nhật giá
    public class UpdateGiaRequest
    {
        public decimal? DonGiaNhap { get; set; }
        public decimal? DonGiaXuat { get; set; }
    }
}