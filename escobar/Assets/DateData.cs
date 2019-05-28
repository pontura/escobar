using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DateData : MonoBehaviour
{
    public List<string> daysList;
    public List<string> monthList;
    public List<string> yearList;

    void Awake()
    {
        daysList = new List<string>();

        for (int a = 1; a < 32; a++)
            daysList.Add(a.ToString());

        monthList = new List<string> { "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic" };
        yearList = new List<string> { "2019", "2020", "2021", "2022", "2023" };
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
}

