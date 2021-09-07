
namespace ChessExample
{
    partial class FrmChessExample
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlPieceToPromote = new System.Windows.Forms.Panel();
            this.btnKnight = new System.Windows.Forms.Button();
            this.btnBishop = new System.Windows.Forms.Button();
            this.btnRook = new System.Windows.Forms.Button();
            this.btnQueen = new System.Windows.Forms.Button();
            this.pnlPieceToPromote.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlPieceToPromote
            // 
            this.pnlPieceToPromote.Controls.Add(this.btnQueen);
            this.pnlPieceToPromote.Controls.Add(this.btnRook);
            this.pnlPieceToPromote.Controls.Add(this.btnBishop);
            this.pnlPieceToPromote.Controls.Add(this.btnKnight);
            this.pnlPieceToPromote.Location = new System.Drawing.Point(139, 96);
            this.pnlPieceToPromote.Name = "pnlPieceToPromote";
            this.pnlPieceToPromote.Size = new System.Drawing.Size(244, 61);
            this.pnlPieceToPromote.TabIndex = 0;
            this.pnlPieceToPromote.Visible = false;
            // 
            // btnKnight
            // 
            this.btnKnight.BackColor = System.Drawing.Color.Transparent;
            this.btnKnight.Location = new System.Drawing.Point(3, 3);
            this.btnKnight.Name = "btnKnight";
            this.btnKnight.Size = new System.Drawing.Size(55, 55);
            this.btnKnight.TabIndex = 0;
            this.btnKnight.UseVisualStyleBackColor = false;
            this.btnKnight.Click += new System.EventHandler(this.btnKnight_Click);
            // 
            // btnBishop
            // 
            this.btnBishop.BackColor = System.Drawing.Color.Transparent;
            this.btnBishop.Location = new System.Drawing.Point(64, 3);
            this.btnBishop.Name = "btnBishop";
            this.btnBishop.Size = new System.Drawing.Size(55, 55);
            this.btnBishop.TabIndex = 1;
            this.btnBishop.UseVisualStyleBackColor = false;
            this.btnBishop.Click += new System.EventHandler(this.btnBishop_Click);
            // 
            // btnRook
            // 
            this.btnRook.BackColor = System.Drawing.Color.Transparent;
            this.btnRook.Location = new System.Drawing.Point(125, 3);
            this.btnRook.Name = "btnRook";
            this.btnRook.Size = new System.Drawing.Size(55, 55);
            this.btnRook.TabIndex = 2;
            this.btnRook.UseVisualStyleBackColor = false;
            this.btnRook.Click += new System.EventHandler(this.btnRook_Click);
            // 
            // btnQueen
            // 
            this.btnQueen.BackColor = System.Drawing.Color.Transparent;
            this.btnQueen.Location = new System.Drawing.Point(186, 3);
            this.btnQueen.Name = "btnQueen";
            this.btnQueen.Size = new System.Drawing.Size(55, 55);
            this.btnQueen.TabIndex = 3;
            this.btnQueen.UseVisualStyleBackColor = false;
            this.btnQueen.Click += new System.EventHandler(this.btnQueen_Click);
            // 
            // FrmChessExample
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Blue;
            this.ClientSize = new System.Drawing.Size(701, 603);
            this.Controls.Add(this.pnlPieceToPromote);
            this.DoubleBuffered = true;
            this.Name = "FrmChessExample";
            this.Text = "Chess Example";
            this.Load += new System.EventHandler(this.FrmChessExample_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FrmChessExample_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FrmChessExample_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FrmChessExample_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FrmChessExample_MouseUp);
            this.pnlPieceToPromote.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlPieceToPromote;
        private System.Windows.Forms.Button btnQueen;
        private System.Windows.Forms.Button btnRook;
        private System.Windows.Forms.Button btnBishop;
        private System.Windows.Forms.Button btnKnight;
    }
}

