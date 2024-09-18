using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChessExample
{
    public partial class FrmChessExample : Form
    {
        private const int CELL_SIZE = 52;

        private ChessLogic logic;

        private Image darkSquareImg;
        private Image lightSquareImg;

        private Image whitePawnImg;
        private Image whiteRookImg;
        private Image whiteKnightImg;
        private Image whiteBishopImg;
        private Image whiteQueenImg;
        private Image whiteKingImg;

        private Image blackPawnImg;
        private Image blackRookImg;
        private Image blackKnightImg;
        private Image blackBishopImg;
        private Image blackQueenImg;
        private Image blackKingImg;

        private List<PictureBox>[] pieces;

        private Point mousePos;
        private Piece draggingPiece;
        private int dx;
        private int dy;

        public FrmChessExample() => InitializeComponent();

        private Point CellToPoint(int row, int col) => new Point(CELL_SIZE * col, CELL_SIZE * row);

        private Point CellToPoint(Cell position) => CellToPoint(position.Row, position.Col);

        public Image GetImage(string name)
        {
            using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
            return Image.FromStream(stream);
        }

        private void CreateBoard()
        {
            pieces = new List<PictureBox>[2];
            pieces[0] = new List<PictureBox>();
            pieces[1] = new List<PictureBox>();

            darkSquareImg = GetImage("ChessExample.img.Dark Square.png");
            lightSquareImg = GetImage("ChessExample.img.Light Square.png");

            whitePawnImg = GetImage("ChessExample.img.White Pawn.png");
            whiteRookImg = GetImage("ChessExample.img.White Rook.png");
            whiteKnightImg = GetImage("ChessExample.img.White Knight.png");
            whiteBishopImg = GetImage("ChessExample.img.White Bishop.png");
            whiteQueenImg = GetImage("ChessExample.img.White Queen.png");
            whiteKingImg = GetImage("ChessExample.img.White King.png");

            blackPawnImg = GetImage("ChessExample.img.Black Pawn.png");
            blackRookImg = GetImage("ChessExample.img.Black Rook.png");
            blackKnightImg = GetImage("ChessExample.img.Black Knight.png");
            blackBishopImg = GetImage("ChessExample.img.Black Bishop.png");
            blackQueenImg = GetImage("ChessExample.img.Black Queen.png");
            blackKingImg = GetImage("ChessExample.img.Black King.png");

            //RefreshGameState();
        }

        private void RefreshGameState()
        {
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < pieces[i].Count; j++)
                {
                    PictureBox picture = pieces[i][j];
                    var piece = picture.Tag as Piece;
                    picture.Location = piece.alive ? new Point(-CELL_SIZE, 0) : CellToPoint(piece.position);
                }
        }

        private void FrmChessExample_Load(object sender, EventArgs e)
        {
            ClientSize = new Size(8 * 52, 8 * 52);

            logic = new ChessLogic();

            CreateBoard(); 
        }

        private void DrawPiece(Graphics g, Piece piece, Point p)
        {
            if (piece.Color == PieceColor.WHITE)
            {
                if (piece is Pawn)
                    g.DrawImage(whitePawnImg, p);
                else if (piece is Rook)
                    g.DrawImage(whiteRookImg, p);
                else if (piece is Knight)
                    g.DrawImage(whiteKnightImg, p);
                else if (piece is Bishop)
                    g.DrawImage(whiteBishopImg, p);
                else if (piece is Queen)
                    g.DrawImage(whiteQueenImg, p);
                else if (piece is King)
                    g.DrawImage(whiteKingImg, p);
            }
            else
            {
                if (piece is Pawn)
                    g.DrawImage(blackPawnImg, p);
                else if (piece is Rook)
                    g.DrawImage(blackRookImg, p);
                else if (piece is Knight)
                    g.DrawImage(blackKnightImg, p);
                else if (piece is Bishop)
                    g.DrawImage(blackBishopImg, p);
                else if (piece is Queen)
                    g.DrawImage(blackQueenImg, p);
                else if (piece is King)
                    g.DrawImage(blackKingImg, p);
            }
        }

        private void DrawPiece(Graphics g, Piece piece) => DrawPiece(g, piece, CellToPoint(piece.Position));

        private void FrmChessExample_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            for (int row = 0; row < 8; row++)
                for (int col = 0; col < 8; col++)
                    g.DrawImage(((row + col) & 1) == 0 ? lightSquareImg : darkSquareImg, CellToPoint(row, col));

            for (int j = 0; j < logic.GetPieceCount(PieceColor.WHITE); j++)
            {
                Piece piece = logic.GetPiece(PieceColor.WHITE, j);
                if (piece != draggingPiece)
                    DrawPiece(g, piece);
            }

            for (int j = 0; j < logic.GetPieceCount(PieceColor.BLACK); j++)
            {
                Piece piece = logic.GetPiece(PieceColor.BLACK, j);
                if (piece != draggingPiece)
                    DrawPiece(g, piece);
            }

            if (mousePos.X >= 0 && mousePos.X <= ClientSize.Width && mousePos.Y >= 0 && mousePos.Y <= ClientSize.Height)
            {
                if (draggingPiece != null)
                    DrawPiece(g, draggingPiece, new Point(mousePos.X - dx, mousePos.Y - dy));

                int row = mousePos.Y / CELL_SIZE;
                int col = mousePos.X / CELL_SIZE;

                using (var pen = new Pen(Color.Blue, 2))
                {
                    Point p = CellToPoint(row, col);
                    g.DrawRectangle(pen, new Rectangle(p.X, p.Y, CELL_SIZE, CELL_SIZE));
                }

                if (logic.IsValidPosition(row, col))
                {
                    Piece piece = logic.GetPiece(row, col);
                    if (piece != null && piece.Color == logic.CurrentTurn)
                    {
                        Cell[] moveList = piece.GetMoveList();

                        for (int i = 0; i < moveList.Length; i++)
                        {
                            Cell position = moveList[i];

                            using var pen = new Pen(Color.Red, 2);
                            Point p = CellToPoint(position);
                            g.DrawRectangle(pen, new Rectangle(p.X, p.Y, CELL_SIZE, CELL_SIZE));
                        }
                    }
                }
            }
        }

        private void FrmChessExample_MouseMove(object sender, MouseEventArgs e)
        {
            mousePos.X = e.X;
            mousePos.Y = e.Y;
            Invalidate();
        }

        private void FrmChessExample_MouseDown(object sender, MouseEventArgs e)
        {
            int x = e.X;
            int y = e.Y;

            int row = y / CELL_SIZE;
            int col = x / CELL_SIZE;

            if (logic.IsValidPosition(row, col))
            {
                Piece piece = logic.GetPiece(row, col);
                if (piece != null && piece.Color == logic.CurrentTurn)
                {
                    draggingPiece = piece;
                    Point p = CellToPoint(row, col);
                    dx = x - p.X;
                    dy = y - p.Y;
                    Invalidate();
                }
            }
        }

        private void FrmChessExample_MouseUp(object sender, MouseEventArgs e)
        {
            if (draggingPiece != null)
            {
                int x = e.X;
                int y = e.Y;

                int row = y / CELL_SIZE;
                int col = x / CELL_SIZE;
                var dst = new Cell(row, col);

                if (logic.IsValidPosition(row, col))
                {
                    Cell[] moveList = draggingPiece.GetMoveList();
                    if (moveList.Contains(dst))
                    {
                        draggingPiece.Move(dst);

                        if (logic.GameOver)
                        {
                            if (logic.Draw)
                                MessageBox.Show("Draw!");
                            else
                                MessageBox.Show((logic.Winner == PieceColor.WHITE ? "White" : "Black") + " wins!");
                        }
                        else if (logic.PawnToPromote != null)
                        {
                            if (logic.CurrentTurn == PieceColor.WHITE)
                            {
                                btnKnight.Image = whiteKnightImg;
                                btnBishop.Image = whiteBishopImg;
                                btnRook.Image = whiteRookImg;
                                btnQueen.Image = whiteQueenImg;
                            }
                            else
                            {
                                btnKnight.Image = blackKnightImg;
                                btnBishop.Image = blackBishopImg;
                                btnRook.Image = blackRookImg;
                                btnQueen.Image = blackQueenImg;
                            }

                            pnlPieceToPromote.Visible = true;
                            pnlPieceToPromote.Location = new Point((ClientSize.Width - pnlPieceToPromote.Width) / 2, (ClientSize.Height - pnlPieceToPromote.Height) / 2);
                        }
                    }
                }

                draggingPiece = null;
                Invalidate();
            }
        }

        private void btnKnight_Click(object sender, EventArgs e)
        {
            logic.PawnToPromote.PromoteToKnight();
            pnlPieceToPromote.Visible = false;
        }

        private void btnBishop_Click(object sender, EventArgs e)
        {
            logic.PawnToPromote.PromoteToBishop();
            pnlPieceToPromote.Visible = false;
        }

        private void btnRook_Click(object sender, EventArgs e)
        {
            logic.PawnToPromote.PromoteToRook();
            pnlPieceToPromote.Visible = false;
        }

        private void btnQueen_Click(object sender, EventArgs e)
        {
            logic.PawnToPromote.PromoteToQueen();
            pnlPieceToPromote.Visible = false;
        }
    }
}
