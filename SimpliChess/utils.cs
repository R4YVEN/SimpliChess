using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SimpliChess.definitions;

namespace SimpliChess
{
    public static class utils
    {
        public static Board clone_board(Board board)
        {
            Board copyBoard = new Board();
            copyBoard.pieces = new List<Piece>();

            foreach(Piece piece in board.pieces)
            {
                Piece copyPiece = null;

                copyPiece = new Piece();
                copyPiece.name = piece.name;
                copyPiece.notation = piece.notation;
                copyPiece.color = piece.color;
                copyPiece.type = piece.type;
                copyPiece.worth = piece.worth;
                copyPiece.location = piece.location;

                copyBoard.pieces.Add(copyPiece);
            }

            
            copyBoard.whiteTurn = board.whiteTurn;
            return copyBoard;
        }
        public static Board fen_to_board(string fen)
        {
            string[] fenParts = fen.Split(' ');
            string rawPieces = fenParts[0];
            string[] rows = rawPieces.Split('/');

            bool whiteTurn = fenParts[1] == "w";

            Board board = create_board();
            board.whiteTurn = whiteTurn;

            int y = 7;
            foreach (string row in rows)
            {
                int x = 0;
                foreach (char c in row)
                {
                    if (Char.IsDigit(c))
                    {
                        x += int.Parse(c.ToString());
                        continue;
                    }

                    PieceType type = notation_to_type(c);

                    Piece piece = new Piece();
                    piece.type = type;
                    piece.notation = c;
                    piece.color = Char.IsUpper(c) ? ChessColor.WHITE : ChessColor.BLACK;
                    piece.worth = type_to_worth(type);
                    piece.location = new Location(x, y);

                    board.pieces.Add(piece);

                    x++;
                }
                y--;
            }

            return board;
        }

        public static Location coords_to_location(int x, int y)
        {
            Location location = new Location(x, y);
            return location;
        }

        public static char int_to_column(int i)
        {
            switch (i)
            {
                case 0:
                    return 'a';
                case 1:
                    return 'b';
                case 2:
                    return 'c';
                case 3:
                    return 'd';
                case 4:
                    return 'e';
                case 5:
                    return 'f';
                case 6:
                    return 'g';
                case 7:
                    return 'h';
            }

            return ' ';
        }

        public static Board create_board()
        {
            Board board = new Board();
            board.pieces = new List<Piece>();

            return board;
        }

        public static PieceWorth type_to_worth(PieceType t)
        {
            switch (t)
            {
                case PieceType.pawn:
                    return PieceWorth.pawn;
                case PieceType.knight:
                    return PieceWorth.knight;
                case PieceType.bishop:
                    return PieceWorth.bishop;
                case PieceType.rook:
                    return PieceWorth.rook;
                case PieceType.queen:
                    return PieceWorth.queen;
            }

            return PieceWorth.king;
        }

        public static PieceType notation_to_type(char n)
        {
            switch (Char.ToLower(n))
            {
                case 'p':
                    return PieceType.pawn;
                case 'n':
                    return PieceType.knight;
                case 'b':
                    return PieceType.bishop;
                case 'r':
                    return PieceType.rook;
                case 'q':
                    return PieceType.queen;
            }

            return PieceType.king;
        }
    }
}
