using System;
using System.Collections.Generic;
using System.Text;

namespace ChessExample
{
    public class Knight : Piece
    {
        public Knight(ChessLogic logic, PieceColor color) : base(logic, color)
        {
        }

        public Knight(ChessLogic logic, Cell position, PieceColor color) : base(logic, position, color)
        {
        }

        internal override void GenerateMoveList(List<Cell> moveList, bool checkCheck)
        {
            Cell src = Position; // agora sei onde estou

            // verei pra onde vou
            for (int dx = -1; dx <= 1; dx += 2)
                for (int dy = -1; dy <= 1; dy += 2)
                {
                    var dst = new Cell(src.Row + 2 * dx, src.Col + dy);
                    if (logic.IsValidPosition(dst))
                    {
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

                    dst = new Cell(src.Row + dx, src.Col + 2 * dy);
                    if (logic.IsValidPosition(dst))
                    {
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
                }
        }
    }
}
