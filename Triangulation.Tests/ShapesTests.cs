using Microsoft.Xna.Framework;

namespace Triangulation.Tests;

public class ShapesTests
{
    public class TriangleTests
    {
        [Fact]
        public void EquilateralTriangle_CreatesCorrectVertices()
        {
            // Arrange
            Vector2 center = new(0, 0);
            float sideLength = 2f;
            float expectedHeight = sideLength * MathF.Sqrt(3) / 2f;

            // Act
            var triangle = Shapes.EquilateralTriangle(center, sideLength);

            var current = triangle;
            var vertices = new List<Vector2>();
            do
            {
                vertices.Add(current.Position);
                current = current.Next;
            } while (current != triangle);

            // Assert
            Assert.Equal(3, vertices.Count);
            Assert.Equal(new Vector2(0, -expectedHeight * 2 / 3f), vertices[0]);
            Assert.Equal(new Vector2(sideLength / 2f, expectedHeight / 3f), vertices[1]);
            Assert.Equal(new Vector2(-sideLength / 2f, expectedHeight / 3f), vertices[2]);
        }

        [Fact]
        public void TriangleArea_CalculatesCorrectly()
        {
            // Arrange
            Vector2 a = new(0, 0);
            Vector2 b = new(2, 0);
            Vector2 c = new(1, 2);

            // Act
            float area = Shapes.TriangleArea(a, b, c);

            // Assert
            Assert.Equal(4, area);
        }
    }

    public class LeftSideTests
    {
        [Fact]
        public void IsLeft_PointIsOnLeftSide_ReturnsTrue()
        {
            // Arrange
            Vector2 a = new(0, 0);
            Vector2 b = new(0, 1);
            Vector2 c = new(-1, 0.5f);

            // Act
            bool result = Shapes.IsLeft(a, b, c);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsLeft_PointIsOnRightSide_ReturnsFalse()
        {
            // Arrange
            Vector2 a = new(0, 0);
            Vector2 b = new(0, 1);
            Vector2 c = new(1, 0.5f);

            // Act
            bool result = Shapes.IsLeft(a, b, c);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsLeft_PointIsOnLine_ReturnsFalse()
        {
            // Arrange
            Vector2 a = new(0, 0);
            Vector2 b = new(0, 1);
            Vector2 c = new(0, 0.5f);

            // Act
            bool result = Shapes.IsLeft(a, b, c);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsLeftOrOn_PointIsOnLeftSide_ReturnsTrue()
        {
            // Arrange
            Vector2 a = new(0, 0);
            Vector2 b = new(0, 1);
            Vector2 c = new(-1, 0.5f);

            // Act
            bool result = Shapes.IsLeftOrOn(a, b, c);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsLeftOrOn_PointIsOnLine_ReturnsTrue()
        {
            // Arrange
            Vector2 a = new(0, 0);
            Vector2 b = new(0, 1);
            Vector2 c = new(0, 0.5f);

            // Act
            bool result = Shapes.IsLeftOrOn(a, b, c);

            // Assert
            Assert.True(result);
        }
    }

    public class CollinearityTests
    {
        [Fact]
        public void Collinear_PointsAreCollinear_ReturnsTrue()
        {
            // Arrange
            Vector2 a = new(0, 0);
            Vector2 b = new(1, 1);
            Vector2 c = new(2, 2);

            // Act
            bool result = Shapes.Collinear(a, b, c);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Collinear_PointsAreNotCollinear_ReturnsFalse()
        {
            // Arrange
            Vector2 a = new(0, 0);
            Vector2 b = new(1, 1);
            Vector2 c = new(2, 3);

            // Act
            bool result = Shapes.Collinear(a, b, c);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(0, 0, 2, 2, 1, 1, true)] // Point between on diagonal
        [InlineData(0, 0, 2, 0, 1, 0, true)] // Point between on horizontal
        [InlineData(0, 0, 0, 2, 0, 1, true)] // Point between on vertical
        [InlineData(0, 0, 2, 2, 3, 3, false)] // Point outside on diagonal
        public void Between_ChecksPointPosition(
            float ax,
            float ay,
            float bx,
            float by,
            float cx,
            float cy,
            bool expected
        )
        {
            // Arrange
            Vector2 a = new(ax, ay);
            Vector2 b = new(bx, by);
            Vector2 c = new(cx, cy);

            // Act
            bool result = Shapes.Between(a, b, c);

            // Assert
            Assert.Equal(expected, result);
        }
    }

    public class IntersectionTests
    {
        [Fact]
        public void IntersectsProperly_LinesIntersect_ReturnsTrue()
        {
            // Arrange
            Vector2 a = new(0, 0);
            Vector2 b = new(2, 2);
            Vector2 c = new(0, 2);
            Vector2 d = new(2, 0);

            // Act
            bool result = Shapes.IntersectsProperly(a, b, c, d);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IntersectsProperly_LinesDoNotIntersect_ReturnsFalse()
        {
            // Arrange
            Vector2 a = new(0, 0);
            Vector2 b = new(1, 1);
            Vector2 c = new(0, 2);
            Vector2 d = new(1, 2);

            // Act
            bool result = Shapes.IntersectsProperly(a, b, c, d);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Intersects_LinesShareEndpoint_ReturnsTrue()
        {
            // Arrange
            Vector2 a = new(0, 0);
            Vector2 b = new(1, 1);
            Vector2 c = new(1, 1);
            Vector2 d = new(2, 0);

            // Act
            bool result = Shapes.Intersects(a, b, c, d);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Intersects_LinesDoNotIntersect_ReturnsFalse()
        {
            // Arrange
            Vector2 a = new(0, 0);
            Vector2 b = new(1, 0);
            Vector2 c = new(0, 2);
            Vector2 d = new(1, 2);

            // Act
            bool result = Shapes.Intersects(a, b, c, d);

            // Assert
            Assert.False(result);
        }
    }
}
