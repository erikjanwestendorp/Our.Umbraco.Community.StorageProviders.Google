namespace Our.Umbraco.Community.StorageProviders.GoogleCloud.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="string"/> class.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Removes the first character from the string if it matches the specified character.
    /// </summary>
    /// <param name="input">The input string to be checked and modified.</param>
    /// <param name="charToRemove">The character to remove if it is the first character of the string.</param>
    /// <returns>
    /// A new string with the first character removed if it matches <paramref name="charToRemove"/>;
    /// otherwise, the original string.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="input"/> is null.</exception>
    public static string RemoveFirstIfEquals(this string input, char charToRemove)
    {
        ArgumentNullException.ThrowIfNull(input, nameof(input));

        if (input.Length > 0 && input[0] == charToRemove)
        {
            input = input.Remove(0, 1);
        }

        return input;
    }
}
