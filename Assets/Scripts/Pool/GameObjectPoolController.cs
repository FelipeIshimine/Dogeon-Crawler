using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameObjectPoolController : MonoBehaviour 
{
	#region Fields / Properties
	static GameObjectPoolController Instance
	{
		get
		{
            if(instance == null)
                CreateSharedInstance();
            return instance;
		}
	}
	static GameObjectPoolController instance;
	
	static Dictionary<string, PoolData> pools = new Dictionary<string, PoolData>();
	#endregion
	
	#region MonoBehaviour


	void Awake ()
	{
		if (instance != null && instance != this)
			Destroy(this);
		else
			instance = this;
	}
	#endregion
	
	#region Public
	public static void SetMaxCount (string key, int maxCount)
	{
		if (!pools.ContainsKey(key))
			return;
		PoolData data = pools[key];
		data.maxCount = maxCount;
	}

	public static bool AddEntry (string key, GameObject prefab, int prepopulate, int maxCount)
	{
		if (pools.ContainsKey(key))
			return false;
		
		PoolData data = new PoolData();
		data.prefab = prefab;
		data.maxCount = maxCount;
		data.pool = new Queue<Poolable>(prepopulate);
		pools.Add(key, data);
        Debug.Log("<color=cyan> Added Entry:" + key + "</color>");

        for (int i = 0; i < prepopulate; ++i)
			Enqueue( CreateInstance(key, prefab) );
		
		return true;
	}
	
	public static void ClearEntry (string key)
	{
		if (!pools.ContainsKey(key))
			return;
		
		PoolData data = pools[key];
		while (data.pool.Count > 0)
		{
			Poolable obj = data.pool.Dequeue();
			if (obj != null)
				GameObject.Destroy(obj.gameObject);
		}
        //Debug.Log($"Removed Key: {key}");
		pools.Remove(key);
	}
	
	public static void Enqueue (Poolable sender)
	{
		if (sender == null || sender.IsPooled || !pools.ContainsKey(sender.Key))
			return;
		
		PoolData data = pools[sender.Key];
		if (data.pool.Count >= data.maxCount)
		{
			GameObject.Destroy(sender.gameObject);
			Debug.Log("Destroyed from pool");
			return;
		}
		
		data.pool.Enqueue(sender);
		sender.IsPooled = true;
		sender.transform.SetParent(Instance.transform);
		sender.gameObject.SetActive(false);
	}

	public static Poolable Dequeue (string key)
	{
        if (!pools.ContainsKey(key))
        {
            foreach (string item in pools.Keys)
                Debug.Log("KEY: " + item);

            return null;
        }

		PoolData data = pools[key];
		if (data.pool.Count == 0)
			return CreateInstance(key, data.prefab);
		
		Poolable obj = data.pool.Dequeue();
		obj.IsPooled = false;
		return obj;
	}
	#endregion
	
	#region Private
	static void CreateSharedInstance ()
	{
		GameObject obj = new GameObject("GameObject Pool Controller");
		DontDestroyOnLoad(obj);
		instance = obj.AddComponent<GameObjectPoolController>();
	}
	
	static Poolable CreateInstance (string key, GameObject prefab)
	{
        GameObject instance = Instantiate(prefab) as GameObject;
        Poolable p = instance.GetComponent<Poolable>();
        if (p == null) p = instance.AddComponent<Poolable>();
        
        p.Key = key;
		return p;
    }
	#endregion
}