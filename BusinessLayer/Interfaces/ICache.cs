
namespace BusinessLayer.Interfaces {
    public interface ICache {
        T Get<T>(string key);
        void Set(string key, object data, int? cacheTime = null);
        bool IsSet(string key);
        void Remove(string key);
        void RemoveByPattern(string pattern);
        void Clear();
    }
}