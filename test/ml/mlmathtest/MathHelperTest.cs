using System;
using Xunit;
using mlmath;

namespace mlmathtest
{
    public class MathHelperTest
    {
        [Fact]
        public void ParseFloatList_ReturnsExpected_ForSpaceSeparated()
        {
            // Arrange
            string input = "1 2 3.5 4.25";
            float[] expected = new float[] { 1f, 2f, 3.5f, 4.25f };

            // Act
            float[] actual = MathHelper.ParseFloatList(input);

            // Assert
            Assert.Equal(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
                Assert.Equal(expected[i], actual[i], 5);
        }

        [Fact]
        public void ParseFloatList_ReturnsExpected_ForCommaSeparated()
        {
            // Arrange
            string input = "1,2,3.5,4.25";
            float[] expected = new float[] { 1f, 2f, 3.5f, 4.25f };

            // Act
            float[] actual = MathHelper.ParseFloatList(input);

            // Assert
            Assert.Equal(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
                Assert.Equal(expected[i], actual[i], 5);
        }

        [Fact]
        public void ParseFloatList_ReturnsExpected_ForMixedDelimitersAndWhitespace()
        {
            // Arrange
            string input = "  1,  2  ,3.5   4.25 , 5 ";
            float[] expected = new float[] { 1f, 2f, 3.5f, 4.25f, 5f };

            // Act
            float[] actual = MathHelper.ParseFloatList(input);

            // Assert
            Assert.Equal(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
                Assert.Equal(expected[i], actual[i], 5);
        }

        [Fact]
        public void ParseFloatList_HandlesNegativeAndDecimalValues()
        {
            // Arrange
            string input = "-1 -2.5,3.14159";
            float[] expected = new float[] { -1f, -2.5f, 3.14159f };

            // Act
            float[] actual = MathHelper.ParseFloatList(input);

            // Assert
            Assert.Equal(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
                Assert.Equal(expected[i], actual[i], 5);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void ParseFloatList_ThrowsArgumentException_OnNullOrWhitespace(string? input)
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => MathHelper.ParseFloatList(input));
            Assert.Equal("input", ex.ParamName);
        }

        [Fact]
        public void ParseFloatList_ThrowsFormatException_OnInvalidNumber()
        {
            // Arrange
            string input = "1 2 abc 4";

            // Act
            var ex = Assert.Throws<FormatException>(() => MathHelper.ParseFloatList(input));

            // Assert message includes the invalid token so it's clear which part failed
            Assert.Contains("Unable to convert", ex.Message, StringComparison.Ordinal);
            Assert.Contains("abc", ex.Message, StringComparison.Ordinal);
        }
    }
}
