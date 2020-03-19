
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using IniParser;
using IniParser.Model;

namespace Timer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int V = 5000;
        long TopStartTime;
        long JugStartTime;
        long MidStartTime;
        long BotStartTime;
        long SupStartTime;
        long GameStartTime;
        bool editflag=false;
        IniData data;
        Window1 flowWindow = new Window1();
        private Hocy_Hook hook_Main = new Hocy_Hook();
        DispatcherTimer timer;
        FileIniDataParser parser = new FileIniDataParser();

        System.EventHandler delegateinstance ;
        public MainWindow()
        {
            InitializeComponent();
            delegateinstance = new System.EventHandler(WindowEditFun);
            data = parser.ReadFile("conf.ini");
            hook_Main.OnKeyDown += new System.Windows.Forms.KeyEventHandler(hook_MainKeyDown);
            //获取当前活动进程的模块名称
            string moduleName = Process.GetCurrentProcess().MainModule.ModuleName;
            //返回指定路径字符串的文件名
            string processName = System.IO.Path.GetFileNameWithoutExtension(moduleName);
            //根据文件名创建进程资源数组
            Process[] processes = Process.GetProcessesByName(processName);
            //如果该数组长度大于1，说明多次运行
            if (processes.Length > 1)
            {
                MessageBox.Show("不允许多开", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();//关闭当前窗体
                return;
            }
            GameButton_Click(new object(), new RoutedEventArgs());
            flowWindow.Top = double.Parse(data["FlowWindow"]["FlowWindowTop"]);
            flowWindow.Left = double.Parse(data["FlowWindow"]["FlowWindowLeft"]);
            flowWindow.Show();
            try
            {
                hook_Main.InstallHook("1");

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + " error code:1", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }
        private void hook_MainKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            string a = e.KeyData.ToString().ToLower();
            if (a == data["key"]["Top"].ToLower())
            {
                this.TopButton_Click(null, null);
            }
            else if (a == data["key"]["Jug"].ToLower())
            {
                this.JugButton_Click(null, null);
            }
            else if (a == data["key"]["Mid"].ToLower())
            {
                this.MidButton_Click(null, null);
            }
            else if (a == data["key"]["Bot"].ToLower())
            {
                this.BotButton_Click(null, null);
            }
            else if (a == data["key"]["Sup"].ToLower())
            {
                this.SupButton_Click(null, null);
            }
            else if (a == data["key"]["Switch"].ToLower())
            {
                this.Switch(null, null);
            }
            else if (a == data["key"]["Decimal"].ToLower())
            {
                GameButton_Click(null, null);
            }
            else if (a == data["key"]["Add"].ToLower())
            {
                GameAdd_Click(null, null);
            }
            else if (a == data["key"]["Subtract"].ToLower())
            {
                GameSubtract_Click(null, null);
            }
        }
        private void Switch(object sender, EventArgs e)
        {
            if (flowWindow.IsVisible)
            {
                WindowEdit.Visibility = Visibility.Hidden;
                flowWindow.Hide();
            }
            else
            {
                if (!flowWindow.IsLoaded)
                {
                    flowWindow = new Window1() {  Top = double.Parse(data["FlowWindow"]["FlowWindowTop"]), Left = double.Parse(data["FlowWindow"]["FlowWindowLeft"]) };
                }

                WindowEdit.Visibility = Visibility.Visible;
                flowWindow.Show();
            }
        }
        private string changeTimeContent(long StartTime, bool IsChecked)
        {
            long timespan = ((StartTime - GameStartTime) / 1000) + (IsChecked ? 270 : 300);
            string content = (timespan / 60).ToString().PadLeft(2, '0') + ":" + (timespan % 60).ToString().PadLeft(2, '0');
            return content;
        }


        /*上单模板 开始*/
        private void TopButton_Click(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            TopStartTime = Environment.TickCount;
            timer.Tick += Toptimer_Tick;
            timer.IsEnabled = true;
        }

        private void Toptimer_Tick(object sender, EventArgs e)
        {
            long time = (((bool)TopBoot.IsChecked ? 270 : 300) - (Environment.TickCount - TopStartTime) / 1000);
            string content = changeTimeContent(TopStartTime, (bool)TopBoot.IsChecked);
            if (time <= 0)
            {
                Top.Content = "就绪";
            }
            else
            {
                Top.Content = time + "秒（" + content + "）";
            }
            flowWindow.TopTime.Content = Top.Content;
        }


        private void TopAdd_Click(object sender, RoutedEventArgs e)
        {
            TopStartTime += V;
        }

        private void TopSubtract_Click(object sender, RoutedEventArgs e)
        {
            TopStartTime -= V;
        }

        private void TopClear_Click(object sender, RoutedEventArgs e)
        {
            TopStartTime -= 1000000;
        }

        /*上单模板 结束*/
        private void JugButton_Click(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            JugStartTime = Environment.TickCount;
            timer.Tick += Jugtimer_Tick;
            timer.IsEnabled = true;
        }

        private void Jugtimer_Tick(object sender, EventArgs e)
        {
            long time = (((bool)JugBoot.IsChecked ? 270 : 300) - (Environment.TickCount - JugStartTime) / 1000);
            string content = changeTimeContent(JugStartTime, (bool)JugBoot.IsChecked);
            if (time <= 0)
            {
                Jug.Content = "就绪";
            }
            else
            {
                Jug.Content = time + "秒（" + content + "）";
            }
            flowWindow.JugTime.Content = Jug.Content;
        }

        private void JugAdd_Click(object sender, RoutedEventArgs e)
        {
            JugStartTime += V;
        }

        private void JugSubtract_Click(object sender, RoutedEventArgs e)
        {
            JugStartTime -= V;
        }

        private void JugClear_Click(object sender, RoutedEventArgs e)
        {
            JugStartTime -= 1000000;
        }


        private void MidButton_Click(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            MidStartTime = Environment.TickCount;
            timer.Tick += Midtimer_Tick;
            timer.IsEnabled = true;
        }

        private void Midtimer_Tick(object sender, EventArgs e)
        {
            long time = (((bool)MidBoot.IsChecked ? 270 : 300) - (Environment.TickCount - MidStartTime) / 1000);
            string content = changeTimeContent(MidStartTime, (bool)MidBoot.IsChecked);
            if (time <= 0)
            {
                Mid.Content = "就绪";
            }
            else
            {
                Mid.Content = time + "秒（" + content + "）";
            }
            flowWindow.MidTime.Content = Mid.Content;
        }

        private void MidAdd_Click(object sender, RoutedEventArgs e)
        {
            MidStartTime += V;
        }

        private void MidSubtract_Click(object sender, RoutedEventArgs e)
        {
            MidStartTime -= V;
        }

        private void MidClear_Click(object sender, RoutedEventArgs e)
        {
            MidStartTime -= 1000000;
        }


        private void BotButton_Click(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            BotStartTime = Environment.TickCount;
            timer.Tick += Bottimer_Tick;
            timer.IsEnabled = true;
        }

        private void Bottimer_Tick(object sender, EventArgs e)
        {
            long time = (((bool)BotBoot.IsChecked ? 270 : 300) - (Environment.TickCount - BotStartTime) / 1000);
            string content = changeTimeContent(BotStartTime, (bool)BotBoot.IsChecked);
            if (time <= 0)
            {
                Bot.Content = "就绪";
            }
            else
            {
                Bot.Content = time + "秒（" + content + "）";
            }
            flowWindow.BotTime.Content = Bot.Content;
        }

        private void BotAdd_Click(object sender, RoutedEventArgs e)
        {
            BotStartTime += V;
        }

        private void BotSubtract_Click(object sender, RoutedEventArgs e)
        {
            BotStartTime -= V;
        }

        private void BotClear_Click(object sender, RoutedEventArgs e)
        {
            BotStartTime -= 1000000;
        }

        private void SupButton_Click(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            SupStartTime = Environment.TickCount;
            timer.Tick += Suptimer_Tick;
            timer.IsEnabled = true;
        }

        private void Suptimer_Tick(object sender, EventArgs e)
        {
            long time = ((bool)SupBoot.IsChecked ? 270 : 300) - (Environment.TickCount - SupStartTime) / 1000;
            string content = changeTimeContent(SupStartTime, (bool)SupBoot.IsChecked);
            if (time <= 0)
            {
                Sup.Content = "就绪";
            }
            else
            {
                Sup.Content = time + "秒（" + content + "）";
            }
            flowWindow.SupTime.Content = Sup.Content;
        }

        private void SupAdd_Click(object sender, RoutedEventArgs e)
        {
            SupStartTime += V;
        }

        private void SupSubtract_Click(object sender, RoutedEventArgs e)
        {
            SupStartTime -= V;
        }

        private void SupClear_Click(object sender, RoutedEventArgs e)
        {
            SupStartTime -= 1000000;
        }

        private void GameButton_Click(object sender, RoutedEventArgs e)
        {
            TopStartTime = 0;
            JugStartTime = 0;
            MidStartTime = 0;
            BotStartTime = 0;
            SupStartTime = 0;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            GameStartTime = Environment.TickCount;
            timer.Tick += Gametimer_Tick;
            timer.IsEnabled = true;
        }

        private void Gametimer_Tick(object sender, EventArgs e)
        {
            long time = ((Environment.TickCount - GameStartTime) / 1000);
            flowWindow.Game.Content = Game.Content = (time / 60).ToString() + ":" + (time % 60).ToString().PadLeft(2, '0');
        }

        private void GameAdd_Click(object sender, RoutedEventArgs e)
        {
            GameStartTime -= V;
        }

        private void GameSubtract_Click(object sender, RoutedEventArgs e)
        {
            GameStartTime += V;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                flowWindow.Close();
                if (timer != null) { timer.Stop(); }

                this.hook_Main.UnInstallHook();
                //获取当前活动进程的模块名称
                string moduleName = Process.GetCurrentProcess().MainModule.ModuleName;
                //返回指定路径字符串的文件名
                string processName = System.IO.Path.GetFileNameWithoutExtension(moduleName);
                //根据文件名创建进程资源数组
                Process[] processes = Process.GetProcessesByName(processName);
                foreach (var process in processes)
                {
                    process.Close();
                }
            }
            catch (Exception e2)
            {
                MessageBox.Show(e2.Message, "error code:2");
            }
        }

        private void WindowEdit_Click(object sender, RoutedEventArgs e)
        {
            editflag = true;
            WindowEditFun(null, null);
            editflag = false;
        }
        private void WindowEditFun(object sender, EventArgs e)
        {
            const double leftOffset = 8;
            const double topOffset = 31;
            if (flowWindow.AllowsTransparency && editflag && flowWindow.IsLoaded)//编辑模式
                {
                    flowWindow.Close();
                    flowWindow = new Window1() { AllowsTransparency = false, WindowStyle = WindowStyle.SingleBorderWindow, Top = double.Parse(data["FlowWindow"]["FlowWindowTop"]) - topOffset, Left = double.Parse(data["FlowWindow"]["FlowWindowLeft"]) - leftOffset };
                    flowWindow.Closed += delegateinstance;
                    flowWindow.Show();
            }
            else if (!flowWindow.AllowsTransparency)
            {
                data["FlowWindow"]["FlowWindowTop"] = (flowWindow.Top + topOffset).ToString();
                data["FlowWindow"]["FlowWindowLeft"] = (flowWindow.Left + leftOffset).ToString();
                parser.WriteFile("conf.ini", data);
                flowWindow.Closed -= delegateinstance;
                flowWindow.Close();
                flowWindow = new Window1() { AllowsTransparency = true, WindowStyle = WindowStyle.None, Top = double.Parse(data["FlowWindow"]["FlowWindowTop"]), Left = double.Parse(data["FlowWindow"]["FlowWindowLeft"]) };
                flowWindow.Show();
            }
        }
    }

}
