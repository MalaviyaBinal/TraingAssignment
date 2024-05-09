using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDocWebEntity.Data;

public partial class TimesheetDetail
{
    [Key]
    public int TimesheetDetailId { get; set; }

    public int TimesheetId { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? Sheetdate { get; set; }

    public int? ShiftHours { get; set; }

    public int? Housecall { get; set; }

    public int? PhoneConsult { get; set; }

    public int? PhysicianId { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? CreatedDate { get; set; }

    public int? CreatedBy { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedBy { get; set; }

    [Column("isWeekend")]
    public bool? IsWeekend { get; set; }

    [ForeignKey("PhysicianId")]
    [InverseProperty("TimesheetDetails")]
    public virtual Physician? Physician { get; set; }

    [ForeignKey("TimesheetId")]
    [InverseProperty("TimesheetDetails")]
    public virtual Timesheet Timesheet { get; set; } = null!;
}
