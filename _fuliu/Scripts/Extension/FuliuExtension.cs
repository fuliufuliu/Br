    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;


public static class Extension
{
    /// <summary>
    /// 获取当前计算机上的时间，单位为毫秒，最长为1小时
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static int AtTheMoment(this DateTime dt)
    {
        return DateTime.Now.Millisecond + DateTime.Now.Second * 1000 + DateTime.Now.Minute * 60000;
    }

    /// <summary>
    /// 转换List为Queue
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static Queue<T> ToQueue<T>(this List<T> list)
    {
        Queue<T> newQueue = new Queue<T>();
        foreach (T t in list) newQueue.Enqueue(t);
        return newQueue;
    }

    /// <summary>
    /// 获取数组的一部分元素
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array1">被读取的数组</param>
    /// <param name="startIndex">array1中从startIndex处开始读取</param>
    /// <param name="count">array1中从startIndex处开始读取count个元素</param>
    /// <returns>返回新数组</returns>
    public static T[] Read<T>(this T[] array1, int startIndex, int count)
    {
        T[] result = new T[count];
        for (int i = 0; i < count; i++)
        {
            result[i] = array1[startIndex + i];
        }
        return result;
    }

    /// <summary>
    /// 连接两个数组
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array1"></param>
    /// <param name="array2"></param>
    /// <returns>返回连接后的新数组</returns>
    public static T[] Connect<T>(this T[] array1, T[] array2)
    {
        List<T> list = new List<T>();
        list = array1.ToList();
        list.AddRange(array2.ToList());
        return list.ToArray();
    }

    /// <summary>
    /// 连接两个数组,不强制执行，出现不匹配时抛出异常.
    /// 异常1: array1的长度超过了指定的array1IndexStart.
    /// 异常2: array1 + array2 的长度超过了指定的maxLength.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array1">数组1</param>
    /// <param name="array1IndexStart">从数组1指定的位置开始连接</param>
    /// <param name="array2">数组2</param>
    /// <param name="maxLength">array1 + array2 的最大长度限制</param>
    /// <param name="isForceExcute">强制执行，不抛出异常</param>
    /// <returns>返回连接后的新数组</returns>
    //public static T[] Connect<T>(this T[] array1, int array1IndexStart, T[] array2, int maxLength)
    //{
    //    return Connect<T>(array1, array1IndexStart, array2, maxLength, false);
    //}


    public static byte[] Connect(byte[] array1, int array1IndexStart, byte[] array2, int maxLength)
    {
        byte[] tArray = new byte[maxLength];
        if (array1.Length < array1IndexStart)
        {
            for (int i = 0; i < array1.Length; i++) //0 ~array1.Length
            {
                tArray[i] = array1[i];
                // UnityEngine.MonoBehaviour.print((char)tArray[i]);
            }
            for (int i = array1.Length; i < array1IndexStart; i++) //array1.Length ~ array1IndexStart
            {
                tArray[i] = 255;
            }
        }
        else
        {
            for (int i = 0; i < array1IndexStart; i++) // 0 ~ array1IndexStart
            {
                tArray[i] = array1[i];
            }
        }
        if (array1IndexStart + array2.Length < maxLength)
        {

            for (int i = 0; i < array2.Length; i++)
            {
                tArray[array1IndexStart + i] = array2[i]; // array1IndexStart ~ array1IndexStart + array2.Length
            }
            for (int i = array1IndexStart + array2.Length; i < maxLength; i++) //array1IndexStart + array2.Length ~ maxLength
            {
                tArray[i] = 255;
            }
        }
        else
        {
            for (int i = 0; i < maxLength - array1IndexStart; i++) // 
            {
                tArray[array1IndexStart + i] = array2[i]; //array1IndexStart ~ array1IndexStart + maxLength - array1IndexStart
            }
        }
        foreach (byte b in tArray) UnityEngine.MonoBehaviour.print((char)b);


        return tArray;
    }

    /// <summary>
    /// 连接两个数组, 强制执行，不抛出异常.
    /// 异常1: array1的长度超过了指定的array1IndexStart.
    /// 异常2: array1 + array2 的长度超过了指定的maxLength.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array1">数组1</param>
    /// <param name="array1IndexStart">从数组1指定的位置开始连接</param>
    /// <param name="array2">数组2</param>
    /// <param name="maxLength">array1 + array2 的最大长度限制</param>
    /// <param name="isForceExcute">强制执行，不抛出异常</param>
    /// <returns>返回连接后的新数组</returns>
    public static T[] Connect<T>(this T[] array1, int array1IndexStart, T[] array2, int maxLength, bool isForceExcute)
    {
        List<T> list = new List<T>();
        list = array1.ToList();
        if (array1IndexStart > array1.Length)
        {
            for (int i = array1.Length; i < array1IndexStart; i++)
            {
                T t = System.Activator.CreateInstance<T>();
                list.Add(t);
            }
        }
        else if (array1IndexStart < array1.Length)
        {
            if (isForceExcute)
            {
                list.RemoveRange(array1IndexStart, list.Count - array1IndexStart);
            }
            else
                throw new Exception("array1的长度超过了指定的array1IndexStart");
        }
        list.AddRange(array2.ToList());
        if (maxLength < list.Count)
        {
            if (isForceExcute) list.RemoveRange(maxLength, list.Count - maxLength);
            else throw new Exception("array1 + array2 的长度超过了指定的maxLength");
        }
        else if (maxLength > list.Count)
        {

            for (int i = list.Count; i < maxLength; i++)
            {
                T t = System.Activator.CreateInstance<T>();
                list.Add(t);
            }
        }

        return list.ToArray();
    }

    public static string ClearZeroOfEnd(this string s)
    {
        string result = "";
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] != 0)
                result += s[i];
            else return result;
        }

        return result;
    }


}
