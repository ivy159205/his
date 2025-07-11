using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

[Table("DM_DoiTuongBHYT")]
public partial class DmDoiTuongBhyt
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string MaDoiTuong { get; set; } = null!;

    [StringLength(100)]
    public string TenDoiTuong { get; set; } = null!;

    [Column(TypeName = "decimal(5, 2)")]
    public decimal MucHuong { get; set; }

    [StringLength(255)]
    public string? MoTa { get; set; }

    public bool? TrangThai { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayTao { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? NguoiTao { get; set; }

    [InverseProperty("MaDoiTuongBhytNavigation")]
    public virtual ICollection<BenhNhan> BenhNhans { get; set; } = new List<BenhNhan>();
}
