using webApp.Data;
using webApp.Dto;

namespace webApp.Iservices
{
    public interface IAuthService
    {

        Task<Tuple<int, string>> LoginUser(UserDto dto);
        Task<Tuple<int, string>> RegisterUser(UserDto dto);


    }
}
