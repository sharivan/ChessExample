using System;
using System.Collections.Generic;
using System.Text;

namespace ChessExample
{
    public class Pawn : Piece
    {
        private Piece promotedPiece;

        public bool WasMoved
        {
            get
            {
                return Color == PieceColor.BLACK ? (Position.Row > 1) : (Position.Row < 6);
            }
        }

        public Piece PromotedPiece
        {
            get
            {
                return promotedPiece;
            }
        }

        public Pawn(ChessLogic logic, Cell position, PieceColor color) : base(logic, position, color)
        {
        }

        internal override void GenerateMoveList(List<Cell> moveList, bool checkCheck)
        {
            if (promotedPiece != null)
            {
                promotedPiece.GenerateMoveList(moveList, checkCheck);
                return;
            }

            Cell src = Position; // agora sei onde estou

            // verifica se tem movimento (sem captura)
            int delta = Color == PieceColor.WHITE ? -1 : 1;
            Cell dst = new Cell(src.Row + delta, src.Col);
            if (logic.IsEmpty(dst)) // verifica se pode andar uma casa
            {
                CheckCheckAndAdd(moveList, src, dst, checkCheck);

                dst = new Cell(src.Row + 2 * delta, src.Col);
                if (!WasMoved && logic.IsEmpty(dst)) // verifica se pode andar duas casas
                    CheckCheckAndAdd(moveList, src, dst, checkCheck);
            }

            // verifica possíveis capturas
            dst = new Cell(src.Row + delta, src.Col + 1);
            if (logic.IsValidPosition(dst))
            {
                Piece other = logic.GetPiece(dst); // obtém a peça que está na posição de destino
                if (other != null && IsEnemy(other)) // verifica se é uma peça inimiga
                    CheckCheckAndAdd(moveList, src, dst, checkCheck);
            }

            dst = new Cell(src.Row + delta, src.Col - 1);
            if (logic.IsValidPosition(dst))
            {
                Piece other = logic.GetPiece(dst); // obtém a peça que está na posição de destino
                if (other != null && IsEnemy(other)) // verifica se é uma peça inimiga
                    CheckCheckAndAdd(moveList, src, dst, checkCheck);
            }

            // verifica se é possível realizar o en passant
            if (logic.pawnWalkedTwoRows)
                switch (Color)
                {
                    case PieceColor.WHITE:
                        if (src.Row == 3)
                        {
                            dst = new Cell(src.Row - 1, src.Col - 1);
                            if (logic.pawnCol == src.Col - 1)
                                CheckCheckAndAdd(moveList, src, dst, checkCheck);

                            dst = new Cell(src.Row - 1, src.Col + 1);
                            if (logic.pawnCol == src.Col + 1)
                                CheckCheckAndAdd(moveList, src, dst, checkCheck);
                        }

                        break;

                    case PieceColor.BLACK:
                        if (src.Row == 4)
                        {
                            dst = new Cell(src.Row + 1, src.Col - 1);
                            if (logic.pawnCol == src.Col - 1)
                                CheckCheckAndAdd(moveList, src, dst, checkCheck);

                            dst = new Cell(src.Row + 1, src.Col + 1);
                            if (logic.pawnCol == src.Col + 1)
                                CheckCheckAndAdd(moveList, src, dst, checkCheck);
                        }

                        break;
                }
        }

        internal override void UnsafeMove(Cell position)
        {
            if (promotedPiece != null)
            {
                promotedPiece.UnsafeMove(position);
                return;
            }

            Cell src = Position;

            bool enPassant = false;
            if (Math.Abs(position.Col - src.Col) == 1) // é uma captura
            {
                Piece piece = logic.GetPiece(position);
                if (piece == null)
                    enPassant = true;
            }

            base.UnsafeMove(position);

            if (enPassant)
            {
                Cell captured = new Cell(src.Row, position.Col);
                Piece other = logic.board[captured.Row, captured.Col];
                if (other != null)
                {
                    other.alive = false;
                    logic.pieces[(int)other.Color].Remove(other);
                }

                logic.board[captured.Row, captured.Col] = null;                          
            }

            logic.pawnWalkedTwoRows = Math.Abs(position.Row - src.Row) == 2;
            logic.pawnCol = position.Col;

            if (Color == PieceColor.WHITE)
            {
                if (position.Row == 0)
                    logic.pawnToPromote = this;
            }
            else
            {
                if (position.Row == 7)
                    logic.pawnToPromote = this;
            }
        }

        private void PromoteTo(Piece piece)
        {
            promotedPiece = piece;
            logic.pawnToPromote = null;
            logic.SwapColor();
            logic.GenerateMoveList();
        }

        public void PromoteToRook()
        {
            if (logic.PawnToPromote == this)
                PromoteTo(new Rook(logic, Position, Color));
        }

        public void PromoteToKnight()
        {
            if (logic.PawnToPromote == this)
                PromoteTo(new Rook(logic, Position, Color));
        }

        public void PromoteToBishop()
        {
            if (logic.PawnToPromote == this)
                PromoteTo(new Rook(logic, Position, Color));
        }

        public void PromoteToQueen()
        {
            if (logic.PawnToPromote == this)
                PromoteTo(new Rook(logic, Position, Color));
        }
    }
}
