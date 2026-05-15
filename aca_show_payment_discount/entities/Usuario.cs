using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace aca_show_payment_discount.entities;
[Table("usuario")]
public class Usuario
{
    [Column("id")] 
    public int Id { get; set; }

    [Column("nombre")] 
    public string NombreUsuario { get; set; }
    
    [Column("dni")]
    public int Dni { get; set; }
    
    [Column("edad")]
    public int Edad { get; set; }
    
    public List<Ticket> Tickets { get; set; }
}