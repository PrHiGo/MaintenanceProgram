using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaintenanceProgram.Models.Entities;

internal class UserEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
        
    public int UserTypeId { get; set; }

    public UserTypeEntity UserType { get; set; } = null!;

    public ICollection<TicketEntity> Tickets { get; set; } = new HashSet<TicketEntity>();
    public ICollection<CommentEntity> Comments { get; set; } = new HashSet<CommentEntity>();

}

