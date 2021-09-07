using System;
using System.Collections.Generic;
using System.Text;

namespace ChessExample
{
    public abstract class Piece
    {
        protected ChessLogic logic;
        internal Cell position;
        private PieceColor color;
        internal List<Cell> moveList;
        internal bool moveListGenerated;
        internal bool alive;

        public Cell Position
        {
            get
            {
                return position;
            }
        }

        public PieceColor Color
        {
            get
            {
                return color;
            }
        }

        public bool Alive
        {
            get
            {
                return alive;
            }
        }

        public bool MyTurn
        {
            get
            {
                return logic.CurrentTurn == color;
            }
        }

        public int MoveCount
        {
            get
            {
                return moveList.Count;
            }
        }

        protected Piece(ChessLogic logic, Cell position, PieceColor color)
        {
            this.logic = logic;
            this.position = position;
            this.color = color;

            moveListGenerated = false;
            alive = true;

            moveList = new List<Cell>();
        }

        internal abstract void GenerateMoveList(List<Cell> moveList, bool checkCheck);

        private static readonly Cell[] EMPTY_MOVE_LIST = new Cell[] {};

        protected void GenerateMoveList(bool checkCheck = true)
        {
            if (!moveListGenerated)
            {
                moveList.Clear();
                GenerateMoveList(moveList, checkCheck);
                moveListGenerated = true;
            }
        }

        public Cell[] GetMoveList(bool checkMyTurn = true, bool checkCheck = true)
        {
            if (checkMyTurn && !MyTurn)
                return EMPTY_MOVE_LIST;

            GenerateMoveList(checkCheck);

            return moveList.ToArray();
        }

        public bool IsEnemy(Piece other)
        {
            return color != other.color;
        }

        internal virtual void UnsafeMove(Cell position)
        {
            logic.board[this.position.Row, this.position.Col] = null;
            this.position = position;

            Piece other = logic.GetPiece(position);
            if (other != null)
            {
                other.alive = false;
                logic.pieces[(int)other.Color].Remove(other);
            }

            logic.board[position.Row, position.Col] = this;
            logic.SwapColor();  
        }

        public bool Move(Cell position)
        {
            if (!alive || !moveList.Contains(position))
                return false;

            logic.pawnWalkedTwoRows = false;
            UnsafeMove(position);

            if (logic.PawnToPromote == null)
                logic.GenerateMoveList();
            else
                logic.SwapColor();

            return true;
        }

        public bool IsAttacking(Cell position)
        {
            //GenerateMoveList();
            return moveList.Contains(position);
        }

        public bool IsAttacking(int row, int col)
        {
            return IsAttacking(new Cell(row, col));
        }

        protected void CheckCheckAndAdd(List<Cell> moveList, Cell src, Cell dst, bool checkCheck)
        {
            if (logic.IsValidPosition(dst) && (checkCheck ? !logic.CheckCheck(src, dst) : true))
                moveList.Add(dst);
        }

        internal void SetPosition(Cell position)
        {
            if (logic.IsValidPosition(this.position))
                logic.board[this.position.Row, this.position.Col] = null;

            this.position = position;

            if (logic.IsValidPosition(position))
                logic.board[position.Row, position.Col] = this;
        }

        internal void SetPosition(int row, int col)
        {
            SetPosition(new Cell(row, col));
        }

        internal void SetPositionAndTurnAlive(Cell position)
        {
            SetPosition(position);
            alive = true;
        }

        internal void SetPositionAndTurnAlive(int row, int col)
        {
            SetPositionAndTurnAlive(new Cell(row, col));
        }
    }
}
