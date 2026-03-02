using System.Diagnostics;
using System.Runtime.Intrinsics.X86;

namespace mlmath.Helper
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

        public static int ArgMax(float[] logits) {
            return Array.IndexOf(logits, logits.Max());
        }

        /// <summary>
        /// Returns the top-`score` distinct values from the `logits` array ordered by frequency
        /// (most frequent first) and by numeric value (descending) for ties.
        /// </summary>
        /// <param name="logits">
        /// Array of float values to evaluate. The method counts occurrences of each distinct value.
        /// This parameter must not be <c>null</c>; a <see cref="NullReferenceException"/> will occur otherwise.
        /// </param>
        /// <param name="score">
        /// The requested number of top distinct values to return. Treated as a count; fractional parts
        /// are discarded when casting to <see cref="int"/>. If <paramref name="score"/> is greater than
        /// <c>logits.Length</c> the method implements a "clamp" behavior and returns the original
        /// <paramref name="logits"/> array unchanged. Must be greater than 0.
        /// </param>
        /// <returns>
        /// An array of distinct float values ordered first by occurrence count (descending) and then
        /// by numeric value (descending) when counts are equal. The returned length is at most
        /// <c>Math.Min((int)score, numberOfDistinctValues)</c>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="score"/> &lt;= 0.
        /// </exception>
        /// <remarks>
        /// - The implementation builds a frequency dictionary of distinct values from <paramref name="logits"/>.
        /// - Duplicate values are collapsed because the dictionary keys represent distinct values.
        /// - Ordering is done by frequency (descending), then by value (descending) for ties.
        /// - Callers should validate <paramref name="logits"/> if they wish to avoid a <see cref="NullReferenceException"/>.
        /// - Because <paramref name="score"/> is a <see cref="float"/>, fractional parts are discarded by the cast to <see cref="int"/>.
        /// - This method intentionally returns the original <paramref name="logits"/> when <paramref name="score"/> &gt; <c>logits.Length</c>
        ///   to provide a clamped behavior rather than throwing.
        /// </remarks>
        /// <example>
        /// var logits = new float[] { 1f, 2f, 2f, 3f }; // counts: 2->2, 3->1, 1->1
        /// var top2 = MathHelper.TopKClamp(logits, 2); // returns new float[] { 2f, 3f }
        /// </example>
        public static float[] TopKClamp(float[] logits, float score)
        {

            if (score > logits.Length) return logits;
            if (score <= 0) 
                throw new ArgumentOutOfRangeException(
                    nameof(score), 
                    "Score must be greater than 0 and less than or equal to the length of logits.");
            Dictionary<float, float> occurances = new Dictionary<float, float>();
            foreach (float element in logits)
            {
                if (occurances.ContainsKey(element))
                    occurances[element]++;
                else
                    occurances[element] = 1;
            }

            return occurances.OrderByDescending(kv => kv.Value)
                .ThenByDescending(kv => kv.Key)
                .Take((int)score)
                .Select(kv => kv.Key)
                .ToArray();
        }

        /// <summary>
        /// Returns the top distinct values from <paramref name="logits"/> ordered by occurrence count (descending)
        /// and by numeric value (descending) when counts are equal.
        /// </summary>
        /// <param name="logits">Array of float values to evaluate. Each distinct value's frequency is counted.</param>
        /// <param name="score">
        /// The number of top distinct values to return. The value is treated as a count; fractional parts are discarded
        /// when casting to <see cref="int"/>. Must be greater than 0 and less than or equal to <c>logits.Length</c>.
        /// </param>
        /// <returns>
        /// An array containing up to <c>(int)score</c> distinct float values selected from <paramref name="logits"/>,
        /// ordered first by frequency (most frequent first) and then by value (larger values first) for ties.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="score"/> &lt;= 0 or when <paramref name="score"/> &gt; <c>logits.Length</c>.
        /// </exception>
        /// <remarks>
        /// - Duplicate values in <paramref name="logits"/> are collapsed because selection is performed over distinct keys.
        /// - If <paramref name="logits"/> is <c>null</c> a <see cref="NullReferenceException"/> will be thrown by the method.
        /// - The method returns at most <c>Math.Min((int)score, numberOfDistinctValues)</c> elements.
        /// </remarks>
        /// <example>
        /// var logits = new float[] { 1f, 2f, 2f, 3f }; // counts: 2->2, 3->1, 1->1
        /// var top2 = MathHelper.TopKStrict(logits, 2); // returns new float[] { 2f, 3f }
        /// </example>
        public static float[] TopKStrict(float[] logits, float score)
        {
            if (score > logits.Length) 
                throw new ArgumentOutOfRangeException(
                    nameof(score), 
                    "Score must be less than or equal to the length of logits.");
            if (score <= 0) throw new ArgumentOutOfRangeException(
                nameof(score), 
                "Score must be greater than 0.");
            Dictionary<float, float> occurances = new Dictionary<float, float>();
            foreach (float element in logits)
            {
                if (occurances.ContainsKey(element))
                    occurances[element]++;
                else
                    occurances[element] = 1;
            }
            return occurances.OrderByDescending(kv => kv.Value)
                .ThenByDescending(kv => kv.Key)
                .Take((int)score)
                .Select(kv => kv.Key)
                .ToArray();
        }

    }
}
