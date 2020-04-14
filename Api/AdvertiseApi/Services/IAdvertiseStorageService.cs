using AdvertiseApi.Models;
using System.Threading.Tasks;

namespace AdvertiseApi.Services
{
    public interface IAdvertiseStorageService
    {
        Task<string> AddAsync(AdvertiseModel model);
        Task ConfirmAsync(ConfirmAdvertiseModel model);
    }
}