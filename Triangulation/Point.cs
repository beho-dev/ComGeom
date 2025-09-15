namespace Triangulation;

// the triangulation course assumes that everything is in int space to avoid
// issues with floating point accuracy
public class Point(int X, int Y)
{
    public int X { get; private set; } = X;
    public int Y { get; private set; } = Y;
}
