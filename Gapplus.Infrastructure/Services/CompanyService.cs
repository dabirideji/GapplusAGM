using BarcodeGenerator.Models;
using Gapplus.Application.Interfaces;
using Gapplus.Domain;

namespace Gapplus.Infrastructure.Services
{
    public class CompanyService : GenericService<Company>, ICompanyService
    {
        public CompanyService(UsersContext context) : base(context)
        {
        }
    }
}
