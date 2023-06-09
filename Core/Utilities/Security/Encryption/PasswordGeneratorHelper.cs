using System.Text;

namespace Core.Utilities.Security.Encryption
{
    public class PasswordGeneratorHelper
    {
        public static string CreatePassword(int length)
        {
            string[] validChars = { "abcdefghijklmnopqrstuvwxyz", "ABCDEFGHIJKLMNOPQRSTUVWXYZ", "1234567890" };


            StringBuilder password = new StringBuilder();

            Random rndIndex = new Random();
            Random rndCase = new Random();

            int caseIndex = 0;

            for (int i = 0; i < length; i++)
            {
                caseIndex = rndCase.Next(0, 3);
                password.Append(validChars[caseIndex][rndIndex.Next(0, validChars[caseIndex].Length)]);
            }
            return password.ToString();
        }
    }
}
