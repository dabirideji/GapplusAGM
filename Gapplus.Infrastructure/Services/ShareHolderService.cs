using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BarcodeGenerator.Models;
using Gapplus.Application.Interfaces.GenericInterfaces;
using Gapplus.Domain.Models.Base;

namespace Gapplus.Infrastructure.Services
{
    public class ShareHolderService : GenericService<ShareHolder>, IShareHolderService
    {
        public ShareHolderService(UsersContext context) : base(context)
        {
        }
    }
}