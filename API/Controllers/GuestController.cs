using BLL.Models;

namespace BLL.Interfaces
{
    public interface IGuestLogic
    {

        string Login(UserLogin user);
        object Register(UserRegister user);
    }
}
