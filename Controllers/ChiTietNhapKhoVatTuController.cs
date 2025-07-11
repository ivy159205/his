using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChiTietNhapKhoVatTuController : ControllerBase
    {
        private readonly DBCHIS _context;

        public ChiTietNhapKhoVatTuController(DBCHIS context)
        {
            _context = context;
        }

        // GET: api/ChiTietNhapKhoVatTu
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChiTietNhapKhoVatTu>>> GetChiTietNhapKhoVatTus()
        {
            return await _context.ChiTietNhapKhoVatTus
                .Include(c => c.MaPhieuNhapNavigation)
                .Include(c => c.MaVatTuNavigation)
                .ToListAsync();
        }

        // GET: api/ChiTietNhapKhoVatTu/PN001/VT001/LO001
        [HttpGet("{maPhieuNhap}/{maVatTu}/{soLo}")]
        public async Task<ActionResult<ChiTietNhapKhoVatTu>> GetChiTietNhapKhoVatTu(string maPhieuNhap, string maVatTu, string soLo)
        {
            var chiTietNhapKhoVatTu = await _context.ChiTietNhapKhoVatTus
                .Include(c => c.MaPhieuNhapNavigation)
                .Include(c => c.MaVatTuNavigation)
                .FirstOrDefaultAsync(c => c.MaPhieuNhap == maPhieuNhap && c.MaVatTu == maVatTu && c.SoLo == soLo);

            if (chiTietNhapKhoVatTu == null)
            {
                return NotFound();
            }

            return chiTietNhapKhoVatTu;
        }

        // GET: api/ChiTietNhapKhoVatTu/phieunhap/PN001
        [HttpGet("phieunhap/{maPhieuNhap}")]
        public async Task<ActionResult<IEnumerable<ChiTietNhapKhoVatTu>>> GetChiTietNhapKhoVatTusByMaPhieuNhap(string maPhieuNhap)
        {
            var chiTietNhapKhoVatTus = await _context.ChiTietNhapKhoVatTus
                .Include(c => c.MaVatTuNavigation)
                .Where(c => c.MaPhieuNhap == maPhieuNhap)
                .ToListAsync();

            return chiTietNhapKhoVatTus;
        }

        // GET: api/ChiTietNhapKhoVatTu/vattu/VT001
        [HttpGet("vattu/{maVatTu}")]
        public async Task<ActionResult<IEnumerable<ChiTietNhapKhoVatTu>>> GetChiTietNhapKhoVatTusByMaVatTu(string maVatTu)
        {
            var chiTietNhapKhoVatTus = await _context.ChiTietNhapKhoVatTus
                .Include(c => c.MaPhieuNhapNavigation)
                .Where(c => c.MaVatTu == maVatTu)
                .OrderBy(c => c.HanSuDung)
                .ToListAsync();

            return chiTietNhapKhoVatTus;
        }

        // GET: api/ChiTietNhapKhoVatTu/vattu/VT001/solo/LO001
        [HttpGet("vattu/{maVatTu}/solo/{soLo}")]
        public async Task<ActionResult<IEnumerable<ChiTietNhapKhoVatTu>>> GetChiTietNhapKhoVatTusByMaVatTuAndSoLo(string maVatTu, string soLo)
        {
            var chiTietNhapKhoVatTus = await _context.ChiTietNhapKhoVatTus
                .Include(c => c.MaPhieuNhapNavigation)
                .Where(c => c.MaVatTu == maVatTu && c.SoLo == soLo)
                .OrderBy(c => c.HanSuDung)
                .ToListAsync();

            return chiTietNhapKhoVatTus;
        }

        // GET: api/ChiTietNhapKhoVatTu/hethan
        [HttpGet("hethan")]
        public async Task<ActionResult<IEnumerable<ChiTietNhapKhoVatTu>>> GetVatTuHetHan([FromQuery] int soNgay = 30)
        {
            var ngayHienTai = DateOnly.FromDateTime(DateTime.Now);
            var ngayKiemTra = ngayHienTai.AddDays(soNgay);

            var vatTuHetHan = await _context.ChiTietNhapKhoVatTus
                .Include(c => c.MaVatTuNavigation)
                .Include(c => c.MaPhieuNhapNavigation)
                .Where(c => c.HanSuDung <= ngayKiemTra && c.HanSuDung >= ngayHienTai)
                .OrderBy(c => c.HanSuDung)
                .ToListAsync();

            return vatTuHetHan;
        }

        // GET: api/ChiTietNhapKhoVatTu/dahethan
        [HttpGet("dahethan")]
        public async Task<ActionResult<IEnumerable<ChiTietNhapKhoVatTu>>> GetVatTuDaHetHan()
        {
            var ngayHienTai = DateOnly.FromDateTime(DateTime.Now);

            var vatTuDaHetHan = await _context.ChiTietNhapKhoVatTus
                .Include(c => c.MaVatTuNavigation)
                .Include(c => c.MaPhieuNhapNavigation)
                .Where(c => c.HanSuDung < ngayHienTai)
                .OrderBy(c => c.HanSuDung)
                .ToListAsync();

            return vatTuDaHetHan;
        }

        // PUT: api/ChiTietNhapKhoVatTu/PN001/VT001/LO001
        [HttpPut("{maPhieuNhap}/{maVatTu}/{soLo}")]
        public async Task<IActionResult> PutChiTietNhapKhoVatTu(string maPhieuNhap, string maVatTu, string soLo, ChiTietNhapKhoVatTu chiTietNhapKhoVatTu)
        {
            if (maPhieuNhap != chiTietNhapKhoVatTu.MaPhieuNhap ||
                maVatTu != chiTietNhapKhoVatTu.MaVatTu ||
                soLo != chiTietNhapKhoVatTu.SoLo)
            {
                return BadRequest();
            }

            _context.Entry(chiTietNhapKhoVatTu).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChiTietNhapKhoVatTuExists(maPhieuNhap, maVatTu, soLo))
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

        // POST: api/ChiTietNhapKhoVatTu
        [HttpPost]
        public async Task<ActionResult<ChiTietNhapKhoVatTu>> PostChiTietNhapKhoVatTu(ChiTietNhapKhoVatTu chiTietNhapKhoVatTu)
        {
            // Kiểm tra xem phiếu nhập có tồn tại không
            var phieuNhapExists = await _context.PhieuNhapKhos.AnyAsync(p => p.MaPhieuNhap == chiTietNhapKhoVatTu.MaPhieuNhap);
            if (!phieuNhapExists)
            {
                return BadRequest("Phiếu nhập không tồn tại");
            }

            // Kiểm tra xem vật tư có tồn tại không
            var vatTuExists = await _context.DmVatTuYtes.AnyAsync(v => v.MaVatTu == chiTietNhapKhoVatTu.MaVatTu);
            if (!vatTuExists)
            {
                return BadRequest("Vật tư y tế không tồn tại");
            }

            // Kiểm tra xem chi tiết nhập kho đã tồn tại chưa
            if (ChiTietNhapKhoVatTuExists(chiTietNhapKhoVatTu.MaPhieuNhap, chiTietNhapKhoVatTu.MaVatTu, chiTietNhapKhoVatTu.SoLo))
            {
                return BadRequest("Chi tiết nhập kho vật tư đã tồn tại");
            }

            // Kiểm tra hạn sử dụng
            if (chiTietNhapKhoVatTu.HanSuDung.HasValue)
            {
                var ngayHienTai = DateOnly.FromDateTime(DateTime.Now);
                if (chiTietNhapKhoVatTu.HanSuDung < ngayHienTai)
                {
                    return BadRequest("Hạn sử dụng không thể trong quá khứ");
                }
            }

            // Kiểm tra số lượng nhập
            if (chiTietNhapKhoVatTu.SoLuongNhap <= 0)
            {
                return BadRequest("Số lượng nhập phải lớn hơn 0");
            }

            // Tính toán thành tiền nếu chưa có
            if (chiTietNhapKhoVatTu.ThanhTien == null && chiTietNhapKhoVatTu.DonGiaNhap.HasValue)
            {
                chiTietNhapKhoVatTu.ThanhTien = chiTietNhapKhoVatTu.SoLuongNhap * chiTietNhapKhoVatTu.DonGiaNhap;
            }

            _context.ChiTietNhapKhoVatTus.Add(chiTietNhapKhoVatTu);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ChiTietNhapKhoVatTuExists(chiTietNhapKhoVatTu.MaPhieuNhap, chiTietNhapKhoVatTu.MaVatTu, chiTietNhapKhoVatTu.SoLo))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetChiTietNhapKhoVatTu",
                new
                {
                    maPhieuNhap = chiTietNhapKhoVatTu.MaPhieuNhap,
                    maVatTu = chiTietNhapKhoVatTu.MaVatTu,
                    soLo = chiTietNhapKhoVatTu.SoLo
                },
                chiTietNhapKhoVatTu);
        }

        // DELETE: api/ChiTietNhapKhoVatTu/PN001/VT001/LO001
        [HttpDelete("{maPhieuNhap}/{maVatTu}/{soLo}")]
        public async Task<IActionResult> DeleteChiTietNhapKhoVatTu(string maPhieuNhap, string maVatTu, string soLo)
        {
            var chiTietNhapKhoVatTu = await _context.ChiTietNhapKhoVatTus
                .FirstOrDefaultAsync(c => c.MaPhieuNhap == maPhieuNhap && c.MaVatTu == maVatTu && c.SoLo == soLo);

            if (chiTietNhapKhoVatTu == null)
            {
                return NotFound();
            }

            _context.ChiTietNhapKhoVatTus.Remove(chiTietNhapKhoVatTu);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/ChiTietNhapKhoVatTu/tongtien/PN001
        [HttpGet("tongtien/{maPhieuNhap}")]
        public async Task<ActionResult<decimal>> GetTongTienPhieuNhap(string maPhieuNhap)
        {
            var tongTien = await _context.ChiTietNhapKhoVatTus
                .Where(c => c.MaPhieuNhap == maPhieuNhap)
                .SumAsync(c => c.ThanhTien ?? 0);

            return tongTien;
        }

        // GET: api/ChiTietNhapKhoVatTu/tongsoluong/PN001
        [HttpGet("tongsoluong/{maPhieuNhap}")]
        public async Task<ActionResult<int>> GetTongSoLuongPhieuNhap(string maPhieuNhap)
        {
            var tongSoLuong = await _context.ChiTietNhapKhoVatTus
                .Where(c => c.MaPhieuNhap == maPhieuNhap)
                .SumAsync(c => c.SoLuongNhap);

            return tongSoLuong;
        }

        // GET: api/ChiTietNhapKhoVatTu/tonkho/VT001
        [HttpGet("tonkho/{maVatTu}")]
        public async Task<ActionResult<int>> GetTonKhoVatTu(string maVatTu)
        {
            var tonKho = await _context.ChiTietNhapKhoVatTus
                .Where(c => c.MaVatTu == maVatTu)
                .SumAsync(c => c.SoLuongNhap);

            return tonKho;
        }

        // GET: api/ChiTietNhapKhoVatTu/tonkho/VT001/solo/LO001
        [HttpGet("tonkho/{maVatTu}/solo/{soLo}")]
        public async Task<ActionResult<int>> GetTonKhoVatTuTheoSoLo(string maVatTu, string soLo)
        {
            var tonKho = await _context.ChiTietNhapKhoVatTus
                .Where(c => c.MaVatTu == maVatTu && c.SoLo == soLo)
                .SumAsync(c => c.SoLuongNhap);

            return tonKho;
        }

        // GET: api/ChiTietNhapKhoVatTu/giatritonkho/VT001
        [HttpGet("giatritonkho/{maVatTu}")]
        public async Task<ActionResult<decimal>> GetGiaTriTonKhoVatTu(string maVatTu)
        {
            var giaTriTonKho = await _context.ChiTietNhapKhoVatTus
                .Where(c => c.MaVatTu == maVatTu)
                .SumAsync(c => c.ThanhTien ?? 0);

            return giaTriTonKho;
        }

        // GET: api/ChiTietNhapKhoVatTu/thongketonkho
        [HttpGet("thongketonkho")]
        public async Task<ActionResult<IEnumerable<object>>> GetThongKeTonKhoVatTu()
        {
            var thongKe = await _context.ChiTietNhapKhoVatTus
                .Include(c => c.MaVatTuNavigation)
                .GroupBy(c => new { c.MaVatTu, c.MaVatTuNavigation.TenVatTu })
                .Select(g => new
                {
                    MaVatTu = g.Key.MaVatTu,
                    TenVatTu = g.Key.TenVatTu,
                    TongSoLuong = g.Sum(c => c.SoLuongNhap),
                    TongGiaTri = g.Sum(c => c.ThanhTien ?? 0),
                    SoLuongLoHang = g.Count()
                })
                .OrderByDescending(x => x.TongGiaTri)
                .ToListAsync();

            return Ok(thongKe);
        }

        // GET: api/ChiTietNhapKhoVatTu/baocaohethan
        [HttpGet("baocaohethan")]
        public async Task<ActionResult<object>> GetBaoCaoVatTuHetHan([FromQuery] int soNgay = 30)
        {
            var ngayHienTai = DateOnly.FromDateTime(DateTime.Now);
            var ngayKiemTra = ngayHienTai.AddDays(soNgay);

            var baoCao = new
            {
                NgayBaoCao = ngayHienTai,
                SoNgayKiemTra = soNgay,
                VatTuSapHetHan = await _context.ChiTietNhapKhoVatTus
                    .Include(c => c.MaVatTuNavigation)
                    .Where(c => c.HanSuDung <= ngayKiemTra && c.HanSuDung >= ngayHienTai)
                    .GroupBy(c => new { c.MaVatTu, c.MaVatTuNavigation.TenVatTu })
                    .Select(g => new
                    {
                        MaVatTu = g.Key.MaVatTu,
                        TenVatTu = g.Key.TenVatTu,
                        TongSoLuong = g.Sum(c => c.SoLuongNhap),
                        TongGiaTri = g.Sum(c => c.ThanhTien ?? 0),
                        NgayHetHanGanNhat = g.Min(c => c.HanSuDung)
                    })
                    .OrderBy(x => x.NgayHetHanGanNhat)
                    .ToListAsync(),

                VatTuDaHetHan = await _context.ChiTietNhapKhoVatTus
                    .Include(c => c.MaVatTuNavigation)
                    .Where(c => c.HanSuDung < ngayHienTai)
                    .GroupBy(c => new { c.MaVatTu, c.MaVatTuNavigation.TenVatTu })
                    .Select(g => new
                    {
                        MaVatTu = g.Key.MaVatTu,
                        TenVatTu = g.Key.TenVatTu,
                        TongSoLuong = g.Sum(c => c.SoLuongNhap),
                        TongGiaTri = g.Sum(c => c.ThanhTien ?? 0),
                        NgayHetHanGanNhat = g.Min(c => c.HanSuDung)
                    })
                    .OrderBy(x => x.NgayHetHanGanNhat)
                    .ToListAsync()
            };

            return Ok(baoCao);
        }

        private bool ChiTietNhapKhoVatTuExists(string maPhieuNhap, string maVatTu, string soLo)
        {
            return _context.ChiTietNhapKhoVatTus.Any(c =>
                c.MaPhieuNhap == maPhieuNhap &&
                c.MaVatTu == maVatTu &&
                c.SoLo == soLo);
        }
    }
}