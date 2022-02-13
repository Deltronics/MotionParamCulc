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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;

namespace MotionParamCulc
{
    
    
    

    /* В данном проекте осуществляется расчёт параметров движения сервоприводов при 
     * управлении от контроллера движения таких как скорость и ускорение в единицах пользователя.
     * Т.е. можно ввести требуемую скорость в об/мин, а она будет пересчитана в скорость в 
     * единицах пользоваля, которая уже используется в инструция движения контроллера. 
     * Тоже самое касается ускорения, которая пересчитывается из мс до макс скорости в об/мин в
     * ед. пользователя в сек. в квадрате.      
     */

    

        class Text
    {
        public string MyText
        {
            get { return string.Format(" Задание позиции в\n в выбранных единицах расстояния"); }

                    }
    }     
    public partial class MainWindow : Window   
    {
        readonly BitmapImage userIcon = new BitmapImage();
        
                public double DistanseForOneTurn;         
        public double MaxSpeed;         
        public uint MinAccDec;         
        public double PosSV;         
        public double SpeedSV;         
        public uint AccSV;         
        public uint DecSV;         
        public uint Numerator=1;         
        public uint Denominator=1;         
        public uint PulsesMotorOrginal;         
        public uint PulseMotorScalled;         
        public uint PosSVpls;         
        public int SpeedSVpps;        
        public double RevolutionsBeforeOverflow;         
        public uint PLCoutputMaxFreq = 200000;         
        public const int PulseMaxValue = 2147483647; 
                static public string MyText2
        {
            get { return string.Format(" Задание скорости в оборотах\n в минуту. Формат 0000,0"); }            
        } 
                static public string MyText3
        {
            get { return string.Format(" Задание участка ускорения до\n выхода на максимальную скорость" +
                " \n Единицы: миллисекунды"); }
        } 
                static public string MyText4
        {
            get
            {
                return string.Format(" Задание участка замедления\n до полного останова (V = 0)" +
              " \n Единицы: миллисекунды");
            }
        } 
        double WindowHeight;         double WindowWidth;          

        public MainWindow()         {
            InitializeComponent();

                       CultureInfo.CurrentCulture = new CultureInfo("ru-Ru", false);

           userIcon.BeginInit();
            userIcon.UriSource = new Uri("pack://application:,,,/AX-308E_ICO.ico", UriKind.RelativeOrAbsolute);
            userIcon.EndInit();
            Icon = userIcon;
            textBox1.Focus();
            
                                    Text t = new Text();
            textBlock3.Text = t.MyText;

                                    textBlock4.Text = MyText2;

                                    textBlock13.Text = MyText3;

                                    textBlock15.Text = MyText4;

                                    textBlock22.Text = Page1;
            textBlock19.Text = Header1;
            textBlock26.Text = Header2;
            textBlock27.Text = Page1_1;
            textBlock28.Text = Page1_2;

                                    textBlock23.Text = Page2;
            textBlock29.Text = Page2_1;
            textBlock30.Text = Page2_2;
            textBlock31.Text = Page2_3;
            textBlock32.Text = Page2_4;
            textBlock33.Text = Page2_5;

                                    textBlock34.Text = Page3;

                                    textBlock35.Text = Page4;
            textBlock36.Text = Page4_1;

                                    textBlock37.Text = Page5;
            textBlock38.Text = Page5_1;
            textBlock39.Text = Header5_1;
            textBlock40.Text = Page5_2;

                                    textBlock41.Text = Page6;

                                    textBlock42.Text = Page7;
            textBlock43.Text = Header7_1;
            textBlock44.Text = Page7_1;

                                    WindowHeight = Height;
            WindowWidth = Width;

            

        } 
                        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
             Height = WindowHeight;
             Width = WindowWidth;

        } 
                private void radioButton1_Checked(object sender, RoutedEventArgs e)
        {
            if (radioButton1.IsChecked == true)
            {
                                if (radioButton2 != null) radioButton2.IsChecked = false;
                if (radioButton3 != null) radioButton3.IsChecked = false;
                if (radioButton4!= null)  radioButton4.IsChecked = false;
                if (textBlock9 != null) textBlock9.Text = "мкм";
                if (textBlock10 != null) textBlock10.Text = "мкм";
            }
        }

                private void radioButton2_Checked(object sender, RoutedEventArgs e)
        {
            if (radioButton2.IsChecked == true)
            {
                radioButton1.IsChecked = false;
                radioButton3.IsChecked = false;
                radioButton4.IsChecked = false;
                textBlock9.Text = "мм";
                textBlock10.Text = "мм";
            }
        }

                private void radioButton3_Checked(object sender, RoutedEventArgs e)
        {
            if (radioButton3.IsChecked == true)
            {
                radioButton2.IsChecked = false;
                radioButton1.IsChecked = false;
                radioButton4.IsChecked = false;
                textBlock9.Text = "см";
                textBlock10.Text = "см";
            }
        }

                private void radioButton4_Checked(object sender, RoutedEventArgs e)
        {
            if (radioButton4.IsChecked == true)
            {
                radioButton2.IsChecked = false;
                radioButton1.IsChecked = false;
                radioButton3.IsChecked = false;
                textBlock9.Text = "м";
                textBlock10.Text = "м";
            }
        }


                        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
                        string specifier;
            CultureInfo culture;
            specifier = "G";
            culture = CultureInfo.DefaultThreadCurrentCulture;
                        if (!double.TryParse(textBox1.Text, out DistanseForOneTurn))
            {
                textBox1.Text = "0";
                return;
            }

            if (DistanseForOneTurn < 0)
            {
                textBox1.Text = Math.Abs(DistanseForOneTurn).ToString(specifier, culture); ;
            }
        }

                private void textBox2_TextChanged(object sender, TextChangedEventArgs e)
        {
                        string specifier;
            CultureInfo culture;
            specifier = "G";
            culture = CultureInfo.DefaultThreadCurrentCulture;
                        if (!double.TryParse(textBox2.Text, out PosSV))
            {
                textBox2.Text = "0";
                return;
            }

            if (PosSV < 0)
            {
                textBox2.Text = Math.Abs(PosSV).ToString(specifier, culture); ;
            }
        }

                private void textBox3_TextChanged(object sender, TextChangedEventArgs e)
        {
                        string specifier;
            CultureInfo culture;
            specifier = "G";
            culture = CultureInfo.DefaultThreadCurrentCulture;

                        if (!double.TryParse(textBox3.Text, out SpeedSV))
            {
                textBox3.Text = "0";
                return;
            }
            if (SpeedSV < 0)
            {
                textBox3.Text = Math.Abs(SpeedSV).ToString(specifier, culture); ;
            }

        }

                private void textBox4_TextChanged(object sender, TextChangedEventArgs e)
        {
                        if (!uint.TryParse(textBox4.Text, out AccSV))
            {
                textBox4.Text = "0";
                return;
            }

        }

                private void textBox5_TextChanged(object sender, TextChangedEventArgs e)
        {
                        if (!uint.TryParse(textBox5.Text, out DecSV))
            {
                textBox5.Text = "0";
                return;
            }
        }

                private void textBox6_TextChanged(object sender, TextChangedEventArgs e)
        {
            string specifier;
            CultureInfo culture;
            specifier = "G";
            culture = CultureInfo.DefaultThreadCurrentCulture;
                        if (!double.TryParse(textBox6.Text, out MaxSpeed))
            {
                textBox6.Text = "0";
                return;
            }

            if (MaxSpeed < 0)
            {
                textBox6.Text = Math.Abs(MaxSpeed).ToString(specifier, culture); ;
            }
        }

                private void textBox7_TextChanged(object sender, TextChangedEventArgs e)
        {
                        if (!uint.TryParse(textBox7.Text, out MinAccDec))
            {
                textBox7.Text = "0";
                return;
            }
        }

                private void textBox35_TextChanged(object sender, TextChangedEventArgs e)
        {
                        if (!uint.TryParse(textBox35.Text, out Numerator))
            {
                textBox35.Text = "1";
                return;
            }
            if (Numerator == 0) Numerator = 1;

        } 
                private void textBox36_TextChanged(object sender, TextChangedEventArgs e)
        {
                        if (!uint.TryParse(textBox36.Text, out Denominator))
            {
                textBox36.Text = "1";
                return;
            }
            if (Denominator == 0) Denominator = 1;

        } 
                private void textBox37_TextChanged(object sender, TextChangedEventArgs e)
        {
                        if (!uint.TryParse(textBox37.Text, out PulsesMotorOrginal))
            {
                textBox37.Text = "0";
                return;
            }
        } 

                private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tabItem1.Focus();
            textBox1.Focus();
                    } 

                                                private void tabItem2_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
                        string specifier;
            CultureInfo culture;
            specifier = "G";
            culture = CultureInfo.DefaultThreadCurrentCulture;                      

                        if (!double.TryParse(textBox1.Text, out DistanseForOneTurn))
            {
                textBox1.Text = "0";                
                return;
            }

            if (DistanseForOneTurn == 0)
            {
                MessageBox.Show("Введите расстояние, проходящее конечным механизмом \n" +
                    "на один оборот мотора серводвигателя.", "Неправильные данные",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                textBox1.Focus();
                return;
            }

                        if (!double.TryParse(textBox6.Text, out MaxSpeed))
            {
                textBox6.Text = "0";
                return;
            }

            if ( MaxSpeed <= 0 )
            {
                MessageBox.Show("Введите максимально допустимую скорость \n" +
                    "вращения двигателя в об/мин. \nЧисло должно быть больше нуля.", 
                    "Неправильные данные",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                textBox6.Focus();
                return;
            }
                        double MaxSpeed_PUU = MaxSpeed / 60 * DistanseForOneTurn;
                        textBox24.Text = MaxSpeed_PUU.ToString(specifier, culture);

            
                        if (!uint.TryParse(textBox7.Text, out MinAccDec))
                {
                    textBox7.Text = "0";                 return;
                }
            if (MinAccDec == 0)
            {
                MinAccDec=1;                
            }
                        double MinAccDec_PUU = MaxSpeed_PUU / ((double)MinAccDec / 1000);
                        textBox25.Text = MinAccDec_PUU.ToString(specifier, culture);



                        if (!double.TryParse(textBox2.Text, out PosSV))
            {
                textBox2.Text = "0";                 return;
            }            
                        textBox22.Text = PosSV.ToString(specifier, culture);
            textBox30.Text = textBox22.Text;

                        if (!double.TryParse(textBox3.Text, out SpeedSV))
            {
                textBox3.Text = "0";                 return;
            }
            if (Math.Abs(SpeedSV) > MaxSpeed)
            {
                MessageBox.Show("Задание скорости не может быть больше \n" +
                    "максимально допустимой скорости.",
                   "Неправильные данные", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                textBox3.Focus();
                return;
            }
                        double SpeedSV_PUU = SpeedSV / 60 * DistanseForOneTurn;
                          textBox23.Text = SpeedSV_PUU.ToString(specifier, culture);
             textBox31.Text = textBox23.Text;

                        if (!uint.TryParse(textBox4.Text, out AccSV))
            {
                textBox4.Text = "0";                 return;
            }
            if (AccSV == 0)
            {
                AccSV = 1;
            }
            if (AccSV < MinAccDec)
            {
                AccSV = MinAccDec;
            }
                        double AccPU_PUU = MaxSpeed_PUU / ((double)AccSV / 1000);
            
                        textBox26.Text = AccPU_PUU.ToString(specifier, culture);
            textBox32.Text = textBox26.Text;

                        if (!uint.TryParse(textBox5.Text, out DecSV))
            {
                textBox5.Text = "0";                 return;
            }
            if (DecSV == 0)
            {
                DecSV = 1;
            }
            if (DecSV < MinAccDec)
            {
                DecSV = MinAccDec;
            }
                        double DecPU_PUU = MaxSpeed_PUU / ((double)DecSV / 1000);
                        textBox27.Text = DecPU_PUU.ToString(specifier, culture);
            textBox33.Text = textBox27.Text;

                        textBox28.Text = AccPU_PUU.ToString(specifier, culture);
            textBox34.Text = textBox28.Text;

                                    double RevolutionsDemand = PosSV / DistanseForOneTurn;
                        textBox29.Text = RevolutionsDemand.ToString(specifier, culture);
            textBox42.Text = RevolutionsDemand.ToString(specifier, culture); ;

                                    tabItem2.Focus();

        } 

                private void tabItem3_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
                        string specifier;
            CultureInfo culture;
            specifier = "G";
            culture = CultureInfo.DefaultThreadCurrentCulture;

                        if (!double.TryParse(textBox1.Text, out DistanseForOneTurn))
            {
                textBox1.Text = "0";
                return;
            }
            if (DistanseForOneTurn == 0)
            {
                MessageBox.Show("Введите расстояние, проходящее конечным механизмом \n" +
                    "на один оборот мотора серводвигателя.", "Неправильные данные",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                textBox1.Focus();
                return;
            }
                                    double RevolutionsDemand = PosSV / DistanseForOneTurn;
                        textBox42.Text = RevolutionsDemand.ToString(specifier, culture); ;

                                    if (!uint.TryParse(textBox35.Text, out Numerator))
            {
                textBox35.Text = "1"; 
                return;
            }
            if (Numerator == 0) Numerator = 1;

                        if (!uint.TryParse(textBox36.Text, out Denominator))
            {
                textBox36.Text = "1"; 
                return;
            }
            if (Denominator == 0) Denominator = 1;

                        if (!uint.TryParse(textBox37.Text, out PulsesMotorOrginal))
            {
                textBox37.Text = "0"; 
                return;
            }

                        PulseMotorScalled = PulsesMotorOrginal / Numerator * Denominator;
                        textBox38.Text = PulseMotorScalled.ToString(specifier, culture);

                        PosSVpls = (uint)(PulseMotorScalled * RevolutionsDemand);
            if (PosSVpls > PulseMaxValue)
            {
                                                
                textBox39.Text = "############";
                textBox44.Text = "############";
                PosSVpls = 0;
            }
            else
                        {
                textBox39.Text = PosSVpls.ToString(specifier, culture);
                textBox44.Text = PosSVpls.ToString(specifier, culture);
            }

                                    if (!uint.TryParse(textBox43.Text, out PLCoutputMaxFreq))
            {
                textBox43.Text = "0";                 
            }
                        if (PLCoutputMaxFreq!=0)
                 SpeedSVpps = (int)(SpeedSV/60 * PulseMotorScalled);
            if (SpeedSVpps <= PLCoutputMaxFreq)
            {
                                textBox40.Text = SpeedSVpps.ToString(specifier, culture);
                textBox45.Text = SpeedSVpps.ToString(specifier, culture);
            }
            else
            {
                                                
                textBox40.Text = "############";
                textBox45.Text = "############";
                SpeedSVpps = 0;
            }

                        if (PulseMotorScalled!=0)
            RevolutionsBeforeOverflow = PulseMaxValue / PulseMotorScalled;
                        textBox41.Text = RevolutionsBeforeOverflow.ToString(specifier, culture);

        } 

                        private void tabItem3_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                tabItem3_PreviewMouseLeftButtonDown(null, null);
            }
        }


                        private void MenuItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            textBox1.Text = "10";             textBox6.Text = "3000";             textBox7.Text = "10";             textBox2.Text = "453,78";             textBox3.Text = "650";             textBox4.Text = "100";             textBox5.Text = "50";             radioButton2.IsChecked = true;             textBox35.Text = "128";             textBox36.Text = "1";             textBox37.Text = "1280000";             textBox43.Text = "200000"; 
        } 

                        private void MenuItem_PreviewMouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            textBox1.Text = "0";             textBox6.Text = "0";             textBox7.Text = "0";             textBox2.Text = "0";             textBox3.Text = "0";             textBox4.Text = "0";             textBox5.Text = "0";             radioButton1.IsChecked = true;
            textBox35.Text = "1";             textBox36.Text = "1";             textBox37.Text = "0";             textBox43.Text = "0";         } 


                static public string Page1
        {
            get
            {
                return string.Format(
                    " Общепринятым подходом при вычислении параметров движения является использование" +
                    "\n традиционных единиц измерения расстояния, скорости и ускорения такие как: " +
                    "\n метры, дюймы, миллиметры, метры в секунду, метры в секунду в квадрате и т.д. " +
                    "\n При расчёте рампы перемещения на уровне сервопривода, т.е. участков " +
                    "\n разгона – движения с постоянной скоростью – замедления, " +
                    "\n используются такие единицы как, количество оборотов, обороты в минуту, " +
                    "\n а длительность участков разгона/замедления исчисляется в миллисекундах." +
                    "\n" +
                    "\n Сервомотор в свою очередь, ориентируется на свой встроенный энкодер, который" +
                    "\n имеет фиксированное значение количества импульсов на 1 оборот." +
                    "\n В итоге задание на перемещение и скорость сводится именно к данным единицам" +
                    "\n сервомотора – количество импульсов для перемещения и количество импульсов " +
                    "\n в секунду для скорости." +
                    "\n" +
                    "\n Таким образом, мы имеем с одной стороны единицы измерения расстояния и скорости " +
                    "\n удобные для человека, а с другой стороны единицы, которые использует в итоге " +
                    "\n сервопривод для вращения своего мотора. С точки зрения человека данные единицы " +
                    "\n являются не очень удобными (хотя необходимо отметить, что в ряде случаев именно " +
                    "\n такой подход будет предпочтительным)." +
                    "\n"
                    );
            }
        } 
                static public string Page1_1
        {
            get
            {
                return string.Format(
                    " В классических системах с импульсным управлением для того, чтобы побудить сервопривод " +
                    "\n к движению на заданное расстояние и указанной скоростью, необходимо было выдать " +
                    "\n определённое количество импульсов с конкретной частотой. Количество импульсов " +
                    "\n определяло расстояние, т.е. сколько оборотов сделает вал мотора, а частота определяла " +
                    "\n скорость, с которой будет вращаться вал мотора. " +
                    "\n" +
                    " В современных контроллерах управления движением применяется управление  " +
                    "\n сервоприводами по интерфейсу, например по EtherCAT или CAN BUS, в которых  уже " +
                    "\n задание передаётся не физическими импульсами, а информационным пакетом данных. " +
                    "\n"
                    );
            }
        } 
                static public string Page1_2
        {
            get
            {
                return string.Format(                    
                    " Но смысл остался прежним – мотор должен получить задание в своих единицах, " +
                    "\n т.е. в импульсах."                     
                    );
            }
        } 

                static public string Header1
        {
            get
            {
                return string.Format("\nЕдиницы расстояния, скорости и ускорения в различных системах" +
                    "\n");
            }
        } 
                static public string Header2
        {
            get
            {
                return string.Format("\nНемного о единицах перемещения пользователя (PUU) и что" +
                    "\n  нужно контроллеру движения для управления сервоприводом\n");
            }
        } 
                static public string Page2
        {
            get
            {
                return string.Format(
                    "\n Таким образом, привод на своём уровне не имеет возможности пересчитать единицы " +
                    "\n измерения человека в единицы мотора. Т.е. по интерфейсу контроллер должен отправить " +
                    "\n задание в единицах понятных мотору, а от человека принять задание в единицах, " +
                    "\n понятных человеку. Следовательно, возникла необходимость как-то взаимоувязать единицы " +
                    "\n измерения человека и мотора." +
                    "\n" +
                    "\n Так появилось понятие – " 
                    );
            }
        } 
                static public string Page2_1
        {
            get
            {
                return string.Format(
                    " единицы перемещения пользователя PUU – Position User Unit, "                    
                    );
            }
        } 
                static public string Page2_2
        {
            get
            {
                return string.Format(
                    " которые выражаются в тех единицах расстояния, которые нужны человеку, чтобы задавать" +
                    "\n требуемые перемещения (мкм, мм, см. и т.д.). Причём скорость и ускорение также измеряются" +
                    "\n исходя из этих единиц, т.е. PUU/сек и PUU/сек в квадрате, а не в оборотах в минуту," +
                    "\n метрах в секунду в квадрате и т.п. Именно в этих единицах пользователя и задаются аргументы" +
                    "\n во всех без исключения Функциональных Блоках управления движением в программе " +
                    "\n контроллера. Эти единицы являются промежуточными между традиционными для человека и " +
                    "\n импульсами для мотора." +
                    "\n" +
                    "\n Таким образом, нужна методика пересчёта единиц задания расстояния и скорости в единицы " +
                    "\n пользователя, которые потом уже поступают как аргументы для ФБ движения." +
                    "\n Основан пересчёт на хорошо известных формулах:" +
                    "\n" +
                    "\n скорость:  V = S / t  PUU/с,  ускорение:  a = V / t = (S/t) / t  PUU/с2"
                    );
            }
        } 
                static public string Page2_3
        {
            get
            {
                return string.Format(
                    " \n Ключевым аргументом является величина перемещения в единицах пользователя на один" +
                    "\n оборот вала мотора, которая в обязательном порядке вносится в настройки оси.  "
                    );
            }
        } 
                static public string Page2_4
        {
            get
            {
                return string.Format(
                    "\n Именно от неё рассчитывается уже скорость и ускорение в единицах пользователя, " +
                    "\n а также пересчитывается задание в единицы мотора (импульсы). Эта величина должна " +
                    "\n быть строго привязана к механическим параметрам установки и должна равняться " +
                    "\n расстоянию, проходящему конечным механизмом на один оборот вала сервомотора. " +
                    "\n Например, если у Вас мотор подключен к ШВП с шагом 20 мм на один оборот, то мы введём " +
                    "\n в параметры оси число 20, и единицы мм. Можно ввести и 20000 мкм, но это будет уже " +
                    "\n не очень удобно для дальнейших расчётов. Так как расстояние можно задавать с дробной " +
                    "\n частью, то лучше так - 20.000 мм. "
                    );
            }
        } 
                static public string Page2_5
        {
            get
            {
                return string.Format(
                    "\n Важно иметь ввиду, что пересчёт в импульсы осуществляется контроллером на основе" +
                    "\n указанного значения количества импульсов встроенного энкодера на 1 оборот вала" +
                    "\n двигателя. Данная величина также в обязательном порядке вносится в настройки оси."
                    );
            }
        } 
                static public string Page3
        {
            get
            {
                return string.Format(
                    "\n Рассмотрим расчёт параметров движения в единицах пользователя (PUU) на примере ШВП," +
                    "\n но для наглядности примем, что на один оборот проходимое расстояние конечным " +
                    "\n механизмом составит 10 мм. " +
                    "\n" + 
                    "\n В первую очередь вычислим максимальную скорость и минимально допустимое ускорение " +
                    "\n в единицах PUU, которые можно будет задать в ФБ движения. Эти параметры в обязательном" +
                    "\n порядке указываются в настройках оси движения в контроллере (в единицах PUU !!). " +
                    "\n" +
                    "\n Предположим, что максимальную скорость вращения мотора мы хотим иметь в 3000 об/мин. " +
                    "\n Тогда расчёт будет выглядеть следующим образом:" +
                    "\n" +
                    "\n\t 1. Приводим скорость к оборотам в секунду:  3000 : 60 = 50 об/сек" +
                    "\n\t 2. Переводим в единицы пользователя: 50 * 10 = 500 PUU / сек" +
                    "\n\t (в нашем случае это 500 мм / сек, так как у нас принято, что на один оборот мотора " +
                    "\n\t конечных механизм проходит 10 мм)." +
                    "\n" +
                    "\n Физически это означает, что привод имеет право вращать вал своего мотора так, " +
                    "\n чтобы в секунду конечный механизм (ШВП в нашем примере) проходил не более 500 PUU " +
                    "\n (мм в нашем примере)." +
                    "\n" +
                    "\n Минимально допустимое ускорение рассчитывается исходя из участка разгона двигателя, " +
                    "\n который указывается обычно в миллисекундах, и физически означает время, за которое " +
                    "\n двигатель разгонится до максимальной скорости (в нашем примере 3000 об/мин). " +
                    "\n Для расчётов интервал в миллисекундах необходимо перевести в ускорение, " +
                    "\n задаваемое в PUU/секунды в квадрате. " +
                    "\n" +
                    "\n Предположим, что мы установили минимальное время разгона в 10 мс. Тогда:" +
                    "\n" +
                    "\n\t 1. Переводим миллисекунды в секунды: 10 мс : 1000 = 0,01 сек" +
                    "\n\t 2. Делим максимальную скорость в PUU на время разгона в секундах:" +
                    "\n\t    500 : 0, 01 = 500 * 100 = 50 000 PUU / сек2" +
                    "\n" +
                    "\n Таким образом, в настройках оси мы должны задать в качестве максимально допустимой " +
                    "\n скорости 500 PUU/сек, а минимально допустимое ускорение как 50 000 PUU/сек2." +
                    "\n" +
                    "\n Непосредственно в ФБ движения аргументы передаются также исключительно в PUU."
                    );
            }
        } 
                static public string Page4
        {
            get
            {
                return string.Format(
                    "\n Например, если Вам необходимо осуществить перемещение вот с такими параметрами:" +
                    "\n S = 453, 78 мм, V = 650 об / мин, участки разгона / замедления по 100 мс каждый, " +
                    "\n то параметры для ФБ движения будут следующие:" +
                    "\n Position = 453,78  PUU (инструкция отработает задание в 453,78  мм)" +
                    "\n" +
                    "\n Velocity = 650 : 60 = 10, 833; 10,833 * 10 = 108,33  PUU/сек (у нас принято 10 мм/оборот)" +                   
                    "\n" +
                    "\n Acceleration = 100 мс: 1000 = 0,1 сек; 500 : 0,1 = 5000 PUU/сек2" +
                    "\n" +
                    "\n Замедление можно сделать равным ускорению: Deceleration = Acceleration = 5000 PUU/сек2" +                   
                    "\n" +
                    "\n Параметр JERK (третья производная от расстояния) обычно делают по значению равным " +
                    "\n ускорению, но единицы измерения PUU/сек3 :" +
                    "\n" +
                    "\n JERK = Acceleration = 5000 PUU/сек3 (определяет S - образность участков " +
                    "\n разгона / замедления)" +
                    "\n" +
                    "\n Таким образом, при значении ключевого параметра – перемещения конечного механизма" +
                    "\n на 1 оборот вала сервомотора равным 10 мм, ФБ движения будет выглядеть так:" +
                "\n"
                    );
            }
        } 
                static public string Page4_1
        {
            get
            {
                return string.Format(
                    "  (перемещение на 453,78 мм, на скорости 650 об/мин с разгоном и замедлением по 100 мс)"
                    );
            }
        } 
                static public string Page5
        {
            get
            {
                return string.Format(
                    "\n Ускорение можно рассчитать и по-другому. Сначала вычислить предельное теоретическое" +
                    "\n ускорение, т.е. за 1 мс:" +
                    "\n" +
                    "\n 500 : 0, 001 = 500 000 PUU/сек2" +
                    "\n" +
                    "\n а потом просто делить на требуемое время в миллисекундах:" +
                    "\n" +
                    "\n Разгон за 100 мс (до макс.скорости в 3000 об/мин): 500 000 : 100 = 5000 PUU/сек2" +
                    "\n" +
                    "\n Скорость также можно рассчитывать исходя из значения максимально допустимой скорости " +
                    "\n (в нашем случае 3000 об/мин):" +
                    "\n"
                    );
            }
        } 
                static public string Page5_1
        {
            get
            {
                return string.Format(
                    "\n Velocity = 650 * 500 / 3000 = 108.33 PUU/сек" + "\n" );
            }
        } 
                static public string Header5_1
        {
            get
            {
                return string.Format("\nЕдиницы мотора" + "\n");
            }
        } 
                static public string Page5_2
        {
            get
            {
                return string.Format(
                    "\n В современных сервоприводах, управляемых по интерфейсу, остались от их импульсных " +
                    "\n предшественников такие параметры как числитель и знаменатель. В сервоприводах Delta " +
                    "\n это параметры Р1-44 и Р1-45. Раньше данные параметры использовались, чтобы согласовать " +
                    "\n количество входных импульсов с количеством импульсов энкодера двигателя для " +
                    "\n осуществления одного оборота. Например, у сервопривода семейства ASD - A2  1 280 000 " +
                    "\n импульсов на оборот энкодера. Подать такое количество импульсов да ещё для достижения " +
                    "\n заданной скорости (частота импульсов) является крайне проблематичной задачей, поэтому " +
                    "\n можно сделать числитель = 128, а знаменатель 1. При таких настройках достаточно подать" +
                    "\n уже 10 000 импульсов, чтобы привод сделал 1 оборот. Т.е. внешнее задание умножается " +
                    "\n внутри сервопривода на числитель и делится на знаменатель. Это позволяло сделать нужное " +
                    "\n количество импульсов задания, чтобы мотор сделал 1 оборот." +
                    "\n");
            }
        } 
                static public string Page6
        {
            get
            {
                return string.Format(
                    " Так как при интерфейсном управлении данные поступают в цифровом виде, то в большинстве " +
                    "\n случаев нет необходимости использовать параметры числителя и знаменателя. Поэтому" +
                    "\n достаточно, чтобы они оба равнялись 1. А в параметрах оси нужно прямо задать количество " +
                    "\n импульсов энкодера на оборот вала двигателя как указанно в характеристиках привода." +
                    "\n Для ASD - A2 это 1 280 000, а для  ASD - B3 это 16 777 216  импульсов на оборот. Подобные" +
                    "\n настройки позволят избежать неожиданных трансформаций задания позиции и скорости. " + 
                    "\n" +
                    "\n Однако в ряде случаев всё - таки приходится выставлять числитель и знаменатель, чтобы" +
                    "\n понизить количество импульсов на оборот. Это актуально для механизмов, у которых ось" +
                    "\n может длительно вращаться в одну сторону, особенно через понижающий редуктор. В итоге" +
                    "\n произойдёт переполнение регистра текущей позиции в единицах пользователя (PUU Feedback)," +
                    "\n из которого контроллер движения и берёт текущую позицию привода для своей работы." +
                    "\n" +
                    "\n Переполнение регистра явление неприятное, так как, дойдя до максимального значения, " +
                    "\n позиция неожиданно из положительного значения перекидывается в такое же отрицательное" +
                    "\n и начинает инкрементироваться в сторону нуля. Так как регистр, где в приводе хранится" +
                    "\n текущая позиция, является целочисленным со знаком двойным словом, то легко вычислить" +
                    "\n количество оборотов, после которого произойдёт переполнение регистра. Например," +
                    "\n у привода ASD-B3-E мы имеем 16 777 216  импульсов на оборот, а предельная мощность" +
                    "\n регистра 2 147 483 648, то получаем: " +
                    "\n" +
                    "\n 2 147 483 648  :  16 777 216 = 128  оборотов вала сервомотора" +
                    "\n" +
                    "\n Как видно из примера выше, всего 128 оборотов до переполнения регистра положения. " +
                    "\n Если у Вас в механизме ось явно сделает большее количество оборотов, то необходимо " +
                    "\n выставить числитель и знаменатель, чтобы уменьшить требуемое количество импульсов " +
                    "\n на оборот. Напомним, что привод в итоге получает задание именно в импульсах." +
                    "\n" +
                    "\n Если мы изменим числитель и знаменатель, к примеру, на такие значения:" +
                    "\n" +
                    "\n Р1-44  -  16 777 216" +
                    "\n Р1-45  -      100 000" +
                    "\n" +
                    "\n тогда получим 100 000 импульсов на оборот. Т.е. при том же значении линейного" +
                    "\n расстояния в единицах человека на один оборот вала мотора, в нашем примере это 10 мм," +
                    "\n в первом случае привод получит задание 16 777 216  импульсов для перемещения механизма" +
                    "\n на 10 мм, а во втором случае всего 100 000 импульсов. Итого можно будет сделать:" +
                    "\n" +
                    "\n 2 147 483 648  :  100 000 = 21 474.836  оборотов вала сервомотора до переполнения"
                    );
            }
        } 
                static public string Page7
        {
            get
            {
                return string.Format(
                    "\n Но нужно понимать, что чем меньше импульсов на оборот мы ставим, тем меньше будет " +
                    "\n разрешающая способность привода, т.е. минимальный шаг отработки задания позиции." +
                    "\n" +
                    "\n Самым крайним случаем является так называемое «бесконечное движение», т.е. когда вал" +
                    "\n привода всегда вращается в одном направлении. В подобных ситуациях остаётся только " +
                    "\n периодически сбрасывать текущую позицию на ноль." +  "\n");
            }
        } 
                static public string Header7_1
        {
            get
            {
                return string.Format("\nВажное примечание" + "\n");
            }
        } 
                static public string Page7_1
        {
            get
            {
                return string.Format(
                    "\n Перед началом настройки привода очень рекомендуется сбросить его на заводские установки:" +
                    "\n Р2-08 = 10 и снять-подать питание. Режим работы должен быть Р1-01 = С (управление по" +
                    "\n EtherCAT),  Р1-44 и Р1-45 = 1, и для приводов ASD-A2 в параметр Р3-18 установить позицию" +
                    "\n А в 1 (0x00012000) и задать адрес приводу в Р3-00. " +
                    "\n" +
                    "\n После этого необходимо выполнить процедуру настройки привода через ASDASoft" +
                    "\n (соотношение моментов инерции, коэффициенты контуров обратной связи), задать сетевой " +
                    "\n адрес, а затем выставить при необходимости числитель и знаменатель (Р1-44 и Р1-45)." +
                    "\n Лучше это делать из программы контроллера при инициализации системы. Однако это можно" +
                    "\n выставить и напрямую в параметрах привода, но тогда необходимо параметр Р3-12" +
                    "\n выставить в 0х0100, чтобы числитель и знаменатель сохранялись при снятии питания с" +
                    "\n привода.");
            }
        } 
        
    } 

} 