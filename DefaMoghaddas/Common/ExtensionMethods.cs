using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;

namespace DefaMoghaddas.Common
{
    public class Extensions
    {
        public static string ToDescriptionEnum(Enum en)
        {
            Type type = en.GetType();

            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return en.ToString();
        }

        public static string ToSpecialPersianDateTime(DateTime dateTime)
        {
            var persianCalendar = new PersianCalendar();
            return string.Format("{0}-{1}-{2}", persianCalendar.GetYear(dateTime),
                persianCalendar.GetMonth(dateTime).ToString().Length == 1
                    ? persianCalendar.GetMonth(dateTime).ToString().PadLeft(2, '0')
                    : persianCalendar.GetMonth(dateTime).ToString(),
                persianCalendar.GetDayOfMonth(dateTime).ToString().Length == 1
                    ? persianCalendar.GetDayOfMonth(dateTime).ToString().PadLeft(2, '0')
                    : persianCalendar.GetDayOfMonth(dateTime).ToString());
        }

        public static string ToPersianDateTime(DateTime geregorianDateTime)
        {
            var persian = new PersianCalendar();
            var day = persian.GetDayOfMonth(geregorianDateTime);
            var month = persian.GetMonth(geregorianDateTime);
            var year = persian.GetYear(geregorianDateTime);
            var hour = persian.GetHour(geregorianDateTime);
            var min = persian.GetMinute(geregorianDateTime);
            var second = persian.GetSecond(geregorianDateTime);
            return String.Format("{0}/{1}/{2} - {3}:{4}:{5}", year, month.ToString().PadLeft(2, '0'),
                day.ToString().PadLeft(2, '0'), hour, min, second);
        }

        public static string ToShortPersianDateTime(DateTime geregorianDateTime)
        {
            var persian = new PersianCalendar();
            var day = persian.GetDayOfMonth(geregorianDateTime);
            var month = persian.GetMonth(geregorianDateTime);
            var year = persian.GetYear(geregorianDateTime);
            return String.Format("{0}/{1}/{2}", year, month.ToString().PadLeft(2, '0'), day);
        }

        public static DateTime ToGeregorianDate(string persianDateTime)
        {
            var calendar = new PersianCalendar();
            var strings = persianDateTime.Split("/".ToCharArray());
            return calendar.ToDateTime(int.Parse(strings[0]), int.Parse(strings[1].PadLeft(2, '0')),
                int.Parse(strings[2].PadLeft(2, '0')), 0, 0, 0, 0);
        }

        public static string ToLogPersianDateTime(DateTime geregorianDateTime)
        {
            var persian = new PersianCalendar();
            var day = persian.GetDayOfMonth(geregorianDateTime);
            var month = persian.GetMonth(geregorianDateTime);
            var year = persian.GetYear(geregorianDateTime);
            return String.Format("{0}-{1}-{2}", year, month.ToString().PadLeft(2, '0'), day.ToString().PadLeft(2, '0'));
        }

    }

}