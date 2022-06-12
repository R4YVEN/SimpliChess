using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SimpliChess.definitions;

namespace SimpliChess
{
    public static class movemath
    {
        public static IEnumerable<Move> get_all_evaluated_moves(Board board, ChessColor color)
        {
            List<Move> allMoves = new List<Move>();
            for(int i = 0; i < board.pieces.Count; i++)
            {
                Piece piece = board.pieces[i];

                if (piece == null)
                    continue;

                if (piece.color != color)
                    continue;

                List<Location> moves;
                if (piece.color == ChessColor.WHITE)
                    moves = get_possible_moves_for_piece(board.get_pieces_for_color(ChessColor.BLACK), board.get_pieces_for_color(ChessColor.WHITE), piece).ToList();
                else
                    moves = get_possible_moves_for_piece(board.get_pieces_for_color(ChessColor.WHITE), board.get_pieces_for_color(ChessColor.BLACK), piece).ToList();
                
                for(int j = 0; j < moves.Count; j++)
                {
                    allMoves.Add(new Move(piece.location, moves[j], piece, 0, 0));
                }

                for(int j = 0; j < allMoves.Count; j++)
                {
                    Move mov = allMoves[j];
                    int scoreForMov = evaluator.get_score_for_move(board, mov, color);
                    mov.scoreWhite = color == ChessColor.WHITE ? scoreForMov : -scoreForMov;
                    mov.scoreBlack = color == ChessColor.WHITE ? -scoreForMov : scoreForMov;

                    allMoves[j] = mov;
                }
            }

            return allMoves;
        }

        //thanks github
        //all code below is not mine (only edited slightly)
        public static IEnumerable<Location> get_possible_moves_for_piece(List<Piece> opponent, List<Piece> self, Piece piece)
        {
            if (piece.type == PieceType.pawn)
            {
                return get_pawn_moves(opponent, self, piece.location.x, piece.location.y, piece.color == ChessColor.WHITE, !((piece.color == ChessColor.WHITE && piece.location.y == 1) || (!(piece.color == ChessColor.WHITE) && piece.location.y == 6)));
            }
            if (piece.type == PieceType.rook)
            {
                return get_rook_moves(opponent, self, piece.location.x, piece.location.y);
            }
            if (piece.type == PieceType.knight)
            {
                return get_knight_moves(self, piece.location.x, piece.location.y);
            }
            if (piece.type == PieceType.bishop)
            {
                return get_bishop_moves(opponent, self, piece.location.x, piece.location.y);
            }
            if (piece.type == PieceType.queen)
            {
                return get_queen_moves(opponent, self, piece.location.x, piece.location.y);
            }
            if (piece.type == PieceType.king)
            {
                return get_king_moves(self, piece.location.x, piece.location.y);
            }
            return null;
        }

        private static IEnumerable<Location> get_king_moves(List<Piece> allies, int x, int y)
        {
            List<Piece> possiblePieces =
                    allies.Where(piece => piece.location.x <= x + 1 && piece.location.x >= x - 1 && piece.location.y >= y - 1 && piece.location.y <= y + 1)
                                .ToList();
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (x + i >= 0 && x + i <= 7 && y + j >= 0 && y + j <= 7 &&
                            !possiblePieces.Any(piece => piece.location.x == x + i && piece.location.y == y + j))
                    {
                        yield return new Location(x + i, y + j);
                    }
                }
            }
        }

        private static IEnumerable<Location> get_queen_moves(List<Piece> opponent, List<Piece> allies, int x, int y)
        {
            for (int i = 1; i < 8; i++)
            {
                if (y + i > 7 || allies.Any(piece => (piece.location.x == x) && (piece.location.y == y + i)))
                {
                    break;
                }

                yield return new Location(x, y + i);

                if (opponent.Any(piece => (piece.location.x == x) && (piece.location.y == y + i)))
                {
                    break;
                }
            }
            for (int i = 1; i < 8; i++)
            {
                if (y - i < 0 || allies.Any(piece => (piece.location.x == x) && (piece.location.y == y - i)))
                {
                    break;
                }

                yield return new Location(x, y - i);

                if (opponent.Any(piece => (piece.location.x == x) && (piece.location.y == y - i)))
                {
                    break;
                }
            }
            for (int i = 1; i < 8; i++)
            {
                if (x + i > 7 || allies.Any(piece => (piece.location.x == x + i) && (piece.location.y == y)))
                {
                    break;
                }

                yield return new Location(x + i, y);

                if (opponent.Any(piece => (piece.location.x == x + i) && (piece.location.y == y)))
                {
                    break;
                }
            }
            for (int i = 1; i < 8; i++)
            {
                if (x - i < 0 || allies.Any(piece => (piece.location.x == x - i) && (piece.location.y == y)))
                {
                    break;
                }

                yield return new Location(x - i, y);

                if (opponent.Any(piece => (piece.location.x == x - i) && (piece.location.y == y)))
                {
                    break;
                }
            }
            for (int i = 1; i < 8; i++)
            {
                if (x + i > 7 || y + i > 7 || allies.Any(piece => (piece.location.x == x + i) && (piece.location.y == y + i)))
                {
                    break;
                }

                yield return new Location(x + i, y + i);

                if (opponent.Any(piece => (piece.location.x == x + i) && (piece.location.y == y + i)))
                {
                    break;
                }
            }
            for (int i = 1; i < 8; i++)
            {
                if (x - i < 0 || y + i > 7 || allies.Any(piece => (piece.location.x == x - i) && (piece.location.y == y + i)))
                {
                    break;
                }

                yield return new Location(x - i, y + i);

                if (opponent.Any(piece => (piece.location.x == x - i) && (piece.location.y == y + i)))
                {
                    break;
                }
            }
            for (int i = 1; i < 8; i++)
            {
                if (x - i < 0 || y - i < 0 || allies.Any(piece => (piece.location.x == x - i) && (piece.location.y == y - i)))
                {
                    break;
                }

                yield return new Location(x - i, y - i);

                if (opponent.Any(piece => (piece.location.x == x - i) && (piece.location.y == y - i)))
                {
                    break;
                }
            }
            for (int i = 1; i < 8; i++)
            {
                if (x + i > 7 || y - i < 0 || allies.Any(piece => (piece.location.x == x + i) && (piece.location.y == y - i)))
                {
                    break;
                }

                yield return new Location(x + i, y - i);

                if (opponent.Any(piece => (piece.location.x == x + i) && (piece.location.y == y - i)))
                {
                    break;
                }
            }
        }

        private static IEnumerable<Location> get_bishop_moves(List<Piece> opponent, List<Piece> allies, int x, int y)
        {
            for (int i = 1; i < 8; i++)
            {
                if (x + i > 7 || y + i > 7 || allies.Any(piece => (piece.location.x == x + i) && (piece.location.y == y + i)))
                {
                    break;
                }

                yield return new Location(x + i, y + i);

                if (opponent.Any(piece => (piece.location.x == x + i) && (piece.location.y == y + i)))
                {
                    break;
                }
            }
            for (int i = 1; i < 8; i++)
            {
                if (x - i < 0 || y + i > 7 || allies.Any(piece => (piece.location.x == x - i) && (piece.location.y == y + i)))
                {
                    break;
                }

                yield return new Location(x - i, y + i);

                if (opponent.Any(piece => (piece.location.x == x - i) && (piece.location.y == y + i)))
                {
                    break;
                }
            }
            for (int i = 1; i < 8; i++)
            {
                if (x - i < 0 || y - i < 0 || allies.Any(piece => (piece.location.x == x - i) && (piece.location.y == y - i)))
                {
                    break;
                }

                yield return new Location(x - i, y - i);

                if (opponent.Any(piece => (piece.location.x == x - i) && (piece.location.y == y - i)))
                {
                    break;
                }
            }
            for (int i = 1; i < 8; i++)
            {
                if (x + i > 7 || y - i < 0 || allies.Any(piece => (piece.location.x == x + i) && (piece.location.y == y - i)))
                {
                    break;
                }

                yield return new Location(x + i, y - i);

                if (opponent.Any(piece => (piece.location.x == x + i) && (piece.location.y == y - i)))
                {
                    break;
                }
            }
        }

        private static IEnumerable<Location> get_knight_moves(List<Piece> allies, int x, int y)
        {
            var possiblePieces = allies.Where(piece =>
                                                                                                ((piece.location.y == y + 2 || piece.location.y == y - 2) &&
                                                                                                 (piece.location.x == x - 1 || piece.location.x == x + 1)) ||
                                                                                                ((piece.location.x == x + 2) || piece.location.x == x - 2) &&
                                                                                                (piece.location.y == y - 1 || piece.location.y == y + 1)).ToList();


            if (x + 1 <= 7 && y + 2 <= 7 && !possiblePieces.Any(piece => piece.location.x == x + 1 && piece.location.y == y + 2))
            {
                yield return new Location(x + 1, y + 2);
            }
            if (x + 2 <= 7 && y + 1 <= 7 && !possiblePieces.Any(piece => piece.location.x == x + 2 && piece.location.y == y + 1))
            {
                yield return new Location(x + 2, y + 1);
            }
            if (x - 1 >= 0 && y + 2 <= 7 && !possiblePieces.Any(piece => piece.location.x == x - 1 && piece.location.y == y + 2))
            {
                yield return new Location(x - 1, y + 2);
            }
            if (x - 2 >= 0 && y + 1 <= 7 && !possiblePieces.Any(piece => piece.location.x == x - 2 && piece.location.y == y + 1))
            {
                yield return new Location(x - 2, y + 1);
            }
            if (x + 1 <= 7 && y - 2 >= 0 && !possiblePieces.Any(piece => piece.location.x == x + 1 && piece.location.y == y - 2))
            {
                yield return new Location(x + 1, y - 2);
            }
            if (x + 2 <= 7 && y - 1 >= 0 && !possiblePieces.Any(piece => piece.location.x == x + 2 && piece.location.y == y - 1))
            {
                yield return new Location(x + 2, y - 1);
            }
            if (x - 1 >= 0 && y - 2 >= 0 && !possiblePieces.Any(piece => piece.location.x == x - 1 && piece.location.y == y - 2))
            {
                yield return new Location(x - 1, y - 2);
            }
            if (x - 2 >= 0 && y - 1 >= 0 && !possiblePieces.Any(piece => piece.location.x == x - 2 && piece.location.y == y - 1))
            {
                yield return new Location(x - 2, y - 1);
            }
        }

        private static IEnumerable<Location> get_rook_moves(List<Piece> opponent, List<Piece> allies, int x, int y)
        {
            List<Piece> possibleAllies = allies.Where(piece => piece.location.x == x || piece.location.y == y).ToList();
            List<Piece> possibleEnemies = opponent.Where(piece => piece.location.x == x || piece.location.y == y).ToList();

            for (int i = 1; i < 8; i++)
            {
                if (y + i > 7 || possibleAllies.Any(piece => (piece.location.x == x) && (piece.location.y == y + i)))
                {
                    break;
                }

                yield return new Location(x, y + i);

                if (possibleEnemies.Any(piece => (piece.location.x == x) && (piece.location.y == y + i)))
                {
                    break;
                }
            }
            for (int i = 1; i < 8; i++)
            {
                if (y - i < 0 || possibleAllies.Any(piece => (piece.location.x == x) && (piece.location.y == y - i)))
                {
                    break;
                }

                yield return new Location(x, y - i);

                if (possibleEnemies.Any(piece => (piece.location.x == x) && (piece.location.y == y - i)))
                {
                    break;
                }
            }
            for (int i = 1; i < 8; i++)
            {
                if (x + i > 7 || possibleAllies.Any(piece => (piece.location.x == x + i) && (piece.location.y == y)))
                {
                    break;
                }

                yield return new Location(x + i, y);

                if (possibleEnemies.Any(piece => (piece.location.x == x + i) && (piece.location.y == y)))
                {
                    break;
                }
            }
            for (int i = 1; i < 8; i++)
            {
                if (x - i < 0 || possibleAllies.Any(piece => (piece.location.x == x - i) && (piece.location.y == y)))
                {
                    break;
                }

                yield return new Location(x - i, y);

                if (possibleEnemies.Any(piece => (piece.location.x == x - i) && (piece.location.y == y)))
                {
                    break;
                }
            }
        }

        private static IEnumerable<Location> get_pawn_moves(List<Piece> opponent, List<Piece> allies, int x, int y, Boolean isMovingUp, Boolean hasMoved)
        {
            if (isMovingUp)
            {
                if (!opponent.Any(piece => (piece.location.x == x) && (piece.location.y == y + 1)) && !allies.Any(piece => (piece.location.x == x) && (piece.location.y == y + 1)))
                {
                    yield return new Location(x, y + 1);
                }
                if (hasMoved == false && !opponent.Any(piece => (piece.location.x == x) && (piece.location.y == y + 2)) && !allies.Any(piece => (piece.location.x == x) && (piece.location.y == y + 2)) && !opponent.Any(piece => (piece.location.x == x) && (piece.location.y == y + 1)) && !allies.Any(piece => (piece.location.x == x) && (piece.location.y == y + 1)))
                {
                    yield return new Location(x, y + 2);
                }
                if (x - 1 >= 0 && y + 1 >= 0 && opponent.Any(piece => (piece.location.x == x - 1) && (piece.location.y == y + 1)))
                {
                    yield return new Location(x - 1, y + 1);
                }
                if (x + 1 < 8 && y + 1 >= 0 && opponent.Any(piece => (piece.location.x == x + 1) && (piece.location.y == y + 1)))
                {
                    yield return new Location(x + 1, y + 1);
                }
            }
            if (!isMovingUp)
            {
                if (!opponent.Any(piece => (piece.location.x == x) && (piece.location.y == y - 1)) && !allies.Any(piece => (piece.location.x == x) && (piece.location.y == y - 1)))
                {
                    yield return new Location(x, y - 1);
                }
                if (hasMoved == false && !opponent.Any(piece => (piece.location.x == x) && (piece.location.y == y - 2)) && !allies.Any(piece => (piece.location.x == x) && (piece.location.y == y - 2)) && !opponent.Any(piece => (piece.location.x == x) && (piece.location.y == y - 1)) && !allies.Any(piece => (piece.location.x == x) && (piece.location.y == y - 1)))
                {
                    yield return new Location(x, y - 2);
                }
                if (x - 1 >= 0 && y - 1 < 8 && opponent.Any(piece => (piece.location.x == x - 1) && (piece.location.y == y - 1)))
                {
                    yield return new Location(x - 1, y - 1);
                }
                if (x + 1 < 8 && y - 1 < 8 && opponent.Any(piece => (piece.location.x == x + 1) && (piece.location.y == y - 1)))
                {
                    yield return new Location(x + 1, y - 1);
                }
            }
        }
    }
}
