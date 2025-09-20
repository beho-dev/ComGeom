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
            Point a = new(0, 0);
            Point b = new(2, 0);
            Point c = new(1, 2);

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
            int sideLength = 22;
            var triangle = Triangle.Equilateral(Point.Zero, sideLength);
            int expectedArea = 396; // Math.floor(2 * sideLength * sideLength * MathF.Sqrt(3) / 4f)

            // Act
            float area = triangle.Area2();

            // Assert
            Assert.Equal(expectedArea, area);
        }

        [Fact]
        public void Area_Square_CalculatesCorrectly()
        {
            // Arrange
            var square = Polygon.FromList(
                [new Point(0, 0), new Point(2, 0), new Point(2, 2), new Point(0, 2)]
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
            var triangle = Polygon.FromList([new Point(0, 0), new Point(2, 0), new Point(1, 2)]);
            float expectedArea = Shapes.TriangleArea2(
                new Point(0, 0),
                new Point(2, 0),
                new Point(1, 2)
            );

            // Act
            float area = triangle.Area2();

            // Assert
            Assert.Equal(expectedArea, area);
        }
    }
}
