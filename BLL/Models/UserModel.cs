
using System;

namespace BLL.Models
{
    public class UserModel
    {
        public string Email { get; set; }
    }

    public class UserCreateModel : UserModel
    {
        public Guid UserId { get; set; }
        public string Phone { get; set; }
        public string FullName { get; set; }
        public string ConfirmationCode { get; set; }
        public Guid PositionId { get; set; }
    }



    public class UserRegister : UserModel
    {
        public string Phone { get; set; }
        public string FullName { get; set; }
        public string PositionName { get; set; }

    }
    public class UserProfile : UserModel
    {
        public Guid Id { get; set; }
        public string PositionName { get; set; }
    }

    public class UserLogin : UserModel
    {
        public string ConfirmationCode { get; set; }

    }
}
