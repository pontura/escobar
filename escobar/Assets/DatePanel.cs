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

    public void Init(string value)
    {
        string[] arr = value.Split("/"[0]);

        InitDays(       int.Parse(arr[0])   );
        InitMonths(     arr[1]              );
        InitYears(      arr[2]   );
    }
    void InitDays(int dayID)
    {
        if (days.options.Count == 0)
            days.AddOptions(Data.Instance.dateData.daysList);
        days.value = dayID-1;
    }
    public void InitMonths(string monthID)
    {       
        if(month.options.Count==0)
            month.AddOptions(Data.Instance.dateData.monthList);
        int value = 0;
        foreach (string n in Data.Instance.dateData.monthList)
        {
            if (n == monthID)
                month.value = value;
            value++;
        }
    }
    void InitYears(string yearID)
    {
        if (year.options.Count == 0)
            year.AddOptions(Data.Instance.dateData.yearList);
        int value = 0;
        foreach(string n in Data.Instance.dateData.yearList)
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
