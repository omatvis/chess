namespace mlmath.Tensor
{
    public class Shape
    {
        int[] _dimensions;

        public int Rank => _dimensions.Length;
        public int ElementCount => _dimensions.Aggregate(1, (acc, dim) => acc * dim);

        public Shape(int[] dimensions) {
            _dimensions = dimensions is null ? Array.Empty<int>() : (int[])dimensions.Clone();
            if (_dimensions.Any<int>(dim => dim <= 0))
            {
                throw new ArgumentException("All dimensions must be positive integers.", nameof(dimensions));
            }
        }
    }
}
