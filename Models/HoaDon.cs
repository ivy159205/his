using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

[Table("HoaDon")]
[Index("NgayHoaDon", Name = "IX_HoaDon_NgayHoaDon")]
public partial class HoaDon
{
    [Key]
    [StringLength(15)]
    [Unicode(false)]
    public string MaHoaDon { get; set; } = null!;

    [Column("MaBN")]
    [StringLength(15)]
    [Unicode(false)]
    public string MaBn { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime NgayHoaDon { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? LoaiHoaDon { get; set; }

    [Column(TypeName = "decimal(15, 2)")]
    public decimal? TongTien { get; set; }

    [Column("TienBHYT", TypeName = "decimal(15, 2)")]
    public decimal? TienBhyt { get; set; }

    [Column("TienBNTra", TypeName = "decimal(15, 2)")]
    public decimal? TienBntra { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? TrangThai { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayThanhToan { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? HinhThucThanhToan { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? NguoiThu { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayTao { get; set; }

    [InverseProperty("MaHoaDonNavigation")]
    public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();

    [ForeignKey("MaBn")]
    [InverseProperty("HoaDons")]
    public virtual BenhNhan MaBnNavigation { get; set; } = null!;

    [ForeignKey("NguoiThu")]
    [InverseProperty("HoaDons")]
    public virtual DmNhanVien? NguoiThuNavigation { get; set; }
}
