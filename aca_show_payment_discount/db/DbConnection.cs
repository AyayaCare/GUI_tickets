using System.Configuration;
using System.IO;

namespace aca_show_payment_discount;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using aca_show_payment_discount.entities;
public class DbConnection : DbContext
{

    public DbConnection()
    {
        //Database.EnsureCreated();
    }
    public DbSet<Ticket> Registros { get; set; }
    public DbSet<Usuario> Usuario { get; set; }
    /// <summary>
    /// Configuración del puente
    /// </summary>
    /// <param name="optionsBuilder">Todos los metodos necesarios para conectar la DB .</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        //string de la direccion para conectar la db
        string connectionString = config.GetConnectionString("DefaultConnection");
            
        var serverVersion = ServerVersion.AutoDetect(connectionString);
        
        optionsBuilder.UseMySql(connectionString, serverVersion);
        
        //optionsBuilder.UseSqlite("Data Source=tickets_db.db");
        base.OnConfiguring(optionsBuilder);
    }

  
}