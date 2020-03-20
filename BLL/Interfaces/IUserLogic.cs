using BLL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IUserLogic
    {
        public List<ChallengeProfile> ViewChallengesList(UserProfile userProfile);
        public ChallengeContent ViewChallengeContent(Guid ChallengeId);
        Task<bool> WritingAnObjectAsync(Stream fileStream, string fileName, UserProfile userProfile);
        string ReadFileUrlAsync(Guid Id);
    }
}
