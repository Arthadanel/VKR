﻿using System;
using UnityEngine;

namespace Utility
{
    public struct Coordinates
    {
        public readonly int Row;
        public readonly int Column;

        public Coordinates(int row, int column)
        {
            Row = row;
            Column = column;
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
                if (Math.Abs(other.Row - Row) == i && Math.Abs(other.Column - Column) == distance - i ||
                    Math.Abs(other.Column - Column) == i && Math.Abs(other.Row - Row) == distance - i)
                    result = true;
            }
            return result;
        }

        public Vector3 GetVector3(int z=0)
        {
            return new Vector3(Row, Column, z);
        }

        public override string ToString()
        {
            return "row: " + Row + "\tcolumn: " + Column;
        }

        public int Distance(Coordinates other)
        {
            return Math.Abs(Row - other.Row) + Math.Abs(Column - other.Column);
        }
    }
}