using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
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
    /// Interaction logic for SoundTest.xaml
    /// </summary>
    public partial class SoundTest : Window
    {
        private SoundPlayer simpleSound = new SoundPlayer();

        public SoundTest()
        {
            InitializeComponent();
        }
        // INTERVAL
        private const int Min_Interval = 2000;
        private const int Max_Interval = 5000;
        // FLAG
        private bool SoundTestGoOn = false;
        // COUNTERS
        private int measureNumber = 0;
        // RANDOM
        private Random rand = new Random();
        //WATCH
        private Stopwatch measuredTime = new Stopwatch();
        //LISTS
        private List<int> soundTestResults = new List<int>();
        private List<TestResults> ResultsList = new List<TestResults>();

        Datavalidation_service check = new Datavalidation_service();
        DispatcherTimer test_timer = new DispatcherTimer();

        private void Sound(string path)
        {
            simpleSound.Stop();
            string rootLocation = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            string FullPath = rootLocation + @"\" + path;
            simpleSound.SoundLocation = FullPath;
            simpleSound.Play();
        }

        //TIMER
        private void test_timer_Tick(object sender, EventArgs e)
        {
            if (!measuredTime.IsRunning)
            {
                measuredTime.Reset();
                if (SoundTestGoOn)
                {
                    if (_50hz_rb.IsChecked == true)
                    {
                        Sound(@"sound\\50Hz.wav");
                    }
                    else if (_1000Hz_rb.IsChecked == true)
                    {
                        Sound(@"sound\\1000Hz.wav");
                    }
                    else if (_2000Hz_rb.IsChecked == true)
                    {
                        Sound(@"sound\\2000Hz.wav");
                    }
                    else if (_5000Hz_rb.IsChecked == true)
                    {
                        Sound(@"sound\\5000Hz.wav");
                    }
                    else if (_12500Hz_rb.IsChecked == true)
                    {
                        Sound(@"sound\\12500Hz.wav");
                    }
                    else if (_15000Hz_rb.IsChecked == true)
                    {
                        Sound(@"sound\\15000Hz.wav");
                    }
                }
                measuredTime.Start();
            }
        }
        private void Start_btn_Click(object sender, RoutedEventArgs e)
        {
            test_timer.Tick += test_timer_Tick;
            SoundTestGoOn = !SoundTestGoOn;
            string rootLocation = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            string FullPathStart = rootLocation + @"\sound\\Start.png";
            string FullPathStop = rootLocation + @"\sound\\Stop.png";
            if (SoundTestGoOn)
            {
                if (!_50hz_rb.IsChecked == true & !_1000Hz_rb.IsChecked == true & !_2000Hz_rb.IsChecked == true & !_5000Hz_rb.IsChecked == true & !_12500Hz_rb.IsChecked == true & !_15000Hz_rb.IsChecked == true)
                {
                    MessageBox.Show("Musisz wybrać rodza testu!", "Informacja", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {

                    ImageBrush imgBrush = new ImageBrush();
                    imgBrush.ImageSource = new BitmapImage(new Uri(FullPathStop, UriKind.Relative));
                    Start_btn.Background = imgBrush;
                    Sound_btn.IsEnabled = true;
                    test_timer.Interval = TimeSpan.FromMilliseconds(rand.Next(Max_Interval - Min_Interval) + Min_Interval);
                    test_timer.IsEnabled = true;
                    Clear_btn.IsEnabled = false;
                    Save_btn.IsEnabled = false;
                    Trialtest_cb.IsEnabled = false;
                    _50hz_rb.IsEnabled = _1000Hz_rb.IsEnabled = _2000Hz_rb.IsEnabled = _5000Hz_rb.IsEnabled = _12500Hz_rb.IsEnabled = _15000Hz_rb.IsEnabled = false;
                }
            }
            else
            {
                ImageBrush imgBrush_2 = new ImageBrush();
                imgBrush_2.ImageSource = new BitmapImage(new Uri(FullPathStart, UriKind.Relative));
                Start_btn.Background = imgBrush_2;
                Sound_btn.IsEnabled = false;
                test_timer.IsEnabled = false;
                Clear_btn.IsEnabled = true;
                Save_btn.IsEnabled = true;
                Trialtest_cb.IsEnabled = true;
                _50hz_rb.IsEnabled = _1000Hz_rb.IsEnabled = _2000Hz_rb.IsEnabled = _5000Hz_rb.IsEnabled = _12500Hz_rb.IsEnabled = _15000Hz_rb.IsEnabled = true;

            }
        }

        private void Sound_btn_Click(object sender, RoutedEventArgs e)
        {
            if (measuredTime.IsRunning)
            {
                measuredTime.Stop();
                if (!Trialtest_cb.IsChecked == true)
                {
                    int reactionTime = (int)measuredTime.ElapsedMilliseconds;
                    measureNumber++;
                    test_timer.Interval = TimeSpan.FromMilliseconds(rand.Next(Max_Interval - Min_Interval) + Max_Interval);
                    Results_tb.Text += (reactionTime.ToString() + " ms.\r\n");
                    soundTestResults.Add(reactionTime);
                    simpleSound.Stop();
                    Average_lb.Content = "Średnia:" + Math.Round(Calculations.Average(soundTestResults), 2).ToString() + "ms";
                    Variance_lb.Content = "Wariancja: " + Math.Round(Calculations.Variance(soundTestResults), 2).ToString();
                    Standarddeviation_lb.Content = "Odchylenie standardowe: " + Math.Round(Calculations.Standard_deviation(soundTestResults), 2).ToString();
                    if (_50hz_rb.IsChecked == true)
                    {
                        ResultsList.Add(new TestResults(reactionTime, "Wykonywany rodzaj testu: Dzwiękowy 50 Hz"));
                    }
                    else if (_1000Hz_rb.IsChecked == true)
                    {
                        ResultsList.Add(new TestResults(reactionTime, "Wykonywany rodzaj testu: Dzwiękowy 1000 Hz"));
                    }
                    else if (_2000Hz_rb.IsChecked == true)
                    {
                        ResultsList.Add(new TestResults(reactionTime, "Wykonywany rodzaj testu: Dzwiękowy 2000 Hz"));
                    }
                    else if (_5000Hz_rb.IsChecked == true)
                    {
                        ResultsList.Add(new TestResults(reactionTime, "Wykonywany rodzaj testu: Dzwiękowy 5000 Hz"));

                    }
                    else if (_12500Hz_rb.IsChecked == true)
                    {
                        ResultsList.Add(new TestResults(reactionTime, "Wykonywany rodzaj testu: Dzwiękowy 12500 Hz"));
                    }
                    else if (_15000Hz_rb.IsChecked == true)
                    {
                        ResultsList.Add(new TestResults(reactionTime, "Wykonywany rodzaj testu: Dzwiękowy 15000 Hz"));
                    }
                }
            }
            else
            {
                MessageBox.Show("Kliknąłeś zbyt szybko!", "Informacja", MessageBoxButton.OK, MessageBoxImage.Warning);


                test_timer.Tick += test_timer_Tick;
                SoundTestGoOn = !SoundTestGoOn;
                string rootLocation = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
                string FullPathStart = rootLocation + @"\sound\\Start.png";
                if (!SoundTestGoOn)
                {
                    ImageBrush imgBrush_2 = new ImageBrush();
                    imgBrush_2.ImageSource = new BitmapImage(new Uri(FullPathStart, UriKind.Relative));
                    Start_btn.Background = imgBrush_2;
                    Sound_btn.IsEnabled = false;
                    test_timer.IsEnabled = false;
                    Clear_btn.IsEnabled = true;
                    Save_btn.IsEnabled = true;
                    Trialtest_cb.IsEnabled = true;
                    _50hz_rb.IsEnabled = _1000Hz_rb.IsEnabled = _2000Hz_rb.IsEnabled = _5000Hz_rb.IsEnabled = _12500Hz_rb.IsEnabled = _15000Hz_rb.IsEnabled = true;

                }

            }
        }

        private void Clear_btn_Click(object sender, RoutedEventArgs e)
        {

            if (MessageBox.Show("Czy na pewno chcesz wyczyścić wszystkie pomiary!", "Ostrzeżenie", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                measureNumber = 0;
                measuredTime.Stop();
                soundTestResults = new List<int>();
                ResultsList = new List<TestResults>();
                Results_tb.Text = "";
                Average_lb.Content = "Średnia:";
                Variance_lb.Content = "Wariancja:";
                Standarddeviation_lb.Content = "Odchylenie standardowe:";
                test_timer.IsEnabled = false;
            }
            else
            {

            }


        }

        private void Back_btn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Czy na pewno chcesz wrócić do menu głównego? \n To skasuje wszystkie pomiary!", "Ostrzeżenie", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                SoundTestGoOn = false;
                Start_btn.Content = "Start";
                Sound_btn.IsEnabled = false;
                measureNumber = 0;
                measuredTime.Stop();
                soundTestResults = new List<int>();
                ResultsList = new List<TestResults>();
                test_timer.IsEnabled = false;
                Results_tb.Text = "";
                Name_tb.Text = "";
                Male_cb.IsChecked = false;
                Female_cb.IsChecked = false;
                Average_lb.Content = "Średnia:";
                Variance_lb.Content = "Wariancja:";
                Standarddeviation_lb.Content = "Odchylenie standardowe:";
                MainWindow main = new MainWindow();
                main.Show();
                this.Close();
            }
            else
            {

            }

        }

        private void Info_btn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Twoim zadaniem podczas testu jest wybór częstotliwości testu a nastepnie jak najszybsze naciśniecię przycisku słyszę dzwięk po jego usłyszeniu. Test możesz wykonać w formie testu próbnego bez zbierania danych po zaznaczeniu pola testu próbnego, aby rozpocząć test kliknij przycisk start. Przycisk Wyzeruj pomiary usuwa wszystkie zebrane podczas testu pomiary.", "Instrukcja", MessageBoxButton.OK, MessageBoxImage.Information);
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
                MessageBox.Show("Muszisz uzupelnić indetyfikator!", "Informacja", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                if ((!Male_cb.IsChecked == true) && (!Female_cb.IsChecked == true))
                {
                    MessageBox.Show("Muszisz wybrać płeć!", "Informacja", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                        sw.WriteLine("Średnia z wszystkich uzyskanych czasów: " + Math.Round(Calculations.Average(soundTestResults), 2).ToString() + " ms.");
                        sw.WriteLine("Wariancja z wszystkich uzyskanych czasów: " + Math.Round(Calculations.Variance(soundTestResults), 2).ToString());
                        sw.WriteLine("Odchylenie standardowe z wszystkich uzyskanych czasów: " + Math.Round(Calculations.Standard_deviation(soundTestResults), 2).ToString());
                        sw.WriteLine("");
                        sw.WriteLine("POSZCZEGÓLNE WYNKI");
                        sw.WriteLine("");

                        bool _50hzExist = ResultsList.Exists(oElement => oElement.Type.Equals("Wykonywany rodzaj testu: Dzwiękowy 50 Hz"));
                        bool _1000hzExist = ResultsList.Exists(oElement => oElement.Type.Equals("Wykonywany rodzaj testu: Dzwiękowy 1000 Hz"));
                        bool _2000hzExist = ResultsList.Exists(oElement => oElement.Type.Equals("Wykonywany rodzaj testu: Dzwiękowy 2000 Hz"));
                        bool _5000hzExist = ResultsList.Exists(oElement => oElement.Type.Equals("Wykonywany rodzaj testu: Dzwiękowy 5000 Hz"));
                        bool _12500hzExist = ResultsList.Exists(oElement => oElement.Type.Equals("Wykonywany rodzaj testu: Dzwiękowy 12500 Hz"));
                        bool _15000hzExist = ResultsList.Exists(oElement => oElement.Type.Equals("Wykonywany rodzaj testu: Dzwiękowy 15000 Hz"));

                        if (_50hzExist == true)
                        {
                            List<TestResults> oFoundGrenBlueList = ResultsList.FindAll(oElement => oElement.Type.Equals("Wykonywany rodzaj testu: Dzwiękowy 50 Hz"));
                            sw.WriteLine("Średnia z uzyskanych czasów dla testu 50 Hz: " + Math.Round(Calculations.AverageFound(oFoundGrenBlueList), 2).ToString() + " ms.");
                            sw.WriteLine("Wariancja z uzyskanych czasów dla testu 50 Hz: " + Math.Round(Calculations.VarianceFound(oFoundGrenBlueList), 2).ToString());
                            sw.WriteLine("Odchylenie standardowe z uzyskanych czasów dla testu 50 Hz: " + Math.Round(Calculations.Standard_deviationFound(oFoundGrenBlueList), 2).ToString());
                            sw.WriteLine("");
                        }
                        if (_1000hzExist == true)
                        {
                            List<TestResults> oFoundWhiteBlackeList = ResultsList.FindAll(oElement => oElement.Type.Equals("Wykonywany rodzaj testu: Dzwiękowy 1000 Hz"));
                            sw.WriteLine("Średnia z uzyskanych czasów dla testu 1000 Hz: " + Math.Round(Calculations.AverageFound(oFoundWhiteBlackeList), 2).ToString() + " ms.");
                            sw.WriteLine("Wariancja z uzyskanych czasów dla testu 1000 Hz: " + Math.Round(Calculations.VarianceFound(oFoundWhiteBlackeList), 2).ToString());
                            sw.WriteLine("Odchylenie standardowe z uzyskanych czasów dla testu 1000 Hz: " + Math.Round(Calculations.Standard_deviationFound(oFoundWhiteBlackeList), 2).ToString());
                            sw.WriteLine("");
                        }

                        if (_2000hzExist == true)
                        {
                            List<TestResults> oFoundRedYellowList = ResultsList.FindAll(oElement => oElement.Type.Equals("Wykonywany rodzaj testu: Dzwiękowy 2000 Hz"));
                            sw.WriteLine("Średnia z uzyskanych czasów dla testu 2000 Hz: " + Math.Round(Calculations.AverageFound(oFoundRedYellowList), 2).ToString() + " ms.");
                            sw.WriteLine("Wariancja z uzyskanych czasów dla testu 2000 Hz: " + Math.Round(Calculations.VarianceFound(oFoundRedYellowList), 2).ToString());
                            sw.WriteLine("Odchylenie standardowe z uzyskanych czasów dla testu 2000 Hz: " + Math.Round(Calculations.Standard_deviationFound(oFoundRedYellowList), 2).ToString());
                            sw.WriteLine("");
                        }
                        if (_5000hzExist == true)
                        {
                            List<TestResults> oFoundRedBlackList = ResultsList.FindAll(oElement => oElement.Type.Equals("Wykonywany rodzaj testu: Dzwiękowy 5000 Hz"));
                            sw.WriteLine("Średnia z uzyskanych czasów dla testu 5000 Hz: " + Math.Round(Calculations.AverageFound(oFoundRedBlackList), 2).ToString() + " ms.");
                            sw.WriteLine("Wariancja z uzyskanych czasów dla testu 5000 Hz: " + Math.Round(Calculations.VarianceFound(oFoundRedBlackList), 2).ToString());
                            sw.WriteLine("Odchylenie standardowe z uzyskanych czasów dla testu 5000 Hz: " + Math.Round(Calculations.Standard_deviationFound(oFoundRedBlackList), 2).ToString());
                            sw.WriteLine("");
                        }
                        if (_12500hzExist == true)
                        {
                            List<TestResults> oFoundRedGreenList = ResultsList.FindAll(oElement => oElement.Type.Equals("Wykonywany rodzaj testu: Dzwiękowy 12500 Hz"));
                            sw.WriteLine("Średnia z uzyskanych czasów dla testu 12500 Hz: " + Math.Round(Calculations.AverageFound(oFoundRedGreenList), 2).ToString() + " ms.");
                            sw.WriteLine("Wariancja z uzyskanych czasów dla testu 12500 Hz: " + Math.Round(Calculations.VarianceFound(oFoundRedGreenList), 2).ToString());
                            sw.WriteLine("Odchylenie standardowe z uzyskanych czasów dla testu 12500 Hz: " + Math.Round(Calculations.Standard_deviationFound(oFoundRedGreenList), 2).ToString());
                            sw.WriteLine("");
                        }
                        if (_15000hzExist == true)
                        {
                            List<TestResults> oFoundRedGreenList = ResultsList.FindAll(oElement => oElement.Type.Equals("Wykonywany rodzaj testu: Dzwiękowy 15000 Hz"));
                            sw.WriteLine("Średnia z uzyskanych czasów dla testu 15000 Hz: " + Math.Round(Calculations.AverageFound(oFoundRedGreenList), 2).ToString() + " ms.");
                            sw.WriteLine("Wariancja z uzyskanych czasów dla testu 15000 Hz: " + Math.Round(Calculations.VarianceFound(oFoundRedGreenList), 2).ToString());
                            sw.WriteLine("Odchylenie standardowe z uzyskanych czasów dla testu 15000 Hz: " + Math.Round(Calculations.Standard_deviationFound(oFoundRedGreenList), 2).ToString());
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

