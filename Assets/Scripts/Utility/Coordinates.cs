using System;
using UnityEngine;

namespace Utility
{
    public struct Coordinates
    {
        public readonly int X;
        public readonly int Y;

        public Coordinates(int row, int column)
        {
            X = row;
            Y = column;
        }


        // public bool NextTo(Coordinates other)
        // {
        //     bool result = Math.Abs(other.Row - Row) == 1 && other.Column == Column ||
        //                   Math.Abs(other.Column - Column) == 1 && other.Row == Row;
        //     return result;
        // }

        public bool NextTo(Coordinates other, int distance = 1)
        {
            bool result = false;
            for (int i = 0; i < distance; i++)
            {
                if (Math.Abs(other.X - X) == i && Math.Abs(other.Y - Y) == distance - i ||
                    Math.Abs(other.Y - Y) == i && Math.Abs(other.X - X) == distance - i)
                    result = true;
            }
            return result;
        }

        public Vector3 GetVector3(int z=0)
        {
            return new Vector3(X, Y, z);
        }

        public override string ToString()
        {
            return "[" + X + ", " + Y +"]";
        }

        public int Distance(Coordinates other)
        {
            return Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
        }
    }
}