using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewObjectPool : MonoBehaviour
{
    // 绑定节点
    private GameObject CachePanel;

    // 字典 多个对象池
    // instanceId作为key
    private Dictionary<string, Queue<GameObject>> m_Pool = new Dictionary<string, Queue<GameObject>>();

    // 储存物体的tag，用来识别不同的对象
    private Dictionary<GameObject, string> m_GoTag = new Dictionary<GameObject, string>();


    /// <summary>
    /// 清空缓存池，释放所有引用
    /// </summary>
    public void ClearCachePool()
    {
        m_Pool.Clear();
        m_GoTag.Clear();
    }

    /// <summary>
    /// 回收GameObject
    /// </summary>
    public void ReturnCacheGameObejct(GameObject go)
    {
        if (CachePanel == null)
        {
            CachePanel = new GameObject();
            CachePanel.name = "CachePanel";
            GameObject.DontDestroyOnLoad(CachePanel);
        }

        if (go == null)
        {
            return;
        }

        // 将回收object绑定在节点上
        go.transform.parent = CachePanel.transform;
        go.SetActive(false);

        if (m_GoTag.ContainsKey(go))
        {
            // 将tag移除
            string tag = m_GoTag[go];
            RemoveOutMark(go);
            // 如果不包含缓存队列 则创建
            if (!m_Pool.ContainsKey(tag))
            {
                m_Pool[tag] = new Queue<GameObject>();
            }

            m_Pool[tag].Enqueue(go);
        }
    }

    /// <summary>
    /// 请求GameObject
    /// </summary>
    public GameObject RequestCacheGameObejct(GameObject prefab)
    {
        // 获取instanceID
        string tag = prefab.GetInstanceID().ToString();
        GameObject go = GetFromPool(tag);
        if (go == null)
        {
            go = GameObject.Instantiate<GameObject>(prefab);
            go.name = prefab.name + Time.time;
        }


        MarkAsOut(go, tag);
        return go;
    }

    // 利用instanceID，来获取对象
    private GameObject GetFromPool(string tag)
    {
        if (m_Pool.ContainsKey(tag) && m_Pool[tag].Count > 0)
        {
            GameObject obj = m_Pool[tag].Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            return null;
        }
    }

    // 增加新的instanceID
    private void MarkAsOut(GameObject go, string tag)
    {
        m_GoTag.Add(go, tag);
    }

    // 移除instanceID
    private void RemoveOutMark(GameObject go)
    {
        if (m_GoTag.ContainsKey(go))
        {
            m_GoTag.Remove(go);
        }
        else
        {
            Debug.LogError("remove out mark error, gameObject has not been marked");
        }
    }
}
