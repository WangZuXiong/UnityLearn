using System;

public static class DateTimeExtension
{
    public static DateTime GetDateTime(long timeStamp)
    {
        DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        TimeSpan toNow = new TimeSpan(timeStamp * 10000);
        return dateTimeStart.Add(toNow);
    }

    /// <summary>
    /// 获取季度
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static int Season(this DateTime dateTime)
    {
        return (dateTime.Month / 4) + 1;
    }

    /// <summary>
    /// x年x月
    /// </summary>
    public static string GetYM(this DateTime dateTime)
    {
        return string.Format("{0}年{1}月", dateTime.Year, dateTime.Month);
    }

    /// <summary>
    /// x月x日xx:xx
    /// </summary>
    public static string GetMDHM(this DateTime dateTime)
    {
        return string.Format("{0}月{1}日{2}", dateTime.Month, dateTime.Day, dateTime.Hour + ":" + dateTime.Minute);
    }

    /// <summary>
    /// x年x季度
    /// </summary>
    public static string GetYS(this DateTime dateTime)
    {
        return string.Format("{0}年{1}季度", dateTime.Year, dateTime.Season());
    }

    /// <summary>
    /// x年
    /// </summary>
    public static string GetY(this DateTime dateTime)
    {
        return string.Format("{0}年", dateTime.Year);
    }
}