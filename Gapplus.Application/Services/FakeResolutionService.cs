using System.Collections.Generic;
using BarcodeGenerator.Models;

namespace Gapplus.Api.Services
{
    public interface IFakeResolutionService
    {
        List<FakeResolutionModel> FakeResolutions { get; }
    }

    public class FakeResolutionService : IFakeResolutionService
    {
        public List<FakeResolutionModel> FakeResolutions { get; } = new List<FakeResolutionModel>();
    }
}
