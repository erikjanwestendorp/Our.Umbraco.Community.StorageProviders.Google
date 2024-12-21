using Our.Umbraco.Community.StorageProviders.GoogleCloud.Extensions;

namespace Our.Umbraco.Community.StorageProviders.GoogleCloud.UnitTests.Extensions;

[TestFixture]
public class StringExtensionsTests
{
    [Test]
    public void RemoveFirstIfEquals_RemovesCharacter_WhenFirstCharacterMatches()
    {
        // Arrange
        const string input = "/umbraco";
        const char charToRemove = '/';

        // Act
        var result = input.RemoveFirstIfEquals(charToRemove);

        // Assert
        Assert.That(result, Is.EqualTo("umbraco"));
    }

    [Test]
    public void RemoveFirstIfEquals_DoesNotRemoveCharacter_WhenFirstCharacterDoesNotMatch()
    {
        // Arrange
        const string input = "uumbraco";
        const char charToRemove = 'u';

        // Act
        var result = input.RemoveFirstIfEquals(charToRemove);

        // Assert
        Assert.That(result, Is.EqualTo("umbraco"));
    }

    [Test]
    public void RemoveFirstIfEquals_ReturnsOriginalString_WhenStringIsEmpty()
    {
        // Arrange
        var input = string.Empty;
        const char charToRemove = 'a';

        // Act
        var result = input.RemoveFirstIfEquals(charToRemove);

        // Assert
        Assert.That(result, Is.EqualTo(string.Empty));
    }

    [Test]
    public void RemoveFirstIfEquals_ThrowsArgumentNullException_WhenInputIsNull()
    {
        // Arrange
        string input = null!;
        const char charToRemove = 'a';

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => input.RemoveFirstIfEquals(charToRemove));
    }

    [Test]
    public void RemoveFirstIfEquals_RemovesCharacter_WhenStringHasOnlyOneMatchingCharacter()
    {
        // Arrange
        const string input = "a";
        const char charToRemove = 'a';

        // Act
        var result = input.RemoveFirstIfEquals(charToRemove);

        // Assert
        Assert.That(result, Is.EqualTo(string.Empty));
    }

    [Test]
    public void RemoveFirstIfEquals_DoesNotRemoveCharacter_WhenStringHasOnlyOneNonMatchingCharacter()
    {
        // Arrange
        const string input = "b";
        const char charToRemove = 'a';

        // Act
        var result = input.RemoveFirstIfEquals(charToRemove);

        // Assert
        Assert.That(result, Is.EqualTo("b"));
    }

    [Test]
    public void RemoveFirstIfEquals_RemovesOnlyFirstOccurrence_WhenMultipleOccurrencesExist()
    {
        // Arrange
        const string input = "aaa";
        const char charToRemove = 'a';

        // Act
        var result = input.RemoveFirstIfEquals(charToRemove);

        // Assert
        Assert.That(result, Is.EqualTo("aa"));
    }
}
