using System;
using System.Linq;
using Xunit;
using mlmath.Helper;

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

        // --- Tests for NormalizeWithSoftmax ---

        [Fact]
        public void NormalizeWithSoftmax_ProducesProbabilities_ThatSumToOne_AndAreBetweenZeroAndOne()
        {
            // Arrange
            float[] logits = new float[] { 0f, 1f, 2f };
            int expectedArgMax = Array.IndexOf(logits, logits.Max());

            // Act
            float[] result = MathHelper.NormalizeWithSoftmax(logits);

            // Assert: reference returned and mutated in-place
            Assert.Same(logits, result);

            // Probabilities between 0 and 1 and sum to 1
            float sum = result.Sum();
            Assert.Equal(1f, sum, 5);

            foreach (var p in result)
                Assert.InRange(p, 0f, 1f);

            // Ordering preserved (argmax)
            int actualArgMax = Array.IndexOf(result, result.Max());
            Assert.Equal(expectedArgMax, actualArgMax);
        }

        [Fact]
        public void NormalizeWithSoftmax_IsNumericallyStable_WithLargeLogitValues()
        {
            // Arrange
            float[] logits = new float[] { 1000f, 1001f, 1002f };
            int expectedArgMax = Array.IndexOf(logits, logits.Max());

            // Act
            float[] result = MathHelper.NormalizeWithSoftmax(logits);

            // Assert: sum ~= 1 and argmax preserved
            Assert.Equal(1f, result.Sum(), 5);
            int actualArgMax = Array.IndexOf(result, result.Max());
            Assert.Equal(expectedArgMax, actualArgMax);

            // Ensure no NaN or Infinity
            foreach (var p in result)
            {
                Assert.False(float.IsNaN(p));
                Assert.False(float.IsInfinity(p));
            }
        }

        [Fact]
        public void NormalizeWithSoftmax_ReturnsUniformDistribution_ForEqualLogits()
        {
            // Arrange
            float[] logits = new float[] { 5f, 5f, 5f, 5f };

            // Act
            float[] result = MathHelper.NormalizeWithSoftmax(logits);

            // Assert: each probability ~= 0.25
            Assert.Equal(1f, result.Sum(), 5);
            foreach (var p in result)
                Assert.Equal(0.25f, p, 5);
        }

        [Fact]
        public void NormalizeWithSoftmax_ReturnsOne_ForSingleElement()
        {
            // Arrange
            float[] logits = new float[] { 42f };

            // Act
            float[] result = MathHelper.NormalizeWithSoftmax(logits);

            // Assert
            Assert.Single(result);
            Assert.Equal(1f, result[0], 5);
        }

        // --- Tests for TopKStrict and TopKClamp (selected code) ---

        [Fact]
        public void TopKStrict_Throws_WhenScoreGreaterThanLength()
        {
            // Arrange
            float[] logits = new float[] { 1f, 2f, 3f };

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => MathHelper.TopKStrict(logits, 4f));
        }

        [Theory]
        [InlineData(0f)]
        [InlineData(-1f)]
        public void TopKStrict_Throws_OnNonPositiveScore(float score)
        {
            // Arrange
            float[] logits = new float[] { 1f, 2f, 3f };

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => MathHelper.TopKStrict(logits, score));
        }

        [Fact]
        public void TopKStrict_ReturnsTopK_ByFrequencyThenByValue()
        {
            // Arrange - example from comment in source
            float[] logits = new float[] { 3f, 1f, 4f, 4f, 5f, 2f, 6f, 1f };
            float score = 2f;
            float[] expected = new float[] { 4f, 1f };

            // Act
            float[] actual = MathHelper.TopKStrict(logits, score);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TopKClamp_ReturnsOriginalLogits_WhenScoreGreaterThanLength()
        {
            // Arrange
            float[] logits = new float[] { 1f, 2f, 3f };
            float score = 5f;

            // Act
            float[] actual = MathHelper.TopKClamp(logits, score);

            // Current implementation returns the original logits array when score > logits.Length.
            // Assert that behavior is preserved (documenting current behavior).
            Assert.Same(logits, actual);
        }

        [Theory]
        [InlineData(0f)]
        [InlineData(-2f)]
        public void TopKClamp_Throws_OnNonPositiveScore(float score)
        {
            // Arrange
            float[] logits = new float[] { 1f, 2f, 2f };

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => MathHelper.TopKClamp(logits, score));
        }

        [Fact]
        public void TopKClamp_ReturnsTopK_ByFrequencyThenByValue()
        {
            // Arrange - same example as TopKStrict
            float[] logits = new float[] { 3f, 1f, 4f, 4f, 5f, 2f, 6f, 1f };
            float score = 2f;
            float[] expected = new float[] { 4f, 1f };

            // Act
            float[] actual = MathHelper.TopKClamp(logits, score);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
