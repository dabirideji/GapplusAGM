using Gapplus.Web.DTO.ShareHolder;
using Refit;

namespace Gapplus.Web.RefitContracts
{
    public interface IShareHolderContract
    {
        [Post("/Login")]
        Task<HttpResponseMessage> Login([Body] LoginDto login);

    }
}
