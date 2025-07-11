using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

[Table("NhapVien")]
[Index("NgayNhapVien", Name = "IX_NhapVien_NgayNhapVien")]
public partial class NhapVien
{
    [Key]
    [StringLength(15)]
    [Unicode(false)]
    public string MaNhapVien { get; set; } = null!;

    [Column("MaBN")]
    [StringLength(15)]
    [Unicode(false)]
    public string MaBn { get; set; } = null!;

    [Column("MaBA")]
    [StringLength(15)]
    [Unicode(false)]
    public string? MaBa { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime NgayNhapVien { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? MaKhoa { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? MaBacSi { get; set; }

    [StringLength(500)]
    public string? LyDoNhapVien { get; set; }

    [StringLength(500)]
    public string? ChanDoanNhapVien { get; set; }

    [StringLength(500)]
    public string? TinhTrangNhapVien { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? LoaiNhapVien { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? SoGiuong { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? TrangThai { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayTao { get; set; }

    [InverseProperty("MaNhapVienNavigation")]
    public virtual ICollection<BenhAnNoiTru> BenhAnNoiTrus { get; set; } = new List<BenhAnNoiTru>();

    [ForeignKey("MaBacSi")]
    [InverseProperty("NhapViens")]
    public virtual DmNhanVien? MaBacSiNavigation { get; set; }

    [ForeignKey("MaBn")]
    [InverseProperty("NhapViens")]
    public virtual BenhNhan MaBnNavigation { get; set; } = null!;

    [ForeignKey("MaKhoa")]
    [InverseProperty("NhapViens")]
    public virtual DmKhoaPhong? MaKhoaNavigation { get; set; }
}
