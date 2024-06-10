using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace SakkDolgozat
{
    public partial class Form1 : Form
    {
        private Button[,] buttons = new Button[8, 8];
        private Point? selectedPiece = null;
        private bool elsoJatekosLep = true;

        public Form1()
        {
            InitializeComponent();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void FormLoad(object sender, EventArgs e)
        {
            string[,] chessBoard = new string[8, 8]
            {
                { "\u265C", "\u265E", "\u265D", "\u265B", "\u265A", "\u265D", "\u265E", "\u265C" },
                { "\u265F", "\u265F", "\u265F", "\u265F", "\u265F", "\u265F", "\u265F", "\u265F" },
                { "", "", "", "", "", "", "", "" },
                { "", "", "", "", "", "", "", "" },
                { "", "", "", "", "", "", "", "" },
                { "", "", "", "", "", "", "", "" },
                { "\u2659", "\u2659", "\u2659", "\u2659", "\u2659", "\u2659", "\u2659", "\u2659" },
                { "\u2656", "\u2658", "\u2657", "\u2655", "\u2654", "\u2657", "\u2658", "\u2656" }
            };

            for (int RowCount = 0; RowCount < 8; RowCount++)
            {
                for (int ColumnCount = 0; ColumnCount < 8; ColumnCount++)
                {
                    Button btn = new Button();
                    btn.Width = 60;
                    btn.Height = 60;
                    btn.Font = new Font("Arial", 24, FontStyle.Bold);

                    if ((RowCount + ColumnCount) % 2 == 0)
                    {
                        btn.BackColor = Color.White;
                    }
                    else
                    {
                        btn.BackColor = Color.DarkOliveGreen;
                    }

                    btn.Text = chessBoard[RowCount, ColumnCount];
                    btn.Click += new EventHandler(Button_Click);
                    buttons[RowCount, ColumnCount] = btn;

                    tableLayoutPanel1.Controls.Add(btn, ColumnCount, RowCount);
                }
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            Point clickedLocation = GetButtonLocation(clickedButton);

            if (selectedPiece == null)
            {
                if (!string.IsNullOrEmpty(clickedButton.Text))
                {
                    
                    if ((elsoJatekosLep && clickedButton.Text.Contains("\u2659")) || (!elsoJatekosLep && clickedButton.Text.Contains("\u265F")))
                    {
                        selectedPiece = clickedLocation;
                        clickedButton.BackColor = Color.ForestGreen;
                    }
                }
            }
            else
            {
                MovePiece(selectedPiece.Value, clickedLocation);
                ResetButtonColors();
                selectedPiece = null;

                
                elsoJatekosLep = !elsoJatekosLep;
            }
        }

        private Point GetButtonLocation(Button btn)
        {
            for (int RowCount = 0; RowCount < 8; RowCount++)
            {
                for (int ColumnCount = 0; ColumnCount < 8; ColumnCount++)
                {
                    if (buttons[RowCount, ColumnCount] == btn)
                    {
                        return new Point(ColumnCount, RowCount);
                    }
                }
            }
            return Point.Empty;
        }

        private void ResetButtonColors()
        {
            for (int RowCount = 0; RowCount < 8; RowCount++)
            {
                for (int ColumnCount = 0; ColumnCount < 8; ColumnCount++)
                {
                    if ((RowCount + ColumnCount) % 2 == 0)
                    {
                        buttons[RowCount, ColumnCount].BackColor = Color.White;
                    }
                    else
                    {
                        buttons[RowCount, ColumnCount].BackColor = Color.DarkOliveGreen;
                    }
                }
            }
        }

        private void MovePiece(Point from, Point to)
        {
            string piece = buttons[from.Y, from.X].Text;

            if (piece == "\u2659" || piece == "\u265F")
            {
                if (IsValidPawnMove(from, to, piece))
                {
                    buttons[to.Y, to.X].Text = piece;
                    buttons[from.Y, from.X].Text = "";
                }
            }
        }

        private bool IsValidPawnMove(Point from, Point to, string piece)
        {
            int direction = (piece == "\u2659") ? -1 : 1;
            int startRow = (piece == "\u2659") ? 6 : 1;

            
            if (to.X == from.X && to.Y == from.Y + direction && string.IsNullOrEmpty(buttons[to.Y, to.X].Text))
            {
                return true;
            }

            
            if (to.X == from.X && to.Y == from.Y + 2 * direction && from.Y == startRow && string.IsNullOrEmpty(buttons[to.Y, to.X].Text) && string.IsNullOrEmpty(buttons[from.Y + direction, from.X].Text))
            {
                return true;
            }

            
            if (Math.Abs(to.X - from.X) == 1 && to.Y == from.Y + direction && !string.IsNullOrEmpty(buttons[to.Y, to.X].Text) && buttons[to.Y, to.X].ForeColor != buttons[from.Y, from.X].ForeColor)
            {
                return true;
            }

            return false;
        }
    }
}