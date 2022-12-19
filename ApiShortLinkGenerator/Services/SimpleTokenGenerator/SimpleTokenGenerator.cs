using ApiShortLinkGenerator.Interfaces;

namespace ApiShortLinkGenerator.Services.SimpleTokenGenerator
{
    public class SimpleTokenGenerator : ITokenGenerator
    {
        public string GetToken()
        {
            const string symbols = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            Random rand = new Random();
            int n = 8;

            var result = new char[n];
            while (n-- > 0)
            {
                result[n] = symbols[rand.Next(symbols.Length)];
            }
            return new string(result);
        }
    }
}
