using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

[Table("ChiDinhXetNghiem")]
public partial class ChiDinhXetNghiem
{
    [Key]
    [StringLength(15)]
    [Unicode(false)]
    public string MaChiDinh { get; set; } = null!;

    [Column("MaBA")]
    [StringLength(15)]
    [Unicode(false)]
    public string? MaBa { get; set; }

    [Column("MaBN")]
    [StringLength(15)]
    [Unicode(false)]
    public string MaBn { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime NgayChiDinh { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? MaBacSi { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? MaDichVu { get; set; }

    [StringLength(500)]
    public string? YeuCau { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? TrangThai { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayTao { get; set; }

    [InverseProperty("MaChiDinhNavigation")]
    public virtual ICollection<KetQuaXetNghiem> KetQuaXetNghiems { get; set; } = new List<KetQuaXetNghiem>();

    [ForeignKey("MaBacSi")]
    [InverseProperty("ChiDinhXetNghiems")]
    public virtual DmNhanVien? MaBacSiNavigation { get; set; }

    [ForeignKey("MaBn")]
    [InverseProperty("ChiDinhXetNghiems")]
    public virtual BenhNhan MaBnNavigation { get; set; } = null!;

    [ForeignKey("MaDichVu")]
    [InverseProperty("ChiDinhXetNghiems")]
    public virtual DmDichVu? MaDichVuNavigation { get; set; }
}
