﻿using Roy_T.AStar.Graphs;
using System;

namespace Roy_T.AStar.Paths;

internal sealed class PathFinderNode : IComparable<PathFinderNode>
{
    public PathFinderNode(INode node, float durationSoFar, float expectedRemainingTime)
    {
        this.Node = node;
        this.DurationSoFar = durationSoFar;
        this.ExpectedRemainingTime = expectedRemainingTime;
        this.ExpectedTotalTime = this.DurationSoFar + this.ExpectedRemainingTime;
    }

    public INode Node { get; }
    public float DurationSoFar { get; }
    public float ExpectedRemainingTime { get; }
    public float ExpectedTotalTime { get; }

    public int CompareTo(PathFinderNode other) => this.ExpectedTotalTime.CompareTo(other.ExpectedTotalTime);
    public override string ToString() => $"📍{{{this.Node.Position.X}, {this.Node.Position.Y}}}, ⏱~{this.ExpectedTotalTime}";
}
