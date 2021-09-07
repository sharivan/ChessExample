using System;
using System.Collections.Generic;
using System.Text;

namespace ChessExample
{
    public class Rook : Piece
    {
        internal bool wasMoved;

        public bool WasMoved
        {
            get
            {
                return wasMoved;
            }
        }

        public Rook(ChessLogic logic, Cell position, PieceColor color) : base(logic, position, color)
        {
        }

        internal override void GenerateMoveList(List<Cell> moveList, bool checkCheck)
        {
            Cell src = Position; // agora sei onde estou

            // verei pra onde vou (direita ou esquerda)
            for (int dx = -1; dx <= 1; dx += 2)
            {
                for (int counter = 1; counter < 8; counter++)
                {
                    Cell dst = new Cell(src.Row + dx * counter, src.Col); // fixa a coluna e varia a linha
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

                        break; // em todo caso, encerra o looping
                    }
                }
            }

            // verei pra onde vou (cima ou baixo)
            for (int dy = -1; dy <= 1; dy += 2)
            {
                for (int counter = 1; counter < 8; counter++)
                {
                    Cell dst = new Cell(src.Row, src.Col + dy * counter); // fixa a linha e varia a coluna
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

                        break; // em todo caso, encerra o looping
                    }
                }
            }
        }

        internal override void UnsafeMove(Cell position)
        {
            base.UnsafeMove(position);
            wasMoved = true;
        }
    }
}
