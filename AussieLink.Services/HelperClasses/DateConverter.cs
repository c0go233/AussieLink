using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Services.HelperClasses
{
    public class DateConverter
    {
        public static string GetAdDate(DateTime adDate)
        {
            var currentDate = DateTime.Now;
            if (!IsSameYearAndMonth(adDate, currentDate))
                return adDate.ToShortDateString();
            else
                return GetDateDifference(adDate, currentDate);
        }

        private static bool IsSameYearAndMonth(DateTime adDate, DateTime currentDate)
        {
            if (adDate.Month != currentDate.Month || adDate.Year != currentDate.Year)
                return false;
            return true;
        }

        private static string GetDateDifference(DateTime adDate, DateTime currentDate)
        {
            var subInDay = currentDate.Day - adDate.Day;
            var subInHour = currentDate.Hour - adDate.Hour;
            var subInMinute = currentDate.Minute - adDate.Minute;

            if (subInDay == 0 && subInHour == 0 && subInMinute == 0)
                return "a second ago";
            else if (subInDay == 0 && subInHour == 0)
                return GetAdDateText(subInMinute, "minute ago", "minutes ago");
            else if (subInDay == 0)
                return GetAdDateText(subInHour, "hour ago", "hours ago");
            else
                return GetAdDateText(subInDay, "day ago", "days ago");
        }

        private static string GetAdDateText(int dateDifference, string singularText, string pluralText)
        {
            if (dateDifference == 1)
                return dateDifference + " " + singularText;
            else
                return dateDifference + " " + pluralText;
        }
    }
}
