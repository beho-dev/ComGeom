namespace Triangulation.PolygonPartitioning;

public class VertexTypeClassifier
{
    public static VertexType ClassifyVertex(VertexStructure vertex)
    {
        var current = vertex.Position;
        var previous = vertex.Previous.Position;
        var next = vertex.Next.Position;

        if (previous.Y < current.Y && next.Y < current.Y)
        {
            // both the previous and next are below the current vertex, then we are at a cusp
            if (Shapes.IsLeft(previous, current, next))
            {
                // we are following the polygon anti clockwise so this is an external cusp
                return VertexType.Start;
            }
            else
            {
                // this must be a cusp inside the polygon
                return VertexType.Split;
            }
        }
        else if (previous.Y > current.Y && next.Y > current.Y)
        {
            // both the previous and next are above the current vertex, then we are at a cusp
            if (Shapes.IsLeft(previous, current, next))
            {
                // we are following the polygon anti clockwise so this is an external cusp
                return VertexType.End;
            }
            else
            {
                // this must be a cusp inside the polygon
                return VertexType.Merge;
            }
        }
        else
        {
            // the previous and next are on either side of the current vertex, so we are a regular vertex
            return VertexType.Regular;
        }
    }
}