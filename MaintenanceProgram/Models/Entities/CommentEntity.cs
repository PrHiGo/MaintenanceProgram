using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MaintenanceProgram.Models.Entities;

internal class CommentEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime Created { get; set; }
    public string Comment { get; set; } = null!;

    [ForeignKey("TicketId")]
    [DeleteBehavior(DeleteBehavior.Restrict)]
    public TicketEntity Ticket { get; set; } = null!;

    [ForeignKey("UserId")]
    [DeleteBehavior(DeleteBehavior.Restrict)]
    public UserEntity User { get; set; } = null!;
}

