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
                var emailClaim = claims.FirstOrDefault(c => c.Type == "user_email");
                var positionClaim = claims.FirstOrDefault(c => c.Type == "position");
                var idClaim = claims.FirstOrDefault(c => c.Type == "user_id");
                if (emailClaim == null || positionClaim == null || idClaim == null)
                {
                    return null;
                }
                else
                {
                    userProfile.Email = emailClaim.Value;
                    userProfile.PositionName = positionClaim.Value;
                    userProfile.Id = Guid.Parse(idClaim.Value);
                }
            }
            return userProfile;
        }
    }
}
