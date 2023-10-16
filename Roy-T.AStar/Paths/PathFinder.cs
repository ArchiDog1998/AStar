using Roy_T.AStar.Collections;
using Roy_T.AStar.Graphs;
using System.Collections.Generic;

namespace Roy_T.AStar.Paths;

public sealed class PathFinder
{
    private readonly MinHeap<PathFinderNode> Interesting;
    private readonly Dictionary<INode, PathFinderNode> Nodes;
    private readonly PathReconstructor PathReconstructor;

    private PathFinderNode NodeClosestToGoal;

    public PathFinder()
    {
        this.Interesting = new MinHeap<PathFinderNode>();
        this.Nodes = new Dictionary<INode, PathFinderNode>();
        this.PathReconstructor = new PathReconstructor();
    }

    public Path FindPath(INode start, INode goal, float maximumVelocity)
    {
        this.ResetState();
        this.AddFirstNode(start, goal, maximumVelocity);

        while (this.Interesting.Count > 0)
        {
            var current = this.Interesting.Extract();
            if (GoalReached(goal, current))
            {
                return this.PathReconstructor.ConstructPathTo(current.Node, goal);
            }

            this.UpdateNodeClosestToGoal(current);

            foreach (var edge in current.Node.Outgoing)
            {
                var oppositeNode = edge.End;
                var costSoFar = current.DurationSoFar + edge.TraversalDuration;

                if (this.Nodes.TryGetValue(oppositeNode, out var node))
                {
                    this.UpdateExistingNode(goal, maximumVelocity, current, edge, oppositeNode, costSoFar, node);
                }
                else
                {
                    this.InsertNode(oppositeNode, edge, goal, costSoFar, maximumVelocity);
                }
            }
        }

        return this.PathReconstructor.ConstructPathTo(this.NodeClosestToGoal.Node, goal);
    }

    private void ResetState()
    {
        this.Interesting.Clear();
        this.Nodes.Clear();
        this.PathReconstructor.Clear();
        this.NodeClosestToGoal = null;
    }

    private void AddFirstNode(INode start, INode goal, float maximumVelocity)
    {
        var head = new PathFinderNode(start, 0, ExpectedDuration(start, goal, maximumVelocity));
        this.Interesting.Insert(head);
        this.Nodes.Add(head.Node, head);
        this.NodeClosestToGoal = head;
    }

    private static bool GoalReached(INode goal, PathFinderNode current) => current.Node == goal;

    private void UpdateNodeClosestToGoal(PathFinderNode current)
    {
        if (current.ExpectedRemainingTime < this.NodeClosestToGoal.ExpectedRemainingTime)
        {
            this.NodeClosestToGoal = current;
        }
    }

    private void UpdateExistingNode(INode goal, float maximumVelocity, PathFinderNode current, IEdge edge, INode oppositeNode, float costSoFar, PathFinderNode node)
    {
        if (node.DurationSoFar > costSoFar)
        {
            this.Interesting.Remove(node);
            this.InsertNode(oppositeNode, edge, goal, costSoFar, maximumVelocity);
        }
    }

    private void InsertNode(INode current, IEdge via, INode goal, float costSoFar, float maximumVelocity)
    {
        this.PathReconstructor.SetCameFrom(current, via);

        var node = new PathFinderNode(current, costSoFar, ExpectedDuration(current, goal, maximumVelocity));
        this.Interesting.Insert(node);
        this.Nodes[current] = node;
    }

    public static float ExpectedDuration(INode a, INode b, float maximumVelocity)
        => (a.Position - b.Position).Length() / maximumVelocity;
}
