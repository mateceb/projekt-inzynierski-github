using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Threading;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for VisualTest.xaml
    /// </summary>
    public partial class VisualTest : Window
    {
        public VisualTest()
        {
            InitializeComponent();
        }
        // INTERVAL
        private const int Min_Interval = 2000;
        private const int Max_Interval = 5000;
        // FLAG
        private bool VisualTestGoOn = false;
        // COUNTERS
        private int measureNumber = 0;
        // RANDOM
        private Random rand = new Random();
        // WATCH
        private Stopwatch measuredTime = new Stopwatch();
        //LISTS
        private List<int> visualTestResults = new List<int>();
        private List<TestResults> ResultsList = new List<TestResults>();

        Datavalidation_service check = new Datavalidation_service();
        DispatcherTimer test_timer = new DispatcherTimer();

        //TIMER
        private void test_timer_Tick(object sender, EventArgs e)
        {
            if (!measuredTime.IsRunning)
            {
                measuredTime.Reset();
                if (VisualTestGoOn)
                {
                    if (GreenBlue_rb.IsChecked == true)
                    {
                        Visual_btn.Background = Brushes.Green;
                    }
                    else if (WhiteBlack_rb.IsChecked == true)
                    {
                        Visual_btn.Background = Brushes.White;
                    }
                    else if (RedYellow_rb.IsChecked == true)
                    {
                        Visual_btn.Background = Brushes.Red;
                    }
                    else if (RedBlack_rb.IsChecked == true)
                    {
                        Visual_btn.Background = Brushes.Red;
                    }
                    else if (RedGreen_rb.IsChecked == true)
                    {
                        Visual_btn.Background = Brushes.Red;
                    }
                }
                measuredTime.Start();
            }
        }
        private void VisualFirstColor()
        {
            if (GreenBlue_rb.IsChecked == true)
            {
                Visual_btn.Background = Brushes.Blue;
            }
            else if (WhiteBlack_rb.IsChecked == true)
            {
                Visual_btn.Background = Brushes.Black;
            }
            else if (RedYellow_rb.IsChecked == true)
            {
                Visual_btn.Background = Brushes.Yellow;
            }
            else if (RedBlack_rb.IsChecked == true)
            {
                Visual_btn.Background = Brushes.Black;
            }
            else if (RedGreen_rb.IsChecked == true)
            {
                Visual_btn.Background = Brushes.Green;
            }
        }


        private void Start_btn_Click(object sender, RoutedEventArgs e)
        {
            test_timer.Tick += test_timer_Tick;
            VisualTestGoOn = !VisualTestGoOn;
            string rootLocation = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            string FullPathStart = rootLocation + @"\sound\\Start.png";
            string FullPathStop = rootLocation + @"\sound\\Stop.png";
            if (VisualTestGoOn)
            {
                if (!GreenBlue_rb.IsChecked == true & !WhiteBlack_rb.IsChecked == true & !RedYellow_rb.IsChecked == true & !RedBlack_rb.IsChecked == true & !RedGreen_rb.IsChecked == true)
                {
                    MessageBox.Show("Musisz wybrać rodzaj testu!", "Informacja", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    TrialTest_cb.IsEnabled = false;
                    ImageBrush imgBrush = new ImageBrush();
                    imgBrush.ImageSource = new BitmapImage(new Uri(FullPathStop, UriKind.Relative));
                    Start_btn.Background = imgBrush;
                    Visual_btn.IsEnabled = true;
                    test_timer.Interval = TimeSpan.FromMilliseconds(rand.Next(Max_Interval - Min_Interval) + Min_Interval);
                    test_timer.IsEnabled = true;
                    Clear_btn.IsEnabled = false;
                    Save_btn.IsEnabled = false;
                    GreenBlue_rb.IsEnabled = WhiteBlack_rb.IsEnabled = RedYellow_rb.IsEnabled = RedBlack_rb.IsEnabled = RedGreen_rb.IsEnabled = false;
                }
            }
            else
            {
                TrialTest_cb.IsEnabled = true;
                ImageBrush imgBrush_2 = new ImageBrush();
                imgBrush_2.ImageSource = new BitmapImage(new Uri(FullPathStart, UriKind.Relative));
                Start_btn.Background = imgBrush_2;
                Visual_btn.IsEnabled = false;
                test_timer.IsEnabled = false;
                Clear_btn.IsEnabled = true;
                Save_btn.IsEnabled = true;
                GreenBlue_rb.IsEnabled = WhiteBlack_rb.IsEnabled = RedYellow_rb.IsEnabled = RedBlack_rb.IsEnabled = RedGreen_rb.IsEnabled = true;
            }
        }

        private void Visual_btn_Click(object sender, RoutedEventArgs e)
        {
            if (measuredTime.IsRunning)
            {
                measuredTime.Stop();
                VisualFirstColor();
                if (!TrialTest_cb.IsChecked == true)
                {
                    int reactionTime = (int)measuredTime.ElapsedMilliseconds;
                    test_timer.Interval = TimeSpan.FromMilliseconds(rand.Next(Max_Interval - Min_Interval) + Min_Interval);
                    measureNumber++;
                    Results_tb.Text += (reactionTime.ToString() + " ms.\r\n");
                    visualTestResults.Add(reactionTime);

                    Averange_lb.Content = "Średnia: " + Math.Round(Calculations.Average(visualTestResults), 2).ToString() + " ms.";
                    Varianc_lb.Content = "Wariancja: " + Math.Round(Calculations.Variance(visualTestResults), 2).ToString();
                    Standarddeviation_lb.Content = "Odchylenie standardowe: " + Math.Round(Calculations.Standard_deviation(visualTestResults), 2).ToString();
                    if (GreenBlue_rb.IsChecked == true)
                    {
                        ResultsList.Add(new TestResults(reactionTime, "Wykonywany rodzaj testu: Wizualny zielono niebieski"));
                    }
                    else if (WhiteBlack_rb.IsChecked == true)
                    {
                        ResultsList.Add(new TestResults(reactionTime, "Wykonywany rodzaj testu: Wizualny czarno biały"));
                    }
                    else if (RedYellow_rb.IsChecked == true)
                    {
                        ResultsList.Add(new TestResults(reactionTime, "Wykonywany rodzaj testu: Wizualny czerwono żółty"));
                    }
                    else if (RedBlack_rb.IsChecked == true)
                    {
                        ResultsList.Add(new TestResults(reactionTime, "Wykonywany rodzaj testu: Wizualny czerwono czarny"));

                    }
                    else if (RedGreen_rb.IsChecked == true)
                    {
                        ResultsList.Add(new TestResults(reactionTime, "Wykonywany rodzaj testu: Wizualny czerwono zielony"));
                    }
                }
            }
            else
            {
                MessageBox.Show("Kliknąłeś zbyt szybko!", "Informacja", MessageBoxButton.OK, MessageBoxImage.Warning);
                test_timer.Tick += test_timer_Tick;
                VisualTestGoOn = !VisualTestGoOn;
                string rootLocation = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
                string FullPathStart = rootLocation + @"\sound\\Start.png";
                if (!VisualTestGoOn)
                {
                    TrialTest_cb.IsEnabled = true;
                    ImageBrush imgBrush_2 = new ImageBrush();
                    imgBrush_2.ImageSource = new BitmapImage(new Uri(FullPathStart, UriKind.Relative));
                    Start_btn.Background = imgBrush_2;
                    Visual_btn.IsEnabled = false;
                    test_timer.IsEnabled = false;
                    Clear_btn.IsEnabled = true;
                    Save_btn.IsEnabled = true;
                    GreenBlue_rb.IsEnabled = WhiteBlack_rb.IsEnabled = RedYellow_rb.IsEnabled = RedBlack_rb.IsEnabled = RedGreen_rb.IsEnabled = true;
                }
                if (measuredTime.IsRunning)
                {
                    measuredTime.Stop();
                    VisualFirstColor();
                }

            }
        }
        private void Clear_btn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Czy na pewno chcesz wyczyścić wszystkie pomiary!", "Ostrzeżenie", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                measureNumber = 0;
                measuredTime.Stop();
                visualTestResults = new List<int>();
                ResultsList = new List<TestResults>();
                Results_tb.Text = "";
                Averange_lb.Content = "Średnia:";
                Varianc_lb.Content = "Wariancja:";
                Standarddeviation_lb.Content = "Odchylenie standardowe:";
                test_timer.IsEnabled = false;
            }
            else
            {

            }

        }
        private void Back_btn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Czy na pewno chcesz wrócić do menu głównego?\n To skasuje wszystkie pomiary!", "Ostrzeżenie", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                VisualTestGoOn = false;
                Start_btn.Content = "Start";
                Visual_btn.IsEnabled = false;
                measureNumber = 0;
                measuredTime.Stop();
                visualTestResults = new List<int>();
                ResultsList = new List<TestResults>();
                Results_tb.Text = "";
                Averange_lb.Content = "Średnia:";
                Varianc_lb.Content = "Wariancja:";
                Standarddeviation_lb.Content = "Odchylenie standardowe:";
                Name_tb.Text = "";
                Male_cb.IsChecked = false;
                Female_cb.IsChecked = false;
                test_timer.IsEnabled = false;
                MainWindow main = new MainWindow();
                main.Show();
                this.Close();
            }
            else
            {

            }

        }

        #region radiobuttons
        private void GreenBlue_rb_Click(object sender, RoutedEventArgs e)
        {
            Visual_btn.Background = Brushes.Blue;
        }

        private void WhiteBlack_rb_Click(object sender, RoutedEventArgs e)
        {
            Visual_btn.Background = Brushes.Black;
        }

        private void RedYellow_rb_Click(object sender, RoutedEventArgs e)
        {
            Visual_btn.Background = Brushes.Yellow;
        }

        private void RedBlack_rb_Click(object sender, RoutedEventArgs e)
        {
            Visual_btn.Background = Brushes.Black;
        }

        private void RedGreen_rb_Click(object sender, RoutedEventArgs e)
        {
            Visual_btn.Background = Brushes.Green;
        }

        #endregion

        private void Info_btn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Twoim zadaniem podczas testu jest wybór koloru testu a nastepnie jak najszybsze naciśniecię prostokąta gdy zmieni on kolor. Test możesz wykonać w formie testu próbnego bez zbierania danych po zaznaczeniu pola testu próbnego, aby rozpocząć test kliknij przycisk start. Przycisk Wyzeruj pomiary usuwa wszystkie zebrane podczas testu pomiary. ", "Instrukcja", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Save_btn_Click(object sender, RoutedEventArgs e)
        {
            string name = Name_tb.Text;
            string rootLocation = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            string FullPath = rootLocation + @"\Pomiary\" + Name_tb.Text + ".txt";
            DateTime dt = DateTime.Now;
            string date = dt.ToString();
            if (name.Length == 0)
            {
                MessageBox.Show("Musisz uzupełnić indetyfikator!", "Informacja", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                if ((!Male_cb.IsChecked == true) && (!Female_cb.IsChecked == true))
                {
                    MessageBox.Show("Musisz wybrać płeć!", "Informacja", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    using (StreamWriter sw = new StreamWriter((FullPath), true))
                    {
                        sw.WriteLine("");
                        sw.WriteLine("Nazwa osoby badanej: " + Name_tb.Text);
                        if (Male_cb.IsChecked == true)
                        {
                            sw.WriteLine("Płeć osoby badanej: Mężczyzna");
                        }
                        else if (Female_cb.IsChecked == true)
                        {
                            sw.WriteLine("Płeć osoby badanej: Kobieta");
                        }
                        sw.WriteLine("Data i czas wykonania testu: " + date);
                        sw.WriteLine("Uzyskane czasy:");
                        foreach (TestResults oTest in ResultsList)
                        {
                            sw.WriteLine("Wynik testu: " + oTest.Results + " ms." + "  " + oTest.Type);
                        }

                        sw.WriteLine("");
                        sw.WriteLine("Średnia z wszystkich uzyskanych czasów: " + Math.Round(Calculations.Average(visualTestResults), 2).ToString() + " ms.");
                        sw.WriteLine("Wariancja z wszystkich uzyskanych czasów: " + Math.Round(Calculations.Variance(visualTestResults), 2).ToString());
                        sw.WriteLine("Odchylenie standardowe z wszystkich uzyskanych czasów: " + Math.Round(Calculations.Standard_deviation(visualTestResults), 2).ToString());
                        sw.WriteLine("");
                        sw.WriteLine("POSZCZEGÓLNE WYNKI");
                        sw.WriteLine("");

                        bool grenblueExist = ResultsList.Exists(oElement => oElement.Type.Equals("Wykonywany rodzaj testu: Wizualny zielono niebieski"));
                        bool whiteblackExist = ResultsList.Exists(oElement => oElement.Type.Equals("Wykonywany rodzaj testu: Wizualny czarno biały"));
                        bool redyellowExist = ResultsList.Exists(oElement => oElement.Type.Equals("Wykonywany rodzaj testu: Wizualny czerwono żółty"));
                        bool redblackExist = ResultsList.Exists(oElement => oElement.Type.Equals("Wykonywany rodzaj testu: Wizualny czerwono czarny"));
                        bool redgreenExist = ResultsList.Exists(oElement => oElement.Type.Equals("Wykonywany rodzaj testu: Wizualny czerwono zielony"));

                        if (grenblueExist == true)
                        {
                            List<TestResults> oFoundGrenBlueList = ResultsList.FindAll(oElement => oElement.Type.Equals("Wykonywany rodzaj testu: Wizualny zielono niebieski"));
                            sw.WriteLine("Średnia z uzyskanych czasów dla testu zielono niebieskiego: " + Math.Round(Calculations.AverageFound(oFoundGrenBlueList), 2).ToString() + " ms.");
                            sw.WriteLine("Wariancja z uzyskanych czasów dla testu zielono niebieskiego: " + Math.Round(Calculations.VarianceFound(oFoundGrenBlueList), 2).ToString());
                            sw.WriteLine("Odchylenie standardowe z uzyskanych czasów dla testu zielono niebieskiego: " + Math.Round(Calculations.Standard_deviationFound(oFoundGrenBlueList), 2).ToString());
                            sw.WriteLine("");
                        }
                        if (whiteblackExist == true)
                        {
                            List<TestResults> oFoundWhiteBlackeList = ResultsList.FindAll(oElement => oElement.Type.Equals("Wykonywany rodzaj testu: Wizualny czarno biały"));
                            sw.WriteLine("Średnia z uzyskanych czasów dla testu czarno białego: " + Math.Round(Calculations.AverageFound(oFoundWhiteBlackeList), 2).ToString() + " ms.");
                            sw.WriteLine("Wariancja z uzyskanych czasów dla testu czarno białego: " + Math.Round(Calculations.VarianceFound(oFoundWhiteBlackeList), 2).ToString());
                            sw.WriteLine("Odchylenie standardowe z uzyskanych czasów dla testu czarno białego: " + Math.Round(Calculations.Standard_deviationFound(oFoundWhiteBlackeList), 2).ToString());
                            sw.WriteLine("");
                        }

                        if (redyellowExist == true)
                        {
                            List<TestResults> oFoundRedYellowList = ResultsList.FindAll(oElement => oElement.Type.Equals("Wykonywany rodzaj testu: Wizualny czerwono żółty"));
                            sw.WriteLine("Średnia z uzyskanych czasów dla testu czerwono żółtego: " + Math.Round(Calculations.AverageFound(oFoundRedYellowList), 2).ToString() + " ms.");
                            sw.WriteLine("Wariancja z uzyskanych czasów dla testu czerwono żółtego: " + Math.Round(Calculations.VarianceFound(oFoundRedYellowList), 2).ToString());
                            sw.WriteLine("Odchylenie standardowe z uzyskanych czasów dla testu czerwono żółtego: " + Math.Round(Calculations.Standard_deviationFound(oFoundRedYellowList), 2).ToString());
                            sw.WriteLine("");
                        }
                        if (redblackExist == true)
                        {
                            List<TestResults> oFoundRedBlackList = ResultsList.FindAll(oElement => oElement.Type.Equals("Wykonywany rodzaj testu: Wizualny czerwono czarny"));
                            sw.WriteLine("Średnia z uzyskanych czasów dla testu czerwono czarnego: " + Math.Round(Calculations.AverageFound(oFoundRedBlackList), 2).ToString() + " ms.");
                            sw.WriteLine("Wariancja z uzyskanych czasów dla testu czerwono czarnego: " + Math.Round(Calculations.VarianceFound(oFoundRedBlackList), 2).ToString());
                            sw.WriteLine("Odchylenie standardowe z uzyskanych czasów dla testu czerwono czarnego: " + Math.Round(Calculations.Standard_deviationFound(oFoundRedBlackList), 2).ToString());
                            sw.WriteLine("");
                        }
                        if (redgreenExist == true)
                        {
                            List<TestResults> oFoundRedGreenList = ResultsList.FindAll(oElement => oElement.Type.Equals("Wykonywany rodzaj testu: Wizualny czerwono zielony"));
                            sw.WriteLine("Średnia z uzyskanych czasów dla testu czerwono zielonego: " + Math.Round(Calculations.AverageFound(oFoundRedGreenList), 2).ToString() + " ms.");
                            sw.WriteLine("Wariancja z uzyskanych czasów dla testu czerwono zielonego: " + Math.Round(Calculations.VarianceFound(oFoundRedGreenList), 2).ToString());
                            sw.WriteLine("Odchylenie standardowe z uzyskanych czasów dla testu czerwono zielonego: " + Math.Round(Calculations.Standard_deviationFound(oFoundRedGreenList), 2).ToString());
                        }
                    }
                    MessageBox.Show("Zapisano!", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void Male_cb_Checked(object sender, RoutedEventArgs e)
        {
            Female_cb.IsChecked = false;
        }
        private void Female_cb_Checked(object sender, RoutedEventArgs e)
        {
            Male_cb.IsChecked = false;
        }

        private void Name_tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool checkstate = check.Checking_name(Name_tb.Text);
            if (checkstate == true)
            {
                MessageBox.Show("Nieprawidłowy znak!\n W indetyfikatorze nie możesz używać znaków\"  /, \\, <,\", >, ? ,: ,*, |,\" ", "Ostrzeżenie", MessageBoxButton.OK, MessageBoxImage.Warning);
                Name_tb.Text = Name_tb.Text.Remove(Name_tb.Text.Length - 1);
            }
        }
    }
}


