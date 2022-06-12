using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SimpliChess
{
    public static class definitions
    {
        public enum ChessColor
        {
            BLACK = -1,
            WHITE = 1
        }

        public enum PieceWorth
        {
            pawn = 100,
            knight = 300,
            bishop = 300,
            rook = 500,
            queen = 900,
            king = 100000
        }

        public enum PieceType
        {
            pawn,
            knight,
            bishop,
            rook,
            queen,
            king
        }

        public class Piece
        {
            public ChessColor color;
            public PieceType type;
            public char notation;
            public string name;
            public Location location;
            public PieceWorth worth;
        }

        public struct Location
        {
            public int x;
            public int y;

            public Location(int _x, int _y)
            {
                x = _x;
                y = _y;
            }
        }

        public struct Move
        {
            public int scoreWhite;
            public int scoreBlack;
            public Location from;
            public Location to;
            public Piece piece;

            public Move(Location _from, Location _to, Piece _piece, int _scoreWhite, int _scoreBlack)
            {
                from = _from;
                to = _to;
                piece = _piece;
                scoreWhite = _scoreWhite;
                scoreBlack = _scoreBlack;
            }
        }

        public class Board
        {
            public List<Piece> pieces;
            //public List<Square> squares;
            public bool whiteTurn;

            public void move_piece(Location _from, Location _to)
            {
                Piece from = piece_by_location(_from);
                Piece to = piece_by_location(_to);

                from.location = _to;

                if(to != null)
                    pieces.Remove(to);
            }

            public Piece piece_by_location(Location location)
            {
                for (int i = 0; i < pieces.Count; i++)
                {
                    if (this.pieces[i].location.x == location.x)
                        if(this.pieces[i].location.y == location.y)
                            return this.pieces[i];
                }

                return null;
            }

            public List<Piece> get_pieces_for_color(ChessColor color)
            {
                return this.pieces.Where(piece => piece.color == color).ToList();
            }
        }
    }
}
