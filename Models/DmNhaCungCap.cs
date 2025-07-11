using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

[Table("DM_NhaCungCap")]
public partial class DmNhaCungCap
{
    [Key]
    [Column("MaNCC")]
    [StringLength(10)]
    [Unicode(false)]
    public string MaNcc { get; set; } = null!;

    [Column("TenNCC")]
    [StringLength(200)]
    public string TenNcc { get; set; } = null!;

    [StringLength(255)]
    public string? DiaChi { get; set; }

    [StringLength(15)]
    [Unicode(false)]
    public string? DienThoai { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Email { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? MaSoThue { get; set; }

    [StringLength(100)]
    public string? NguoiLienHe { get; set; }

    public bool? TrangThai { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayTao { get; set; }

    [InverseProperty("MaNccNavigation")]
    public virtual ICollection<PhieuNhapKho> PhieuNhapKhos { get; set; } = new List<PhieuNhapKho>();
}
