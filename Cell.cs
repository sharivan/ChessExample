using System;
using System.Collections.Generic;
using System.Text;

namespace ChessExample
{
    public struct Cell
    {
        public static readonly Cell INVALID_CELL = new Cell(-1, -1);

        private int row;
        private int col;

        public int Row
        {
            get
            {
                return row;
            }
        }

        public int Col
        {
            get
            {
                return col;
            }
        }

        public bool Valid 
        {
            get
            {
                return row >= 0 && row < 8 && col >= 0 && col < 8;
            }
        }

        public Cell(int row, int col)
        {
            this.row = row;
            this.col = col;
        }

        public override string ToString()
        {
            return "[" + row + ", " + col + "]";
        }
    }
}
