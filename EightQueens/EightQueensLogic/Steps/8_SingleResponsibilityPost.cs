using System;
using System.Collections.Generic;
using System.Linq;

namespace EQL_AbstractionPost
{
    public class EightQueensSolver
    {
        int boardSize;

        public EightQueensSolver ()
        {
            boardSize = 8;
        }

        public List<Tuple<int,int>> Solve()
        {
            var board = new Board(boardSize);
            FindSolution(board);
            return ExtractSolution(board);
        }

        void FindSolution(Board board)
        {
            var startingfile = 0;
            for (int rank = 0; rank < 8; rank++)
            {
                TryPlaceQueenOnRank(board, ref rank, ref startingfile);
            }
        }

        List<Tuple<int, int>> ExtractSolution(Board board)
        {
            List<Tuple<int, int>> result = new List<Tuple<int, int>>();
            board.GetAllSquares().ToList().ForEach(square => AddQueenToResult(square, result));
            return result;
        }

        void AddQueenToResult(Square square, List<Tuple<int, int>> result)
        {
            if (square.IsOccupied())
            {
                result.Add(new Tuple<int, int>(square.Rank, square.File));
            }
        }

        void TryPlaceQueenOnRank(Board board, ref int rank, ref int startingFile)
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

        bool TryPlaceQueenOnFile(Board board, int rank, int startingFile)
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

        bool TryPlaceQueenOnSquare(Board board, int rank, int file)
        {
            if (board.CanIPlaceQueen(rank, file))
            {
                board.OccupySquare(rank, file);
                board.MarkThreatenedSquares(rank, file);
                return true;
            }

            return false;
        }

        int RevertLastQueenPlacement(Board board, ref int rank)
        {
            rank = MoveToPreviousRank(rank);
            var startingFile = RemoveQueenOnRank(board, rank);
            rank = MoveToPreviousRank(rank);
            board.ClearAllThreatenings();
            board.CalculateAllThreatenings();
            return startingFile;
        }

        static int MoveToPreviousRank(int rank)
        {
            rank = rank - 1;
            return rank;
        }
            
        int RemoveQueenOnRank(Board board, int rank)
        {
            var occupiedFile = board.FindOccupiedFileOnRank(rank);
            board.ClearSquare(rank, occupiedFile);
            return MoveToNextFile(occupiedFile);
        }

        static int MoveToNextFile(int occupiedFile)
        {
            return occupiedFile + 1;
        }
    }

    public class Board 
    {
        private Square[,] squares;

        int boardSize;

        public Board (int boardSize)
        {
            this.boardSize = boardSize;
            squares = new Square[boardSize, boardSize];
            InitializeSquares();
        }

        void InitializeSquares()
        {
            foreach (int rank in GetAllRanks())
            {
                foreach (int file in GetAllFiles())
                {
                    squares[rank, file] = Square.WithRankAndFile(rank, file);
                }
            }
        }

        public bool CanIPlaceQueen(int rank, int file)
        {
            return squares[rank, file].IsEmpty();
        }

        public void MarkThreatenedSquares(int rankToClear, int fileToClear)
        {
            ThreatenSameRank(rankToClear);
            ThreatenSameFile(fileToClear);
            ThreatenRightToLeftDiagonal(rankToClear, fileToClear);
            ThreatenLeftToRightDiagonal(rankToClear, fileToClear);
        }

        public void ThreatenLeftToRightDiagonal(int rankToClear, int fileToClear)
        {
            ThreatenUpperRightDiagonal(rankToClear, fileToClear);
            ThreateLowerLeftDiagonal(rankToClear, fileToClear);
        }

        public void ThreatenRightToLeftDiagonal(int rankToClear, int fileToClear)
        {
            ThreatenUpperLeftDiagonal(rankToClear, fileToClear);
            ThreatenLowerRightDiagonal(rankToClear, fileToClear);
        }

        public void ThreatenSquare(int rank, int file)
        {
            squares[rank, file].ThreatenSquare();
        }

        public void ThreatenNonOccupiedSquare(int rank, int file)
        {
            if (SquareIsNotOccupied(rank, file))
            {
                ThreatenSquare(rank, file);
            }
        }

        public int FindOccupiedFileOnRank(int rank)
        {
            foreach (int file in GetAllFiles())
            {
                if (squares[rank,file].IsOccupied())
                {
                    return file;
                }
            }

            return 0;
        }

        public void ThreateLowerLeftDiagonal(int rankToClear, int fileToClear)
        {
            for (int fileToUpdate = fileToClear, rankToUpdate = rankToClear;
                fileToUpdate >= 0 && rankToUpdate >= 0;
                fileToUpdate--, rankToUpdate--)
            {
                ThreatenNonOccupiedSquare(rankToUpdate, fileToUpdate);
            }
        }

        public void ThreatenUpperRightDiagonal(int rankToClear, int fileToClear)
        {
            for (int fileToUpdate = fileToClear, rankToUpdate = rankToClear;
                fileToUpdate >= 0 && rankToUpdate < boardSize;
                fileToUpdate--, rankToUpdate++)
            {
                ThreatenNonOccupiedSquare(rankToUpdate, fileToUpdate);
            }
        }

        public void ThreatenLowerRightDiagonal(int rankToClear, int fileToClear)
        {
            for (int fileToUpdate = fileToClear, rankToUpdate = rankToClear;
                fileToUpdate < boardSize && rankToUpdate >= 0;
                fileToUpdate++, rankToUpdate--)
            {
                ThreatenNonOccupiedSquare(rankToUpdate, fileToUpdate);
            }
        }

        public void ThreatenUpperLeftDiagonal(int rankToClear, int fileToClear)
        {
            for (int fileToUpdate = fileToClear, rankToUpdate = rankToClear;
                fileToUpdate < boardSize && rankToUpdate < boardSize;
                fileToUpdate++, rankToUpdate++)
            {
                ThreatenNonOccupiedSquare(rankToUpdate, fileToUpdate);
            }
        }

        public void ThreatenSameFile(int file)
        {
            foreach(int rank in GetAllRanks())
            {
                ThreatenNonOccupiedSquare(rank, file);
            }
        }

        public void ThreatenSameRank(int rank)
        {
            foreach (int file in GetAllFiles())
            {
                ThreatenNonOccupiedSquare(rank, file);
            }
        }

        public bool SquareIsOccupied(int rank, int file)
        {
            return squares[rank, file].IsOccupied();
        }

        public bool SquareIsNotOccupied(int rank, int file)
        {
            return squares[rank, file].IsNotOccupied();
        }

        public void ThreatenFromOccupied(int rank, int file)
        {
            if (SquareIsOccupied(rank, file))
            {
                MarkThreatenedSquares(rank, file);
            }
        }

        public void ClearNonOccupied(Square square)
        {
            if (square.IsNotOccupied())
            {
                square.ClearSquare();
            }
        }

        public void CalculateAllThreatenings()
        {
            foreach (int rank in GetAllRanks())
            {
                foreach (int file in GetAllFiles())
                {
                    ThreatenFromOccupied(rank, file);
                }
            }
        }

        public void ClearAllThreatenings()
        {
            foreach (Square square in GetAllSquares())
            {
               ClearNonOccupied(square);
            }
        }

        public IEnumerable<Square> GetAllSquares()
        {
            foreach(int rank in GetAllRanks())
            {
                foreach (int file in GetAllFiles())
                {
                    yield return squares[rank, file];
                }
            }
        }

        public void ClearSquare(int rank, int file)
        {
            squares[rank, file].ClearSquare();
        }

        public IEnumerable<int> GetAllRanks()
        {
            return Enumerable.Range(0, boardSize);
        }

        public IEnumerable<int> GetAllFiles()
        {
            return Enumerable.Range(0, boardSize);
        }

        public void OccupySquare (int rank, int file)
        {
            squares[rank, file].OccupySquare();
        }
    }

    public class Square
    {
        SquareStatus status;
        int rank;
        int file;

        private Square()
        {
            
        }

        public static Square WithRankAndFile(int rank, int file)
        {
            var square = new Square();
            square.rank = rank;
            square.file = file;
            return square;
        }

        public SquareStatus Status
        {
            get { return status;}
        }

        public int Rank
        {
            get {return rank;}
        }

        public int File
        {
            get {return file;}
        }

        public void OccupySquare()
        {
            status = SquareStatus.Occupied;
        }

        public void ClearSquare()
        {
            status = SquareStatus.Empty;
        }

        public void ThreatenSquare()
        {
            status = SquareStatus.Threatened;
        }

        public bool IsEmpty()
        {
            return status == SquareStatus.Empty;
        }

        public bool IsOccupied()
        {
            return status == SquareStatus.Occupied;
        }

        public bool IsNotOccupied()
        {
            return status != SquareStatus.Occupied;
        }
    }

    public enum SquareStatus
    {
        Empty,
        Occupied,
        Threatened
    }
}

