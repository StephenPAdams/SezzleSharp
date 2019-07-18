using System;
using System.Threading.Tasks;
using NUnit.Framework;
using SixFourThree.SezzleSharp.Endpoints;
using SixFourThree.SezzleSharp.Exceptions.Specific;
using FluentAssertions;

namespace SixFourThree.SezzleSharp.Tests.Endpoints
{
    [TestFixture]

    public class AuthenticationClientTests : TestBase
    {
        [Test]
        public async Task CreateToken_ValidCredentials_ValidTokenReturned()
        {
            //arrange
            var authenticationClient = new AuthenticationEndpoint(_baseConfiguration, _authenticationConfiguration, GetDefaultSezzleHttpClient());

            //act
            var resp = await authenticationClient.CreateTokenAsync();

            //assert            
            Assert.AreNotEqual(0, resp.Token.Length);
            Assert.Greater(resp.ExpirationDate, DateTime.Now);
        }

        [Test]
        public async Task CreateToken_InvalidCredentials_BadPublicKey_TargetedExceptionThrown()
        {
            Func<Task> act = async () =>
            {
               //arrange
               _authenticationConfiguration.ApiPublicKey += "bad";
               var authenticationClient = new AuthenticationEndpoint(_baseConfiguration, _authenticationConfiguration,
                   GetDefaultSezzleHttpClient());

               //act
               var resp = await authenticationClient.CreateTokenAsync();
            };

            //assert
            act.Should().Throw<SezzleErrorResponseException>();
        }

        [Test]
        public void CreateToken_InvalidCredentials_BadPrivateKey_TargetedExceptionThrown()
        {
            Func<Task> act = async () =>
            {
                //arrange
                _authenticationConfiguration.ApiPrivateKey += "bad";
                var authenticationClient = new AuthenticationEndpoint(_baseConfiguration, _authenticationConfiguration,
                    GetDefaultSezzleHttpClient());

                //act
                var resp = await authenticationClient.CreateTokenAsync();
            };

            //assert
            act.Should().Throw<SezzleErrorResponseException>();
        }

        [Test]
        public void CreateToken_BadSezzleUrlEndpoint_ExceptionThrown()
        {
            Func<Task> act = async () =>
            {
                //arrange
                _baseConfiguration.ApiUrl += "aaaaa/";
                var authenticationClient = new AuthenticationEndpoint(_baseConfiguration, _authenticationConfiguration,
                    GetDefaultSezzleHttpClient());

                //act
                var resp = await authenticationClient.CreateTokenAsync();
            };

            //assert
            act.Should().Throw<SezzleErrorResponseException>();
        }

        [Test]
        public void CreateToken_BlankUrl_ExceptionThrown()
        {
            Func<Task> act = async () =>
            {
                //arrange
                _baseConfiguration.ApiUrl = string.Empty;
                var authenticationClient = new AuthenticationEndpoint(_baseConfiguration, _authenticationConfiguration,
                    GetDefaultSezzleHttpClient());

                //act
                var resp = await authenticationClient.CreateTokenAsync();
            };

            //assert
            //todo: get these other unit tests working.
            act.Should().Throw<System.UriFormatException>();
        }

        [Test]
        public void CreateToken_ValidUrlThatIsNotSezzle_KnownExceptionThrown()
        {
            Func<Task> act = async () =>
            {
                //arrange
                _baseConfiguration.ApiUrl = "http://www.google.com";
                var authenticationClient = new AuthenticationEndpoint(_baseConfiguration, _authenticationConfiguration,
                    GetDefaultSezzleHttpClient());

                //act
                var resp = await authenticationClient.CreateTokenAsync();
            };

            //assert
            act.Should().Throw<SezzleErrorResponseException>();
        }
    }
}
