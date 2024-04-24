using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Assignment_WebEntity.Data;

[Table("borrower")]
public partial class Borrower
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("city")]
    [StringLength(50)]
    public string? City { get; set; }

    [InverseProperty("Borrower")]
    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
