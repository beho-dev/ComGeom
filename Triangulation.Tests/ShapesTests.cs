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
            Point a = new(0, 0);
            Point b = new(0, 2);
            Point c = new(-2, 1);

            // Act
            bool result = Shapes.IsLeft(a, b, c);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsLeft_PointIsOnRightSide_ReturnsFalse()
        {
            // Arrange
            Point a = new(0, 0);
            Point b = new(0, 2);
            Point c = new(2, 1);

            // Act
            bool result = Shapes.IsLeft(a, b, c);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsLeft_PointIsOnLine_ReturnsFalse()
        {
            // Arrange
            Point a = new(0, 0);
            Point b = new(0, 2);
            Point c = new(0, 1);

            // Act
            bool result = Shapes.IsLeft(a, b, c);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsLeftOrOn_PointIsOnLeftSide_ReturnsTrue()
        {
            // Arrange
            Point a = new(0, 0);
            Point b = new(0, 2);
            Point c = new(-2, 1);

            // Act
            bool result = Shapes.IsLeftOrOn(a, b, c);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsLeftOrOn_PointIsOnLine_ReturnsTrue()
        {
            // Arrange
            Point a = new(0, 0);
            Point b = new(0, 2);
            Point c = new(0, 1);

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
            Point a = new(0, 0);
            Point b = new(1, 1);
            Point c = new(2, 2);

            // Act
            bool result = Shapes.Collinear(a, b, c);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Collinear_PointsAreNotCollinear_ReturnsFalse()
        {
            // Arrange
            Point a = new(0, 0);
            Point b = new(1, 1);
            Point c = new(2, 3);

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
            int ax,
            int ay,
            int bx,
            int by,
            int cx,
            int cy,
            bool expected
        )
        {
            // Arrange
            Point a = new(ax, ay);
            Point b = new(bx, by);
            Point c = new(cx, cy);

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
            Point a = new(0, 0);
            Point b = new(2, 2);
            Point c = new(0, 2);
            Point d = new(2, 0);

            // Act
            bool result = Shapes.IntersectsProperly(a, b, c, d);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IntersectsProperly_LinesDoNotIntersect_ReturnsFalse()
        {
            // Arrange
            Point a = new(0, 0);
            Point b = new(1, 1);
            Point c = new(0, 2);
            Point d = new(1, 2);

            // Act
            bool result = Shapes.IntersectsProperly(a, b, c, d);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Intersects_LinesIntersect_ReturnsTrue()
        {
            // Arrange
            Point a = new(0, 0);
            Point b = new(2, 2);
            Point c = new(0, 2);
            Point d = new(2, 0);

            // Act
            bool result = Shapes.Intersects(a, b, c, d);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Intersects_LinesShareEndpoint_ReturnsTrue()
        {
            // Arrange
            Point a = new(0, 0);
            Point b = new(1, 1);
            Point c = new(1, 1);
            Point d = new(2, 0);

            // Act
            bool result = Shapes.Intersects(a, b, c, d);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Intersects_LinesDoNotIntersect_ReturnsFalse()
        {
            // Arrange
            Point a = new(0, 0);
            Point b = new(1, 0);
            Point c = new(0, 2);
            Point d = new(1, 2);

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
            Point a = new(0, 1);
            Point b = new(0, 0);
            Point c = new(1, 0); // This forms a 90-degree convex angle abc (counterclockwise from a to c)

            // Act
            bool result = Shapes.AngleIsConvex(a, b, c);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void AngleIsConvex_ReflexAngle_ReturnsFalse()
        {
            // Arrange
            Point a = new(1, 0);
            Point b = new(0, 0);
            Point c = new(0, 1); // This forms a 270-degree reflex angle abc (counterclockwise from a to c)

            // Act
            bool result = Shapes.AngleIsConvex(a, b, c);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void AngleIsConvex_StraightAngle_ReturnsTrue()
        {
            // Arrange
            Point a = new(-1, 0);
            Point b = new(0, 0);
            Point c = new(1, 0); // This forms a 180-degree straight angle abc

            // Act
            bool result = Shapes.AngleIsConvex(a, b, c);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(0, 2, 0, 0, 2, 0, 1, 1, true)] // Point inside 90-degree convex angle
        [InlineData(0, 2, 0, 0, 2, 0, -1, -1, false)] // Point outside 90-degree convex angle
        [InlineData(0, 2, 0, 0, 2, 0, 0, 0, false)] // Point at vertex b
        [InlineData(-2, 0, 0, 0, 2, 0, 0, 1, true)] // Point inside 180-degree angle
        [InlineData(2, 0, 0, 0, 0, 2, 1, 1, false)] // Point outside reflex angle
        [InlineData(2, 0, 0, 0, 0, 2, -1, -1, true)] // Point inside reflex angle
        public void InAngle_ChecksPointPosition(
            int ax,
            int ay,
            int bx,
            int by,
            int cx,
            int cy,
            int px,
            int py,
            bool expected
        )
        {
            // Arrange
            Point a = new(ax, ay);
            Point b = new(bx, by);
            Point c = new(cx, cy);
            Point point = new(px, py);

            // Act
            bool result = Shapes.InAngle(a, b, c, point);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void InAngle_PointOnBoundary_ReturnsFalse()
        {
            // Arrange
            Point a = new(0, 2);
            Point b = new(0, 0);
            Point c = new(2, 0); // Forms a convex 90-degree angle
            Point point = new(0, 1); // Point on ray from b to a

            // Act
            bool result = Shapes.InAngle(a, b, c, point);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void InAngle_PointAtVertex_ReturnsFalse()
        {
            // Arrange
            Point a = new(0, 1);
            Point b = new(0, 0);
            Point c = new(1, 0); // Forms a convex 90-degree angle
            Point point = b; // Point at vertex b

            // Act
            bool result = Shapes.InAngle(a, b, c, point);

            // Assert
            Assert.False(result);
        }
    }
}
