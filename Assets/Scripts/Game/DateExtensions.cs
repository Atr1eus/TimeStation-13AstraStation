using System;
using System.Globalization;

[Serializable]
public static class DateExtensions
{
    private static DateTime BaseDate = new DateTime(5070, 6, 5);

    public static DateTime ToGameDate(this int dayOffset)
    {
        return BaseDate.AddDays(dayOffset);
    }

    public static string ToGameDateString(this int dayOffset)
    {
        DateTime date = dayOffset.ToGameDate();
        return date.ToString("yyyy.M.d");
    }
    public static string GetDate()
    {
        return BaseDate.ToString("yyyy.M.d");
    }
    private static readonly string[] SupportedFormats = new[]
{
        "yyyy.M.d",
        "yyyy.MM.dd",
        "yyyy/M/d",
        "yyyy/MM/dd"
    };

    // 尝试将字符串转换为DateTime
    public static bool TryParseDate(string dateString, out DateTime result)
    {
        return DateTime.TryParseExact(dateString,
                                    SupportedFormats,
                                    CultureInfo.InvariantCulture,
                                    DateTimeStyles.None,
                                    out result);
    }

    // 比较两个字符串日期的早晚
    public static int CompareDates(string dateStr1, string dateStr2)
    {
        if (TryParseDate(dateStr1, out DateTime date1) &&
            TryParseDate(dateStr2, out DateTime date2))
        {
            return DateTime.Compare(date1, date2);
        }
        throw new ArgumentException("无效的日期格式");
    }

    // 判断dateStr1是否早于dateStr2
    public static bool IsEarlier(string dateStr1, string dateStr2)
    {
        return CompareDates(dateStr1, dateStr2) < 0;
    }

    // 判断dateStr1是否晚于dateStr2
    public static bool IsLater(string dateStr1, string dateStr2)
    {
        return CompareDates(dateStr1, dateStr2) > 0;
    }

    // 判断两个字符串日期是否相同
    public static bool IsSameDate(string dateStr1, string dateStr2)
    {
        return CompareDates(dateStr1, dateStr2) == 0;
    }
}
