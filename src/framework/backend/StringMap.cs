
namespace framework;

public class StringMap<T>
{
    
    private Dictionary<string, T> _dict = new Dictionary<string, T>();

    public void Set(string key, T value) => _dict[key] = value;
    public T Get(string key) => _dict.TryGetValue(key, out var v) ? v : default!;
    public bool Exists(string key) => _dict.ContainsKey(key);
    public void Remove(string key) => _dict.Remove(key);

    public IEnumerable<string> Keys => _dict.Keys;
    public IEnumerable<T> Values => _dict.Values;

    // So foreach works directly
    public IEnumerator<KeyValuePair<string, T>> GetEnumerator() => _dict.GetEnumerator();
}
