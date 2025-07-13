namespace WebApplication1.DTOs
{
    public class DmDichVuDto
    {
        public string MaDichVu { get; set; } = null!;
        public string TenDichVu { get; set; } = null!;
        public string? LoaiDichVu { get; set; }
        public decimal DonGia { get; set; }
        public string? MaKhoa { get; set; }
        public bool? TrangThai { get; set; }
        public DateTime? NgayTao { get; set; }
        public string? TenKhoa { get; set; }
    }
}
