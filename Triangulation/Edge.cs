namespace Triangulation;

// an edge is undirected so the which is from and which is to does not matter
// we use a generic so that we can define for points and for vertex structures
public class Edge<T>(T from, T to)
{
    public T From { get; private set; } = from;
    public T To { get; private set; } = to;

    // override object.Equals
    public override bool Equals(object? obj)
    {
        //
        // See the full list of guidelines at
        //   http://go.microsoft.com/fwlink/?LinkID=85237
        // and also the guidance for operator== at
        //   http://go.microsoft.com/fwlink/?LinkId=85238
        //
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Edge<T> other = (Edge<T>)obj;
        if ((From == null) || (To == null))
        {
            throw new System.Exception("Unexpectedly null From or To field in edge");
        }

        // we are undirected so either the they match in the same direction or the opposite direction
        return From.Equals(other.From) && To.Equals(other.To)
            || From.Equals(other.To) && To.Equals(other.From);
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
        return System.HashCode.Combine(From, To);
    }

    public override string ToString()
    {
        return $"Edge({From}->{To})";
    }
}
