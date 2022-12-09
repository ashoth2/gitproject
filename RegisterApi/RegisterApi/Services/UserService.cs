using Microsoft.EntityFrameworkCore;
using RegisterApi.Models;
using RegisterApiDAL.EFModels;
using RegisterApiDAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace RegisterApi.Services
{
    public class UserService : IUserService
    {
        private readonly UserDbContext dbContext;

        public UserService(UserDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
       


       public async Task<IEnumerable<UserResponseModel>> GetUser()
        {
            List<UserResponseModel> userResponseModel = new List<UserResponseModel>();
            var data = await dbContext.Registrations.Where(x => x.Statusid == 1).ToListAsync();
            foreach (var item in data)
            {
                var role = dbContext.Roles.FirstOrDefault(x => x.Roleid == item.Roleid).Roles;
                UserResponseModel userResponseModels = new UserResponseModel()
                {
                    UserId = item.Userid,
                    FirstName = item.Firstname,
                    LastName = item.Lastname,
                    Email = item.Email,
                    Address = item.Address,
                    Role = role,
                    Statusid = item.Statusid
                };
                userResponseModel.Add(userResponseModels);
            }
            return userResponseModel;
        }

        public async Task<UserResponseModel> GetUser(int UserId)
        {
            UserResponseModel userResponseModel = new UserResponseModel();
            var data = await dbContext.Registrations.Where(x => x.Userid == UserId).FirstOrDefaultAsync();
            var role = await dbContext.Roles.Where(x => x.Roleid == data.Roleid).FirstOrDefaultAsync();
            if (data != null)
            {
                userResponseModel.UserId = data.Userid;
                userResponseModel.FirstName = data.Firstname;
                userResponseModel.LastName = data.Lastname;
                userResponseModel.Email = data.Email;
                userResponseModel.Address = data.Address;
                userResponseModel.Role = role.Roles;
                userResponseModel.Statusid = data.Statusid;
            }
            return userResponseModel;
        }

        public async Task<ResponseModel> Register(RegisterModel registerModel, string role)
        {
            Registration registration = new Registration();

            try
            {
                registration.Firstname = registerModel.FirstName;
                registration.Lastname = registerModel.LastName;
                registration.Email = registerModel.Email;
                registration.Address = registerModel.Address;
                registration.Statusid = 1;
                var roleID = await dbContext.Roles.Where(x => x.Roles == role).FirstOrDefaultAsync();
                if (roleID != null)
                    registration.Roleid = roleID.Roleid;
                else
                    return new ResponseModel { StatusCode = StatusCodes.Status400BadRequest, Message = "Incorrect role" };

                await dbContext.Registrations.AddAsync(registration);
                await dbContext.SaveChangesAsync();
                return new ResponseModel { StatusCode = StatusCodes.Status200OK, Message = "Success" };
            }
            catch (Exception Handler)
            {
                return new ResponseModel { StatusCode = StatusCodes.Status500InternalServerError, Message = Handler.Message };
            }
          }

       
    }
}
