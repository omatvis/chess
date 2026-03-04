namespace mlmath.Tensor
{
    public class Shape
    {
        readonly int[] _dimensions;

        public int Rank => _dimensions.Length;
        public int ElementCount => _dimensions.Aggregate(1, (acc, dim) => acc * dim);

        public Shape(int[] dimensions) {
            _dimensions = dimensions is null ? Array.Empty<int>() : (int[])dimensions.Clone();
            if (_dimensions.Any<int>(dim => dim <= 0))
            {
                throw new ArgumentException("All dimensions must be positive integers.", nameof(dimensions));
            }
        }

        public int IndexNchw(int n, int c, int h, int w) {
            if (Rank != 4)
                throw new InvalidOperationException("Shape must have a rank of 4 for NCHW indexing.");
            return n * _dimensions[1] * _dimensions[2] * _dimensions[3] + c * _dimensions[2] * _dimensions[3] + h * _dimensions[3] + w;
        }

        public int IndexNhwc(int n, int h, int w, int c) {
            if (Rank != 4)
                throw new InvalidOperationException("Shape must have a rank of 4 for NHWC indexing.");
            return n * _dimensions[1] * _dimensions[2] * _dimensions[3] + h * _dimensions[2] * _dimensions[3] + w * _dimensions[3] + c;
        }
    }
}
