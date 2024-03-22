using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDocWebEntity.Data;

[Table("aspnetuserroles")]
public partial class Aspnetuserrole
{
    [Key]
    [Column("roleid")]
    [StringLength(128)]
    public string Roleid { get; set; } = null!;

    [Column("userid")]
    [StringLength(128)]
    public string Userid { get; set; } = null!;
}
