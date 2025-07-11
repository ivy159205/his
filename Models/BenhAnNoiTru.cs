using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

[Table("BenhAnNoiTru")]
public partial class BenhAnNoiTru
{
    [Key]
    [Column("MaBANoiTru")]
    [StringLength(15)]
    [Unicode(false)]
    public string MaBanoiTru { get; set; } = null!;

    [StringLength(15)]
    [Unicode(false)]
    public string MaNhapVien { get; set; } = null!;

    [Column("MaBN")]
    [StringLength(15)]
    [Unicode(false)]
    public string MaBn { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? NgayNhapVien { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayXuatVien { get; set; }

    public int? SoNgayDieuTri { get; set; }

    [StringLength(500)]
    public string? ChanDoanNhapVien { get; set; }

    [Column("ChanDoanChinhICD")]
    [StringLength(10)]
    [Unicode(false)]
    public string? ChanDoanChinhIcd { get; set; }

    [StringLength(500)]
    public string? ChanDoanChinh { get; set; }

    [StringLength(500)]
    public string? ChanDoanPhu { get; set; }

    [StringLength(500)]
    public string? ChanDoanXuatVien { get; set; }

    [StringLength(2000)]
    public string? QuaTrinhBenhLy { get; set; }

    [StringLength(2000)]
    public string? QuaTrinhDieuTri { get; set; }

    [StringLength(1000)]
    public string? PhauThuat { get; set; }

    [StringLength(500)]
    public string? TinhTrangXuatVien { get; set; }

    [StringLength(1000)]
    public string? HuongDieuTri { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? KetQuaDieuTri { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? TrangThai { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayTao { get; set; }

    [ForeignKey("MaBn")]
    [InverseProperty("BenhAnNoiTrus")]
    public virtual BenhNhan MaBnNavigation { get; set; } = null!;

    [ForeignKey("MaNhapVien")]
    [InverseProperty("BenhAnNoiTrus")]
    public virtual NhapVien MaNhapVienNavigation { get; set; } = null!;
}
