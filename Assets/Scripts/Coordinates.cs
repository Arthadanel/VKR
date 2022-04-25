using System;

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
            if (Math.Abs(other.Row - Row) == i && (other.Column - Column) == distance - i ||
                Math.Abs(other.Column - Column) == i && (other.Row - Row) == distance - i)
                result = true;
        }
        return result;
    }
}