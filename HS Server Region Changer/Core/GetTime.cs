using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS_Server_Region_Changer.Core
{
    class GetTime
    {
        public static (KeyValuePair<string, String>[], string, string, DateTime) getTime()
        {
            DateTime utc = DateTime.UtcNow;
            DateTime now = DateTime.Now;
            TimeSpan eus = new TimeSpan(5, 0, 0);//-5
            TimeSpan cus = new TimeSpan(6, 0, 0);//-6
            TimeSpan scus = new TimeSpan(6, 0, 0);//-6
            TimeSpan wus = new TimeSpan(8, 0, 0);//-8
            TimeSpan sbr = new TimeSpan(3, 0, 0);//-3
            TimeSpan neu = new TimeSpan(2, 0, 0);//1~3
            TimeSpan weu = new TimeSpan(0, 0, 0);//0~1
            TimeSpan eas = new TimeSpan(8, 0, 0);//+8
            TimeSpan seas = new TimeSpan(8, 0, 0);//+7~9
            TimeSpan eau = new TimeSpan(10, 0, 0);//+10
            TimeSpan wja = new TimeSpan(9, 0, 0);//+9

            DateTime eus_utc = utc - eus;
            DateTime cus_utc = utc - cus;
            DateTime scus_utc = utc - scus;
            DateTime wus_utc = utc - wus;
            DateTime sbr_utc = utc - sbr;
            DateTime neu_utc = utc + neu;
            DateTime weu_utc = utc + weu;
            DateTime eas_utc = utc + eas;
            DateTime seas_utc = utc + seas;
            DateTime eau_utc = utc + eau;
            DateTime wja_utc = utc + wja;

            KeyValuePair<string, String>[] AuthGroup = new KeyValuePair<string, String>[] {
                                                    new KeyValuePair<string, String>("default","default"),
                                                    new KeyValuePair<string, String>("eus <現地時間:" + eus_utc.ToString("MM/dd HH:mm") + ">","eus"),
                                                    new KeyValuePair<string, String>("cus <現地時間:" + cus_utc.ToString("MM/dd HH:mm") + ">","cus"),
                                                    new KeyValuePair<string, String>("scus <現地時間:" + scus_utc.ToString("MM/dd HH:mm") + ">","scus"),
                                                    new KeyValuePair<string, String>("wus <現地時間:" + wus_utc.ToString("MM/dd HH:mm") + ">","wus"),
                                                    new KeyValuePair<string, String>("sbr <現地時間:" + sbr_utc.ToString("MM/dd HH:mm") + ">","sbr"),
                                                    new KeyValuePair<string, String>("neu <現地時間:" + neu_utc.ToString("MM/dd HH:mm") + ">","neu"),
                                                    new KeyValuePair<string, String>("weu <現地時間:" + weu_utc.ToString("MM/dd HH:mm") + ">","weu"),
                                                    new KeyValuePair<string, String>("eas <現地時間:" + eas_utc.ToString("MM/dd HH:mm") + ">","eas"),
                                                    new KeyValuePair<string, String>("seas <現地時間:" + seas_utc.ToString("MM/dd HH:mm") + ">","seas"),
                                                    new KeyValuePair<string, String>("eau <現地時間:" + eau_utc.ToString("MM/dd HH:mm") + ">","eau"),
                                                    new KeyValuePair<string, String>("wja <現地時間:" + wja_utc.ToString("MM/dd HH:mm") + ">","wja"),
                                                    };
            return (AuthGroup, "Key", "Value", now);
        }
    }
}
