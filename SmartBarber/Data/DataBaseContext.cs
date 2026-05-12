using Microsoft.EntityFrameworkCore;

public class DatabaseContext : DbContext
{
  public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {}
  public DbSet<Pessoa> Pessoa { get; set; }
  public DbSet<Cliente> Cliente { get; set; }
  public DbSet<Barbeiro> Barbeiro { get; set; }
  public DbSet<Atendimento> Atendimento { get; set; }
  public DbSet<Servico> Servico { get; set; }
  public DbSet<Categoria> Categoria { get; set; }
  public DbSet<Cep> Cep { get; set; }
  public DbSet<Cidade> Cidade { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    // TPT
    modelBuilder.Entity<Pessoa>().ToTable("Pessoa");

    modelBuilder.Entity<Cliente>().ToTable("Cliente");

    modelBuilder.Entity<Barbeiro>().ToTable("Barbeiro");

    // Atendimento
    modelBuilder.Entity<Atendimento>()
        .HasOne(a => a.Cliente)
        .WithMany(c => c.Atendimentos)
        .HasForeignKey(a => a.ClienteId)
        .OnDelete(DeleteBehavior.NoAction);

    modelBuilder.Entity<Atendimento>()
        .HasOne(a => a.Barbeiro)
        .WithMany(b => b.Atendimentos)
        .HasForeignKey(a => a.BarbeiroId)
        .OnDelete(DeleteBehavior.NoAction);
  }
}