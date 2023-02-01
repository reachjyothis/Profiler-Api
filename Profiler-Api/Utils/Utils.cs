using System.Security.Cryptography;
using System.Text;

namespace Profiler_Api.Utils;

public static class Utils
{
    public static string HashPassword(string password, string salt, bool generateSalt, out string generatedSalt)
    {
        const int keySize = 64;
        const int iterations = 350000;
        HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

        byte[] saltBytes;

        if (generateSalt)
        {
            saltBytes = RandomNumberGenerator.GetBytes(keySize);
            generatedSalt = Convert.ToHexString(saltBytes);
        }
        else
        {
            saltBytes = StringToByteArray(salt);
            generatedSalt = Convert.ToHexString(saltBytes);
        }

        var hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            saltBytes,
            iterations,
            hashAlgorithm,
            keySize);

        return Convert.ToHexString(hash);
    }

    public static byte[] StringToByteArray(string hex)
    {
        return Enumerable.Range(0, hex.Length)
                         .Where(x => x % 2 == 0)
                         .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                         .ToArray();
    }
}
