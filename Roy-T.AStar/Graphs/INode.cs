using System.Collections.Generic;
using System.Numerics;

namespace Roy_T.AStar.Graphs;

public interface INode
{
    Vector3 Position { get; }
    IList<IEdge> Incoming { get; }
    IList<IEdge> Outgoing { get; }

    void Connect(INode node, float traversalVelocity);

    void Disconnect(INode node);
}
