using System.Collections.Generic;
using UnityEngine;

public static class FuliuExtensionUnity 
{
    /// <summary>
    /// 安全地获取组件
    /// 扩展Component
    /// </summary>
    public static T GetSafeComponent<T>(this Component obj) where T : MonoBehaviour
    {
        T component;
        if (obj != null) component = obj.GetComponent<T>();
        else
        {
            Debug.LogError("引用物体为空！");
            return null;
        }
        if (component == null)
        {
            Debug.LogError("未找到相应的组件： " + typeof(T) + " ，查找对象是：" + obj.name, obj);
        }
        return component;
    }

    /// <summary>
    /// 安全地获取组件
    /// 扩展GameObject
    /// </summary>
    public static T GetSafeComponent<T>(this GameObject obj) where T : MonoBehaviour
    {
        T component;
        if (obj != null) component = obj.GetComponent<T>();
        else
        {
            Debug.LogError("引用物体为空！");
            return null;
        }
        if (component == null)
        {
            Debug.LogError("未找到相应的组件： " + typeof(T) + " ，查找对象是：" + obj.name, obj);
        }
        return component;
    }

    /// <summary>
    /// 设置transform.position.x
    /// 扩展Transform
    /// </summary>
    public static void SetPositionX(this Transform transform, float x)
    {
        Vector3 newPosition = new Vector3(x, transform.position.y, transform.position.z);
        transform.position = newPosition;
    }
    /// <summary>
    /// 设置transform.position.y
    /// 扩展Transform
    /// </summary>
    public static void SetPositionY(this Transform transform, float y)
    {
        Vector3 newPosition = new Vector3(transform.position.x, y, transform.position.z);
        transform.position = newPosition;
    }
    /// <summary>
    /// 设置transform.position.y
    /// 扩展Transform
    /// </summary>
    public static void SetPositionZ(this Transform transform, float z)
    {
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, z);
        transform.position = newPosition;
    }

    /// <summary>
    /// 设置transform.position.x
    /// 扩展Transform
    /// </summary>
    public static void SetEulerAnglesX(this Transform transform, float x)
    {
        transform.eulerAngles = new Vector3(x, transform.eulerAngles.y, transform.eulerAngles.z);
    }

    /// <summary>
    /// 设置transform.position.y
    /// 扩展Transform
    /// </summary>
    public static void SetEulerAnglesY(this Transform transform, float y)
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, y, transform.eulerAngles.z);
    }

    /// <summary>
    /// 设置transform.position.z
    /// 扩展Transform
    /// </summary>
    public static void SetEulerAnglesZ(this Transform transform, float z)
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, z);
    }

    /// <summary>
    /// 移除UIGrid对象中所有元素
    /// </summary>
    /// <param name="uiGrid"></param>
    public static void Clear(this UIGrid uiGrid)
    {
        List<Transform> childs = uiGrid.GetChildList();
        childs.RemoveAll(ListClearPredicate);
    }

    /// <summary>
    /// 移除UIGrid对象中所有元素
    /// </summary>
    /// <param name="uiGrid"></param>
    public static void ClearAllChildGO(this UIGrid uiGrid)
    {
        List<Transform> childs = uiGrid.GetChildList();
        for (int i = childs.Count - 1; i >= 0; i--)
        {
            GameObject.Destroy(childs[i].gameObject);
        }
        childs.RemoveAll(ListClearPredicate);
    }

    public static void Clear<T>(this List<T> list)
    {
        list.RemoveAll(ListClearPredicate);
    }

    private static bool ListClearPredicate<T>(T obj)
    {
        return true;
    }

    private static bool ListClearPredicate(Transform obj)
    {
        return true;
    }

}