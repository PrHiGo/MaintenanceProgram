namespace MaintenanceProgram.Models.Entities;

internal class StatusTypeEntity
{
    public int Id { get; set; }
    public string StatusName { get; set; } = null!;

    public ICollection<TicketEntity> Tickets { get; set; } = new HashSet<TicketEntity>();
}

