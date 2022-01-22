using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using System.IO;
using System.Diagnostics;

namespace SnakeGame
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Snake franckie = new Snake();
        Manzana manzana = new Manzana();
        List<Snake> cuerpo = new List<Snake>();
        int i = 1;
        public MainWindow()
        {
            InitializeComponent();

            start();
            int caRow;
            int caColumn;
            int cbRow;
            int cbColumn;
            int ccRow;
            int ccColumn;

            DispatcherTimer miDispacher = new DispatcherTimer();
            miDispacher.Interval = new TimeSpan(0, 0, 0, 0, 100);
            miDispacher.Tick += (a, b) =>
            {
                caRow = Grid.GetRow(franckie.Serpiente);
                caColumn = Grid.GetColumn(franckie.Serpiente);
                movimiento(franckie, manzana);
                cbRow = Grid.GetRow(cuerpo[0].Serpiente);
                cbColumn = Grid.GetColumn(cuerpo[0].Serpiente);
                Grid.SetRow(cuerpo[0].Serpiente, caRow);
                Grid.SetColumn(cuerpo[0].Serpiente, caColumn);
                if (Grid.GetRow(franckie.Serpiente) == Grid.GetRow(manzana.CuerpoManzana) && Grid.GetColumn(franckie.Serpiente) == Grid.GetColumn(manzana.CuerpoManzana))
                {
                    manzana.newManzana();
                    for (int cosa = 0; cosa < cuerpo.Count; cosa++)
                    {
                        if (manzana.Y == Grid.GetColumn(cuerpo[cosa].Serpiente) && manzana.X == Grid.GetRow(cuerpo[cosa].Serpiente))
                        {
                            manzana.newManzana();
                        }
                    }
                    Grid.SetRow(manzana.CuerpoManzana, manzana.X);
                    Grid.SetColumn(manzana.CuerpoManzana, manzana.Y);
                    Score.Content = (int.Parse(Score.Content.ToString()) + 1).ToString();
                    cuerpo.Add(new Snake());
                    Output.Children.Add(cuerpo[i].Serpiente);
                    cuerpo[i].Serpiente.Fill = new SolidColorBrush(Colors.Yellow);
                    Grid.SetRow(cuerpo[i].Serpiente, Grid.GetRow(cuerpo[(i - 1)].Serpiente));
                    Grid.SetColumn(cuerpo[i].Serpiente, Grid.GetColumn(cuerpo[(i - 1)].Serpiente));
                    i++;
                }
                for(int bloques = 1; bloques < cuerpo.Count; bloques++)
                {
                    ccRow = Grid.GetRow(cuerpo[bloques].Serpiente);
                    ccColumn = Grid.GetColumn(cuerpo[bloques].Serpiente);
                    Grid.SetRow(cuerpo[bloques].Serpiente, cbRow);
                    Grid.SetColumn(cuerpo[bloques].Serpiente, cbColumn);
                    cbRow = ccRow;
                    cbColumn = ccColumn;
                    if (Grid.GetRow(franckie.Serpiente) == Grid.GetRow(cuerpo[bloques].Serpiente) && Grid.GetColumn(franckie.Serpiente) == Grid.GetColumn(cuerpo[bloques].Serpiente))
                    {
                        fail();
                    }
                }
            };
            miDispacher.Start();
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down && franckie.Direccion != "arriba")
            {
                franckie.Direccion = "abajo";
            }
            else if(e.Key == Key.Up && franckie.Direccion != "abajo")
            {
                franckie.Direccion = "arriba";
            }
            else if(e.Key == Key.Left && franckie.Direccion != "derecha")
            {
                franckie.Direccion = "izquierda";
            }
            else if( e.Key == Key.Right && franckie.Direccion != "izquierda")
            {
                franckie.Direccion = "derecha";
            }
        }
        private void start()
        {
            Score.Content = 0;
            //SCORE1_PLAYER.Content = 0;
            Output.Children.Add(franckie.Serpiente);
            manzana.newManzana();
            Output.Children.Add(manzana.CuerpoManzana);
            Grid.SetRow(franckie.Serpiente, 9);
            Grid.SetColumn(franckie.Serpiente, 9);
            cuerpo.Add(new Snake());
            Output.Children.Add(cuerpo[0].Serpiente);
            cuerpo[0].Serpiente.Fill = new SolidColorBrush(Colors.Yellow);
            Grid.SetRow(manzana.CuerpoManzana, manzana.X);
            Grid.SetColumn(manzana.CuerpoManzana, manzana.Y);
            franckie.Direccion = "abajo";
        }
        private void fail()
        {
            if (MessageBox.Show("¿deseas continuar?", "PERDISTE", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
            {
                manzana.newManzana();
                Output.Children.Clear();
                cuerpo.Clear();
                i = 1;
                start();
            }
            else
            {
                Application.Current.Shutdown();
            }
        }
        private void movimiento(Snake serpiente, Manzana manzana)
        {
            if (serpiente.Direccion == "abajo")
            {
                Grid.SetRow(serpiente.Serpiente, (Grid.GetRow(serpiente.Serpiente)) + 1);
                if (Grid.GetRow(serpiente.Serpiente) == 20)
                {
                    fail();
                }
            }
            else if (serpiente.Direccion == "arriba")
            {
                try
                {
                    Grid.SetRow(serpiente.Serpiente, (Grid.GetRow(serpiente.Serpiente)) - 1);
                }
                catch (ArgumentException)
                {
                    fail();
                }
            }
            else if (serpiente.Direccion == "derecha")
            {
                Grid.SetColumn(serpiente.Serpiente, (Grid.GetColumn(serpiente.Serpiente)) + 1);
                if (Grid.GetColumn(serpiente.Serpiente) == 20)
                {
                    fail();
                }
            }
            else if (serpiente.Direccion == "izquierda")
            {
                try
                {
                    Grid.SetColumn(serpiente.Serpiente, (Grid.GetColumn(serpiente.Serpiente)) - 1);
                }
                catch (ArgumentException)
                {
                    fail();
                }
            }
        }
    }
    public class Snake
    {
        public Snake()
        {
            serpiente.Width = 40;
            serpiente.Height = 40;
            serpiente.Fill = new SolidColorBrush(Colors.DarkGray);
            serpiente.HorizontalAlignment = HorizontalAlignment.Center;
            serpiente.VerticalAlignment = VerticalAlignment.Center;
            Serpiente.StrokeThickness = 4;
            Serpiente.Stroke = Brushes.Black;
            Largo = 1;
        }
        public Rectangle Serpiente
        {
            get { return serpiente; }
            set { serpiente = value; }
        }
        public int Largo { get; set; }
        public string Direccion { get; set; }
        private Rectangle serpiente = new Rectangle();
    }

    public class Manzana
    {
        public Manzana()
        {
            manzana.Width = 40;
            manzana.Height = 40;
            manzana.Fill = new SolidColorBrush(Colors.Red);
            manzana.HorizontalAlignment = HorizontalAlignment.Center;
            manzana.VerticalAlignment = VerticalAlignment.Center;
        }
        public void newManzana()
        {
            var guid = Guid.NewGuid();
            var justNumbers = new String(guid.ToString().Where(Char.IsDigit).ToArray());
            var seed = int.Parse(justNumbers.Substring(0, 9));
            Random random = new Random();
            X = random.Next(0, 20);
            Y = random.Next(0, 20);
        }
        public Rectangle CuerpoManzana
        {
            get { return manzana; }
            set { }
        }
        public int X { get; set; }
        public int Y { get; set; } 
        private Rectangle manzana = new Rectangle();
    }
}