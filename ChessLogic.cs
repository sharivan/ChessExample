using System;
using System.Collections.Generic;
using System.Text;

namespace ChessExample
{
    public class ChessLogic
    {
        private Piece[,] oldBoard;
        internal Piece[,] board;
        private PieceColor currentTurn;
        private bool check;
        private List<Piece>[] oldPieces;
        internal List<Piece>[] pieces;
        private List<Cell[]>[] oldPieceMoveList;
        private List<bool>[] oldPieceMoveListGenerated;
        private Pawn[,] pawns;
        private Rook[,] rooks;
        private Knight[,] knights;
        private Bishop[,] bishops;
        private Queen[] queens;
        private King[] kings;        
        private bool gameOver;
        private bool draw;
        private PieceColor winner;
        internal bool pawnWalkedTwoRows;
        internal int pawnCol;
        internal Pawn pawnToPromote;

        public PieceColor CurrentTurn
        {
            get
            {
                return currentTurn;
            }
        }

        public bool Check
        {
            get
            {
                return check;
            }
        }

        public PieceColor OtherColor
        {
            get
            {
                return GetOtherColor(currentTurn);
            }
        }

        public bool GameOver
        {
            get
            {
                return gameOver;
            }
        }

        public bool Draw
        {
            get
            {
                return draw;
            }
        }

        public PieceColor Winner
        {
            get
            {
                return winner;
            }
        }

        public Pawn PawnToPromote
        {
            get
            {
                return pawnToPromote;
            }
        }

        public ChessLogic()
        {
            oldBoard = new Piece[8, 8];
            board = new Piece[8, 8];
            oldPieces = new List<Piece>[2];
            pieces = new List<Piece>[2];
            oldPieceMoveList = new List<Cell[]>[2];
            oldPieceMoveListGenerated = new List<bool>[2];
            pawns = new Pawn[2, 8];
            rooks = new Rook[2, 2];
            knights = new Knight[2, 2];
            bishops = new Bishop[2, 2];
            queens = new Queen[2];
            kings = new King[2];           

            oldPieces[0] = new List<Piece>();
            oldPieces[1] = new List<Piece>();

            pieces[0] = new List<Piece>();
            pieces[1] = new List<Piece>();

            oldPieceMoveList[0] = new List<Cell[]>();
            oldPieceMoveList[1] = new List<Cell[]>();

            oldPieceMoveListGenerated[0] = new List<bool>();
            oldPieceMoveListGenerated[1] = new List<bool>();

            // cria os peões brancos
            for (int col = 0; col < 8; col++)
                pawns[0, col] = new Pawn(this, new Cell(6, col), PieceColor.WHITE);

            // cria os peões negros
            for (int col = 0; col < 8; col++)
                pawns[1, col] = new Pawn(this, new Cell(1, col), PieceColor.BLACK);

            // cria as torrres brancas
            rooks[0, 0] = new Rook(this, new Cell(7, 0), PieceColor.WHITE);
            rooks[0, 1] = new Rook(this, new Cell(7, 7), PieceColor.WHITE);

            // cria as torrres negras
            rooks[1, 0] = new Rook(this, new Cell(0, 0), PieceColor.BLACK);
            rooks[1, 1] = new Rook(this, new Cell(0, 7), PieceColor.BLACK);

            // cria os cavalos brancos
            knights[0, 0] = new Knight(this, new Cell(7, 1), PieceColor.WHITE);
            knights[0, 1] = new Knight(this, new Cell(7, 6), PieceColor.WHITE);

            // cria os cavalos negros
            knights[1, 0] = new Knight(this, new Cell(0, 1), PieceColor.BLACK);
            knights[1, 1] = new Knight(this, new Cell(0, 6), PieceColor.BLACK);

            // cria os bispos brancos
            bishops[0, 0] = new Bishop(this, new Cell(7, 2), PieceColor.WHITE);
            bishops[0, 1] = new Bishop(this, new Cell(7, 5), PieceColor.WHITE);

            // cria os bispos negros
            bishops[1, 0] = new Bishop(this, new Cell(0, 2), PieceColor.BLACK);
            bishops[1, 1] = new Bishop(this, new Cell(0, 5), PieceColor.BLACK);

            // cria a dama branca
            queens[0] = new Queen(this, new Cell(7, 3), PieceColor.WHITE);

            // cria o rei branco
            kings[0] = new King(this, new Cell(7, 4), PieceColor.WHITE);

            // cria a dama negra
            queens[1] = new Queen(this, new Cell(0, 3), PieceColor.BLACK);

            // cria o rei negro
            kings[1] = new King(this, new Cell(0, 4), PieceColor.BLACK);

            Reset();
        }

        public bool IsValidPosition(int row, int col)
        {
            return 0 <= row && row < 8 && 0 <= col && col < 8;
        }

        public bool IsValidPosition(Cell position)
        {
            return IsValidPosition(position.Row, position.Col);
        }

        public Piece GetPiece(int row, int col)
        {
            return board[row, col];
        }

        public Piece GetPiece(Cell position)
        {
            return GetPiece(position.Row, position.Col);
        }

        public bool IsEmpty(int row, int col)
        {
            return board[row, col] == null;
        }

        public bool IsEmpty(Cell position)
        {
            return IsEmpty(position.Row, position.Col);
        }

        private void Clear()
        {
            for (int row = 0; row < 8; row++)
                for (int col = 0; col < 8; col++)
                    board[row, col] = null;

            pieces[0].Clear();
            pieces[1].Clear();
        }

        public void Reset()
        {
            currentTurn = PieceColor.WHITE;
            gameOver = false;
            draw = false;
            pawnWalkedTwoRows = false;

            Clear();

            // reseta as posições dos peões brancos
            for (int col = 0; col < 8; col++)
                pawns[0, col].SetPositionAndTurnAlive(6, col);

            // reseta as posições dos peões negros
            for (int col = 0; col < 8; col++)
                pawns[1, col].SetPositionAndTurnAlive(1, col);

            // reseta as posiçõe das torrres brancas
            rooks[0, 0].SetPositionAndTurnAlive(7, 0);
            rooks[0, 0].wasMoved = false;
            rooks[0, 1].SetPositionAndTurnAlive(7, 7);
            rooks[0, 1].wasMoved = false;

            // reseta as posiçõe das torrres negras
            rooks[1, 0].SetPositionAndTurnAlive(0, 0);
            rooks[1, 0].wasMoved = false;
            rooks[1, 1].SetPositionAndTurnAlive(0, 7);
            rooks[1, 1].wasMoved = false;

            // reseta as posiçõe dos cavalos brancos
            knights[0, 0].SetPositionAndTurnAlive(7, 1);
            knights[0, 1].SetPositionAndTurnAlive(7, 6);

            // reseta as posiçõe dos cavalos negros
            knights[1, 0].SetPositionAndTurnAlive(0, 1);
            knights[1, 1].SetPositionAndTurnAlive(0, 6);

            // reseta as posiçõe dos bispos brancos
            bishops[0, 0].SetPositionAndTurnAlive(7, 2);
            bishops[0, 1].SetPositionAndTurnAlive(7, 5);

            // reseta as posiçõe dos bispos negros
            bishops[1, 0].SetPositionAndTurnAlive(0, 2);
            bishops[1, 1].SetPositionAndTurnAlive(0, 5);

            // reseta a posição da dama branca
            queens[0].SetPositionAndTurnAlive(7, 3);

            // reseta a posição do rei branco
            kings[0].SetPositionAndTurnAlive(7, 4);
            kings[0].wasMoved = false;

            // reseta a posição da dama negro
            queens[1].SetPositionAndTurnAlive(0, 3);

            // reseta a posição do rei negro
            kings[1].SetPositionAndTurnAlive(0, 4);
            kings[1].wasMoved = false;


            for (int row = 0; row < 2; row++)
                for (int col = 0; col < 8; col++)
                    pieces[1].Add(board[row, col]);

            for (int row = 6; row < 8; row++)
                for (int col = 0; col < 8; col++)
                    pieces[0].Add(board[row, col]);

            GenerateMoveList();
        }

        public PieceColor GetOtherColor(PieceColor color)
        {
            return color == PieceColor.BLACK ? PieceColor.WHITE : PieceColor.BLACK;
        }

        internal void SwapColor()
        {
            currentTurn = currentTurn == PieceColor.BLACK ? PieceColor.WHITE : PieceColor.BLACK;
        }

        public Pawn GetPawn(PieceColor color, int index)
        {
            return pawns[(int)color, index];
        }

        public Rook GetRook(PieceColor color, int index)
        {
            return rooks[(int)color, index];
        }

        public Knight GetKnight(PieceColor color, int index)
        {
            return knights[(int)color, index];
        }

        public Bishop GetBishop(PieceColor color, int index)
        {
            return bishops[(int)color, index];
        }

        public Queen GetQueen(PieceColor color)
        {
            return queens[(int)color];
        }

        public King GetKing(PieceColor color)
        {
            return kings[(int) color];
        }

        public int GetPieceCount(PieceColor color)
        {
            return pieces[(int) color].Count;
        }

        public Piece GetPiece(PieceColor color, int index)
        {
            return pieces[(int) color][index];
        }

        public bool IsAttacked(Cell position, PieceColor color)
        {
            foreach (Piece piece in pieces[(int) color])
            {
                //piece.GetMoveList(false, false);
                if (piece.IsAttacking(position))
                    return true;
            }

            return false;
        }

        internal void GenerateMoveList()
        {
            if (gameOver)
                return;

            for (int i = 0; i < 2; i++)
                for (int j = 0; j < pieces[i].Count; j++)
                {
                    Piece piece = pieces[i][j];
                    piece.moveListGenerated = false;
                }

            // verifica se o meu rei não está em cheque
            PieceColor otherColor = GetOtherColor(currentTurn);

            // gera a lista de movimentos do oponente
            foreach (Piece piece in pieces[(int) otherColor])
            {
                piece.GetMoveList(false, false);
            }

            King myKing = GetKing(currentTurn);
            check = myKing.IsCheck();

            // gera a lista de movimentos de minhas peças
            int moveCount = 0;
            for (int i = 0; i < pieces[(int) currentTurn].Count; i++)
            {
                Piece piece = pieces[(int) currentTurn][i];
                piece.moveListGenerated = false;
                piece.GetMoveList();
                moveCount += piece.MoveCount;
            }

            gameOver = moveCount == 0;
            if (gameOver)
            {
                draw = !myKing.IsCheck();
                if (!draw)
                    winner = GetOtherColor(currentTurn);
            }
        }

        internal bool CheckCheck(Cell src, Cell dst)
        {
            bool[,] rookMoved = new bool[2, 2];
            bool[] kingMoved = new bool[2];

            Piece srcPiece = GetPiece(src);
            if (srcPiece == null)
                return false;

            // salva o estado atual do tabuleiro
            bool pawnWalkedTwoRows = this.pawnWalkedTwoRows;
            int pawnCol = this.pawnCol;

            for (int row = 0; row < 8; row++)
                for (int col = 0; col < 8; col++)
                    oldBoard[row, col] = board[row, col];

            for (int i = 0; i < 2; i++)
            {
                oldPieces[i].Clear();
                oldPieceMoveList[i].Clear();
                oldPieceMoveListGenerated[i].Clear();
                for (int j = 0; j < pieces[i].Count; j++)
                {
                    oldPieces[i].Add(pieces[i][j]);
                    oldPieceMoveList[i].Add(pieces[i][j].moveList.ToArray());
                    oldPieceMoveListGenerated[i].Add(pieces[i][j].moveListGenerated);
                }
            }

            kingMoved[0] = kings[0].wasMoved;
            kingMoved[1] = kings[1].wasMoved;

            rookMoved[0, 0] = rooks[0, 0].wasMoved;
            rookMoved[0, 1] = rooks[0, 1].wasMoved;
            rookMoved[1, 0] = rooks[1, 0].wasMoved;
            rookMoved[1, 1] = rooks[1, 1].wasMoved;

            PieceColor myColor = currentTurn;

            // faz o movimento
            srcPiece.UnsafeMove(dst);

            // verifica se ainda está em cheque
            King myKing = GetKing(myColor);
            bool alreadyInCheck = myKing.IsCheck();

            // voltar pro estado anterior do tabieleiro
            this.pawnWalkedTwoRows = pawnWalkedTwoRows;
            this.pawnCol = pawnCol;

            for (int row = 0; row < 8; row++)
                for (int col = 0; col < 8; col++)
                {
                    Piece piece = oldBoard[row, col];
                    board[row, col] = piece;

                    if (piece != null)
                    {
                        piece.alive = true;
                        piece.position = new Cell(row, col);
                    }
                }

            for (int i = 0; i < 2; i++)
            {
                pieces[i].Clear();
                for (int j = 0; j < oldPieces[i].Count; j++)
                {
                    Piece piece = oldPieces[i][j];
                    pieces[i].Add(piece);
                    piece.moveListGenerated = oldPieceMoveListGenerated[i][j];
                    piece.moveList.Clear();
                    piece.moveList.AddRange(oldPieceMoveList[i][j]);
                }
            }

            kings[0].wasMoved = kingMoved[0];
            kings[1].wasMoved = kingMoved[1];

            rooks[0, 0].wasMoved = rookMoved[0, 0];
            rooks[0, 1].wasMoved = rookMoved[0, 1];
            rooks[1, 0].wasMoved = rookMoved[1, 0];
            rooks[1, 1].wasMoved = rookMoved[1, 1];

            currentTurn = myColor;

            // retorna o resultado
            return alreadyInCheck;
        }
    }
}
