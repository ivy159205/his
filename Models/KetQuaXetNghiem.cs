using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

[Table("KetQuaXetNghiem")]
public partial class KetQuaXetNghiem
{
    [Key]
    [StringLength(15)]
    [Unicode(false)]
    public string MaKetQua { get; set; } = null!;

    [StringLength(15)]
    [Unicode(false)]
    public string MaChiDinh { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? NgayThucHien { get; set; }

    [StringLength(2000)]
    public string? KetQua { get; set; }

    [StringLength(1000)]
    public string? KetLuan { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? NguoiThucHien { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? NguoiDuyet { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayDuyet { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? TrangThai { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayTao { get; set; }

    [ForeignKey("MaChiDinh")]
    [InverseProperty("KetQuaXetNghiems")]
    public virtual ChiDinhXetNghiem MaChiDinhNavigation { get; set; } = null!;

    [ForeignKey("NguoiThucHien")]
    [InverseProperty("KetQuaXetNghiems")]
    public virtual DmNhanVien? NguoiThucHienNavigation { get; set; }
}
