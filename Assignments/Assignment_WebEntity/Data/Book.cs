using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Assignment_WebEntity.Data;

[Table("book")]
public partial class Book
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("bookname")]
    [StringLength(100)]
    public string? Bookname { get; set; }

    [Column("author")]
    [StringLength(100)]
    public string? Author { get; set; }

    [Column("borrowerid")]
    public int Borrowerid { get; set; }

    [Column("borrowername")]
    [StringLength(100)]
    public string? Borrowername { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? DateOfIssue { get; set; }

    [StringLength(50)]
    public string? City { get; set; }

    public int? Genere { get; set; }

    [Column("isDeleted")]
    public bool? IsDeleted { get; set; }

    [ForeignKey("Borrowerid")]
    [InverseProperty("Books")]
    public virtual Borrower Borrower { get; set; } = null!;

    [ForeignKey("Genere")]
    [InverseProperty("Books")]
    public virtual Genere? GenereNavigation { get; set; }
}
