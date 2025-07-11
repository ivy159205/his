using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

[Table("LogHoatDong")]
public partial class LogHoatDong
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("MaNV")]
    [StringLength(10)]
    [Unicode(false)]
    public string? MaNv { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? ChucNang { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? HanhDong { get; set; }

    [StringLength(1000)]
    public string? NoiDung { get; set; }

    [Column("IPAddress")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Ipaddress { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayGio { get; set; }

    [ForeignKey("MaNv")]
    [InverseProperty("LogHoatDongs")]
    public virtual DmNhanVien? MaNvNavigation { get; set; }
}
