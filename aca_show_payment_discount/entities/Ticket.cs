using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace aca_show_payment_discount.entities;

[Table("registros")]
public class Ticket
{
    [Column("id")] public int Id { get; set; }

    [Column("nombre")] public string Nombre { get; set; }

    [Column("fecha_de_venta")] public DateTime FechaDeVenta { get; set; }

    [Column("compras")] public int Compras { get; set; }

    [Column("usuario_id")] public int? UsuarioId { get; set; }

    [ForeignKey("UsuarioId")] public Usuario Usuario { get; set; }
    
    [Column("cantidad_tickets")] public int? CantidadTickets { get; set; }

}