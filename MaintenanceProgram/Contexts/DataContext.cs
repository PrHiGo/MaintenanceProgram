using System.Runtime.InteropServices.ComTypes;
using MaintenanceProgram.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace MaintenanceProgram.Contexts;

internal class DataContext : DbContext
{
    #region constructors and overrides
    public DataContext()
    {
    }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\thoma\OneDrive\Skrivbord\Webbutveckling_.NET\Skolarbete\Databashantering\MaintenanceProgram\MaintenanceProgram\MaintenaceSQL.mdf;Integrated Security=True;Connect Timeout=30");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
    #endregion

    public DbSet<TicketEntity> Tickets { get; set; }
    public DbSet<CommentEntity> Comments { get; set; }
    public DbSet<StatusTypeEntity> StatusTypes { get; set; }
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<UserTypeEntity> UserTypes { get; set; }


}

