namespace Roy_T.AStar.Graphs;

public sealed class Edge : IEdge
{
    private float traversalVelocity;

    public Edge(INode start, INode end, float traversalVelocity)
    {
        this.Start = start;
        this.End = end;

        this.Distance = (start.Position - end.Position).Length();
        this.TraversalVelocity = traversalVelocity;
    }

    public float TraversalVelocity
    {
        get => this.traversalVelocity;
        set
        {
            this.traversalVelocity = value;
            this.TraversalDuration = this.Distance / value;
        }
    }

    public float TraversalDuration { get; private set; }

    public float Distance { get; }

    public INode Start { get; }
    public INode End { get; }

    public override string ToString() => $"{this.Start} -> {this.End} @ {this.TraversalVelocity}";
}
