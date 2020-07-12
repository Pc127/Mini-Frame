using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    private GameObject _prefab;

    // 一个gameObj的队列
    private Queue<GameObject> _pooledInstanceQueue = new Queue<GameObject>();

    public GameObject GetInstance()
    {
        if (_pooledInstanceQueue.Count > 0)
        {
            // 返回开头的Obj
            GameObject instanceToReuse = _pooledInstanceQueue.Dequeue();
            instanceToReuse.SetActive(true);
            return instanceToReuse;
        }

        return Instantiate(_prefab);
    }

    public void ReturnInstance(GameObject gameObjectToPool)
    {
        // 归还入栈
        _pooledInstanceQueue.Enqueue(gameObjectToPool);
        gameObjectToPool.SetActive(false);
        gameObjectToPool.transform.SetParent(gameObject.transform);
    }
}
