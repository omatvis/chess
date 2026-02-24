namespace mlmath
{
    public static class MathHelper
    {
        /// <summary>
        /// Parses a string containing float values separated by spaces and/or commas
        /// into an array of <see cref="float"/> values.
        /// </summary>
        /// <param name="input">
        /// The input string containing numeric tokens. Tokens may be separated by one or more
        /// spaces and/or commas. Leading and trailing whitespace around tokens is ignored.
        /// </param>
        /// <returns>
        /// An array of parsed <see cref="float"/> values in the same order they appear in the input.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="input"/> is <c>null</c>, empty, or consists only of whitespace.
        /// </exception>
        /// <exception cref="FormatException">
        /// Thrown when any token cannot be converted to a <see cref="float"/>.
        /// The exception message includes the offending token.
        /// </exception>
        /// <example>
        /// var values = ParseFloatList("1.0, 2 3.5"); // returns new float[] { 1.0f, 2.0f, 3.5f }
        /// </example>
        public static float[] ParseFloatList(string? input)
        {
            if (string.IsNullOrWhiteSpace(input) == true)
                throw new ArgumentException("Input string cannot be null or whitespace.", nameof(input));

            string[] parts = input.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            float[] floats = new float[parts.Length];
            for (int i = 0; i < parts.Length; i++)
            {
                bool isConverted = float.TryParse(parts[i].Trim(), out floats[i]);
                if (!isConverted)
                    throw new FormatException($"Unable to convert '{parts[i]}' to a float.");
            }
            return floats;
        }

        /// <summary>
        /// Converts logits into probabilities using the Softmax function.
        /// </summary>
        /// <param name="logits">An array of logit values to be normalized.</param>
        /// <returns>
        /// An array of probabilities where each element p[i] = exp(logit[i]) / sum_j(exp(logit[j])).
        /// </returns>
        /// <remarks>
        /// The Softmax function transforms logits into a probability distribution with the following properties:
        /// <list type="bullet">
        /// <item><description>Each probability value is bounded: 0 &lt; p[i] &lt; 1</description></item>
        /// <item><description>All probabilities sum to 1: sum(p) = 1</description></item>
        /// <item><description>Preserves ordering: ArgMax(logits) == ArgMax(p)</description></item>
        /// </list>
        /// </remarks>
        public static float[] NormalizeWithSoftmax(float[] logits)
        {
            float maxLogit = logits.Max();

            for (int i = 0; i < logits.Length; i++)
                logits[i] = (float)Math.Exp(logits[i] - maxLogit);

            float sumLogits = logits.Sum();
            for (int i = 0; i < logits.Length; i++)
                logits[i] /= sumLogits;
            return logits;
        }
    }
}
