using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Threading;

namespace WinProjectCuarentaV2
{
    abstract class CUser
    {
        protected byte[] receivedBuf = new byte[1024];
        protected Socket userSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        protected int turno, cartaanterior, turnoActual, numberPlayers;
        protected Button[] b;
        protected Button[] c;
        protected TextBox[] t;
        public CUser(Button[]b,Button[]c,TextBox[] t)
        {
            //this line is only a test
            turnoActual = 1;
            this.c = c;
            this.b = b;
            this.t = t;
            cartaanterior = 0;
            numberPlayers = 4;
            userSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            
        }
        public abstract void SendGeneral(string data);
        public abstract void Send(string data);
        public abstract void SendT(string data);
        public abstract void identificacionCadenaRecibida(string[] textoRecibido);

        public void pasoTurnoCartas()
        {
            for (int i = 0; i < 5; i++)
            {
                TryToUpdateButtonCheckAccess(b[i], true);
            }
        }

        private void TryToUpdateButtonCheckAccess(object uiObject, bool estado)
        {
            Button theButton = uiObject as Button;
            if (theButton != null)
            {
                if (theButton.Dispatcher.CheckAccess())
                {
                    StateButton(theButton, estado);
                }
                else
                {
                    theButton.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new IniciatializeButtonDelegate(StateButton), theButton, estado);
                }
            }
        }

        private delegate void IniciatializeButtonDelegate(Button b, bool estado);

        private void StateButton(Button b, bool estado)
        {
            b.IsEnabled = estado;
        }
        public void LoadDeck(String barajeada)
        {
           // bool estadoCarta;
            string[] cardsSeparated = barajeada.Split(',');
            int turnoRecibido = int.Parse(cardsSeparated[5]);
           // if (turnoRecibido == turno)
               // estadoCarta = true;
           // else
              //  estadoCarta = false;

            for (int j = 0; j < 5; j++)
            {             
                TryToUpdateButtonCheckAccess(b[j], cardsSeparated[j],false); //estado carta
            }
        }

        private void TryToUpdateButtonCheckAccess(object uiObject, string card, bool estadoCarta)
        {
            Button theButton = uiObject as Button;

            if (theButton != null)
            {
                
                // Checking if this thread has access to the object.
                if (theButton.Dispatcher.CheckAccess())
                {
                    // This thread has access so it can update the UI thread.
                    //UpdateButtonUI(theButton);
                    IniciatializeButton(theButton,card, estadoCarta);
                }
                else
                {
                    // This thread does not have access to the UI thread.
                    // Place the update method on the Dispatchr of the UI thread.
                    theButton.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                        new IniciatializeButtonDelegate2(IniciatializeButton), theButton, card, estadoCarta);
                }
            }
        }

        private delegate void IniciatializeButtonDelegate2(Button b, string card, bool estadoCarta);

        private void IniciatializeButton(Button b, string card, bool estadoCarta)
        {
            var brush = new ImageBrush();
            brush.ImageSource = new BitmapImage(new Uri("C:\\Cuarenta Recursos\\Cards\\Classic\\" + card + ".png"));
            b.IsEnabled = estadoCarta;
            b.Visibility = Visibility.Visible;
            b.Background = brush;
            b.Content = card;
            b.FontSize = 1;
        }
        public int getCartaAnterior()
        {
            return cartaanterior;
        }

        public int getTurnoActual()
        {
            return turnoActual;
        }
        public int getNumberP()
        {
            return numberPlayers;
        }
        public void UpdateGame(String texto)
        {
            String[] dataSeparated = texto.Split('@');
            if (dataSeparated[0] != "1") {
                cartaanterior = int.Parse(dataSeparated[1]);
                LoadTable(dataSeparated[2]);
                //LoadPoints(dataSeparated[3]);
            }
            else
            {
                MessageBox.Show("Juego terminado, ultimo equipo en lanzar carta gana el juego", "Aviso");
            }
            LoadPoints(dataSeparated[3]);
        }
        public void LoadTable(String texto)
        {
            String[] cardsSeparated=texto.Split(',');
            for (int j = 0; j < 10; j++)
            {
                TryToUpdateButtonCheckAccess(c[j], cardsSeparated[j],true);
            }
        }
        public void LoadPoints(String texto)
        {
            String[] pointsSeparated = texto.Split(',');
            for (int j = 0; j < 4; j++)
            {
                TryToUpdateTextBoxCheckAccess(t[j], pointsSeparated[j]);
            }
        }
        private void TryToUpdateTextBoxCheckAccess(object uiObject, string card)
        {
            TextBox theText = uiObject as TextBox;

            if (theText != null)
            {

                // Checking if this thread has access to the object.
                if (theText.Dispatcher.CheckAccess())
                {
                    // This thread has access so it can update the UI thread.
                    //UpdateButtonUI(theButton);
                    initializeTextBox(theText, card);
                }
                else
                {
                    // This thread does not have access to the UI thread.
                    // Place the update method on the Dispatchr of the UI thread.
                    theText.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                        new iniciatializeTextBoxDelegate2(initializeTextBox), theText, card);
                }
            }
        }
        private delegate void iniciatializeTextBoxDelegate2(TextBox b, string data);
        private void initializeTextBox(TextBox t,string data)
        {
            t.Text = data;
        }
        public int getTurno()
        {
            return turno;
        }

        public void setTurnoActual(int turnoActual)
        {
            this.turnoActual = turnoActual;
        }

    }
}
