using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

public interface ITempDataManager
{
    void SetTempData(string key, object value);
    T GetTempData<T>(string key);
}

public class TempDataManager : ITempDataManager
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TempDataManager(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void SetTempData(string key, object value)
    {
        _httpContextAccessor.HttpContext.Items[key] = JsonConvert.SerializeObject(value);
    }

    public T GetTempData<T>(string key)
    {
        if (_httpContextAccessor.HttpContext.Items.TryGetValue(key, out var storedValue))
        {
            return JsonConvert.DeserializeObject<T>(storedValue.ToString());
        }
        return default(T);
    }
}
