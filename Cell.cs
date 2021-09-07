using System;
using System.Collections.Generic;
using System.Text;

namespace ChessExample
{
    public struct Cell
    {
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
