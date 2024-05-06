﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDocWebEntity.Data;

[Table("Timesheet")]
public partial class Timesheet
{
    [Key]
    public int TimesheetId { get; set; }

    public int PhysicianId { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? Startdate { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? Enddate { get; set; }

    [Column("isApproved")]
    public bool? IsApproved { get; set; }

    [Column("isFinalized")]
    public bool? IsFinalized { get; set; }

    [Column("adminNote", TypeName = "character varying")]
    public string? AdminNote { get; set; }

    [Column("invoiceAmount")]
    public int? InvoiceAmount { get; set; }

    [Column("bonusAmount")]
    public int? BonusAmount { get; set; }

    [ForeignKey("PhysicianId")]
    [InverseProperty("Timesheets")]
    public virtual Physician Physician { get; set; } = null!;

    [InverseProperty("Timesheet")]
    public virtual ICollection<TimesheetDetail> TimesheetDetails { get; set; } = new List<TimesheetDetail>();

    [InverseProperty("Timesheet")]
    public virtual ICollection<TimesheetReimbursement> TimesheetReimbursements { get; set; } = new List<TimesheetReimbursement>();
}
