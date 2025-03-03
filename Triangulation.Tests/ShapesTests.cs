using Microsoft.Xna.Framework;

namespace Triangulation.Tests;

public class ShapesTests
{
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
        [InlineData(0, 0, 2, 2, -1, -1, false)] // Point outside on diagonal
        [InlineData(0, 0, 0, 2, 0, -1, false)] // Point outside on vertical
        [InlineData(0, 0, 0, 2, 0, 3, false)] // Point outside on vertical
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
        public void Intersects_LinesIntersect_ReturnsTrue()
        {
            // Arrange
            Vector2 a = new(0, 0);
            Vector2 b = new(2, 2);
            Vector2 c = new(0, 2);
            Vector2 d = new(2, 0);

            // Act
            bool result = Shapes.Intersects(a, b, c, d);

            // Assert
            Assert.True(result);
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

    public class AngleTests
    {
        [Fact]
        public void AngleIsConvex_ConvexAngle_ReturnsTrue()
        {
            // Arrange
            Vector2 a = new(0, 1);
            Vector2 b = new(0, 0);
            Vector2 c = new(1, 0); // This forms a 90-degree convex angle abc (counterclockwise from a to c)

            // Act
            bool result = Shapes.AngleIsConvex(a, b, c);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void AngleIsConvex_ReflexAngle_ReturnsFalse()
        {
            // Arrange
            Vector2 a = new(1, 0);
            Vector2 b = new(0, 0);
            Vector2 c = new(0, 1); // This forms a 270-degree reflex angle abc (counterclockwise from a to c)

            // Act
            bool result = Shapes.AngleIsConvex(a, b, c);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void AngleIsConvex_StraightAngle_ReturnsTrue()
        {
            // Arrange
            Vector2 a = new(-1, 0);
            Vector2 b = new(0, 0);
            Vector2 c = new(1, 0); // This forms a 180-degree straight angle abc

            // Act
            bool result = Shapes.AngleIsConvex(a, b, c);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(0, 1, 0, 0, 1, 0, 0.5f, 0.5f, true)] // Point inside 90-degree convex angle
        [InlineData(0, 1, 0, 0, 1, 0, -0.5f, -0.5f, false)] // Point outside 90-degree convex angle
        [InlineData(0, 1, 0, 0, 1, 0, 0, 0, false)] // Point at vertex b
        [InlineData(-1, 0, 0, 0, 1, 0, 0, 0.5f, true)] // Point inside 180-degree angle
        [InlineData(1, 0, 0, 0, 0, 1, 0.5f, 0.5f, false)] // Point outside reflex angle
        [InlineData(1, 0, 0, 0, 0, 1, -0.5f, -0.5f, true)] // Point inside reflex angle
        public void InAngle_ChecksPointPosition(
            float ax,
            float ay,
            float bx,
            float by,
            float cx,
            float cy,
            float px,
            float py,
            bool expected
        )
        {
            // Arrange
            Vector2 a = new(ax, ay);
            Vector2 b = new(bx, by);
            Vector2 c = new(cx, cy);
            Vector2 point = new(px, py);

            // Act
            bool result = Shapes.InAngle(a, b, c, point);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void InAngle_PointOnBoundary_ReturnsFalse()
        {
            // Arrange
            Vector2 a = new(0, 1);
            Vector2 b = new(0, 0);
            Vector2 c = new(1, 0); // Forms a convex 90-degree angle
            Vector2 point = new(0, 0.5f); // Point on ray from b to a

            // Act
            bool result = Shapes.InAngle(a, b, c, point);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void InAngle_PointAtVertex_ReturnsFalse()
        {
            // Arrange
            Vector2 a = new(0, 1);
            Vector2 b = new(0, 0);
            Vector2 c = new(1, 0); // Forms a convex 90-degree angle
            Vector2 point = b; // Point at vertex b

            // Act
            bool result = Shapes.InAngle(a, b, c, point);

            // Assert
            Assert.False(result);
        }
    }
}
