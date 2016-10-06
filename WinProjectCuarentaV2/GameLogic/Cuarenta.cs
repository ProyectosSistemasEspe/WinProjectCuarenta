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

namespace WinProjectCuarentaV2
{
    class Cuarenta
    {
        private List<int> mesa = new List<int>();
        private List<int> mano = new List<int>();
        public Cuarenta()
        {
            for(int i = 0; i < 10; i++)
            {
                mesa.Add(0);
                if (i < 5)
                {
                    mano.Add(0);
                }
            }
        }
        public void setMesa(List<int>mesa)
        {
            this.mesa = mesa;
        }
        public String findeljuego(TextBox txtCartonGrupoActual, TextBox txtPuntosGrupoActual, TextBox txtCartonGrupoContrario, TextBox txtPuntosGrupoContrario, int cartaAjugar)
        {
            String p = txtPuntosGrupoActual.Text;
            String p2 = txtPuntosGrupoContrario.Text;
            int puntaje,puntaje2;
            puntaje = int.Parse(p);
            puntaje2 = int.Parse(p2);
            if (puntaje >= 40||puntaje2>=40)
            {
                return getEnvioFinal(cartaAjugar, txtCartonGrupoActual, txtPuntosGrupoActual, txtCartonGrupoContrario, txtPuntosGrupoContrario);
                //llamar a funcion de envio(se enviara el estado de fin y el nombre del equipo actual)
            }
            return "";//esto se controla afuera
        }
        public String getEnvio(int cartalanzada, TextBox txtCartonGrupoActual, TextBox txtPuntosGrupoActual, TextBox txtCartonGrupoContrario, TextBox txtPuntosGrupoContrario)
        {
            return "0@" +  cartalanzada + "@" + ConcatenarArrNumero(mesa) + "@" + txtPuntosGrupoActual.Text + "," + txtPuntosGrupoContrario.Text + "," + txtCartonGrupoActual.Text + "," + txtCartonGrupoContrario.Text;
        }
        public String getEnvioFinal(int cartalanzada, TextBox txtCartonGrupoActual, TextBox txtPuntosGrupoActual, TextBox txtCartonGrupoContrario, TextBox txtPuntosGrupoContrario)
        {
            return "1@"+ cartalanzada + "@" + ConcatenarArrNumero(mesa) + "@" + txtPuntosGrupoActual.Text + "," + txtPuntosGrupoContrario.Text + "," + txtCartonGrupoActual.Text + "," + txtCartonGrupoContrario.Text;
        }
        public String ConcatenarArrNumero(List<int> arreglo)
        {
            String cadena = "";

            cadena = cadena + arreglo[0].ToString();
            for (int i = 1; i < arreglo.Count; i++)
            {
                cadena = cadena + "," + arreglo[i].ToString();
            }
            return cadena;
        }
        public void carton(TextBox puntos, TextBox carton)
        {
            String p = puntos.Text, c = carton.Text;
            int carto = int.Parse(c);
            int punto = int.Parse(p);
            int punto2 = 0;
            if (punto < 30)
            {
                if (carto >= 19)
                {
                    punto2 = carto - 14;
                    if (punto2 % 2==1)
                    {
                        punto2++;
                    }
                    puntos.Text = (punto+ punto2).ToString();
                }
            }
           
        }

        public bool lanzarcarta(int cartaanterior, int cartaactual, TextBox txtCartonGrupoActual, TextBox txtPuntosGrupoActual, TextBox txtCartonGrupoContrario, TextBox txtPuntosGrupoContrario)
        // a es carta anterior, carta es carta seleccionada, grupo jugadores actuales para puntos 
        {
            //Recibe el numero de la carta y toca transformar su valor.
            String tCGA = txtCartonGrupoActual.Text;
            String tPGA = txtPuntosGrupoActual.Text;
            String tCGC = txtCartonGrupoContrario.Text;
            String tCPC = txtPuntosGrupoContrario.Text;
            int cartavalor = RetornarValor(cartaactual);
            int cartavaloranterior = RetornarValor(cartaanterior);
            int coeficienteatual = mesa.Count;

            int puntaje, puntaje2;
            if (cartavalor == cartavaloranterior)//Busca si la carta actual es igua a la anterior
            {

                buscarcarta(cartavaloranterior);//Borra la carta de la caida (carta anterior) del arraylist, la iguala a cero

                puntaje = int.Parse(tCGA);
                puntaje = puntaje + 2;
                txtCartonGrupoActual.Text = puntaje.ToString();
                puntaje2 = int.Parse(tPGA);
                puntaje2 = puntaje2 + 2;
                txtPuntosGrupoActual.Text = puntaje2.ToString();
                //findeljuego(txtCartonGrupoActual, txtPuntosGrupoActual, txtCartonGrupoContrario, txtPuntosGrupoContrario, jugadoractual);

                MessageBox.Show("Caida Grupo Actual, 2+ Carton, 2+Puntos", "Aviso");
                return true;
            }
            else
            {
                if (buscarcarta(cartavalor))//Busca si exite la carta lanzada en el maso
                {
                    puntaje = int.Parse(tCGA);
                    puntaje = puntaje + 2;
                    txtCartonGrupoActual.Text = puntaje.ToString();
                    MessageBox.Show("Carton Grupo Actual, 2+ Carton", "Aviso");
                    return true;
                }
            }
            ingresarcartaenelmaso(cartaactual);
            return false;
        }

        public bool escaleraB(int cartaactual, TextBox txtCartonActual, TextBox txtCartonContrario)
        { 
            String tCA = txtCartonActual.Text;
            String tCC = txtCartonContrario.Text;
            int cont = 0;
            int valorCarta = RetornarValor(cartaactual);
            for (int i = 0; i < mesa.Count; i++)
                {
                    if (valorCarta == RetornarValor(mesa[i]) && valorCarta <= 14)
                    {
                        mesa[i] = 0;
                        cont++;
                        valorCarta++;
                        i = -1;
                    }
                }
            if (cont > 1)
            {
                MessageBox.Show("Carton Grupo Actual, +"+(cont+1), "Aviso ");
                txtCartonActual.Text = (int.Parse(txtCartonActual.Text) + cont+1).ToString();
                return true;
            }
            if (cont == 1)
            {
                MessageBox.Show("Carton Grupo Contrario, +" + (cont + 1), "Aviso ");
                txtCartonContrario.Text = (int.Parse(txtCartonContrario.Text) + cont+1).ToString();
                return false;
            }
            if (cont < 1)
            {
                MessageBox.Show("Carton Grupo Contrario, +" + (cont + 1), "Aviso ");
                txtCartonContrario.Text = (int.Parse(txtCartonContrario.Text) + cont+1).ToString();
                ingresarcartaenelmaso(cartaactual);
                return false;
            }
            return false;
        }

        public bool sumacartas(int cartaactual, TextBox txtCartonGrupoActual, TextBox txtCartonGrupoContrario)
        {
            String tCA = txtCartonGrupoActual.Text;
            String tCC = txtCartonGrupoContrario.Text;
            int cartavalor = RetornarValor(cartaactual);
            int puntaje;

            for (int i = 0; i < mesa.Count; i++)
            {
                for (int j = i + 1; j < mesa.Count; j++)
                {
                    if (mesa[i] != 0 && mesa[j] != 0)
                    {
                        if (cartavalor == RetornarValor(mesa[i]) + RetornarValor(mesa[j]))
                        {
                            mesa[i] = 0;
                            mesa[j] = 0;
                            puntaje = int.Parse(tCA);
                            puntaje = puntaje + 3;
                            txtCartonGrupoActual.Text = puntaje.ToString();
                            MessageBox.Show("Carton Grupo Actual, +3", "Aviso ");
                            return true;
                        }
                    }
                }
            }
            if (buscarcarta(cartavalor))
            {
                puntaje = int.Parse(tCC);
                puntaje = puntaje + 3;
                txtCartonGrupoContrario.Text = puntaje.ToString();
                MessageBox.Show("Carton Grupo Contrario, +2", "Aviso Error");
                return true;
            }
            ingresarcartaenelmaso(cartaactual);
            return false;
        }
        public bool buscarcarta(int carta)
        {
            for (int i = 0; i < mesa.Count; i++)
            {
                if (carta == RetornarValor(mesa[i]))
                {
                    mesa[i] = 0;
                    return true;
                }

            }
            return false;
        }
        public int numerocarta(int carta)
        {
            int u = 0;
            for (int i = 0; i < 5; i++)
            {
                if (carta == RetornarValor(mano[i]))
                {
                    mano[i] = 0;
                    u++;
                }

            }
            return u;
        }
        public void Limpia(TextBox txtPuntosGrupoActual)
        {
            String tPGA = txtPuntosGrupoActual.Text;
            bool ad = true;
            int puntaje;
            for (int i = 0; i < mesa.Count; i++)
            {
                if (mesa[i] != 0)
                {
                    ad = false;
                    break;
                }
            }

            if (ad == true)
            {
                puntaje = int.Parse(tPGA);
                if (puntaje < 38)
                {
                    puntaje = puntaje + 2;
                    txtPuntosGrupoActual.Text = puntaje.ToString();
                    MessageBox.Show("Limpia Grupo Actual, +2", "Aviso ");
                }

            }
        }
        private int RetornarValor(int cartaI)
        {
            if (cartaI == 1 || cartaI == 11 || cartaI == 21 || cartaI == 31)
                return 1;
            if (cartaI == 2 || cartaI == 12 || cartaI == 22 || cartaI == 32)
                return 2;
            if (cartaI == 3 || cartaI == 13 || cartaI == 23 || cartaI == 33)
                return 3;
            if (cartaI == 4 || cartaI == 14 || cartaI == 24 || cartaI == 34)
                return 4;
            if (cartaI == 5 || cartaI == 15 || cartaI == 25 || cartaI == 35)
                return 5;
            if (cartaI == 6 || cartaI == 16 || cartaI == 26 || cartaI == 36)
                return 6;
            if (cartaI == 7 || cartaI == 17 || cartaI == 27 || cartaI == 37)
                return 7;
            if (cartaI == 8 || cartaI == 18 || cartaI == 28 || cartaI == 38)
                return 8;
            if (cartaI == 9 || cartaI == 19 || cartaI == 29 || cartaI == 39)
                return 9;
            if (cartaI == 10 || cartaI == 20 || cartaI == 30 || cartaI == 40)
                return 10;

            return 0;
        }
        private void ingresarcartaenelmaso(int cartaactual)
        {
            bool m = false;
            for (int i = 0; i < mesa.Count; i++)
            {
                if (mesa[i] == 0)
                {
                    //mesa.Insert(i, cartaactual);
                    mesa[i] = cartaactual;
                    m = true;
                    break;
                }
            }
            if (m == false)
            {
                mesa.Add(cartaactual);
            }
        }

    }
}
