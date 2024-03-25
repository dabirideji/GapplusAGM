using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

namespace Gapplus.Application.Helpers
{

public interface ICacheService
{
    void DisableCache(HttpResponse response);
    void SetCacheDuration(HttpResponse response, int seconds);
    void SetPrivateCache(HttpResponse response, int seconds);
    void SetPublicCache(HttpResponse response, int seconds);
}

public class CacheService : ICacheService
{
    public void DisableCache(HttpResponse response)
    {
        response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
        response.Headers["Expires"] = "0";
        response.Headers["Pragma"] = "no-cache";
    }

    public void SetCacheDuration(HttpResponse response, int seconds)
    {
        response.Headers["Cache-Control"] = "public, max-age=" + seconds;
    }

    public void SetPrivateCache(HttpResponse response, int seconds)
    {
        response.Headers["Cache-Control"] = "private, max-age=" + seconds;
    }

    public void SetPublicCache(HttpResponse response, int seconds)
    {
        response.Headers["Cache-Control"] = "public, max-age=" + seconds;
    }
}

}