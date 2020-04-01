using BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace BLL.Helpers
{
    public class UserClaimInfo
    {
        public UserClaimInfo()
        {

        }

        public UserProfile GetProfile(ClaimsIdentity identity)
        {
            UserProfile userProfile = new UserProfile();
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                userProfile.Email = claims.FirstOrDefault(c => c.Type == "user_email").Value;
                userProfile.PositionName = claims.FirstOrDefault(c => c.Type == "position").Value;
                userProfile.Id = Guid.Parse(claims.FirstOrDefault(c => c.Type == "user_id").Value);
            }
            return userProfile;
        }
    }
}
