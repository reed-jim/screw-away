using System;
using System.Collections.Generic;
using UnityEngine;

public enum SaferioPrefabIdentifier
{
    Explosion
}

public class ObjectPoolingEverything : MonoBehaviour
{
    private static ObjectPoolingEverything instance;
    private Dictionary<string, ObjectPool> _poolGroup;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        _poolGroup = new Dictionary<string, ObjectPool>();

        DontDestroyOnLoad(gameObject);
    }

    public static void AddToPool(string key, ObjectPool pool)
    {
        instance._poolGroup.Add(key, pool);
    }

    public static GameObject SpawnGameObject(GameObject prefab)
    {
        return Instantiate(prefab, instance.transform);
    }

    public static T GetFromPool<T>(string identifier)
    {
        foreach (var group in instance._poolGroup)
        {
            if (identifier == group.Key)
            {
                return group.Value.GetFromPool<T>();
            }
        }

        return default;
    }
}

[Serializable]
public struct PrefabWithIdentifier
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private string identifier;
    [SerializeField] private int poolSize;

    public GameObject Prefab => prefab;
    public string Identifier => identifier;
}

[Serializable]
public class ObjectPool
{
    private GameObjectWithComponent[] _pool;

    private int _currentIndex;

    public void Init<T>(GameObject prefab, int poolSize)
    {
        _pool = new GameObjectWithComponent[poolSize];

        for (int i = 0; i < poolSize; i++)
        {
            GameObject gameObject = ObjectPoolingEverything.SpawnGameObject(prefab);

            GameObjectWithComponent gameObjectWithComponent = new GameObjectWithComponent();

            gameObjectWithComponent.gameObject = gameObject;
            gameObjectWithComponent.component = gameObject.GetComponent<T>();

            _pool[i] = gameObjectWithComponent;

            gameObject.SetActive(false);
        }
    }

    public T GetFromPool<T>()
    {
        _pool[_currentIndex].gameObject.SetActive(true);

        T componentToGet = (T)_pool[_currentIndex].component;

        _currentIndex++;

        if (_currentIndex >= _pool.Length)
        {
            _currentIndex = 0;
        }

        return componentToGet;
    }

    // public void ReturnPool(GameObject gameObject)
    // {

    // }
}

public class GameObjectWithComponent
{
    public GameObject gameObject;
    public object component;
}
