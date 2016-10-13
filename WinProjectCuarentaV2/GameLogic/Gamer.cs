using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
    class Gamer
    {
        //String[] barajeada = { "1", "", "", "", "", "" };
        String[] barajeada;
        int turno, contCartas;
        bool stateGamers; //true - 4 gamers , false - 2 gamers

        public Gamer(int turno, bool stateGamers)
        {
            this.turno = turno;
            contCartas = 0;
            this.stateGamers = stateGamers;
            if (this.stateGamers) //true - 4 gamers , false - 2 gamers
            {
                String [] auxBarajeada = { "1", "", "", "", "", "" };
                barajeada = auxBarajeada;
            }
            else
            {
                String[] auxBarajeada = { "1", "", "", "" };
                barajeada = auxBarajeada;
            }
        }

        public void barajear()
        {
            String[] retorno;
            String[] aux;
            String baraja;
            int cont = 0;
            if (stateGamers) //true - 4 gamers , false - 2 gamers
            {
                retorno = new String[6];
                if (barajeada[0].Equals("1")) // Condicion 1, Barajear y repartir
                {
                    retorno[0] = "2"; // la siguiente jugadada no tendra que barajear
                    baraja = BarajearCuarenta();
                    aux = Regex.Split(baraja, ",");
                    for (int i = 1; i < 5; i++)
                    {
                        retorno[i] = retorno[i] + aux[cont]; cont++;
                        for (int j = 0; j < 4; j++)
                        {
                            retorno[i] = retorno[i] + "," + aux[cont];
                            cont++;
                        }
                    }
                    retorno[5] = retorno[5] + aux[20];
                    for (int i = 21; i < 40; i++)
                    {
                        retorno[5] = retorno[5] + "," + aux[i];
                    }
                }
                else // Condicion 2, Solo repartir
                {
                    baraja = barajeada[5];
                    retorno[0] = "1"; // la siguiente jugada tendra que barajear
                    retorno[5] = "";
                    aux = Regex.Split(baraja, ",");
                    for (int i = 1; i < 5; i++)
                    {
                        retorno[i] = retorno[i] + aux[cont]; cont++;
                        for (int j = 0; j < 4; j++)
                        {
                            retorno[i] = retorno[i] + "," + aux[cont];
                            cont++;
                        }
                    }
                }

            }
            else
            {
                retorno = new String[4];
                if (barajeada[0].Equals("1")) // Condicion 1, Barajear y repartir
                {
                    contCartas = 0;
                    retorno[0] = "2"; // el siguiente es el numero 2
                    baraja = BarajearCuarenta();    // obtengo las cuarenta cartas
                    aux = Regex.Split(baraja, ","); // creo un arreglo de 40 cartas separand a cada una por comas

                    for (int i = 1; i < 3; i++) // para ubicar las cartas en cada una de las manos
                    {
                        retorno[i] = retorno[i] + aux[cont]; cont++; // proceso para ubicar las 5 cartas en cada mano
                        for (int j = 0; j < 4; j++)
                        {
                            retorno[i] = retorno[i] + "," + aux[cont];
                            cont++;
                        }
                    }
                    contCartas += 10;
                    retorno[3] = retorno[3] + aux[contCartas];
                    for (int i = contCartas + 1; i < 40; i++)                    {
                        retorno[3] = retorno[3] + "," + aux[i];
                    }
                }
                else // Condicion 2, Solo repartir
                {
                    baraja = barajeada[3];
                    int auxNum = Int32.Parse(barajeada[0]) + 1;
                    retorno[0] = auxNum.ToString(); // la siguiente jugada tendra que barajear solo si ya esta en el numero 4
                    aux = Regex.Split(baraja, ",");
                    for (int i = 1; i < 3; i++)
                    {
                        retorno[i] = retorno[i] + aux[cont]; cont++;
                        for (int j = 0; j < 4; j++)
                        {
                            retorno[i] = retorno[i] + "," + aux[cont];
                            cont++;
                        }
                    }
                    if (contCartas == 30)
                        retorno[0] = "1";
                    else
                    {
                        retorno[3] = retorno[3] + aux[10];
                        for (int i = 11; i < 40 - contCartas; i++){
                            retorno[3] = retorno[3] + "," + aux[i];
                        }
                        contCartas += 10;
                    }
                }
            }

            barajeada = retorno;
        }

        private String BarajearCuarenta()
        {
            String barajaCuarenta;
            int[] arr = new int[40];
            int cont = 0, iB, jB, auxB;
            Random r = new Random();
            for (int i = 0; i < 40; i++) // Ubicando las cartas 
            {
                arr[i] = i + 1;
            }


            while (cont < 40) // Aplicando burbuja para desordenar
            {
                iB = r.Next(1, 40);
                jB = r.Next(1, 40);
                auxB = arr[iB];
                arr[iB] = arr[jB];
                arr[jB] = auxB;
                cont++;
            }

            barajaCuarenta = ConcatenarArrNumero(arr);

            return barajaCuarenta;
        }


        private String ConcatenarArrNumero(int[] arreglo)
        {
            String cadena = "";

            cadena = cadena + arreglo[0].ToString();
            for (int i = 1; i < arreglo.Length; i++)
            {
                cadena = cadena + "," + arreglo[i].ToString();
            }
            return cadena;
        }
        public String ConcatenaraTodo()
        {
            String res;
            if (stateGamers) //true - 4 gamers , false - 2 gamers
            {
                res = barajeada[1] + "@" + barajeada[2] + "@" + barajeada[3] + "@" + barajeada[4] + "@" + barajeada[5];
            }
            else
            {
                res = barajeada[1] + "@" + barajeada[2] + "@" + barajeada[3];  //mano1 @ mano2 @ manosRestantes
            }
            return res;
        }
    }
}
