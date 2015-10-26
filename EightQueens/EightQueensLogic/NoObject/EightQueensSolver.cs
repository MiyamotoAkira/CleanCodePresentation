using System;
using System.Collections.Generic;
using System.Linq;

namespace EightQueensLogic
{
    public class EightQueensSolver
    {
        public enum SquareStatus
        {
            Empty,
            Occupied,
            Threatened
        }

        int boardSize;

        public EightQueensSolver ()
        {
            boardSize = 8;
        }

        public List<Tuple<int,int>> Solve()
        {
            var board = CreateBoard();

            FindSolution(board);

            return ExtractSolution(board);
        }

        SquareStatus[,] CreateBoard()
        {
            SquareStatus[,] board = new SquareStatus[boardSize, boardSize];
            return board;
        }

        void FindSolution(SquareStatus[,] board)
        {
            var startingfile = 0;
            for (int rank = 0; rank < 8; rank++)
            {
                TryPlaceQueenOnRank(board, ref rank, ref startingfile);
            }
        }

        List<Tuple<int, int>> ExtractSolution(SquareStatus[,] board)
        {
            List<Tuple<int, int>> result = new List<Tuple<int, int>>();
            foreach (int rank in GetAllRanks())
            {
                foreach (int file in GetAllFiles())
                {
                    if (SquareIsOccupied(board[rank, file]))
                    {
                        result.Add(new Tuple<int, int>(rank, file));
                    }
                }
            }
            return result;
        }

        void TryPlaceQueenOnRank(SquareStatus[,] board, ref int rank, ref int startingFile)
        {
            var queenIsPlaced = TryPlaceQueenOnFile(board, rank, startingFile);

            if (queenIsPlaced)
            {
                startingFile = 0;
            }
            else
            {
                startingFile = RevertLastQueenPlacement(board, ref rank);
            }
        }
         
        bool TryPlaceQueenOnFile(SquareStatus[,] board, int rank, int startingFile)
        {
            var queenIsPlaced = false;
            for (int file = startingFile; file < boardSize; file++)
            {
                queenIsPlaced = TryPlaceQueenOnSquare(board, rank, file);
                if (queenIsPlaced)
                {
                    break;
                }
            }

            return queenIsPlaced;
        }

        bool TryPlaceQueenOnSquare(SquareStatus[,] board, int rank, int file)
        {
            if (ICanPlaceQueen(board[rank, file]))
            {
                board[rank, file] = SquareStatus.Occupied;
                MarkThreatenedSquares(board, rank, file);
                return true;
            }
            return false;
        }

        int RevertLastQueenPlacement(SquareStatus[,] board, ref int rank)
        {
            rank = MoveToPreviousRank(rank);
            var startingfile = RemoveLastQueen(board, rank);
            rank = MoveToPreviousRank(rank);
            ClearAllThreatenings(board);
            CalculateAllThreatenings(board);
            return startingfile;
        }

        static int MoveToPreviousRank(int rank)
        {
            rank = rank - 1;
            return rank;
        }

        IEnumerable<int> GetAllRanks()
        {
            return Enumerable.Range(0, boardSize);
        }
        
        IEnumerable<int> GetAllFiles()
        {
            return Enumerable.Range(0, boardSize);
        }
        
        void ThreatenFromOccupied(SquareStatus[,] board, int rank, int file)
        {
            if (SquareIsOccupied(board[rank, file]))
            {
                MarkThreatenedSquares(board, rank, file);
            }
        }

        void ClearNonOccupied(SquareStatus[,] board, int rank, int file)
        {
            if (SquareIsNotOccupied(board[rank, file]))
            {
                ClearSquare(board, rank, file);
            }
        }

        void CalculateAllThreatenings(SquareStatus[,] board)
        {
            foreach (int rank in GetAllRanks())
            {
                foreach (int file in GetAllFiles())
                {
                    ThreatenFromOccupied(board, rank, file);
                }
            }
        }
            
        void ClearAllThreatenings(SquareStatus[,] board)
        {
            foreach (int rank in GetAllRanks())
            {
                foreach (int file in GetAllFiles())
                {
                    ClearNonOccupied(board, rank, file);
                }
            }
        }

        static void ClearSquare(SquareStatus[,] board, int rank, int file)
        {
            board[rank, file] = SquareStatus.Empty;
        }

        int RemoveLastQueen(SquareStatus[,] board, int rank)
        {
            int startingfile = 0;
            foreach (int file in GetAllFiles())
            {
                if (SquareIsOccupied(board[rank, file]))
                {
                    ClearSquare(board, rank, file);
                    startingfile = file + 1;
                }
            }
            return startingfile;
        }

        void MarkThreatenedSquares(SquareStatus[,] board, int rankToClear, int fileToClear)
        {
            ThreatenSameRank(board, rankToClear);
            ThreatenSameFile(board, fileToClear);
            ThreatenRightToLeftDiagonal(board, rankToClear, fileToClear);
            ThreatenLeftToRightDiagonal(board, rankToClear, fileToClear);
        }

        void ThreatenLeftToRightDiagonal(SquareStatus[,] board, int rankToClear, int fileToClear)
        {
            ThreatenUpperRightDiagonal(board, rankToClear, fileToClear);
            ThreateLowerLeftDiagonal(board, rankToClear, fileToClear);
        }

        void ThreatenRightToLeftDiagonal(SquareStatus[,] board, int rankToClear, int fileToClear)
        {
            ThreatenUpperLeftDiagonal(board, rankToClear, fileToClear);
            ThreatenLowerRightDiagonal(board, rankToClear, fileToClear);
        }

        static void ThreatenSquare(SquareStatus[,] board, int rank, int file)
        {
            board[rank, file] = SquareStatus.Threatened;
        }

        void ThreatenNonOccupiedSquare(SquareStatus[,] board, int rank, int file)
        {
            if (SquareIsNotOccupied(board[rank, file]))
            {
                ThreatenSquare(board, rank, file);
            }
        }

        void ThreateLowerLeftDiagonal(SquareStatus[,] board, int rankToClear, int fileToClear)
        {
            for (int fileToUpdate = fileToClear, rankToUpdate = rankToClear; fileToUpdate >= 0 && rankToUpdate >= 0; fileToUpdate--, rankToUpdate--)
            {
                ThreatenNonOccupiedSquare(board, rankToUpdate, fileToUpdate);
            }
        }

        void ThreatenUpperRightDiagonal(SquareStatus[,] board, int rankToClear, int fileToClear)
        {
            for (int fileToUpdate = fileToClear, rankToUpdate = rankToClear; fileToUpdate >= 0 && rankToUpdate < boardSize; fileToUpdate--, rankToUpdate++)
            {
                ThreatenNonOccupiedSquare(board, rankToUpdate, fileToUpdate);
            }
        }

        void ThreatenLowerRightDiagonal(SquareStatus[,] board, int rankToClear, int fileToClear)
        {
            for (int fileToUpdate = fileToClear, rankToUpdate = rankToClear; fileToUpdate < boardSize && rankToUpdate >= 0; fileToUpdate++, rankToUpdate--)
            {
                ThreatenNonOccupiedSquare(board, rankToUpdate, fileToUpdate);
            }
        }

        void ThreatenUpperLeftDiagonal(SquareStatus[,] board, int rankToClear, int fileToClear)
        {
            for (int fileToUpdate = fileToClear, rankToUpdate = rankToClear; fileToUpdate < boardSize && rankToUpdate < boardSize; fileToUpdate++, rankToUpdate++)
            {
                ThreatenNonOccupiedSquare(board, rankToUpdate, fileToUpdate);
            }
        }

        void ThreatenSameFile(SquareStatus[,] board, int file)
        {
            for (int rank = 0; rank < boardSize; rank++)
            {
                ThreatenNonOccupiedSquare(board, rank, file);
            }
        }

        void ThreatenSameRank(SquareStatus[,] board, int rank)
        {
            for (int file = 0; file < boardSize; file++)
            {
                ThreatenNonOccupiedSquare(board, rank, file);
            }
        }
            
        static bool ICanPlaceQueen(SquareStatus square)
        {
            return square == SquareStatus.Empty;
        }

        bool SquareIsOccupied(SquareStatus square)
        {
            return square == SquareStatus.Occupied;
        }
        
        bool SquareIsNotOccupied(SquareStatus square)
        {
            return square != SquareStatus.Occupied;
        }

    }
}

