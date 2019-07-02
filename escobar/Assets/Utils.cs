 ﻿using UnityEngine;
 using System.Collections;
 using System.Collections.Generic;
 
 public static class Utils {
 
     public static void RemoveAllChildsIn(Transform container)
     {
         int num = container.transform.childCount;
         for (int i = 0; i < num; i++) UnityEngine.Object.DestroyImmediate(container.transform.GetChild(0).gameObject);
     }
     public static void ShuffleListTexts(List<string> texts)
     {
         if (texts.Count < 2) return;
         for (int a = 0; a < 100; a++)
         {
             int id = Random.Range(1, texts.Count);
             string value1 = texts[0];
             string value2 = texts[id];
             texts[0] = value2;
             texts[id] = value1;
         }
     }
    public static void ShuffleListNums(int[] nums)
    {
        if (nums.Length < 2) return;
        for (int a = 0; a < 100; a++)
        {
            int id = Random.Range(1, nums.Length);
            int value1 = nums[0];
            int value2 = nums[id];
            nums[0] = value2;
            nums[id] = value1;
        }
    }
   
    public static class CoroutineUtil
	{
		public static IEnumerator WaitForRealSeconds(float time)
		{
			float start = Time.realtimeSinceStartup;
			while (Time.realtimeSinceStartup < start + time)
			{
				yield return null;
			}
		}
	}
	public static string FormatNumbers(int num)
	{
		return string.Format ("{0:#,#}",  num);
	}
    public static int ConvertToToTimestamp(System.DateTime value)
    {
        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        int cur_time = (int)(value - epochStart).TotalSeconds;
        return cur_time;
    }
}
