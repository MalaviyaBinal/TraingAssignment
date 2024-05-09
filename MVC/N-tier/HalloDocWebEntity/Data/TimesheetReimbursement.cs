using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDocWebEntity.Data;

[Table("TimesheetReimbursement")]
public partial class TimesheetReimbursement
{
    [Key]
    public int TimesheetReimbursementId { get; set; }

    public int TimesheetId { get; set; }

    public string? Item { get; set; }

    public int? Amount { get; set; }

    public string? Filename { get; set; }

    public int PhysicianId { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? ReimbursementDate { get; set; }

    public int? CreatedBy { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? ModifiedDate { get; set; }

    [ForeignKey("PhysicianId")]
    [InverseProperty("TimesheetReimbursements")]
    public virtual Physician Physician { get; set; } = null!;

    [ForeignKey("TimesheetId")]
    [InverseProperty("TimesheetReimbursements")]
    public virtual Timesheet Timesheet { get; set; } = null!;
}
