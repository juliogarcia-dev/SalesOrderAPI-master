using Microsoft.EntityFrameworkCore;
using SalesOrderAPI.Models;

public class AppDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    public DbSet<SalesOrder> SalesOrders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Item> Items { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderItem>()
            .Property(oi => oi.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<OrderItem>()
            .ToTable("order_item")
            .Property(oi => oi.Id)
            .HasColumnName("oi_id");
        modelBuilder.Entity<OrderItem>()
            .Property(oi => oi.SalesOrderId)
            .HasColumnName("oi_salesorderid");
        modelBuilder.Entity<OrderItem>()
            .Property(oi => oi.ItemId)
            .HasColumnName("oi_itemid");
        modelBuilder.Entity<OrderItem>()
            .Property(oi => oi.Quantity)
            .HasColumnName("oi_quantity");
        modelBuilder.Entity<OrderItem>()
            .Property(oi => oi.Price)
            .HasColumnName("oi_price");

        modelBuilder.Entity<SalesOrder>()
            .ToTable("sales_order")
            .Property(so => so.Id)
            .HasColumnName("so_id");
        modelBuilder.Entity<SalesOrder>()
            .Property(so => so.OrderDate)
            .HasColumnName("so_date");
        modelBuilder.Entity<SalesOrder>()
            .Property(so => so.Total)
            .HasColumnName("so_total");

        modelBuilder.Entity<Item>()
            .ToTable("items")
            .Property(it => it.Id)
            .HasColumnName("it_id");
        modelBuilder.Entity<Item>()
            .Property(it => it.Name)
            .HasColumnName("it_name");
        modelBuilder.Entity<Item>()
            .Property(it => it.Price)
            .HasColumnName("it_price");

        base.OnModelCreating(modelBuilder);
    }

   protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Aqui, pegamos a string de conexão diretamente do arquivo de configuração
        var connectionString = _configuration.GetConnectionString("DefaultConnection");

        // Substituímos as variáveis de ambiente na string de conexão
        connectionString = connectionString?.Replace("{DB_HOST}", Environment.GetEnvironmentVariable("DB_HOST"))
                                            .Replace("{DB_PORT}", Environment.GetEnvironmentVariable("DB_PORT"))
                                            .Replace("{DB_USERNAME}", Environment.GetEnvironmentVariable("DB_USERNAME"))
                                            .Replace("{DB_PASSWORD}", Environment.GetEnvironmentVariable("DB_PASSWORD"))
                                            .Replace("{DB_DATABASE}", Environment.GetEnvironmentVariable("DB_DATABASE"));

        optionsBuilder.UseNpgsql(connectionString);
    }
}