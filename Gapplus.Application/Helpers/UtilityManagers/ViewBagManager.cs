using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

public interface IViewBagManager
{
    void SetValue(string key, object value);
    T GetValue<T>(string key);
}

public class ViewBagManager : IViewBagManager
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ViewBagManager(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void SetValue(string key, object value)
    {
        _httpContextAccessor.HttpContext.Items[key] = JsonConvert.SerializeObject(value);
    }

    public T GetValue<T>(string key)
    {
        if (_httpContextAccessor.HttpContext.Items.TryGetValue(key, out var storedValue))
        {
            return JsonConvert.DeserializeObject<T>(storedValue.ToString());
        }
        return default(T);
    }
}
