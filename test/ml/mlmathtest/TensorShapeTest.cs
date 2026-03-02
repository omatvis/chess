using mlmath.Tensor;

namespace mlmathtest;

public class TensorShapeTest
{
    [Fact]
    public void Shape_ReturnsExpected_Rank_ForValidShape()
    {
        // Arrange
        int[] input = [1, 3, 2, 2];
        int rank = 4;

        // Act
        Shape tensorShape = new(input);

        // Assert
        Assert.Equal(rank, tensorShape.Rank);
    }

    [Fact]
    public void Shape_ReturnsExpected_ElementCount_ForValidShape()
    {
        // Arrange
        int[] input = [1, 3, 2, 2];
        int elementCount = 12;

        // Act
        Shape tensorShape = new(input);

        // Assert
        Assert.Equal(elementCount, tensorShape.ElementCount);
    }

    [Fact]
    public void Shape_Constructor_ThrowsArgumentException_ForInvalidShape()
    {
        // Arrange
        int[] input = [1, -3, 2, 2];

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Shape(input));
    }

    [Fact]
    public void Shape_Constructor_HandlesNullInput()
    {
        // Arrange
        int[] input = null!;
        // Act
        Shape tensorShape = new(input);
        // Assert
        Assert.Equal(0, tensorShape.Rank);
        Assert.Equal(1, tensorShape.ElementCount);
    }


    [Theory]
    [InlineData(new int[] { 5 })]
    [InlineData(new int[] { 2, 3 })]
    [InlineData(new int[] { 1, 3, 2, 2 })]
    public void Shape_Constructor_CreatesShape_ForValidInput(int[] input)
    {
        // Act
        Shape tensorShape = new(input);
        // Assert
        Assert.NotNull(tensorShape);
        Assert.Equal(input.Length, tensorShape.Rank);
        Assert.Equal(input.Aggregate(1, (acc, dim) => acc * dim), tensorShape.ElementCount);
    }
}
