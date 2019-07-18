using System.Threading.Tasks;
using SixFourThree.SezzleSharp.Models.Authentication;

namespace SixFourThree.SezzleSharp.Interfaces
{
    public interface IAuthenticationEndpoint
    {

        /// <summary>
        /// Gets a valid token.  You may wish to implement this interface with a caching solution.
        /// </summary>
        /// <returns></returns>
        Task<AuthenticationResponse> CreateTokenAsync();
    }
}
