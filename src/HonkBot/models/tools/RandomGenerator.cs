using System.Security;
using System.Security.Cryptography;

namespace HonkBot.Models.Tools;

/// <summary>
/// Houses methods related to random generators.
/// </summary>
public static class RandomGenerator
{
    /// <summary>
    /// Generate a random number within a specified range.
    /// </summary>
    /// <param name="minValue">The minimum value that can be returned. Defaults to '0'.</param>
    /// <param name="maxValue">The maximum value that can be returned. Defaults to '100'.</param>
    /// <returns>A random number within the specified range.</returns>
    public static int GetRandomNumber(int minValue = 0, int maxValue = 100)
    {
        byte[] randomNumBytes = new byte[4];

        using RandomNumberGenerator rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumBytes);

        int generatedNumber = Math.Abs(
            value: BitConverter.ToInt32(randomNumBytes, 0)
        );

        int minMaxDiff = maxValue + 1 - minValue;

        return minValue + (generatedNumber % minMaxDiff);
    }
}
