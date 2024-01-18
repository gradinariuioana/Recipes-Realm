using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Caching;
using BusinessLayer.Interfaces;

public class MemoryCacheService : ICache {

    private readonly int _cacheTime = 60;
    public MemoryCacheService(int cacheTime = 60) {
        _cacheTime = cacheTime;
    }

    protected ObjectCache Cache {
        get {
            return MemoryCache.Default;
        }
    }

    public T Get<T>(string key) {
        BinaryFormatter deserializer = new BinaryFormatter();
        using (MemoryStream memStream = new MemoryStream((byte[])Cache[key])) {
            return (T)deserializer.Deserialize(memStream);
        }
    }

    public void Set(string key, object objectData, int? cacheTime = null) {
        if (objectData == null) {
            return;
        }
        var policy = new CacheItemPolicy();
        if (!cacheTime.HasValue) {
            cacheTime = _cacheTime;
        }
        policy.AbsoluteExpiration = DateTime.Now
        + TimeSpan.FromMinutes(cacheTime.Value);
        BinaryFormatter serializer = new BinaryFormatter();
        using (MemoryStream memStream = new MemoryStream()) {
            serializer.Serialize(memStream, objectData);
            Cache.Add(new CacheItem(key, memStream.ToArray()), policy);
        }
    }

    public bool IsSet(string key) {
        return Cache.Contains(key);
    }

    public void Remove(string key) {
        Cache.Remove(key);
    }

    public void RemoveByPattern(string pattern) {
        foreach (var item in Cache) {
            if (item.Key.StartsWith(pattern)) {
                Remove(item.Key);
            }
        }
    }

    public void Clear() {
        foreach (var item in Cache) {
            Remove(item.Key);
        }
    }
}