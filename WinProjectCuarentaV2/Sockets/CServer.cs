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
    class CServer : CUser
    {
        private List<Socket> clientSockets { get; set; } //se modico el acceso
        private EndPoint epLocal;
       //RichTextBox rchMessages;
        public CServer(Button[] b,Button[]c,TextBox[]t) : base(b,c,t)
        {
            //CheckForIllegalCrossThreadCalls = false; IMPORTANTE
            clientSockets = new List<Socket>();
            SetupServer();
            turno = 1;
            MessageBox.Show("Juego Conectado, usted es el anfitrion");
        }
        private void SetupServer()
        {
            //lb_stt.Text = "Setting up server . . .";
            epLocal = new IPEndPoint(IPAddress.Parse(GetLocalIP()), 1433);
            userSocket.Bind(epLocal);
            userSocket.Listen(0);
            userSocket.BeginAccept(new AsyncCallback(AppceptCallback), null);
        }
        private string GetLocalIP()
        {
            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "127.0.0.1";
        }

        private void AppceptCallback(IAsyncResult ar)
        {
            Socket socket = userSocket.EndAccept(ar);
            clientSockets.Add(socket);
            //list_Client.Items.Add(socket.RemoteEndPoint.ToString());

            //lb_soluong.Text = "Số client đang kết nối: " + __ClientSockets.Count.ToString();
            //lb_stt.Text = "Client connected. . .";
            socket.BeginReceive(receivedBuf, 0, receivedBuf.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
            userSocket.BeginAccept(new AsyncCallback(AppceptCallback), null);

            //MessageBox.Show("Se conecto");//que frustante
            string message = (clientSockets.Count).ToString();
            SendData(socket, message);
            if (clientSockets.Count == 3)
            {
                MessageBox.Show("Empieza la Partida...");
                TryToUpdateButtonCheckAccess(b[5], true);
            }

        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            Socket socket = (Socket)ar.AsyncState;
            //bool type;
            if (socket.Connected)
            {
                int received;
                try
                {
                    received = socket.EndReceive(ar);
                }
                catch (Exception)
                {
                    for (int i = 0; i < clientSockets.Count; i++)
                    {
                        if (clientSockets[i].RemoteEndPoint.ToString().Equals(socket.RemoteEndPoint.ToString()))
                        {
                            clientSockets.RemoveAt(i);
                        }
                    }
                    return;
                }
                if (received != 0)
                {
                    byte[] dataBuf = new byte[received];
                    Array.Copy(receivedBuf, dataBuf, received);
                    string text = Encoding.ASCII.GetString(dataBuf);
                    //aqui se puede analizar con otro identificador el envio de puntos
                    if (text.IndexOf("@") != -1) //verifica si el string recibido tiene @ para saber que recibio una barajeada
                        Send(text);
                    else{ //si no es barajeada entonces analizara lo que recibio
                        string[] textoRecibido = text.Split(',');
                        identificacionCadenaRecibida(textoRecibido);
                    }
                }
                else{
                    for (int i = 0; i < clientSockets.Count; i++)
                    {
                        if (clientSockets[i].RemoteEndPoint.ToString().Equals(socket.RemoteEndPoint.ToString()))
                        {
                            clientSockets.RemoveAt(i);
                            //lb_soluong.Text = "Số client đang kết nối: " + __ClientSockets.Count.ToString();
                        }
                    }
                }
            }
            socket.BeginReceive(receivedBuf, 0, receivedBuf.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
        }
        override
        public void identificacionCadenaRecibida(string[] textoRecibido)
        {
            switch (textoRecibido.Length)
            {
                case 2:
                    if (textoRecibido[1] == 5.ToString())
                        TryToUpdateButtonCheckAccess(b[5], true);
                    else
                        SendT(textoRecibido[0] + "," + textoRecibido[1]);
                    break;
                case 3:
                    if (textoRecibido[2] == 5.ToString())
                        pasoTurnoCartas();
                    else
                        SendT(textoRecibido[0] + "," + textoRecibido[1]+","+textoRecibido[2]);
                    break;
                case 4:
                    turnoActual = int.Parse(textoRecibido[3]);
                    if (turnoActual == turno)
                        TryToUpdateButtonCheckAccess(b[5], true);
                    else
                        SendT(textoRecibido[0] + "," + textoRecibido[1] + "," + textoRecibido[2]+","+textoRecibido[3]);
                    break;
                default:
                    break;
            }
            //if (text.IndexOf("turno") != -1)
            //{
            //    if (text.IndexOf("turno,5") != -1)
            //    {
            //        TryToUpdateButtonCheckAccess(b[5], true);
            //        return;
            //    }
            //    else
            //        type = false;
            //}
            //else
            //    type = true;//se dee realizar un afuncion de identificacion
            //SendGeneral(type, text);
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
        private void SendData(Socket socket, string noidung) //se modifico el acceso
        {
            byte[] data = Encoding.ASCII.GetBytes(noidung);
            socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket); //
            userSocket.BeginAccept(new AsyncCallback(AppceptCallback), null);
        }
        private void SendCallback(IAsyncResult AR)
        {
            Socket socket = (Socket)AR.AsyncState;
            socket.EndSend(AR);
        }
        override
        public void SendGeneral(string data)
        {
            String[] data2;
            
            data2 = data.Split('@');
            if (data.IndexOf("@") != -1 && data2.Length > 4)
            {
                data2 = data.Split('@');
                Send(data2);
            }
            else {
                Send(data);
            }

        }
        override
        public void Send(string data)
        {
            for (int j = 0; j < clientSockets.Count; j++)
            {
                SendData(clientSockets[j], data); //
            }
            UpdateGame(data);
        }
        override
        public void SendT(string data)
        {
            for (int j = 0; j < clientSockets.Count; j++)
            {
                SendData(clientSockets[j], data); //
            }
        }
        public void Send(string[] data)
        {
            for (int j = 0,k=0; j < clientSockets.Count; j++,k++)
            {
               SendData(clientSockets[j], data[k]+","+data[5]);
            }
            LoadDeck(data[3] + "," + data[5]);

        }
    }
}
