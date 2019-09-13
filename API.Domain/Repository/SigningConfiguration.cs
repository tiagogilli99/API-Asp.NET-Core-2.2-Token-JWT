using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace API.Domain.Repository
{
    public class SigningConfiguration
    {
        public SigningConfiguration()
        {
            using (var provider = new RSACryptoServiceProvider(2048))
            {
                Key = new RsaSecurityKey(provider.ExportParameters(true));
            }

            Credentials = new SigningCredentials(Key, SecurityAlgorithms.RsaSha256Signature);
        }

        public SecurityKey Key { get; set; }

        public SigningCredentials Credentials { get; set; }
    }
}
