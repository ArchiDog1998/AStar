using Roy_T.AStar.Graphs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Roy_T.AStar.Serialization;

public struct GraphDto
{
    public struct EdgeDto
    {
        public int Start { get; set; }
        public int End { get; set; }
        public float Velocity { get; set; }
    }

    public Vector3[] Nodes { get; set; }

    public EdgeDto[] Edges { get; set; }

    public GraphDto(params INode[] nodes)
    {
        IEnumerable<INode> relatedNodes = Array.Empty<INode>();
        IEnumerable<IEdge> edges = Array.Empty<IEdge>();
        foreach (var node in nodes)
        {
            AddOneNode(node, ref relatedNodes, ref edges);
        }

        var hashedNodes = new HashSet<INode>(relatedNodes).ToArray();
        var hashedEdges = new HashSet<IEdge>(edges);

        Nodes = hashedNodes.Select(n => n.Position).ToArray();
        Edges = hashedEdges.Select(e => new EdgeDto()
        {
            Start = Array.IndexOf(hashedNodes, e.Start),
            End = Array.IndexOf(hashedNodes, e.End),
            Velocity = e.TraversalVelocity,
        }).ToArray();

        static void AddOneNode(INode node, ref IEnumerable<INode> relatedNodes, ref IEnumerable<IEdge> edges)
        {
            if (relatedNodes.Contains(node)) return;

            relatedNodes = relatedNodes.Append(node);
            GetRelatedNodes(node, out var subNodes, out var subEdges);
            edges = edges.Concat(subEdges);
            foreach (var subNode in subNodes)
            {
                AddOneNode(subNode, ref relatedNodes, ref edges);
            }

            static void GetRelatedNodes(INode node, out IEnumerable<INode> subNodes, out IEnumerable<IEdge> edges)
            {
                subNodes = node.Outgoing.Select(e => e.End).Concat(node.Incoming.Select(e => e.Start));
                edges = node.Outgoing.Concat(node.Incoming);
            }
        }
    }

    public readonly INode[] ToNodes()
    {
        var nodes = Nodes.Select(n => new Node(n)).ToArray();

        foreach (var edge in Edges)
        {
            var node = nodes[edge.Start];
            node.Connect(nodes[edge.End], edge.Velocity);
        }

        return nodes;
    }
}