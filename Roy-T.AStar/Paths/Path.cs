using Roy_T.AStar.Graphs;
using System.Collections.Generic;

namespace Roy_T.AStar.Paths;

public sealed class Path
{
    public Path(PathType type, IReadOnlyList<IEdge> edges)
    {
        this.Type = type;
        this.Edges = edges;

        for (var i = 0; i < this.Edges.Count; i++)
        {
            this.Duration += this.Edges[i].TraversalDuration;
            this.Distance += this.Edges[i].Distance;
        }
    }

    public PathType Type { get; }

    public float Duration { get; }

    public IReadOnlyList<IEdge> Edges { get; }
    public float Distance { get; }
}
