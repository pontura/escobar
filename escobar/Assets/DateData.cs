using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using System.Net;
using System.Net.Sockets;
using UnityEngine.UI;


public class DateData : MonoBehaviour
{
    public List<string> daysList;
    public List<string> monthList;
    public List<string> yearList;
    public System.DateTime dateTime;
    public int diferenciaHoraria = -178;

    void Awake()
    {
        daysList = new List<string>();

        for (int a = 1; a < 32; a++)
            daysList.Add(a.ToString());

        monthList = new List<string> { "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic" };
        yearList = new List<string> { "2019", "2020", "2021", "2022", "2023" };
    }
    public void GetRealTime()
    {
        dateTime = NetworkTime();
        print("SERVER NOW: " + dateTime);
       // Loop();
    }
    //void Loop()
    //{
    //    print( NetworkTime() );
    //    Invoke("Loop", 2);
    //}
    public bool IsToday(string _date)
    {
        int day = GetDay(_date);
        int month = GetMonth(_date);
        int year = GetYear(_date);

        if (year == System.DateTime.Today.Year
            &&
            month == System.DateTime.Today.Month
            &&
            day == System.DateTime.Today.Day)
            return true;
        return false;
    }
    public string GetTodayParsed()
    {
        return Data.Instance.dateData.ParseDate(dateTime.Day, dateTime.Month, dateTime.Year);
    }
    public string ParseDate(int day, int month, int year)
    {
        return day + "/" + monthList[month - 1] + "/" + year;
    }
    public bool IsFromThePast(string _date)
    {
        int day = GetDay(_date);
        int month = GetMonth(_date);
        int year = GetYear(_date);

        if (year < System.DateTime.Today.Year)
            return true;
        if (month < System.DateTime.Today.Month)
            return true;
        if (day < System.DateTime.Today.Day)
            return true;
        return false;
    }
    public int GetDay(string dateString)
    {
        string[] all = dateString.Split("/"[0]);
        return int.Parse(all[0]);
    }
    public string GetDayAsString(string dateString)
    {
        string[] all = dateString.Split("/"[0]);

        string day = all[0];
        string month = GetMonth(dateString).ToString();
        string year = all[2];

        if (day.Length == 1)
            day = "0" + day;
        if (month.Length == 1)
            month = "0" + month;

        return year + month + day;
    }
    public int GetMonth(string dateString)
    {
        string[] all = dateString.Split("/"[0]);
        string m = all[1];
        int id = 1;
        foreach (string s in monthList)
        {
            if (s == m)
                return id;
            id++;
        }
        return 0;
    }
    public int GetYear(string dateString)
    {
        string[] all = dateString.Split("/"[0]);
        return int.Parse(all[2]);
    }






    public System.DateTime NetworkTime()
    {
        //default Windows time server
        const string ntpServer = "time.windows.com";

        // NTP message size - 16 bytes of the digest (RFC 2030)
        var ntpData = new byte[48];

        //Setting the Leap Indicator, Version Number and Mode values
        ntpData[0] = 0x1B; //LI = 0 (no warning), VN = 3 (IPv4 only), Mode = 3 (Client Mode)

        var addresses = Dns.GetHostEntry(ntpServer).AddressList;

        //The UDP port number assigned to NTP is 123
        var ipEndPoint = new IPEndPoint(addresses[0], 123);
        //NTP uses UDP
        var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        socket.Connect(ipEndPoint);

        //Stops code hang if NTP is blocked
        socket.ReceiveTimeout = 3000;

        socket.Send(ntpData);
        socket.Receive(ntpData);
        socket.Close();

        //Offset to get to the "Transmit Timestamp" field (time at which the reply 
        //departed the server for the client, in 64-bit timestamp format."
        const byte serverReplyTime = 40;

        //Get the seconds part
        ulong intPart = System.BitConverter.ToUInt32(ntpData, serverReplyTime);

        //Get the seconds fraction
        ulong fractPart = System.BitConverter.ToUInt32(ntpData, serverReplyTime + 4);

        //Convert From big-endian to little-endian
        intPart = SwapEndianness(intPart);
        fractPart = SwapEndianness(fractPart);

        double milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
        milliseconds += diferenciaHoraria * (60 * 1000);
        //**UTC** time
        var networkDateTime = (new System.DateTime(1900, 1, 1, 0, 0, 0, System.DateTimeKind.Utc)).AddMilliseconds((long)milliseconds);

        //return networkDateTime.ToLocalTime();
        return networkDateTime;
    }

    // stackoverflow.com/a/3294698/162671
    static uint SwapEndianness(ulong x)
    {
        return (uint)(((x & 0x000000ff) << 24) +
                       ((x & 0x0000ff00) << 8) +
                       ((x & 0x00ff0000) >> 8) +
                       ((x & 0xff000000) >> 24));
    }
}

