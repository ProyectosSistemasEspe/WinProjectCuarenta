using System;
using System.Collections.Generic;
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
    class Client : User
    {
        EndPoint epRemote;
        string IPremote;
        //int turno;
        public Client(string IPRemote,Button[]b,Button[]c,TextBox[]t) : base(b,c,t)
        {
            IPremote = IPRemote;
            Conectar(IPremote);
            numberOfRep = 1;
            numberPlayers = 4;
            //MessageBox.Show("Usted es un jugador, bienvenido al juego");
        }

        private void ReceiveData(IAsyncResult ar)
        {
            try
            {
                Socket socket = (Socket)ar.AsyncState;
                int received = socket.EndReceive(ar);
                byte[] dataBuf = new byte[received];
                Array.Copy(receivedBuf, dataBuf, received);
                string text = (Encoding.ASCII.GetString(dataBuf));
                string[] textoRecibido = text.Split(',');
                if (textoRecibido[0].Equals ("isTwo"))
                {
                    numberPlayers = 2;
                    numberOfRep = 3;
                }
                else
                {
                    identificacionCadenaRecibida(textoRecibido);
                }
                userSocket.BeginReceive(receivedBuf, 0, receivedBuf.Length, SocketFlags.None, new AsyncCallback(ReceiveData), userSocket);
                
            }
            catch (SocketException)
            {
                MessageBox.Show("Servidor Desconectado...\n\nJuego Finalizado");
                //Application.Exit();
            } 
        }

        override
        public void identificacionCadenaRecibida(string []textoRecibido)
        {
            //bool type = false;
            switch (textoRecibido.Length)
            {
                case 1:
                    turno = 1 + int.Parse(textoRecibido[0]);
                    MessageBox.Show("Bienvenido...\n\nJugador " + turno);
                    break;
                case 2:
                    if (textoRecibido[1] == turno.ToString())
                        TryToUpdateButtonCheckAccess(b[5], true);
                    break;
                case 3:
                    if (textoRecibido[2] == turno.ToString())
                        pasoTurnoCartas();
                    break;
                case 4:
                    turnoActual = int.Parse(textoRecibido[3]);
                    if (turnoActual == turno)
                        TryToUpdateButtonCheckAccess(b[5], true);
                    break;
                case 6:
                    string text = textoRecibido[0] + "," + textoRecibido[1]+ "," + textoRecibido[2]+ "," + textoRecibido[3]+ "," + textoRecibido[4]+","+textoRecibido[5];
                    LoadDeck(text);
                    break;
                default:
                    break;
            }
            if (textoRecibido.Length > 6)
            {
                UpdateGame(string.Join(",",textoRecibido.ToArray())); 
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

        private void LoopConnect(string IPRemote)
        {
            int attempts = 0;
            epRemote = new IPEndPoint(IPAddress.Parse(IPRemote), 1433);
            while (attempts<3&&!userSocket.Connected)
            {
                try
                {
                    userSocket.Connect(epRemote);
                    attempts++;
                    
                }
                catch (SocketException)
                {
                    
                    MessageBox.Show("Numero de intentos: " + attempts);
                }
            }
            
            if (attempts > 2)
            {
                MessageBox.Show("No Connectado");
            }
            else {
                MessageBox.Show("Conectado"); 
            }
        }
        override
        public void SendGeneral(string data)
        {
           Send(data);
        }
        override
        public void Send(string data)
        {
            if (userSocket.Connected)
            {
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                userSocket.Send(buffer);
                //IMPORTANTE rchMessages.AppendText("\nClient: " + txt_text.Text);
                //string text = Encoding.ASCII.GetString(buffer);
                //AddListBoxItem("\nClient: " + text);
                //rchMessages.AppendText("\nClient: " + text);
            }

        }
        override
        public void SendT(string data)
        {
            if (userSocket.Connected)
            {
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                userSocket.Send(buffer);
                //IMPORTANTE rchMessages.AppendText("\nClient: " + txt_text.Text);
                //string text = Encoding.ASCII.GetString(buffer);
                //AddListBoxItem("\nClient: " + text);
                //rchMessages.AppendText("\nClient: " + text);
            }
        }
        private void Conectar(string IPRemote)
        {
            LoopConnect(IPRemote);
            // SendLoop();
            userSocket.BeginReceive(receivedBuf, 0, receivedBuf.Length, SocketFlags.None, new AsyncCallback(ReceiveData), userSocket);
            //byte[] buffer = Encoding.ASCII.GetBytes("@@");//+ txName.Text);
            //userSocket.Send(buffer);
        }

        

    }
}
