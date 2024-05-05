using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDocWebEntity.Data;

[Table("payrate")]
public partial class Payrate
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("physicianId")]
    public int PhysicianId { get; set; }

    public int? NightshiftWeekend { get; set; }

    public int? Shift { get; set; }

    public int? HousecallsNightsWeekend { get; set; }

    public int? PhoneConsults { get; set; }

    public int? PhoneConsultsNightsWeekend { get; set; }

    public int? BatchTesting { get; set; }

    public int? Housecall { get; set; }

    [Column("createdDate")]
    public DateTime? CreatedDate { get; set; }

    [Column("modifiedDate", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedDate { get; set; }

    [ForeignKey("PhysicianId")]
    [InverseProperty("Payrates")]
    public virtual Physician Physician { get; set; } = null!;
}
