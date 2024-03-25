using System;
using System.Threading.Tasks;
using BarcodeGenerator.Models;

namespace Gapplus.Application.Interfaces.IContracts
{
    public interface IBarcodeContract
    {
        Task<string> PresentAsync(int id, QuestionStatus data);
        Task<string> EditAsync(int id, PresentModel collection);
        Task<BarcodeModel> Create(BarcodeModelDto model);
    }
}
