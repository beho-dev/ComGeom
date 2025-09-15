namespace Triangulation.PolygonPartitioning;

public interface ILineSweep
{
    void Sweep(float sweepY);

    /// <summary>
    /// Add both of the edges that are connected by the vertex.
    /// Assumes that the vertex is a of Start or Split type.
    /// If the vertex is a Split type then the returned vertex is the
    /// supporting vertex for the edge that is on the left of the vertex.
    /// </summary>
    /// <param name="vertex">The vertex to add the edges for</param>
    /// <returns>The supporting vertex for the edge that is on the left of the vertex</returns>
    VertexStructure? AddEdges(VertexStructure vertex);

    /// <summary>
    /// Remove both of the edges that are connected by the vertex.
    /// Assumes that the vertex is a of Merge or End type.
    /// </summary>
    /// <param name="vertex">The vertex to remove the edges for</param>
    /// <returns>The supporting vertex for a possible merge vertex above</returns>
    VertexStructure? RemoveEdges(VertexStructure vertex);

    /// <summary>
    /// Replace the edge that is connected by the vertex.
    /// Assumes that the vertex is a of Regular type.
    /// </summary>
    /// <param name="vertex">The vertex to replace the edge for</param>
    /// <returns>The supporting vertex for the edge that is on the left of the vertex</returns>
    VertexStructure? ReplaceEdge(VertexStructure vertex);
}
