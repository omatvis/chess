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

    [Theory]
    [InlineData(new int[] { 1, 3, 2, 2 })]
    [InlineData(new int[] { 3, 5, 1, 2 })]
    [InlineData(new int[] { 8, 3, 1, 2 })]
    [InlineData(new int[] { 2, 2, 2, 2 })]
    public void Shape_IndexNchw_ReturnsExpectedIndex_ForValidInput(int[] input)
    {
        // Arrange
        Shape tensorShape = new(input);

        // Act
        int index = tensorShape.IndexNchw(1, 2, 1, 2);

        // Assert
        int expectedIndex = 1 * (input[1] * input[2] * input[3]) + 2 * (input[2] * input[3]) + 1 * input[3] + 2;
        Assert.Equal(expectedIndex, index);

    }

    [Theory]
    [InlineData(new int[] { 1, 3, 2, 2 })]
    [InlineData(new int[] { 3, 5, 1, 2 })]
    [InlineData(new int[] { 8, 3, 1, 2 })]
    [InlineData(new int[] { 2, 2, 2, 2 })]
    public void Shape_IndexNhwc_ReturnsExpectedIndex_ForValidInput(int[] input)
    {
        // Arrange        
        Shape tensorShape = new(input);

        //Act
        int index = tensorShape.IndexNhwc(1, 1, 2, 2);

        //Assert
        int expectedIndex = 1 * (input[1] * input[2] * input[3]) + 1 * (input[2] * input[3]) + 2 * input[3] + 2;
        Assert.Equal(expectedIndex, index);
    }

    [Fact]
    public void Shape_IndexNchw_ThrowsInvalidOperationException_ForInvalidRank()
    {
        Shape tensorShape = new(new int[] { 1, 3, 2 });
        Assert.Throws<InvalidOperationException>(() => tensorShape.IndexNchw(0, 0, 0, 0));
    }

    [Fact]
    public void Shape_IndexNhwc_ThrowsInvalidOperationException_ForInvalidRank()
    {   
        Shape tensorShape = new(new int[] { 1, 3, 2 });
        Assert.Throws<InvalidOperationException>(() => tensorShape.IndexNhwc(0, 0, 0, 0));
    }
}
