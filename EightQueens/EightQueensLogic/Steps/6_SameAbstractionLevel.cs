
using System;
using System.Collections.Generic;

namespace EQL_Abstraction
{
    public class Solver
    {
        readonly int boardSize;
        const int initialRank = 0;
        const int initialFile = 0;

        enum SquareStatus 
        {
            Empty,
            QueenPlaced,
            Threatened
        }

        public Solver()
        {
            boardSize = 8;
        }

        public List<Tuple<int,int>> Solve()
        {
            var board = CreateInitializedBoard();

            var startingFile = 0;
            for (int rank = initialRank; rank < boardSize; rank++)
            {
                var queenIsPlaced = false;
                for (int file = startingFile; file < boardSize; file++)
                {
                    if (board[rank, file] == SquareStatus.Empty)
                    {
                        queenIsPlaced = true;
                        board[rank, file] = SquareStatus.QueenPlaced;
                        for (int fileToUpdate = initialFile; fileToUpdate < boardSize; fileToUpdate++)
                        {
                            if (board[rank, fileToUpdate] != SquareStatus.QueenPlaced)
                            {
                                board[rank, fileToUpdate] = SquareStatus.Threatened;
                            }
                        }

                        for (int rankToUpdate = initialRank; rankToUpdate < boardSize; rankToUpdate++)
                        {
                            if (board[rankToUpdate, file] != SquareStatus.QueenPlaced)
                            {
                                board[rankToUpdate, file] = SquareStatus.Threatened;
                            }
                        }

                        for (int fileToUpdate = file, rankToUpdate = rank; fileToUpdate < boardSize && rankToUpdate < boardSize; fileToUpdate++, rankToUpdate++)
                        {
                            if (board[rankToUpdate, fileToUpdate] != SquareStatus.QueenPlaced)
                            {
                                board[rankToUpdate, fileToUpdate] = SquareStatus.Threatened;
                            }
                        }

                        for (int fileToUpdate = file, rankToUpdate = rank; fileToUpdate < boardSize && rankToUpdate >= 0; fileToUpdate++, rankToUpdate--)
                        {
                            if (board[rankToUpdate, fileToUpdate] != SquareStatus.QueenPlaced)
                            {
                                board[rankToUpdate, fileToUpdate] = SquareStatus.Threatened;
                            }

                        }

                        for (int fileToUpdate = file, rankToUpdate = rank; fileToUpdate >= 0 && rankToUpdate < boardSize; fileToUpdate--, rankToUpdate++)
                        {
                            if (board[rankToUpdate, fileToUpdate] != SquareStatus.QueenPlaced)
                            {
                                board[rankToUpdate, fileToUpdate] = SquareStatus.Threatened;
                            }
                        }

                        for (int fileToUpdate = file, rankToUpdate = rank; fileToUpdate >= 0 && rankToUpdate >= 0; fileToUpdate--, rankToUpdate--)
                        {
                            if (board[rankToUpdate, fileToUpdate] != SquareStatus.QueenPlaced)
                            {
                                board[rankToUpdate, fileToUpdate] = SquareStatus.Threatened;
                            }
                        }

                        break;
                    }
                }
                if (!queenIsPlaced)
                {
                    rank = GoBackToPreviousRank(rank);
                    RemovePlacedQueenOnRank(board, ref startingFile, rank);

                    rank = rank - 1;
                    for (int rankToUpdate = initialRank; rankToUpdate < boardSize; rankToUpdate++)
                    {
                        for (int file = initialFile; file < boardSize; file++)
                        {
                            if (board[rankToUpdate, file] != SquareStatus.QueenPlaced)
                            {
                                board[rankToUpdate, file] = SquareStatus.Threatened;
                            }
                        }
                    }

                    for (int rankToCheck = initialRank; rankToCheck < boardSize; rankToCheck++)
                    {
                        for (int fileToCheck = initialFile; fileToCheck < boardSize; fileToCheck++)
                        {
                            if (board[rankToCheck, fileToCheck] == SquareStatus.QueenPlaced)
                            {
                                for (int fileToUpdate = initialFile; fileToUpdate < boardSize; fileToUpdate++)
                                {
                                    if (board[rankToCheck, fileToUpdate] != SquareStatus.QueenPlaced)
                                    {
                                        board[rankToCheck, fileToUpdate] = SquareStatus.Threatened;
                                    }
                                }

                                for (int rankToUpdate = initialRank; rankToUpdate < boardSize; rankToUpdate++)
                                {
                                    if (board[rankToUpdate, fileToCheck] != SquareStatus.QueenPlaced)
                                    {
                                        board[rankToUpdate, fileToCheck] = SquareStatus.Threatened;
                                    }
                                }

                                for (int fileToUpdate = fileToCheck, rankToUpdate = rankToCheck; fileToUpdate < boardSize && rankToUpdate < boardSize; fileToUpdate++, rankToUpdate++)
                                {
                                    if (board[rankToUpdate, fileToUpdate] != SquareStatus.QueenPlaced)
                                    {
                                        board[rankToUpdate, fileToUpdate] = SquareStatus.Threatened;
                                    }
                                }

                                for (int fileToUpdate = fileToCheck, rankToUpdate = rankToCheck; fileToUpdate < boardSize && rankToUpdate >= 0; fileToUpdate++, rankToUpdate--)
                                {
                                    if (QueenIsNotPlacedOnSquare(board[rankToUpdate, fileToUpdate]))
                                    {
                                        board[rankToUpdate, fileToUpdate] = SquareStatus.Threatened;
                                    }
                                }

                                for (int fileToUpdate = fileToCheck, rankToUpdate = rankToCheck; fileToUpdate >= 0 && rankToUpdate < boardSize; fileToUpdate--, rankToUpdate++)
                                {
                                    if (board[rankToUpdate, fileToUpdate] != SquareStatus.QueenPlaced)
                                    {
                                        board[rankToUpdate, fileToUpdate] = SquareStatus.Threatened;
                                    }
                                }

                                for (int fileToUpdate = fileToCheck, rankToUpdate = rankToCheck; fileToUpdate >= 0 && rankToUpdate >= 0; fileToUpdate--, rankToUpdate--)
                                {
                                    if (board[rankToUpdate, fileToUpdate] != SquareStatus.QueenPlaced)
                                    {
                                        board[rankToUpdate, fileToUpdate] = SquareStatus.Threatened;
                                    }
                                }
                            }
                        }
                    }
                } 
                else
                {
                    startingFile = 0;
                }
            }

            return ExtractSolutionFromBoardState(board);
        }

        static int GoBackToPreviousRank(int rank)
        {
            rank = rank - 1;
            return rank;
        }

        void RemovePlacedQueenOnRank(SquareStatus[,] board, ref int startingFile, int rank)
        {
            for (int file = initialFile; file < boardSize; file++)
            {
                if (board[rank, file] == SquareStatus.QueenPlaced)
                {
                    board[rank, file] = SquareStatus.Empty;
                    startingFile = file + 1;
                }
            }
        }

        static bool QueenIsNotPlacedOnSquare(SquareStatus squareStatus)
        {
            return squareStatus != SquareStatus.QueenPlaced;
        }

        SquareStatus[,] CreateInitializedBoard()
        {
            SquareStatus[,] board = new SquareStatus[boardSize, boardSize];
            for (int rank = initialRank; rank < boardSize; rank++)
            {
                for (int file = initialFile; file < boardSize; file++)
                {
                    board[rank, file] = SquareStatus.Empty;
                }
            }

            return board;
        }

        List<Tuple<int, int>> ExtractSolutionFromBoardState(SquareStatus[,] board)
        {
            List<Tuple<int, int>> result = new List<Tuple<int, int>>();
            for (int rank = initialRank; rank < boardSize; rank++)
            {
                for (int file = initialFile; file < boardSize; file++)
                {
                    AddQueenToResult(board, rank, file, result);
                }
            }

            return result;
        }
     
        void AddQueenToResult(SquareStatus[,] board, int rank, int file, List<Tuple<int, int>> result)
        {
            if (board[rank, file] == SquareStatus.QueenPlaced)
            {
                result.Add(new Tuple<int, int>(rank, file));
            }
        }
    }
}