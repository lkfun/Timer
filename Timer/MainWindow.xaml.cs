
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
        /// <summary>
        /// 对位记录时间
        /// </summary>
        long TopStartTime, JugStartTime, MidStartTime, BotStartTime, SupStartTime, GameStartTime;
        /// <summary>
        /// 对位触发器
        /// </summary>
        DispatcherTimer TopTimer, JugTimer, MidTimer, BotTimer, SupTimer, GameTimer;
        /// <summary>
        /// 加减步长 固定五秒
        /// </summary>
        private const int stepLength = 5000;
        /// <summary>
        /// 复位步长 固定一千秒
        /// </summary>
        private const int clearLength = 1000000;
        /// <summary>
        /// 弹窗对象
        /// </summary>
        FlowWindow flowWindow;
        /// <summary>
        /// 钩子对象
        /// </summary>
        private Hocy_Hook hook_Main = new Hocy_Hook();
        /// <summary>
        /// ini解析对象
        /// </summary>
        FileIniDataParser parser = new FileIniDataParser();
        /// <summary>
        /// ini数据对象
        /// </summary>
        IniData iniData;
        /// <summary>
        /// 从ini文件读出来的对应Key值
        /// </summary>
        string TopKey, JugKey, MidKey, BotKey, SupKey, GameKey, SwitchKey, DecimalKey, AddKey, SubtractKey;
        /// <summary>
        /// 委托对象，开关弹窗用途
        /// </summary>
        System.EventHandler delegateInstance;
        /// <summary>
        /// 编辑模式的左偏移
        /// </summary>
        const double leftOffset = 3;
        /// <summary>
        /// 编辑模式的上偏移
        /// </summary>
        const double topOffset = 26;
        public MainWindow()
        {
            InitializeComponent();
            #region 多开判断
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
            #endregion
            #region 获取ini文件并读取Key值
            iniData = parser.ReadFile("conf.ini");
            if (iniData != null) {
                TopKey = iniData["key"]["Top"].ToLower();
                JugKey = iniData["key"]["Jug"].ToLower();
                MidKey = iniData["key"]["Mid"].ToLower();
                BotKey = iniData["key"]["Bot"].ToLower();
                SupKey = iniData["key"]["Sup"].ToLower();
                SwitchKey = iniData["key"]["Switch"].ToLower();
                DecimalKey = iniData["key"]["Decimal"].ToLower();
                AddKey = iniData["key"]["Add"].ToLower();
                SubtractKey = iniData["key"]["Subtract"].ToLower();
            }
            #endregion
            #region 注册热键
            delegateInstance = new System.EventHandler(WindowEditFun);
            hook_Main.OnKeyDown += new System.Windows.Forms.KeyEventHandler(Hook_MainKeyDown);
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
            #endregion
            flowWindow = new FlowWindow();
            Switch(null, null);//按一下弹窗复选框按钮
            GameButton_Click(new object(), new RoutedEventArgs());//按一下游戏开始按钮
        }
        /// <summary>
        /// 钩子回调函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Hook_MainKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            string key = e.KeyData.ToString().ToLower();
            if (key == TopKey)
            {
                TopButton_Click(null, null);
            }
            else if (key == JugKey)
            {
                JugButton_Click(null, null);
            }
            else if (key == MidKey)
            {
                MidButton_Click(null, null);
            }
            else if (key == BotKey)
            {
                BotButton_Click(null, null);
            }
            else if (key == SupKey)
            {
                SupButton_Click(null, null);
            }
            else if (key == SwitchKey)
            {
                Switch(null, null);
            }
            else if (key == DecimalKey)
            {
                GameButton_Click(null, null);
            }
            else if (key == AddKey)
            {
                GameAdd_Click(null, null);
            }
            else if (key == SubtractKey)
            {
                GameSubtract_Click(null, null);
            }
        }
        /// <summary>
        /// 切换弹窗事件触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    flowWindow = new FlowWindow() { Top = double.Parse(iniData["FlowWindow"]["FlowWindowTop"]), Left = double.Parse(iniData["FlowWindow"]["FlowWindowLeft"]) };
                }
                WindowEdit.Visibility = Visibility.Visible;
                flowWindow.Show();
            }
            if (e == null) { chkSwitch.IsChecked = (bool)chkSwitch.IsChecked ? false : true; }
        }



        /*上单模板 开始*/
        private void TopButton_Click(object sender, RoutedEventArgs e)
        {
            if (TopTimer != null)
            {
                TopTimer.Stop();
            }
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
            bool isReady = true;
            Top.Content = TimerUtil.ChangeTimeContent(TopStartTime, GameStartTime, (bool)TopBoot.IsChecked, (bool)TopStar.IsChecked, out isReady);
            if (isReady)
            {
                TopTimer.Stop();
            }
            flowWindow.TopTime.Content = Top.Content;
        }


        private void TopAdd_Click(object sender, RoutedEventArgs e)
        {
            TopStartTime += stepLength;
        }

        private void TopSubtract_Click(object sender, RoutedEventArgs e)
        {
            TopStartTime -= stepLength;
        }

        private void TopClear_Click(object sender, RoutedEventArgs e)
        {
            TopStartTime -= clearLength;
        }

        /*上单模板 结束*/
        private void JugButton_Click(object sender, RoutedEventArgs e)
        {
            if (JugTimer != null)
            {
                JugTimer.Stop();
            }
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
            bool isReady = true;
            Jug.Content = TimerUtil.ChangeTimeContent(JugStartTime, GameStartTime, (bool)JugBoot.IsChecked, (bool)JugStar.IsChecked, out isReady);
            if (isReady)
            {
                JugTimer.Stop();
            }
            flowWindow.JugTime.Content = Jug.Content;
        }

        private void JugAdd_Click(object sender, RoutedEventArgs e)
        {
            JugStartTime += stepLength;
        }

        private void JugSubtract_Click(object sender, RoutedEventArgs e)
        {
            JugStartTime -= stepLength;
        }

        private void JugClear_Click(object sender, RoutedEventArgs e)
        {
            JugStartTime -= clearLength;
        }


        private void MidButton_Click(object sender, RoutedEventArgs e)
        {
            if (MidTimer != null) {
                MidTimer.Stop();
            }
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
            bool isReady = true;
            Mid.Content = TimerUtil.ChangeTimeContent(MidStartTime, GameStartTime, (bool)MidBoot.IsChecked, (bool)MidStar.IsChecked, out isReady);
            if (isReady)
            {
                MidTimer.Stop();
            }
            flowWindow.MidTime.Content = Mid.Content;
        }

        private void MidAdd_Click(object sender, RoutedEventArgs e)
        {
            MidStartTime += stepLength;
        }

        private void MidSubtract_Click(object sender, RoutedEventArgs e)
        {
            MidStartTime -= stepLength;
        }

        private void MidClear_Click(object sender, RoutedEventArgs e)
        {
            MidStartTime -= clearLength;
        }


        private void BotButton_Click(object sender, RoutedEventArgs e)
        {
            if (BotTimer != null)
            {
                BotTimer.Stop();
            }
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
            bool isReady = true;
            Bot.Content = TimerUtil.ChangeTimeContent(BotStartTime, GameStartTime, (bool)BotBoot.IsChecked, (bool)BotStar.IsChecked, out isReady);
            if (isReady)
            {
                BotTimer.Stop();
            }
            flowWindow.BotTime.Content = Bot.Content;
        }

        private void BotAdd_Click(object sender, RoutedEventArgs e)
        {
            BotStartTime += stepLength;
        }

        private void BotSubtract_Click(object sender, RoutedEventArgs e)
        {
            BotStartTime -= stepLength;
        }

        private void BotClear_Click(object sender, RoutedEventArgs e)
        {
            BotStartTime -= clearLength;
        }

        private void SupButton_Click(object sender, RoutedEventArgs e)
        {
            if (SupTimer != null)
            {
                SupTimer.Stop();
            }
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
            bool isReady = true;
            Sup.Content = TimerUtil.ChangeTimeContent(SupStartTime, GameStartTime, (bool)SupBoot.IsChecked, (bool)SupStar.IsChecked, out isReady);
            if (isReady)
            {
                SupTimer.Stop();
            }
            flowWindow.SupTime.Content = Sup.Content;
        }

        private void SupAdd_Click(object sender, RoutedEventArgs e)
        {
            SupStartTime += stepLength;
        }

        private void SupSubtract_Click(object sender, RoutedEventArgs e)
        {
            SupStartTime -= stepLength;
        }

        private void SupClear_Click(object sender, RoutedEventArgs e)
        {
            SupStartTime -= clearLength;
        }

        private void GameButton_Click(object sender, RoutedEventArgs e)
        {
            if (GameTimer != null) {
                GameTimer.Stop();
            }
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
            GameStartTime -= stepLength;
        }

        private void GameSubtract_Click(object sender, RoutedEventArgs e)
        {
            GameStartTime += stepLength;
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
            }
            catch (Exception e2)
            {
                MessageBox.Show(e2.Message, "error code:2");
            }
        }

        private void WindowEditFun(object sender, EventArgs e)
        {
            if (flowWindow.AllowsTransparency && e != EventArgs.Empty && flowWindow.IsLoaded)//编辑模式
            {
                flowWindow.Close();
                flowWindow = new FlowWindow() { AllowsTransparency = false, WindowStyle = WindowStyle.SingleBorderWindow, Top = double.Parse(iniData["FlowWindow"]["FlowWindowTop"]) - topOffset, Left = double.Parse(iniData["FlowWindow"]["FlowWindowLeft"]) - leftOffset };
                flowWindow.Closed += delegateInstance;
                flowWindow.Show();
            }
            else if (!flowWindow.AllowsTransparency)
            {
                iniData["FlowWindow"]["FlowWindowTop"] = (flowWindow.Top + topOffset).ToString();
                iniData["FlowWindow"]["FlowWindowLeft"] = (flowWindow.Left + leftOffset).ToString();
                parser.WriteFile("conf.ini", iniData);
                flowWindow.Closed -= delegateInstance;
                flowWindow.Close();
                flowWindow = new FlowWindow() { AllowsTransparency = true, WindowStyle = WindowStyle.None, Top = double.Parse(iniData["FlowWindow"]["FlowWindowTop"]), Left = double.Parse(iniData["FlowWindow"]["FlowWindowLeft"]) };
                flowWindow.Show();
            }
        }

    }

}
