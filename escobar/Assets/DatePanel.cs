using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DatePanel : MonoBehaviour
{
    public string value;

    public Dropdown days;
    public Dropdown month;
    public Dropdown year;

    List<string> daysList;
    public List<string> monthList;
    List<string> yearList;

    public void Init(string value)
    {
        daysList = new List<string>();

        for (int a = 1; a < 32; a++)
            daysList.Add ( a.ToString() );

        monthList = new List<string> { "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic" };
        yearList = new List<string> { "2019", "2020", "2021", "2022", "2023" };

        string[] arr = value.Split("/"[0]);

        InitDays(       int.Parse(arr[0])   );
        InitMonths(     arr[1]              );
        InitYears(      arr[2]   );
    }
    void InitDays(int dayID)
    {
        if (days.options.Count == 0)
            days.AddOptions(daysList);
        days.value = dayID-1;
    }
    public void InitMonths(string monthID)
    {       
        if(month.options.Count==0)
            month.AddOptions(monthList);
        int value = 0;
        foreach (string n in monthList)
        {
            if (n == monthID)
                month.value = value;
            value++;
        }
    }
    void InitYears(string yearID)
    {
        if (year.options.Count == 0)
            year.AddOptions(yearList);
        int value = 0;
        foreach(string n in yearList)
        {
            if(n == yearID)
                year.value = value;
            value++;
        }
    }
    public string GetValue()
    {
        value = (days.value + 1) + "/" + month.options[month.value].text + "/" + year.options[year.value].text;
        return value;
    }
}
