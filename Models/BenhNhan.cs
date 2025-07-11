using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

[Table("BenhNhan")]
[Index("Cccd", Name = "IX_BenhNhan_CCCD")]
[Index("SoTheBhyt", Name = "IX_BenhNhan_SoTheBHYT")]
public partial class BenhNhan
{
    [Key]
    [Column("MaBN")]
    [StringLength(15)]
    [Unicode(false)]
    public string MaBn { get; set; } = null!;

    [StringLength(100)]
    public string HoTen { get; set; } = null!;

    public DateOnly? NgaySinh { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string? GioiTinh { get; set; }

    [Column("CCCD")]
    [StringLength(12)]
    [Unicode(false)]
    public string? Cccd { get; set; }

    [StringLength(255)]
    public string? DiaChi { get; set; }

    [Column("SDT")]
    [StringLength(15)]
    [Unicode(false)]
    public string? Sdt { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Email { get; set; }

    [StringLength(100)]
    public string? NgheNghiep { get; set; }

    [Column("SoTheBHYT")]
    [StringLength(15)]
    [Unicode(false)]
    public string? SoTheBhyt { get; set; }

    [Column("MaDoiTuongBHYT")]
    [StringLength(10)]
    [Unicode(false)]
    public string? MaDoiTuongBhyt { get; set; }

    public DateOnly? GiaTriTu { get; set; }

    public DateOnly? GiaTriDen { get; set; }

    [Column("NoiDangKyKCB")]
    [StringLength(200)]
    public string? NoiDangKyKcb { get; set; }

    [StringLength(100)]
    public string? NguoiLienHe { get; set; }

    [Column("SDTLienHe")]
    [StringLength(15)]
    [Unicode(false)]
    public string? SdtlienHe { get; set; }

    public bool? TrangThai { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayTao { get; set; }

    [InverseProperty("MaBnNavigation")]
    public virtual ICollection<BenhAnNgoaiTru> BenhAnNgoaiTrus { get; set; } = new List<BenhAnNgoaiTru>();

    [InverseProperty("MaBnNavigation")]
    public virtual ICollection<BenhAnNoiTru> BenhAnNoiTrus { get; set; } = new List<BenhAnNoiTru>();

    [InverseProperty("MaBnNavigation")]
    public virtual ICollection<ChiDinhXetNghiem> ChiDinhXetNghiems { get; set; } = new List<ChiDinhXetNghiem>();

    [InverseProperty("MaBnNavigation")]
    public virtual ICollection<DangKyKham> DangKyKhams { get; set; } = new List<DangKyKham>();

    [InverseProperty("MaBnNavigation")]
    public virtual ICollection<DonThuoc> DonThuocs { get; set; } = new List<DonThuoc>();

    [InverseProperty("MaBnNavigation")]
    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    [ForeignKey("MaDoiTuongBhyt")]
    [InverseProperty("BenhNhans")]
    public virtual DmDoiTuongBhyt? MaDoiTuongBhytNavigation { get; set; }

    [InverseProperty("MaBnNavigation")]
    public virtual ICollection<NhapVien> NhapViens { get; set; } = new List<NhapVien>();
}
