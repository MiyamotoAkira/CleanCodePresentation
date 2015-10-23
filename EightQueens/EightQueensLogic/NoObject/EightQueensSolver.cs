using System;
using System.Collections.Generic;

namespace EightQueensLogic
{
    public class EightQueensSolver
    {
        public enum CellStatus
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

        CellStatus[,] CreateBoard()
        {
            CellStatus[,] board = new CellStatus[boardSize, boardSize];
            return board;
        }

        void FindSolution(CellStatus[,] board)
        {
            var startingColumn = 0;
            for (int row = 0; row < 8; row++)
            {
                TryPlaceQueenOnRow(board, ref row, ref startingColumn);
            }
        }

        List<Tuple<int, int>> ExtractSolution(CellStatus[,] board)
        {
            List<Tuple<int, int>> result = new List<Tuple<int, int>>();
            for (int row = 0; row < boardSize; row++)
            {
                for (int column = 0; column < boardSize; column++)
                {
                    if (CellIsOccupied(board[row, column]))
                    {
                        result.Add(new Tuple<int, int>(row, column));
                    }
                }
            }
            return result;
        }

        void TryPlaceQueenOnRow(CellStatus[,] board, ref int row, ref int startingColumn)
        {
            var queenIsPlaced = TryPlaceQueenOnColumn(board, row, startingColumn);

            if (queenIsPlaced)
            {
                startingColumn = 0;
            }
            else
            {
                startingColumn = RevertLastQueenPlacement(board, ref row);
            }
        }
         
        bool TryPlaceQueenOnColumn(CellStatus[,] board, int row, int startingColumn)
        {
            var queenIsPlaced = false;
            for (int column = startingColumn; column < boardSize; column++)
            {
                queenIsPlaced = TryPlaceQueenOnCell(board, row, column);
                if (queenIsPlaced)
                {
                    break;
                }
            }

            return queenIsPlaced;
        }

        bool TryPlaceQueenOnCell(CellStatus[,] board, int row, int column)
        {
            if (ICanPlaceQueen(board[row, column]))
            {
                board[row, column] = CellStatus.Occupied;
                MarkThreatenedCells(board, row, column);
                return true;
            }
            return false;
        }

        int RevertLastQueenPlacement(CellStatus[,] board, ref int row)
        {
            row = MoveToPreviousRow(row);
            var startingColumn = RemoveLastQueen(board, row);
            row = MoveToPreviousRow(row);
            ClearAllThreatenings(board);
            CalculateAllThreatenings(board);
            return startingColumn;
        }

        static int MoveToPreviousRow(int row)
        {
            row = row - 1;
            return row;
        }

        void CalculateAllThreatenings(CellStatus[,] board)
        {
            for (int rowToClear = 0; rowToClear < boardSize; rowToClear++)
            {
                for (int columnToClear = 0; columnToClear < boardSize; columnToClear++)
                {
                    if (CellIsOccupied(board[rowToClear, columnToClear]))
                    {
                        MarkThreatenedCells(board, rowToClear, columnToClear);
                    }
                }
            }
        }

        void ClearAllThreatenings(CellStatus[,] board)
        {
            for (int rowToUpdate = 0; rowToUpdate < boardSize; rowToUpdate++)
            {
                for (int column = 0; column < boardSize; column++)
                {
                    if (CellIsNotOccupied(board[rowToUpdate, column]))
                    {
                        ClearCell(board, rowToUpdate, column);
                    }
                }
            }
        }

        static void ClearCell(CellStatus[,] board, int row, int column)
        {
            board[row, column] = CellStatus.Empty;
        }

        int RemoveLastQueen(CellStatus[,] board, int row)
        {
            int startingColumn = 0;
            for (int column = 0; column < boardSize; column++)
            {
                if (CellIsOccupied(board[row, column]))
                {
                    ClearCell(board, row, column);
                    startingColumn = column + 1;
                }
            }
            return startingColumn;
        }

        void MarkThreatenedCells(CellStatus[,] board, int rowToClear, int columnToClear)
        {
            ThreatenSameRow(board, rowToClear);

            ThreatenSameColumn(board, columnToClear);

            ThreatenRightToLeftDiagonal(board, rowToClear, columnToClear);

            ThreatenLeftToRightDiagonal(board, rowToClear, columnToClear);
        }

        void ThreatenLeftToRightDiagonal(CellStatus[,] board, int rowToClear, int columnToClear)
        {
            ThreatenUpperRightDiagonal(board, rowToClear, columnToClear);
            ThreateLowerLeftDiagonal(board, rowToClear, columnToClear);
        }

        void ThreatenRightToLeftDiagonal(CellStatus[,] board, int rowToClear, int columnToClear)
        {
            ThreatenUpperLeftDiagonal(board, rowToClear, columnToClear);
            ThreatenLowerRightDiagonal(board, rowToClear, columnToClear);
        }

        static void ThreatenCell(CellStatus[,] board, int row, int column)
        {
            board[row, column] = CellStatus.Threatened;
        }

        void ThreateLowerLeftDiagonal(CellStatus[,] board, int rowToClear, int columnToClear)
        {
            for (int columnToUpdate = columnToClear, rowToUpdate = rowToClear; columnToUpdate >= 0 && rowToUpdate >= 0; columnToUpdate--, rowToUpdate--)
            {
                if (CellIsNotOccupied(board[rowToUpdate, columnToUpdate]))
                {
                    ThreatenCell(board, rowToUpdate, columnToUpdate);
                }
            }
        }

        void ThreatenUpperRightDiagonal(CellStatus[,] board, int rowToClear, int columnToClear)
        {
            for (int columnToUpdate = columnToClear, rowToUpdate = rowToClear; columnToUpdate >= 0 && rowToUpdate < boardSize; columnToUpdate--, rowToUpdate++)
            {
                if (CellIsNotOccupied(board[rowToUpdate, columnToUpdate]))
                {
                    ThreatenCell(board, rowToUpdate, columnToUpdate);
                }
            }
        }

        void ThreatenLowerRightDiagonal(CellStatus[,] board, int rowToClear, int columnToClear)
        {
            for (int columnToUpdate = columnToClear, rowToUpdate = rowToClear; columnToUpdate < boardSize && rowToUpdate >= 0; columnToUpdate++, rowToUpdate--)
            {
                if (CellIsNotOccupied(board[rowToUpdate, columnToUpdate]))
                {
                    ThreatenCell(board, rowToUpdate, columnToUpdate);
                }
            }
        }

        void ThreatenUpperLeftDiagonal(CellStatus[,] board, int rowToClear, int columnToClear)
        {
            for (int columnToUpdate = columnToClear, rowToUpdate = rowToClear; columnToUpdate < boardSize && rowToUpdate < boardSize; columnToUpdate++, rowToUpdate++)
            {
                if (CellIsNotOccupied(board[rowToUpdate, columnToUpdate]))
                {
                    ThreatenCell(board, rowToUpdate, columnToUpdate);
                }
            }
        }

        void ThreatenSameColumn(CellStatus[,] board, int column)
        {
            for (int row = 0; row < boardSize; row++)
            {
                if (CellIsNotOccupied(board[row, column]))
                {
                    ThreatenCell(board, row, column);
                }
            }
        }

        void ThreatenSameRow(CellStatus[,] board, int row)
        {
            for (int column = 0; column < boardSize; column++)
            {
                if (CellIsNotOccupied(board[row, column]))
                {
                    ThreatenCell(board, row, column);
                }
            }
        }
            
        static bool ICanPlaceQueen(CellStatus cell)
        {
            return cell == CellStatus.Empty;
        }

        bool CellIsOccupied(CellStatus cell)
        {
            return cell == CellStatus.Occupied;
        }
        
        bool CellIsNotOccupied(CellStatus cell)
        {
            return cell != CellStatus.Occupied;
        }

    }
}

