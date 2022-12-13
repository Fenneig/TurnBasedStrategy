using System;

namespace Grid
{
    public struct GridPosition : IEquatable<GridPosition>
    {
        public int X;
        public int Z;

        public GridPosition(int x, int z)
        {
            X = x;
            Z = z;
        }

        public override string ToString()
        {
            return $"x: {X}; z: {Z}";
        }

        public static bool operator ==(GridPosition a, GridPosition b) => a.X == b.X && a.Z == b.Z;

        public static bool operator !=(GridPosition a, GridPosition b) => !(a == b);
        
        public bool Equals(GridPosition other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            return obj is GridPosition other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Z);
        }

        public static GridPosition operator +(GridPosition a, GridPosition b) => new(a.X + b.X, a.Z + b.Z);
        public static GridPosition operator -(GridPosition a, GridPosition b) => new(a.X - b.X, a.Z - b.Z);
    }
}