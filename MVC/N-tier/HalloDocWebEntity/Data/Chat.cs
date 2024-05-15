using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDocWebEntity.Data;

[Table("chat")]
public partial class Chat
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("senderId")]
    public int SenderId { get; set; }

    [Column("receiverId")]
    public int ReceiverId { get; set; }

    [Column("sentTime")]
    public TimeOnly SentTime { get; set; }

    [Column("sentDate", TypeName = "timestamp without time zone")]
    public DateTime SentDate { get; set; }

    [ForeignKey("ReceiverId")]
    [InverseProperty("ChatReceivers")]
    public virtual Aspnetuser Receiver { get; set; } = null!;

    [ForeignKey("SenderId")]
    [InverseProperty("ChatSenders")]
    public virtual Aspnetuser Sender { get; set; } = null!;
}
