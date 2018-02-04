using SecretService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretService.BuinessLogik
{
    public class SecretManager
    {
        
        public static List<Token> credentialDB;
        public SecretManager()
        {
            credentialDB = new List<Token>
            {
                new Token { Tokenstring = "credential1", Username = "user1", PW = "123" },
                new Token { Tokenstring = "credential2", Username = "user2", PW = "123" },
                new Token { Tokenstring = "credential3", Username = "user3", PW = "123" },
                new Token { Tokenstring = "credential4", Username = "user4", PW = "123" },
                new Token { Tokenstring = "credential5", Username = "user5", PW = "123" }
            };
        }


        public Token findToken(String user, String PW)
        {
            Token result = credentialDB.Find(u => u.Username == user);
            if (result.PW.Equals(PW)) return result;
            return null;
        }
        

    }
}
