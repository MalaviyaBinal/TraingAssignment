using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Assignment_WebEntity.Data;

[Table("genere")]
public partial class Genere
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("generename")]
    [StringLength(50)]
    public string? Generename { get; set; }

    [InverseProperty("GenereNavigation")]
    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
