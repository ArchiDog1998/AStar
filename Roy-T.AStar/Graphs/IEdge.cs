namespace Roy_T.AStar.Graphs;

public interface IEdge
{
    float TraversalVelocity { get; set; }
    float TraversalDuration { get; }
    float Distance { get; }
    INode Start { get; }
    INode End { get; }
}