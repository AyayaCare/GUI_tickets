using System.Windows;
using aca_show_payment_discount.entities;

namespace aca_show_payment_discount;

public partial class RegistroUsuario : Window
{
    public bool MayorEdad => int.TryParse(intEdad.Text, out int edad) && edad >= 18;
    
    public string? Nombre => vUsuario.Text;
    
    public int? NewDNI => int.TryParse(varDni.Text, out int dni) ? dni : null;

    public int? EdadValidada => int.TryParse(intEdad.Text, out int edad2) ? edad2 : 0;
    
    public RegistroUsuario()
    {
        InitializeComponent();
    }

    private void b_clRegistrar(object sender, RoutedEventArgs e)
    {
        
        if (string.IsNullOrWhiteSpace(Nombre))
        {
            lblResultado.Text = "Debe ingresar un nombre";
            lblResultado.Foreground = System.Windows.Media.Brushes.Red;
            return;
        }
        if (!MayorEdad)
        {
            lblResultado2.Text = "El cliente debe ser mayor de edad";
            lblResultado2.Foreground = System.Windows.Media.Brushes.Red;
            return;
        }

       

        if (EdadValidada is null)
        {
            lblResultado2.Text = "Debe ingresar su edad";
            lblResultado2.Foreground = System.Windows.Media.Brushes.Red;
            return;
        }

        if (EdadValidada <= 0)
        {
            lblResultado2.Text = "La edad no puede ser cero o negativa";
            return;
        }
        
        if (string.IsNullOrWhiteSpace(Nombre))
        {
            lblResultado.Text = "Debe ingresar un nombre";
            lblResultado2.Foreground = System.Windows.Media.Brushes.Red;
            return;
        }

        if (NewDNI is null)
        {
            lblResultado3.Text = "Debe ingresar su DNI";
            lblResultado3.Foreground = System.Windows.Media.Brushes.Red;
            return;
        }
        
        

        DbConnection _db = new DbConnection();

        var usuario = new Usuario()
        {
            Dni = NewDNI ?? 0000,
            NombreUsuario = Nombre,
            Edad = EdadValidada ?? 0
        };
        
        /*int count = usuario.Tickets.Count();
        for (int i = 0; i < count; i++)
        {
            if (count >= 4)
            {
                lblResultado4.Text = "Este usuario ha alcanzado el número máximo de compras";
                return;
            }
        }*/
       
        _db.Add(usuario);
        _db.SaveChanges();
        
        lblResultado4.Text = "HECHO! El usuario ha sido registrado";
        lblResultado4.Foreground = System.Windows.Media.Brushes.AntiqueWhite;
        vUsuario.Clear();
        intEdad.Clear();
        varDni.Clear();
        lblResultado.Text = "";
        lblResultado2.Text = "";
        lblResultado3.Text = "";
        

    }
}