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
            var starter = 0;
            for (int row = 0; row < 8; row++)
            {
                TryPlaceQueenOnRow(board, ref row, ref starter);
            }
        }

        List<Tuple<int, int>> ExtractSolution(CellStatus[,] board)
        {
            List<Tuple<int, int>> result = new List<Tuple<int, int>>();
            for (int row = 0; row < boardSize; row++)
            {
                for (int column = 0; column < boardSize; column++)
                {
                    if (board[row, column] == CellStatus.Occupied)
                    {
                        result.Add(new Tuple<int, int>(row, column));
                    }
                }
            }
            return result;
        }

        void TryPlaceQueenOnRow(CellStatus[,] board, ref int row, ref int starter)
        {
            var queenIsPlaced = false;
            for (int column = starter; column < boardSize; column++)
            {
                queenIsPlaced = TryPlaceQueenOnCell(board, row, column);

                if (queenIsPlaced)
                {
                    break;
                }
            }

            if (!queenIsPlaced)
            {
                RevertLastQueenPlacement(board, ref row, ref starter);
            }
            else
            {
                starter = 0;
            }
        }

        void RevertLastQueenPlacement(CellStatus[,] board, ref int row, ref int starter)
        {
            row = row - 1;
            for (int column = 0; column < boardSize; column++)
            {
                if (CellIsOccupied(board[row,column]))
                {
                    board[row, column] = CellStatus.Empty;
                    starter = column + 1;
                }
            }
            row = row - 1;
            for (int rowToUpdate = 0; rowToUpdate < boardSize; rowToUpdate++)
            {
                for (int column = 0; column < boardSize; column++)
                {
                    if (CellIsNotOccupied(board[rowToUpdate,column]))
                    {
                        board[rowToUpdate, column] = CellStatus.Empty;
                    }
                }
            }
            for (int rowToClear = 0; rowToClear < boardSize; rowToClear++)
            {
                for (int columnToClear = 0; columnToClear < boardSize; columnToClear++)
                {
                    if (board[rowToClear, columnToClear] == CellStatus.Occupied)
                    {
                        MarkThreatenedCells(board, rowToClear, columnToClear);
                    }
                }
            }
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

        void ThreateLowerLeftDiagonal(CellStatus[,] board, int rowToClear, int columnToClear)
        {
            for (int columnToUpdate = columnToClear, rowToUpdate = rowToClear; columnToUpdate >= 0 && rowToUpdate >= 0; columnToUpdate--, rowToUpdate--)
            {
                if (CellIsNotOccupied(board[rowToUpdate, columnToUpdate]))
                {
                    board[rowToUpdate, columnToUpdate] = CellStatus.Threatened;
                }
            }
        }

        void ThreatenUpperRightDiagonal(CellStatus[,] board, int rowToClear, int columnToClear)
        {
            for (int columnToUpdate = columnToClear, rowToUpdate = rowToClear; columnToUpdate >= 0 && rowToUpdate < boardSize; columnToUpdate--, rowToUpdate++)
            {
                if (CellIsNotOccupied(board[rowToUpdate, columnToUpdate]))
                {
                    board[rowToUpdate, columnToUpdate] = CellStatus.Threatened;
                }
            }
        }

        void ThreatenLowerRightDiagonal(CellStatus[,] board, int rowToClear, int columnToClear)
        {
            for (int columnToUpdate = columnToClear, rowToUpdate = rowToClear; columnToUpdate < boardSize && rowToUpdate >= 0; columnToUpdate++, rowToUpdate--)
            {
                if (CellIsNotOccupied(board[rowToUpdate, columnToUpdate]))
                {
                    board[rowToUpdate, columnToUpdate] = CellStatus.Threatened;
                }
            }
        }

        void ThreatenUpperLeftDiagonal(CellStatus[,] board, int rowToClear, int columnToClear)
        {
            for (int columnToUpdate = columnToClear, rowToUpdate = rowToClear; columnToUpdate < boardSize && rowToUpdate < boardSize; columnToUpdate++, rowToUpdate++)
            {
                if (CellIsNotOccupied(board[rowToUpdate, columnToUpdate]))
                {
                    board[rowToUpdate, columnToUpdate] = CellStatus.Threatened;
                }
            }
        }

        void ThreatenSameColumn(CellStatus[,] board, int columnToClear)
        {
            for (int rowToUpdate = 0; rowToUpdate < boardSize; rowToUpdate++)
            {
                if (CellIsNotOccupied(board[rowToUpdate, columnToClear]))
                {
                    board[rowToUpdate, columnToClear] = CellStatus.Threatened;
                }
            }
        }

        void ThreatenSameRow(CellStatus[,] board, int rowToClear)
        {
            for (int columnToUpdate = 0; columnToUpdate < boardSize; columnToUpdate++)
            {
                if (CellIsNotOccupied(board[rowToClear, columnToUpdate]))
                {
                    board[rowToClear, columnToUpdate] = CellStatus.Threatened;
                }
            }
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

