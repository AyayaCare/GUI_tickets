
using System;
using System.Linq;
using System.Printing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using aca_show_payment_discount.entities;

namespace aca_show_payment_discount;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
//Todo lo que sea x:Name se convierte en variable global
public partial class MainWindow : Window
{
    //Propiedades

    //Devuelve true si consigue cambiar el valor de string a int y si cumple la condicion


    //Ejecuta el metodo cada vez que aplique "Descuento"
    public int Descuento => (int)CalcularDescuento();
    public string DNI => txtDNI.Text;
    public string Cuantos => cbNumTicket.Text;

    public int NuevaCantidadTickets => CantidadTickets();



    private Usuario usuario_actual;

    private bool _ejecutar = true;


    private DbConnection _db = new DbConnection();

    public MainWindow()
    {
        InitializeComponent();
        mostrarMensajes();
    }

    private void btnVender_Click(object sender, RoutedEventArgs e)
    {
        // Validamos que el nombre no esté vacío




        if (string.IsNullOrWhiteSpace(txtDNI.Text))
        {
            lblResultado.Text = "Escribe un DNI existente";
            lblResultado.Foreground = System.Windows.Media.Brushes.Red;
            return;
        }

        /*if (string.IsNullOrEmpty(intEdad.Text))
        {
            lblResultado2.Text = "Error: Edad Necesaria";
            lblResultado2.Foreground = System.Windows.Media.Brushes.Red;
        }*/


        double precioConDescuento = Descuento;

        if (precioConDescuento == 0)
        {
            lblResultado3.Text = "Error: Selecciona el número de tickets a comprar";
            lblResultado3.Foreground = System.Windows.Media.Brushes.Red;
        }

        if (usuario_actual == null)
        {
            MessageBox.Show("Debe haber un usuario verificado");
            return;
        }

        int? cantidadTicketsDB = _db.Registros.Where(t => t.Nombre
                .Equals(usuario_actual.NombreUsuario))
            .Sum(t => (int?)t.CantidadTickets);

        if (cantidadTicketsDB + NuevaCantidadTickets > 4)
        {
            lblResultado5.Text = "Este usuario ya ha comprado 4 boletos o excede el límite";
            return;
        }






        // Lógica sencilla de venta


        lblResultado3.Text =
            $"¡Hecho! {Cuantos} TICKET(s) vendido(s) a {usuario_actual.NombreUsuario} por ${Descuento}.";
        lblResultado3.Foreground = System.Windows.Media.Brushes.Green;

        // Limpiamos el campo de nombre para la siguiente venta

        var newData = new Ticket
        {
            Nombre = usuario_actual.NombreUsuario,
            Compras = Descuento,
            FechaDeVenta = DateTime.Now,
            CantidadTickets = NuevaCantidadTickets,
            Usuario = usuario_actual

        };


        _db.Add(newData);
        _db.SaveChanges();
        txtDNI.Clear();
        usuario_actual = null;
        lblResultado5.Text = "";

    }

    /*private void BtnVender_OnClick(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }*/

    /// <summary>
    /// accede al valor del Tag, que es un objeto, aunque por patrones es ComBoxItem
    /// Comprueba si pertenece a ese tipo y lo almacena en variable temporal item
    /// Si no es del tipo mencionado, devuelve false
    /// </summary>

    public double CalcularDescuento()
    {
        if (cbNumTicket.SelectedItem is ComboBoxItem item && item.Tag != null)
        {
            double seleccion = double.Parse(item.Tag.ToString());
            double valorFinal;
            int cantidadTickets;
            switch (seleccion.ToString())
            {
                case "20000":
                    return valorFinal = seleccion;
                case "40000":
                    valorFinal = 40000 - (40000 * 0.10);

                    return valorFinal;
                case "60000":
                    valorFinal = 60000 - (60000 * 0.15);
                    return valorFinal;
                case "80000":
                    valorFinal = 80000 - (80000 * 0.20);
                    return valorFinal;
            }

        }

        return 0;
    }

    public int CantidadTickets()
    {
        if (cbNumTicket.SelectedItem is ComboBoxItem item && item.Tag != null)
        {
            double seleccion = double.Parse(item.Tag.ToString());

            int cantidadTickets;
            switch (seleccion.ToString())
            {
                case "20000":
                    cantidadTickets = 1;
                    return cantidadTickets;
                case "40000":
                    cantidadTickets = 2;

                    return cantidadTickets;
                case "60000":
                    cantidadTickets = 3;
                    return cantidadTickets;
                case "80000":
                    cantidadTickets = 4;
                    return cantidadTickets;
            }

        }

        return 0;
    }

    private void cbNumTicket_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        TotalPagar.Text = $"Valor total a cancelar: ${Descuento}";
    }

    private void verificar_usuario_Cl(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(DNI))
        {
            lblResultado.Text = "Debe ingresar el DNI";
            lblResultado.Foreground = System.Windows.Media.Brushes.Red;
            return;
        }

        if (DNI is not null)
        {
            lblResultado.Text = "";
            Resultado6.Text = "";
        }

        if (!int.TryParse(DNI, out int dniInteger))
        {
            MessageBox.Show("El DNI debe ser numérico");
        }

        var usuario = _db.Usuario.FirstOrDefault(u => u.Dni == dniInteger);


        if (usuario != null)
        {
            lblResultado5.Text = $"Usuario: {usuario.NombreUsuario}";
            usuario_actual = usuario;
        }
        else
        {
            usuario_actual = null;
            lblResultado.Text = "Cliente no registrado";
            lblResultado.Foreground = System.Windows.Media.Brushes.Red;


        }
    }

    private void Registrar_cl(object sender, RoutedEventArgs e)
    {
        RegistroUsuario registroUsuario = new RegistroUsuario();
        registroUsuario.ShowDialog();
        verificar_usuario_Cl(null, null);
    }

    private void registro_usuario_Cl(object sender, RoutedEventArgs e)
    {

        if (usuario_actual == null)
        {
            Resultado6.Text = "Debe ingresar un usuario para verificar datos";
            return;
        }

        var registro_ventas_usuario = _db.Registros.Where(v => v.UsuarioId == usuario_actual.Id).ToList();
        string resultado = "";
        if (registro_ventas_usuario.Count == 0)
        {
            MessageBox.Show("Este usuario no cuenta con registros");
            return;
        }


        for (int i = 0; i < registro_ventas_usuario.Count; i++)
        {
            var total = registro_ventas_usuario[i];
            resultado += $"Fecha de Venta: {total.FechaDeVenta}" + "\n" +
                         $"Cantidad de Tickets: {total.CantidadTickets}" + "\n" +
                         $"Total Cancelado: {total.Compras}" +
                         "\n" + "\n";

        }

        MessageBox.Show(resultado);
        Resultado6.Text = "";

    }

    public async void mostrarMensajes()
    {
        string[] mensajes =
        {
            "¡Bienvenido al sistema de ventas!",
            "No olvides registrar antes de realizar cualquier acción",
            "Tras una venta, ¡La sesión se cierra!",
            "¡Verifica siempre el usuario actual!",
            "Sé feliz"
        };
        Brush[] colores =
        {
            Brushes.Red,
            Brushes.Purple,
            Brushes.OrangeRed,
            Brushes.DarkBlue,
            Brushes.DeepPink
        };
        int j = 0;
        
        while (_ejecutar)
        {
            AkaneTalk.Text = mensajes[j];
            AkaneTalk.Foreground= colores[j];
            j = (j + 1) % mensajes.Length;
            await Task.Delay(3000);
        }
    }

  
}


