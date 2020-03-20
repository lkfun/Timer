
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using IniParser;
using IniParser.Model;
using Timer;

namespace Timer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        long TopStartTime, JugStartTime, MidStartTime, BotStartTime, SupStartTime, GameStartTime;
        DispatcherTimer TopTimer, JugTimer, MidTimer, BotTimer, SupTimer, GameTimer;
        bool editflag = false;
        IniData data;
        private const int FiveSecond = 5000;
        Window1 flowWindow = new Window1();
        private Hocy_Hook hook_Main = new Hocy_Hook();
        FileIniDataParser parser = new FileIniDataParser();
        System.EventHandler delegateinstance;
        public MainWindow()
        {
            data = parser.ReadFile("conf.ini");
            InitializeComponent();
            Switch(null, null);
            delegateinstance = new System.EventHandler(WindowEditFun);
            hook_Main.OnKeyDown += new System.Windows.Forms.KeyEventHandler(Hook_MainKeyDown);
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
            try
            {
                bool flag = hook_Main.InstallHook("1");
                if (!flag)
                {
                    MessageBox.Show("热键注册失败 error code:3", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + " error code:1", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }
        private void Hook_MainKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            string key = e.KeyData.ToString().ToLower();
            if (key == data["key"]["Top"].ToLower())
            {
                TopButton_Click(null, null);
            }
            else if (key == data["key"]["Jug"].ToLower())
            {
                JugButton_Click(null, null);
            }
            else if (key == data["key"]["Mid"].ToLower())
            {
                MidButton_Click(null, null);
            }
            else if (key == data["key"]["Bot"].ToLower())
            {
                BotButton_Click(null, null);
            }
            else if (key == data["key"]["Sup"].ToLower())
            {
                SupButton_Click(null, null);
            }
            else if (key == data["key"]["Switch"].ToLower())
            {
                Switch(null, null);
            }
            else if (key == data["key"]["Decimal"].ToLower())
            {
                GameButton_Click(null, null);
            }
            else if (key == data["key"]["Add"].ToLower())
            {
                GameAdd_Click(null, null);
            }
            else if (key == data["key"]["Subtract"].ToLower())
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
                    flowWindow.Close();
                    flowWindow = new Window1() { Top = double.Parse(data["FlowWindow"]["FlowWindowTop"]), Left = double.Parse(data["FlowWindow"]["FlowWindowLeft"]) };
                }
                WindowEdit.Visibility = Visibility.Visible;
                flowWindow.Show();
            }
            if (e == null) { chkSwitch.IsChecked = (bool)chkSwitch.IsChecked ? false : true; }
        }



        /*上单模板 开始*/
        private void TopButton_Click(object sender, RoutedEventArgs e)
        {
            TopTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(100)
            };
            TopStartTime = Environment.TickCount;
            TopTimer.Tick += Toptimer_Tick;
            TopTimer.IsEnabled = true;
        }

        private void Toptimer_Tick(object sender, EventArgs e)
        {
            Top.Content = TimerUtil.ChangeTimeContent(TopStartTime, GameStartTime, (bool)TopBoot.IsChecked, (bool)TopStar.IsChecked);
            flowWindow.TopTime.Content = Top.Content;
        }


        private void TopAdd_Click(object sender, RoutedEventArgs e)
        {
            TopStartTime += FiveSecond;
        }

        private void TopSubtract_Click(object sender, RoutedEventArgs e)
        {
            TopStartTime -= FiveSecond;
        }

        private void TopClear_Click(object sender, RoutedEventArgs e)
        {
            TopStartTime -= 1000000;
        }

        /*上单模板 结束*/
        private void JugButton_Click(object sender, RoutedEventArgs e)
        {
            JugTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(100)
            };
            JugTimer.Tick += Jugtimer_Tick;
            JugTimer.IsEnabled = true;
            JugStartTime = Environment.TickCount;
        }

        private void Jugtimer_Tick(object sender, EventArgs e)
        {
            Jug.Content = TimerUtil.ChangeTimeContent(JugStartTime, GameStartTime, (bool)JugBoot.IsChecked, (bool)JugStar.IsChecked);
            flowWindow.JugTime.Content = Jug.Content;
        }

        private void JugAdd_Click(object sender, RoutedEventArgs e)
        {
            JugStartTime += FiveSecond;
        }

        private void JugSubtract_Click(object sender, RoutedEventArgs e)
        {
            JugStartTime -= FiveSecond;
        }

        private void JugClear_Click(object sender, RoutedEventArgs e)
        {
            JugStartTime -= 1000000;
        }


        private void MidButton_Click(object sender, RoutedEventArgs e)
        {
            MidTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(100)
            };
            MidStartTime = Environment.TickCount;
            MidTimer.Tick += Midtimer_Tick;
            MidTimer.IsEnabled = true;
        }

        private void Midtimer_Tick(object sender, EventArgs e)
        {
            Mid.Content = TimerUtil.ChangeTimeContent(MidStartTime, GameStartTime, (bool)MidBoot.IsChecked, (bool)MidStar.IsChecked);
            flowWindow.MidTime.Content = Mid.Content;
        }

        private void MidAdd_Click(object sender, RoutedEventArgs e)
        {
            MidStartTime += FiveSecond;
        }

        private void MidSubtract_Click(object sender, RoutedEventArgs e)
        {
            MidStartTime -= FiveSecond;
        }

        private void MidClear_Click(object sender, RoutedEventArgs e)
        {
            MidStartTime -= 1000000;
        }


        private void BotButton_Click(object sender, RoutedEventArgs e)
        {
            BotTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(100)
            };
            BotStartTime = Environment.TickCount;
            BotTimer.Tick += Bottimer_Tick;
            BotTimer.IsEnabled = true;
        }

        private void Bottimer_Tick(object sender, EventArgs e)
        {
            Bot.Content = TimerUtil.ChangeTimeContent(BotStartTime, GameStartTime, (bool)BotBoot.IsChecked, (bool)BotStar.IsChecked);
            flowWindow.BotTime.Content = Bot.Content;
        }

        private void BotAdd_Click(object sender, RoutedEventArgs e)
        {
            BotStartTime += FiveSecond;
        }

        private void BotSubtract_Click(object sender, RoutedEventArgs e)
        {
            BotStartTime -= FiveSecond;
        }

        private void BotClear_Click(object sender, RoutedEventArgs e)
        {
            BotStartTime -= 1000000;
        }

        private void SupButton_Click(object sender, RoutedEventArgs e)
        {
            SupTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(100)
            };
            SupStartTime = Environment.TickCount;
            SupTimer.Tick += Suptimer_Tick;
            SupTimer.IsEnabled = true;
        }

        private void Suptimer_Tick(object sender, EventArgs e)
        {
            Sup.Content = TimerUtil.ChangeTimeContent(SupStartTime, GameStartTime, (bool)SupBoot.IsChecked, (bool)SupStar.IsChecked);
            flowWindow.SupTime.Content = Sup.Content;
        }

        private void SupAdd_Click(object sender, RoutedEventArgs e)
        {
            SupStartTime += FiveSecond;
        }

        private void SupSubtract_Click(object sender, RoutedEventArgs e)
        {
            SupStartTime -= FiveSecond;
        }

        private void SupClear_Click(object sender, RoutedEventArgs e)
        {
            SupStartTime -= 1000000;
        }

        private void GameButton_Click(object sender, RoutedEventArgs e)
        {
            TopStartTime = JugStartTime = MidStartTime = BotStartTime = SupStartTime = 0;
            GameTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(100)
            };
            GameStartTime = Environment.TickCount;
            GameTimer.Tick += Gametimer_Tick;
            GameTimer.IsEnabled = true;
        }

        private void Gametimer_Tick(object sender, EventArgs e)
        {
            long time = (Environment.TickCount - GameStartTime) / 1000;
            flowWindow.Game.Content = Game.Content = (time / 60).ToString() + ":" + (time % 60).ToString().PadLeft(2, '0');
        }

        private void GameAdd_Click(object sender, RoutedEventArgs e)
        {
            GameStartTime -= FiveSecond;
        }

        private void GameSubtract_Click(object sender, RoutedEventArgs e)
        {
            GameStartTime += FiveSecond;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                flowWindow.Close();
                if (TopTimer != null) { TopTimer.Stop(); }
                if (JugTimer != null) { JugTimer.Stop(); }
                if (MidTimer != null) { MidTimer.Stop(); }
                if (BotTimer != null) { BotTimer.Stop(); }
                if (SupTimer != null) { SupTimer.Stop(); }
                if (GameTimer != null) { GameTimer.Stop(); }
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
