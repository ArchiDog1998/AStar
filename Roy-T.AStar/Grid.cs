﻿using System;
using System.Collections.Generic;

namespace RoyT.AStar
{
    /// <summary>
    /// Representation of your world for the pathfinding algorithm.
    /// Use SetCellCost to give different costs to for traversing cells
    /// Use Block/UnblockCell to make the cell inaccessible for the 
    /// path finding algorithm
    /// </summary>
    public sealed class Grid
    {
        private readonly double DefaultCost;
        private readonly double[] Weights;

        /// <summary>
        /// Creates a grid
        /// </summary>
        /// <param name="dimX">The x-dimension of your world</param>
        /// <param name="dimY">The y-dimesion of your world</param>
        /// <param name="defaultCost">The default cost every cell is initialized with</param>
        public Grid(int dimX, int dimY, double defaultCost)
        {
            if (defaultCost <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    $"Argument {nameof(defaultCost)} with value {defaultCost} is invalid. The cost of traversing a cell cannot be less than, or equal to, zero");
            }

            this.DefaultCost = defaultCost;
            this.Weights = new double[dimX * dimY];
            this.DimX = dimX;
            this.DimY = dimY;

            for (var n = 0; n < this.Weights.Length; n++)
            {
                this.Weights[n] = defaultCost;
            }
        }

        public int DimX { get; }
        public int DimY { get; }

        /// <summary>
        /// Sets the cost for traversing a cell
        /// </summary>
        /// <param name="position">A position inside the grid</param>
        /// <param name="cost">The cost of traversing the cell, always larger than zero</param>
        public void SetCellCost(Position position, double cost)
        {
            if (cost < 0)
            {
                throw new ArgumentOutOfRangeException(
                    $"Argument {nameof(cost)} with value {cost} is invalid. The cost of traversing a cell cannot be less than, or equal to, zero");
            }

            this.Weights[GetIndex(position.X, position.Y)] = cost;
        }

        /// <summary>
        /// Makes the cell inaccessible
        /// </summary>
        /// <param name="position">A position inside the grid</param>
        public void BlockCell(Position position) => SetCellCost(position, double.PositiveInfinity);

        /// <summary>
        /// Makes the cell accessible, gives it the default traversal cost as provided in the constructor
        /// </summary>
        /// <param name="position">A position inside the grid</param>
        public void UnblockCell(Position position) => SetCellCost(position, this.DefaultCost);

        /// <summary>
        /// Looks-up the cost for traversing a given cell
        /// </summary>
        /// <param name="position">A position inside the grid</param>
        /// <returns>The cost</returns>
        public double GetCellCost(Position position)
        {
            return this.Weights[GetIndex(position.X, position.Y)];
        }

        /// <summary>
        /// Computs the path lowest-cost path from start to end inside the grid
        /// </summary>
        /// <param name="start">The start position</param>
        /// <param name="end">The end position</param>
        /// <returns>A list of positions inside the grid from start to end</returns>
        public IList<Position> GetPath(Position start, Position end)
        {
            var steps = new List<Position>();

            var path = PathFinder.FindPath(this, start, end);            

            var current = path;
            while (current != null)
            {
                steps.Add(current.Position);
                current = current.Next;
            }

            return steps;
        }

        private int GetIndex(int x, int y)
        {
            if (x < 0 || x >= this.DimX)
            {
                throw new ArgumentOutOfRangeException(
                    $"The x-coordinate {x} is outside of the expected range [0...{this.DimX})");
            }

            if (y < 0 || y >= this.DimY)
            {
                throw new ArgumentOutOfRangeException(
                    $"The y-coordinate {y} is outside of the expected range [0...{this.DimY})");
            }

            return this.DimX * y + x;
        }
    }    
}