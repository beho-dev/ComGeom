using Microsoft.Xna.Framework;

namespace Triangulation.Tests;

public class PolygonTests
{
    public class TriangleTests
    {
        [Fact]
        public void TriangleArea_CalculatesCorrectly()
        {
            // Arrange
            Vector2 a = new(0, 0);
            Vector2 b = new(2, 0);
            Vector2 c = new(1, 2);

            // Act
            float area = Shapes.TriangleArea2(a, b, c);

            // Assert
            Assert.Equal(4, area);
        }
    }

    public class AreaTests
    {
        [Fact]
        public void Area_EquilateralTriangle_CalculatesCorrectly()
        {
            // Arrange
            float sideLength = 2f;
            var triangle = Polygon.EquilateralTriangle(Vector2.Zero, sideLength);
            float expectedArea = 2 * sideLength * sideLength * MathF.Sqrt(3) / 4f;

            // Act
            float area = triangle.Area2();

            // Assert
            Assert.Equal(expectedArea, area, precision: 4);
        }

        [Fact]
        public void Area_Square_CalculatesCorrectly()
        {
            // Arrange
            var square = Polygon.FromList(
                [new Vector2(0, 0), new Vector2(2, 0), new Vector2(2, 2), new Vector2(0, 2)]
            );
            float expectedArea = 2 * 4f; // 2x2 square

            // Act
            float area = square.Area2();

            // Assert
            Assert.Equal(expectedArea, area);
        }

        [Fact]
        public void Area_Triangle_MatchesTriangleAreaMethod()
        {
            // Arrange
            var triangle = Polygon.FromList(
                [new Vector2(0, 0), new Vector2(2, 0), new Vector2(1, 2)]
            );
            float expectedArea = Shapes.TriangleArea2(
                new Vector2(0, 0),
                new Vector2(2, 0),
                new Vector2(1, 2)
            );

            // Act
            float area = triangle.Area2();

            // Assert
            Assert.Equal(expectedArea, area);
        }
    }
}
