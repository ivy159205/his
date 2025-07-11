using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

[Table("DangKyKham")]
[Index("NgayKham", Name = "IX_DangKyKham_NgayKham")]
public partial class DangKyKham
{
    [Key]
    [StringLength(15)]
    [Unicode(false)]
    public string MaDangKy { get; set; } = null!;

    [Column("MaBN")]
    [StringLength(15)]
    [Unicode(false)]
    public string MaBn { get; set; } = null!;

    public DateOnly NgayKham { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? MaKhoa { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? MaBacSi { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? LoaiKham { get; set; }

    [StringLength(500)]
    public string? LyDoKham { get; set; }

    [Column("STT")]
    public int? Stt { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? TrangThai { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayTao { get; set; }

    [InverseProperty("MaDangKyNavigation")]
    public virtual ICollection<BenhAnNgoaiTru> BenhAnNgoaiTrus { get; set; } = new List<BenhAnNgoaiTru>();

    [ForeignKey("MaBacSi")]
    [InverseProperty("DangKyKhams")]
    public virtual DmNhanVien? MaBacSiNavigation { get; set; }

    [ForeignKey("MaBn")]
    [InverseProperty("DangKyKhams")]
    public virtual BenhNhan MaBnNavigation { get; set; } = null!;

    [ForeignKey("MaKhoa")]
    [InverseProperty("DangKyKhams")]
    public virtual DmKhoaPhong? MaKhoaNavigation { get; set; }
}
