using System;
using System.Linq;

namespace BLL.Helpers
{
    class ConfirmationCodeManager
    {
        public string GenerateConfimationCode()
        {
            Random random = new Random();
            const string pool = "abcdefghijklmnopqrstuvwxyz0123456789";
            var chars = Enumerable.Range(0, 32)
                .Select(x => pool[random.Next(0, pool.Length)]);
            return new string(chars.ToArray());
        }
    }
}
