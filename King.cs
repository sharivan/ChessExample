using System;
using System.Collections.Generic;
using System.Text;

namespace ChessExample
{
    public class King : Piece
    {
        internal bool wasMoved;

        public bool WasMoved
        {
            get
            {
                return wasMoved;
            }
        }

        public King(ChessLogic logic, PieceColor color) : base(logic, color)
        {
            wasMoved = false;
        }

        public King(ChessLogic logic, Cell position, PieceColor color) : base(logic, position, color)
        {
            wasMoved = false;
        }

        internal override void GenerateMoveList(List<Cell> moveList, bool checkCheck)
        {
            Cell src = Position; // agora sei onde estou

            // verei pra onde vou (diagonais)
            for (int dx = -1; dx <= 1; dx += 2)
                for (int dy = -1; dy <= 1; dy += 2)
                {
                    Cell dst = new Cell(src.Row + dx, src.Col + dy);
                    if (!logic.IsValidPosition(dst)) // verifica se a posição de destino é válida
                        break;

                    if (logic.IsEmpty(dst)) // verifica se a posição de desitno é vazia
                        CheckCheckAndAdd(moveList, src, dst, checkCheck);
                    else
                    {
                        Piece other = logic.GetPiece(dst); // obtém a peça que está na posição de destino

                        if (IsEnemy(other)) // verifica se é uma peça inimiga
                            CheckCheckAndAdd(moveList, src, dst, checkCheck);

                        // caso contrário, não é movimento válido
                    }
                }

            // verei pra onde vou (direita ou esquerda)
            for (int dx = -1; dx <= 1; dx += 2)
            {
                Cell dst = new Cell(src.Row + dx, src.Col); // fixa a coluna e varia a linha
                if (!logic.IsValidPosition(dst)) // verifica se a posição de destino é válida
                    continue;

                if (logic.IsEmpty(dst)) // verifica se a posição de desitno é vazia
                    CheckCheckAndAdd(moveList, src, dst, checkCheck);
                else
                {
                    Piece other = logic.GetPiece(dst); // obtém a peça que está na posição de destino

                    if (IsEnemy(other)) // verifica se é uma peça inimiga
                        CheckCheckAndAdd(moveList, src, dst, checkCheck);

                    // caso contrário, não é movimento válido
                }
            }

            // verei pra onde vou (cima ou baixo)
            for (int dy = -1; dy <= 1; dy += 2)
            {
                Cell dst = new Cell(src.Row, src.Col + dy); // fixa a linha e varia a coluna
                if (!logic.IsValidPosition(dst)) // verifica se a posição de destino é válida
                    continue;

                if (logic.IsEmpty(dst)) // verifica se a posição de desitno é vazia
                    CheckCheckAndAdd(moveList, src, dst, checkCheck);
                else
                {
                    Piece other = logic.GetPiece(dst); // obtém a peça que está na posição de destino

                    if (IsEnemy(other)) // verifica se é uma peça inimiga
                        CheckCheckAndAdd(moveList, src, dst, checkCheck);

                    // caso contrário, não é movimento válido
                }
            }

            // verifica a possibilidade de roque
            if (wasMoved || (checkCheck ? IsCheck() : false))
                return;

            PieceColor otherColor = logic.OtherColor;

            // verifica o grande roque
            Rook leftRook = logic.GetRook(Color, 0);
            if (leftRook.Alive && !leftRook.WasMoved)
            {
                bool valid = true;
                for (int delta = 1; delta <= 2; delta++)
                {
                    Cell path = new Cell(src.Row, src.Col - delta);
                    if (!logic.IsEmpty(path) || logic.IsAttacked(path, otherColor))
                    {
                        valid = false;
                        break;
                    }
                }

                if (valid)
                    moveList.Add(new Cell(src.Row, src.Col - 2));
            }

            // verifica o pequeno roque
            Rook rightRook = logic.GetRook(Color, 1);
            if (rightRook.Alive && !rightRook.WasMoved)
            {
                bool valid = true;
                for (int delta = 1; delta <= 2; delta++)
                {
                    Cell path = new Cell(src.Row, src.Col + delta);
                    if (!logic.IsEmpty(path) || logic.IsAttacked(path, otherColor))
                    {
                        valid = false;
                        break;
                    }
                }

                if (valid)
                    moveList.Add(new Cell(src.Row, src.Col + 2));
            }
        }

        public bool IsCheck()
        {
            PieceColor otherColor = logic.GetOtherColor(Color);
            Cell position = Position;

            // gera a lista de movimentos do oponente
            foreach (Piece piece in logic.pieces[(int) otherColor])
            {
                piece.moveListGenerated = false;
                piece.GetMoveList(false, false);
                if (piece.IsAttacking(position))
                    return true;
            }

            return false;
        }

        internal override void UnsafeMove(Cell position)
        {
            Cell src = Position;

            base.UnsafeMove(position);

            if (position.Col - src.Col == -2) // é um grande roque
            {
                Rook leftRook = logic.GetRook(Color, 0);
                leftRook.SetPosition(position.Row, position.Col + 1);
            }
            else if (position.Col - src.Col == 2) // é um pequeno roque
            {
                Rook rightRook = logic.GetRook(Color, 1);
                rightRook.SetPosition(position.Row, position.Col - 1);
            }

            wasMoved = true;
        }
    }
}
