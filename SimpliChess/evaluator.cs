using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SimpliChess.definitions;

namespace SimpliChess
{
    public static class evaluator
    {
        public static ChessColor teamToEval;
        public static int alpha_beta_max(Board board, int alpha, int beta, int depthleft, int turn)
        {
            if (depthleft == 0) return get_score_for_board(board, (ChessColor)teamToEval);

            var movs = movemath.get_all_evaluated_moves(board, (ChessColor)turn);
            foreach(Move mov in movs)
            {
                Board newBoard = utils.clone_board(board);
                newBoard.move_piece(mov.from, mov.to);
                int score = alpha_beta_min(newBoard, alpha, beta, depthleft - 1, -turn);

                if (score >= beta)
                    return beta;
                if (score > alpha)
                    alpha = score;
            }

            return alpha;
        }

        public static int alpha_beta_min(Board board, int alpha, int beta, int depthleft, int turn)
        {
            if (depthleft == 0) return get_score_for_board(board, (ChessColor)teamToEval);

            var movs = movemath.get_all_evaluated_moves(board, (ChessColor)turn);
            foreach (Move mov in movs)
            {
                Board newBoard = utils.clone_board(board);
                newBoard.move_piece(mov.from, mov.to);
                int score = alpha_beta_max(newBoard, alpha, beta, depthleft - 1, -turn);

                if (score <= alpha)
                    return alpha;
                if (score < beta)
                    beta = score;
            }

            return beta;
        }

        public static Move get_best_move_for_depth(Board board, int team, int depth)
        {
            Move bestMove = new Move();
            int bestScore = int.MinValue;

            var movs = movemath.get_all_evaluated_moves(board, (ChessColor)team);

            teamToEval = (ChessColor)team;

            int movsCount = movs.Count();
            int currentMov = 0;
            Parallel.ForEach(movs, new ParallelOptions { MaxDegreeOfParallelism = 50}, mov =>
            {
                Board newBoard = utils.clone_board(board);
                newBoard.move_piece(mov.from, mov.to);

                int score = alpha_beta_min(newBoard, -100000000, 100000000, depth, -team);
                Console.WriteLine("Move (" + currentMov + "/" + movsCount + ")" + ": " +
                        (mov.piece.color) + " " + mov.piece.notation +
                        " | from " + (utils.int_to_column(mov.from.x) + "" + mov.from.y) +
                        " to " + (utils.int_to_column(mov.to.x) + "" + mov.to.y) +
                        " | score: " + score);
                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = mov;

                    bestMove.scoreWhite = (ChessColor)team == ChessColor.WHITE ? score : -score;
                    bestMove.scoreBlack = -bestMove.scoreWhite;
                }

                currentMov++;
            });

            if(bestMove.piece == null)
                Console.WriteLine("Move empty?");

            return bestMove;
        }

        public static ChessColor myColor;
        private static int _internal_get_best_move_for_board_depth(Board board, ChessColor color, int depthLeft)
        {
            if (depthLeft == 0)
                return get_score_for_board(board, color);

            var movs = movemath.get_all_evaluated_moves(board, color);

            int score = -9999999;
            int bestScore = -9999999;
            for(int i = 0; i < movs.Count(); i++)
            {
                Move mov = movs.ElementAt(i);

                Board newBoard = utils.clone_board(board);
                newBoard.move_piece(mov.from, mov.to);

                score = _internal_get_best_move_for_board_depth(newBoard, color == ChessColor.WHITE ? ChessColor.BLACK : ChessColor.WHITE, depthLeft - 1);
                if (score > bestScore)
                    bestScore = score;
            }

            return bestScore;
        }

        public static int get_score_for_board(Board board, ChessColor color)
        {
            int score = 0;
            if (color == ChessColor.WHITE)
            {
                foreach (Piece piece in board.pieces)
                {
                    if (piece.color == ChessColor.WHITE)
                        score += (int)piece.worth;
                    else
                        score -= (int)piece.worth;
                }
            }
            else
            {
                foreach (Piece piece in board.pieces)
                {
                    if (piece.color == ChessColor.WHITE)
                        score -= (int)piece.worth;
                    else
                        score += (int)piece.worth;
                }
            }

            return score;
        }

        public static int get_score_for_move(Board board, Move mov, ChessColor color)
        {
            Board copyBoard = board;
            copyBoard.move_piece(mov.from, mov.to);

            int score = 0;
            if (color == ChessColor.WHITE)
            {
                for (int i = 0; i < copyBoard.pieces.Count; i++)
                {
                    Piece piece = copyBoard.pieces[i];
                    if (piece.color == ChessColor.WHITE)
                        score += (int)piece.worth;
                    else
                        score -= (int)piece.worth;
                }
            }
            else
            {
                for (int i = 0; i < copyBoard.pieces.Count; i++)
                {
                    Piece piece = copyBoard.pieces[i];
                    if (piece.color == ChessColor.WHITE)
                        score -= (int)piece.worth;
                    else
                        score += (int)piece.worth;
                }
            }

            copyBoard.move_piece(mov.to, mov.from);

            return score;
        }

        public static Move get_best_move_from_moves(IEnumerable<Move> moves, ChessColor color)
        {
            Move bestMove = new Move();
            int bestScore = -999999;
            foreach (Move mov in moves)
            {
                if (color == ChessColor.WHITE ? (mov.scoreWhite > bestScore) : (mov.scoreBlack > bestScore))
                {
                    bestScore = color == ChessColor.WHITE ? mov.scoreWhite : mov.scoreBlack;
                    bestMove = mov;
                }
            }

            return bestMove;
        }
    }
}
