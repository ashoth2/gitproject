using RegisterApi.Models;

namespace RegisterApi.Services
{
    public interface IUserService
    {
        Task<ResponseModel> Register(RegisterModel registerModel, string role);
        
        Task<IEnumerable<UserResponseModel>> GetUser();
        Task<UserResponseModel> GetUser(int UserId);
    }
}
