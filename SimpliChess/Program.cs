using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SimpliChess.definitions;
using static SimpliChess.utils;

namespace SimpliChess
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 1 && args[0] == "calc")
            {
                string fen = args[1];
                string team = args[2];
                int depth = int.Parse(args[3]);

                Board board = fen_to_board(fen);
                Move bestMove = evaluator.get_best_move_for_depth(board, team == "w" ? (int)ChessColor.WHITE : (int)ChessColor.BLACK, depth);

                Console.WriteLine((utils.int_to_column(bestMove.from.x) + ""+ (bestMove.from.y + 1)) + "" + (utils.int_to_column(bestMove.to.x) + "" + (bestMove.to.y + 1)));
            }
            else
            {
                string fen = "rnbqkbnr/pppp1ppp/4p3/8/8/3P4/PPP1PPPP/RNBQKBNR w KQkq - 0 1";

                Board board = fen_to_board(fen);
                print_board(board);

                Move bestMove = evaluator.get_best_move_for_depth(board, (int)ChessColor.BLACK, 4);
                Console.WriteLine("Best Move: " + (bestMove.piece.color) + " " + bestMove.piece.notation + " | from " + (utils.int_to_column(bestMove.from.x) + "" + bestMove.from.y) + " to " + (utils.int_to_column(bestMove.to.x) + "" + bestMove.to.y) + " | scoreForWhite: " + bestMove.scoreWhite + " | scoreForBlack: " + bestMove.scoreBlack);

                Console.ReadKey();
            }
        }

        public static void print_board(Board board)
        {
            for(int y = 7; y >= 0; y--)
            {
                Console.Write(y + "|");
                for(int x = 0; x < 8; x++)
                {
                    Location location = coords_to_location(x, y);
                    Piece piece = board.piece_by_location(location);

                    if (piece != null)
                        Console.Write(" " + piece.notation + " ");
                    else
                        Console.Write(" " + "-" + " ");
                }
                Console.WriteLine();
            }

            Console.Write("  ");
            Console.Write("------------------------");
            Console.WriteLine();
            Console.Write("  ");
            string lowerLetters = "abcdefgh";
            foreach(char c in lowerLetters)
                Console.Write(" " + c + " ");

            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
