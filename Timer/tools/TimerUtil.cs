using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Timer
{
    public static class TimerUtil
    {
        static long flashTime = 295;
        public static string Content(long StartTime, long GameStartTime, bool BootIsChecked, bool StarIsChecked)
        {
            long timespan = ((StartTime - GameStartTime) / 1000) + flashTime - ((bool)BootIsChecked ? 30 : 0) - ((bool)StarIsChecked ? 15 : 0);
            string content = (timespan / 60).ToString().PadLeft(2, '0') + ":" + (timespan % 60).ToString().PadLeft(2, '0');
            return content;
        }

        public static string ChangeTimeContent(long StartTime, long GameStartTime, bool BootIsChecked, bool StarIsChecked)
        {
            long time = (flashTime - ((bool)BootIsChecked ? 30 : 0) - ((bool)StarIsChecked ? 15 : 0) - (Environment.TickCount - StartTime) / 1000);
            string content = Content(StartTime, GameStartTime, BootIsChecked, StarIsChecked);
            if (time <= 0)
            {
                return "就绪";
            }
            else
            {
                return  time + "秒（" + content + "）";
            }
        }
    }
}
