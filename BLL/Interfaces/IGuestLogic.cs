using BLL.Models;

namespace BLL.Interfaces
{
    public interface IGuestLogic
    {

        string Login(UserLogin user);
        UserLogin Register(UserRegister user);
    }
}
