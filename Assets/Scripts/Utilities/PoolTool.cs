using UnityEngine;
using UnityEngine.Pool;

public class PoolTool : MonoBehaviour
{
    public GameObject objPrefab;
    private ObjectPool<GameObject> pool;// unity自带对象池

    private void Start()
    {
        // 1.创建对象池
        pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(objPrefab, transform),
            actionOnGet: (obj) => obj.SetActive(true),
            actionOnRelease: (obj) => obj.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: false,
            defaultCapacity: 10, // 默认大小
            maxSize: 50
            );

        PrefillPool(7);
    }

    private void PrefillPool(int count)
    {
        GameObject[] preFillArray = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            preFillArray[i] = pool.Get();
        }

        foreach (var item in preFillArray)
        {
            pool.Release(item);
        }
    }
    // 有点类似工厂模式
    public GameObject GetObjectFromPool()
    {
        return pool.Get();
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        pool.Release(obj);
    }
}
