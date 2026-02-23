namespace mlmath
{
    public static class MathHelper
    {
        public static float[] ParseFloatList(string input)
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
    }
}
