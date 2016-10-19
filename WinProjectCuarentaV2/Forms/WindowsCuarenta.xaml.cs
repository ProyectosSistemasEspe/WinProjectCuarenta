using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Net;
using System.Net.Sockets;
using System.Threading;


namespace WinProjectCuarentaV2
{
    /// <summary>
    /// Interaction logic for WindowsCuarenta.xaml
    /// </summary>
    public partial class WindowsCuarenta : Window
    {
        static int contadorBarajear = 0;
        int cartaActual = 0, cartaAnterior = 0;
        int turnoActual, turnoSiguiente;
        int numberPlayers,numberRep;

        Cuarenta c=new Cuarenta();
        Gamer j;
        User objUser;
        bool bandera;
        string IP;
       
        public WindowsCuarenta(bool usuario,string IP,int nP,int nR)
        {
            InitializeComponent();
            bandera = usuario;
            this.IP = IP;
            this.numberPlayers = nP;
            this.numberRep = nR;
            cartaActual = 0;cartaAnterior = 0;
            turnoActual = 1;turnoSiguiente = 2;
        }
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            Button[] b = { btnCard1, btnCard2, btnCard3, btnCard4, btnCard5, button };
            Button[] c = { btnC0, btnC1, btnC2, btnC3, btnC4, btnC5, btnC6, btnC7, btnC8, btnC9 };
            TextBox[] t = { txtPuntosGrupo1, txtPuntosGrupo2, txtCartonGrupo1, txtCartonGrupo2 };
            if (bandera)
            {
                button.IsEnabled = false;
                objUser = new Server(b,c,t,numberPlayers,numberRep);
                j = new Gamer(1, (numberPlayers == 4) ? true : false);
                turnoActual = 1;
                turnoSiguiente = turnoActual + 1;
            }
            else {
                button.IsEnabled = false;
                objUser = new Client(IP,b,c,t);
                j = new Gamer(objUser.getTurno(), (numberPlayers == 4) ? true : false);
                turnoActual = objUser.getTurno();
                objUser.Send("TwoPlayersReturn");
                Thread.Sleep(500);
                if (turnoActual == objUser.getNumberP())
                    turnoSiguiente = 1;
                else
                    turnoSiguiente = turnoActual + 1;
            }
        }
        private void btnCard1_Click(object sender, RoutedEventArgs e)
        {
            btnCard1.BorderThickness = new Thickness(5);
            btnCard1.BorderBrush = Brushes.Red;
            cartaActual = int.Parse(btnCard1.Content.ToString());
            activateRound(btnCard2, btnCard3, btnCard4, btnCard5);
        }
        private void btnCard1_MouseMove(object sender, MouseEventArgs e)
        {
            btnCard1.Margin = new Thickness(296, 455, 905, 0);
        }
        private void btnCard1_MouseLeave(object sender, MouseEventArgs e)
        {
            btnCard1.Margin = new Thickness(296, 470, 905, 0);
        }
        private void btnCard2_Click(object sender, RoutedEventArgs e)
        {
            btnCard2.Margin = new Thickness(431, 455, 0, 0);
            btnCard2.BorderBrush = Brushes.Red;
            cartaActual = int.Parse(btnCard2.Content.ToString());
            activateRound(btnCard1, btnCard3, btnCard4, btnCard5);
        }
        private void btnCard2_MouseMove_1(object sender, MouseEventArgs e)
        {
            btnCard2.Margin = new Thickness(431, 455, 0, 0);
        }
        private void btnCard2_MouseLeave(object sender, MouseEventArgs e)
        {
            btnCard2.Margin = new Thickness(431, 470, 0, 0);
        }
        private void btnCard3_Click(object sender, RoutedEventArgs e)
        {
            btnCard3.Margin = new Thickness(566, 455, 0, 0);
            btnCard3.BorderBrush = Brushes.Red;
            cartaActual = int.Parse(btnCard3.Content.ToString());
            activateRound(btnCard1, btnCard2, btnCard4, btnCard5);
        }
        private void btnCard3_MouseMove(object sender, MouseEventArgs e)
        {
            btnCard3.Margin = new Thickness(566, 455, 0, 0);
        }
        private void btnCard3_MouseLeave(object sender, MouseEventArgs e)
        {
            btnCard3.Margin = new Thickness(566, 470, 0, 0);
        }
        private void btnCard4_Click(object sender, RoutedEventArgs e)
        {
            btnCard4.Margin = new Thickness(701, 455, 0, 0);
            btnCard4.BorderBrush = Brushes.Red;
            cartaActual = int.Parse(btnCard4.Content.ToString());
            activateRound(btnCard1, btnCard3, btnCard2, btnCard5);
        }
        private void btnCard4_MouseMove(object sender, MouseEventArgs e)
        {
            btnCard4.Margin = new Thickness(701, 455, 0, 0);
        }
        private void btnCard4_MouseLeave(object sender, MouseEventArgs e)
        {
            btnCard4.Margin = new Thickness(701, 470, 0, 0);
        }
        private void btnCard5_Click(object sender, RoutedEventArgs e)
        {
            btnCard5.Margin = new Thickness(836, 455, 0, 0);
            btnCard5.BorderBrush = Brushes.Red;
            cartaActual = int.Parse(btnCard5.Content.ToString());
            activateRound(btnCard1, btnCard3, btnCard4, btnCard2);
        }
        private void btnCard5_MouseMove(object sender, MouseEventArgs e)
        {
            btnCard5.Margin = new Thickness(836, 455, 0, 0);
        }
        private void btnCard5_MouseLeave(object sender, MouseEventArgs e)
        {
            btnCard5.Margin = new Thickness(836, 470, 0, 0);
        }
        private void activateRound(Button a, Button b, Button c, Button d)
        {
            this.btnJugar.IsEnabled = true;
            this.btnEscalera.IsEnabled = true;
            this.btnSuma.IsEnabled = true;
            this.btnCancelar.IsEnabled = true;
            a.IsEnabled = false; b.IsEnabled = false; c.IsEnabled = false; d.IsEnabled = false;
        }
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            cancelarJugada();
            //btnCard5.IsEnabled = true; btnCard4.IsEnabled = true; btnCard3.IsEnabled = true; btnCard2.IsEnabled = true; btnCard1.IsEnabled = true;
        }
        private void btnJugar_Click(object sender, RoutedEventArgs e)
        {
            Button[] b = { btnC0, btnC1, btnC2, btnC3, btnC4, btnC5, btnC6, btnC7, btnC8, btnC9 };
            String fin="", siguiente="";
            bool play=true;
            cartaAnterior = objUser.getCartaAnterior();
            c.setMesa(getMesa(b));
            if (objUser.getTurno() == 1 || objUser.getTurno() == 3)
            {
                play = c.lanzarcarta(cartaAnterior, cartaActual, txtCartonGrupo1, txtPuntosGrupo1, txtCartonGrupo2, txtPuntosGrupo2);
                c.Limpia(txtPuntosGrupo1);
            }
            else if (objUser.getTurno() == 2 || objUser.getTurno() == 4)
            {
                play = c.lanzarcarta(cartaAnterior, cartaActual, txtCartonGrupo2, txtPuntosGrupo2, txtCartonGrupo1, txtPuntosGrupo1);
                c.Limpia(txtPuntosGrupo2); 
            }
            if (play)
            {
                fin = c.findeljuego(txtCartonGrupo1, txtPuntosGrupo1, txtCartonGrupo2, txtPuntosGrupo2, 0);
                siguiente = c.getEnvio(0, txtCartonGrupo1, txtPuntosGrupo1, txtCartonGrupo2, txtPuntosGrupo2);
            }
            else
            {
                fin = c.findeljuego(txtCartonGrupo1, txtPuntosGrupo1, txtCartonGrupo2, txtPuntosGrupo2, cartaActual);
                siguiente = c.getEnvio(cartaActual, txtCartonGrupo1, txtPuntosGrupo1, txtCartonGrupo2, txtPuntosGrupo2);
                cartaAnterior = cartaActual;
            }
            desactivarCarta();
            cancelarJugada();
            if (fin != "")
            {
                objUser.Send(fin);
                //MessageBox.Show("Datos son " + fin);
            }
            else
            {
                objUser.SendT("turno,carta," + (objUser.getTurno() + 1).ToString());
               //aqui se debe enviar el resultado de lo que quedara en la mesa

                Thread.Sleep(500);
                objUser.Send(siguiente);
                //MessageBox.Show("Datos son " + siguiente);
            }
            
            verificarTurno();
            desactivarCartasUsuario();
        }
        private void btnSuma_Click(object sender, RoutedEventArgs e)
        {
            Button[] b = { btnC0, btnC1, btnC2, btnC3, btnC4, btnC5, btnC6, btnC7, btnC8, btnC9 };
            String fin = "", siguiente = "";
            bool play = true;
            cartaAnterior = objUser.getCartaAnterior();
            c.setMesa(getMesa(b));
            if (objUser.getTurno() == 1 || objUser.getTurno() == 3)
            {
                play = c.sumacartas(cartaActual,txtCartonGrupo1,txtCartonGrupo2);
                c.Limpia(txtPuntosGrupo1);
            }
            else if (objUser.getTurno() == 2 || objUser.getTurno() == 4)
            {
                play = c.sumacartas(cartaActual,txtCartonGrupo2,txtCartonGrupo1);
                c.Limpia(txtPuntosGrupo2);
            }
            if (play)
            {
                fin = c.findeljuego(txtCartonGrupo1, txtPuntosGrupo1, txtCartonGrupo2, txtPuntosGrupo2, 0);
                siguiente = c.getEnvio(0, txtCartonGrupo1, txtPuntosGrupo1, txtCartonGrupo2, txtPuntosGrupo2);
            }
            else
            {
                fin = c.findeljuego(txtCartonGrupo1, txtPuntosGrupo1, txtCartonGrupo2, txtPuntosGrupo2, cartaActual);
                siguiente = c.getEnvio(cartaActual, txtCartonGrupo1, txtPuntosGrupo1, txtCartonGrupo2, txtPuntosGrupo2);
                cartaAnterior = cartaActual;
            }
            desactivarCarta();
            cancelarJugada();
            if (fin != "")
            {
                objUser.Send(fin);
                //MessageBox.Show("Datos son " + fin);
            }
            else
            {
                objUser.SendT("turno,carta," + (objUser.getTurno() + 1).ToString());
                //aqui se debe enviar el resultado de lo que quedara en la mesa
                Thread.Sleep(500);
                objUser.Send(siguiente);
                //MessageBox.Show("Datos son " + siguiente);
            }
            verificarTurno();
            desactivarCartasUsuario();
        }
        private void btnEscalera_Click(object sender, RoutedEventArgs e)
        {
            Button[] b = { btnC0, btnC1, btnC2, btnC3, btnC4, btnC5, btnC6, btnC7, btnC8, btnC9 };
            String fin = "", siguiente = "";
            bool play = true;
            cartaAnterior = objUser.getCartaAnterior();
            c.setMesa(getMesa(b));
            if (objUser.getTurno() == 1 || objUser.getTurno() == 3)
            {
                play = c.escaleraB(cartaActual, txtCartonGrupo1, txtCartonGrupo2);
                c.Limpia(txtPuntosGrupo1);
            }
            else if (objUser.getTurno() == 2 || objUser.getTurno() == 4)
            {
                play = c.escaleraB(cartaActual, txtCartonGrupo2, txtCartonGrupo1);
                c.Limpia(txtPuntosGrupo2);
            }
            if (play)
            {
                fin = c.findeljuego(txtCartonGrupo1, txtPuntosGrupo1, txtCartonGrupo2, txtPuntosGrupo2, 0);
                siguiente = c.getEnvio(0, txtCartonGrupo1, txtPuntosGrupo1, txtCartonGrupo2, txtPuntosGrupo2);
            }
            else
            {
                fin = c.findeljuego(txtCartonGrupo1, txtPuntosGrupo1, txtCartonGrupo2, txtPuntosGrupo2, cartaActual);
                siguiente = c.getEnvio(cartaActual, txtCartonGrupo1, txtPuntosGrupo1, txtCartonGrupo2, txtPuntosGrupo2);
                cartaAnterior = cartaActual;
            }
            desactivarCarta();
            cancelarJugada();
            if (fin != "")
            {
                objUser.Send(fin);
                //MessageBox.Show("Datos son " + fin);
            }
            else
            {
                objUser.SendT("turno,carta," + (objUser.getTurno() + 1).ToString());
                //aqui se debe enviar el resultado de lo que quedara en la mesa
                Thread.Sleep(500);
                objUser.Send(siguiente);
                //MessageBox.Show("Datos son " + siguiente);
            }
            verificarTurno();
            desactivarCartasUsuario();

        }
        private void button_Click(object sender, RoutedEventArgs e)//boton para barajear las cartas
        {
            j.barajear();
            objUser.SendGeneral(j.ConcatenaraTodo()+"@"+objUser.getTurno().ToString()+"@xd@"+ objUser.getTurno().ToString());
            Thread.Sleep(500);
            button.IsEnabled = false;
            objUser.SendT("turno,carta," + (objUser.getTurno() + 1).ToString());
            if (contadorBarajear < 1)
            {
                contadorBarajear=objUser.getNumberR();
                //objUser.SendT("turno,carta,actual," + (objUser.getTurnoActual()).ToString());
            }
            else
                contadorBarajear--;
            //objUser.pasoTurnoCartas();
            
        }
        private void desactivarCarta()
        {
            Button[] b = { btnCard1, btnCard2, btnCard3, btnCard4, btnCard5 };
            for(int i = 0; i < 5; i++)
            {
                if (cartaActual == int.Parse(b[i].Content.ToString()))
                {
                    b[i].IsEnabled = false;
                    b[i].Visibility = Visibility.Hidden;
                }
            }
        }
        private void cancelarJugada()
        {
            Button[] b = { btnCard1, btnCard2, btnCard3, btnCard4, btnCard5 };
            this.btnJugar.IsEnabled = false;
            this.btnEscalera.IsEnabled = false;
            this.btnSuma.IsEnabled = false;
            this.btnCancelar.IsEnabled = false;
            for (int i = 0; i < 5; i++)
            {
                b[i].IsEnabled = true;
                b[i].BorderBrush = Brushes.Gray;
                b[i].BorderThickness = new Thickness(1);
            }
        }
        private void verificarTurno()
        {
            string fin = "";
            Button[] b = { btnCard1, btnCard2, btnCard3, btnCard4, btnCard5 };
            int contadorCartasDesac = 0;
            for (int i = 0; i < 5; i++)
            {
                if(b[i].IsVisible == false)
                {
                    contadorCartasDesac++;
                }
            }
            if (contadorCartasDesac == 5)
            {
                if (contadorBarajear == 0)
                {
                    if (objUser.getTurnoActual() == objUser.getTurno())
                    {
                        if (objUser.getTurnoActual() < objUser.getNumberP())
                            objUser.setTurnoActual(objUser.getTurnoActual() + 1);
                        else
                            objUser.setTurnoActual(1);
                        //objUser.SendT("turno," + (objUser.getTurno()).ToString());
                        c.carton(txtPuntosGrupo1,txtCartonGrupo1);
                        c.carton(txtPuntosGrupo2, txtCartonGrupo2);
                        fin = c.findeljuego(txtCartonGrupo1, txtPuntosGrupo1, txtCartonGrupo2, txtPuntosGrupo2, 0);
                        if (fin == "")
                        {
                            objUser.SendT("turno,carta,actual," + (objUser.getTurnoActual()).ToString());
                            Thread.Sleep(500);
                            objUser.Send(initialSend());
                        }
                        else
                            objUser.Send(fin);
                    }
                }
                else
                    button.IsEnabled = true;
            }
        }
        private List<int> getMesa(Button[]b)
        {
            List<int> mesa=new List<int>();
            for(int i = 0; i < b.Length; i++)
            {
                mesa.Add(int.Parse(b[i].Content.ToString()));
            }
            return mesa;
        }
        private void desactivarCartasUsuario()
        {
            Button[] b = { btnCard1, btnCard2, btnCard3, btnCard4, btnCard5};
            for (int i = 0; i < 5; i++)
            {
                b[i].IsEnabled = false;
            }
        }
        private string initialSend()
        {
            return "0@0@0,0,0,0,0,0,0,0,0,0@" + txtPuntosGrupo1.Text +","+ txtPuntosGrupo2.Text+",0,0";
        }
   }
}
