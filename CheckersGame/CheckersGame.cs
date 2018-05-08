using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using PlayerSoldier;
using Player;
using Board;
using SoldierKindEnum;

namespace CheckersGame
{
    public class Game
    {
        private const char k_ColConvertor = 'A', k_RowConvertor = 'a';
        private const int k_computerTurn = 1;
        private GameBoard m_Board;

        public Game(int i_BoardSize)
        {
            m_Board = new GameBoard();
            m_Board.InitialBoard(i_BoardSize);
        }

        public void PrintMatrix()
        {
            m_Board.PrintMatrix();
        }

        public int BoardSize()
        {
            return m_Board.BoardSize;
        }

        private void setDirection(Point i_DifferencePoint, ref Point io_DirectionPoint)
        {
            if (i_DifferencePoint.X > 0)
            {
                io_DirectionPoint.X = 1;
            }
            else
            {
                io_DirectionPoint.X = -1;
            }

            if (i_DifferencePoint.Y > 0)
            {
                io_DirectionPoint.Y = 1;
            }
            else
            {
                io_DirectionPoint.Y = -1;
            }
        }

        public bool PlayerValidation(GamePlayer i_CurrentPlayer, Point i_SourcePoint, Point i_DestinationPoint, out bool o_IsCaptureOption)
        {
            bool isLegalMove = true;
            Point directionPoint = new Point();
            Point differencePoint = new Point(i_DestinationPoint.X - i_SourcePoint.X, i_DestinationPoint.Y - i_SourcePoint.Y);
            o_IsCaptureOption = false;

            isLegalMove = true;
            setDirection(differencePoint, ref directionPoint);

            if (m_Board.CheckLegalMove(i_SourcePoint, i_DestinationPoint, i_CurrentPlayer.PlayerKind, directionPoint) == false)
            {
                isLegalMove = false;
            }

            o_IsCaptureOption = (checkCaptureOption(i_CurrentPlayer) == true) && (isLegalMove == true);

            if (o_IsCaptureOption == true)
            {
                if (m_Board.CheckCapture(i_SourcePoint, directionPoint, i_CurrentPlayer.PlayerKind) == false)
                {
                    isLegalMove = false;
                }
            }

            if (o_IsCaptureOption == false && Math.Abs(differencePoint.X) == 2 && Math.Abs(differencePoint.Y) == 2)
            {
                isLegalMove = false;
            }

            return isLegalMove;
        }

        private bool checkCaptureOption(GamePlayer i_CurrentPlayer)
        {
            Point destinationPoint = new Point();
            bool isCaptureOption = false;

            for (int i = 0; i < i_CurrentPlayer.PlayersSoldier.Count; ++i)
            {
                isCaptureOption = IsAbleToCapture(i_CurrentPlayer.PlayersSoldier[i], i_CurrentPlayer.PlayerKind, ref destinationPoint);
                if (isCaptureOption == true)
                {
                    break;
                }
            }

            return isCaptureOption;
        }

        public bool IsAbleToCapture(Soldier i_CurrentSoldier, int i_PlayerTurn, ref Point io_DestinationPoint)
        {
            bool ableToCapture = false;
            Point directionPoint = new Point();

            for (int i = -1; i < 2 && ableToCapture == false; i += 2)
            {
                directionPoint.Y = i;

                for (int j = -1; j < 2 && ableToCapture == false; j += 2)
                {
                    directionPoint.X = j;

                    if (m_Board.CheckCapture(i_CurrentSoldier.SoldierPoint, directionPoint, i_PlayerTurn))
                    {
                        io_DestinationPoint.Y = i_CurrentSoldier.Y + (2 * directionPoint.Y);
                        io_DestinationPoint.X = i_CurrentSoldier.X + (2 * directionPoint.X);
                        ableToCapture = true;
                    }
                }
            }

            return ableToCapture;
        }

        public void PlayerOperation(GamePlayer[] i_PlayersGame, Point i_SourcePoint, Point i_DestinationPoint, bool i_IsCapture, int i_PlayerTurn, out Soldier o_CurrentSoldier)
        {
            Point directionPoint = new Point();
            Point differencePoint = new Point(i_DestinationPoint.X - i_SourcePoint.X, i_DestinationPoint.Y - i_SourcePoint.Y);
            int enemyTurn = (i_PlayerTurn + 1) % i_PlayersGame.Length;

            setDirection(differencePoint, ref directionPoint);
            o_CurrentSoldier = i_PlayersGame[i_PlayerTurn].FindCurrentSoldier(i_SourcePoint);
            o_CurrentSoldier.X = i_DestinationPoint.X;
            o_CurrentSoldier.Y = i_DestinationPoint.Y;

            if (i_IsCapture == true)
            {
                removeEnemySoldier(i_PlayersGame[enemyTurn], i_SourcePoint, directionPoint);
            }

            if (checkKingUpdating(o_CurrentSoldier.Y) == true)
            {
                i_PlayersGame[i_PlayerTurn].AmountOfSoldiersValue += 3;
                o_CurrentSoldier.SoldierKind = eSoldierKind.KING;
            }

            m_Board.UpdateSoldierPosition(i_SourcePoint, i_DestinationPoint, directionPoint, i_PlayerTurn, i_IsCapture);
        }

        private bool checkKingUpdating(int i_RowCordinate)
        {
            return i_RowCordinate == 0 || i_RowCordinate == m_Board.BoardSize - 1;
        }

        private void removeEnemySoldier(GamePlayer i_EnemyPlayer, Point i_SourcePoint, Point i_DirectionPoint)
        {
            for (int i = 0; i < i_EnemyPlayer.PlayersSoldier.Count; ++i)
            {
                if (i_EnemyPlayer.PlayersSoldier[i].X == i_SourcePoint.X + i_DirectionPoint.X
                     && i_EnemyPlayer.PlayersSoldier[i].Y == i_SourcePoint.Y + i_DirectionPoint.Y)
                {
                    decreasePlayerScore(i_EnemyPlayer, i);
                    i_EnemyPlayer.PlayersSoldier.RemoveAt(i);
                    break;
                }
            }
        }

        private void decreasePlayerScore(GamePlayer i_EnemyPlayer, int i_SoldierIndex)
        {
            if (i_EnemyPlayer.PlayersSoldier[i_SoldierIndex].SoldierKind == eSoldierKind.MAN)
            {
                i_EnemyPlayer.AmountOfSoldiersValue -= 1;
            }
            else
            {
                i_EnemyPlayer.AmountOfSoldiersValue -= 4;
            }
        }

        public bool PlayerAbleToMove(GamePlayer i_CurrentPlayer)
        {
            bool ableToMove = false;
            Point currentDestinationPoint = new Point();

            for (int i = 0; i < i_CurrentPlayer.PlayersSoldier.Count && ableToMove == false; ++i)
            {
                for (int rowDirection = -1; rowDirection < 2 && ableToMove == false; rowDirection += 2)
                {
                    for (int colDirection = -1; colDirection < 2 && ableToMove == false; colDirection += 2)
                    {
                        currentDestinationPoint.X = i_CurrentPlayer.PlayersSoldier[i].X + rowDirection;
                        currentDestinationPoint.Y = i_CurrentPlayer.PlayersSoldier[i].Y + colDirection;

                        if (m_Board.isSoldierInBoardLimits(currentDestinationPoint) == true)
                        {
                            ableToMove = soldierAbleToMove(i_CurrentPlayer.PlayersSoldier[i], currentDestinationPoint, i_CurrentPlayer.PlayerKind);
                        }
                    }
                }
            }

            return ableToMove;
        }

        private bool soldierAbleToMove(Soldier i_CurrentSoldier, Point i_DestinationPoint, int i_PlayerTurn)
        {
            Point differencePoint = new Point(i_DestinationPoint.X - i_CurrentSoldier.X, i_DestinationPoint.Y - i_CurrentSoldier.Y);
            Point directionPoint = new Point();

            setDirection(differencePoint, ref directionPoint);

            return m_Board.CheckLegalMove(i_CurrentSoldier.SoldierPoint, i_DestinationPoint, i_PlayerTurn, directionPoint);
        }

        private List<Soldier> soldiersAbleToCapture(GamePlayer i_ComputerPlayer)
        {
            List<Soldier> soldiersAbleToCapture = new List<Soldier>(i_ComputerPlayer.PlayersSoldier.Count);
            bool currentSoldierAbleToCapture = false;
            Point destinationPoint = new Point();

            for (int i = 0; i < soldiersAbleToCapture.Capacity; ++i)
            {
                currentSoldierAbleToCapture = IsAbleToCapture(i_ComputerPlayer.PlayersSoldier[i], k_computerTurn, ref destinationPoint);

                if (currentSoldierAbleToCapture == true)
                {
                    soldiersAbleToCapture.Add(i_ComputerPlayer.PlayersSoldier[i]);
                }
            }

            return soldiersAbleToCapture;
        }

        private Soldier findMostCaptureSoldier(List<Soldier> i_SoldiersAbleToCapture, ref Point io_DestinationPoint)
        {
            Soldier currentSoldierThatWillPlay = null;
            int maximumSoldierCapturesAmount = 0;
            int currentSoldierCapturesAmount;
            Point currentDestinationPoint = new Point();

            while (i_SoldiersAbleToCapture.Count != 0)
            {
                currentSoldierCapturesAmount = 0;
                checkAmountSoldierCapture(i_SoldiersAbleToCapture[0].Y, i_SoldiersAbleToCapture[0].X, ref currentDestinationPoint, ref currentSoldierCapturesAmount, 0, 0);

                if (currentSoldierThatWillPlay == null || maximumSoldierCapturesAmount < currentSoldierCapturesAmount)
                {
                    currentSoldierThatWillPlay = i_SoldiersAbleToCapture[0];
                    maximumSoldierCapturesAmount = currentSoldierCapturesAmount;
                    io_DestinationPoint = currentDestinationPoint;
                }

                i_SoldiersAbleToCapture.RemoveAt(0);
            }

            return currentSoldierThatWillPlay;
        }

        private int checkAmountSoldierCapture(int i_YCordinate, int i_XCordinate, ref Point io_Destination, ref int maximumCaptures, int sourceYDirection, int sourceXDirection)
        {
            int amountOfSoldierCaptures = 0;
            Point directionPoint = new Point();
            Point sourcerPoint = new Point(i_XCordinate, i_YCordinate);

            for (int i = -1; i < 2; i += 2)
            {
                directionPoint.Y = i;

                for (int j = -1; j < 2; j += 2)
                {
                    directionPoint.X = j;

                    if (m_Board.CheckCapture(sourcerPoint, directionPoint, 1) == true)
                    {
                        if (sourceYDirection != i && sourceXDirection != j)
                        {
                            amountOfSoldierCaptures++;
                            amountOfSoldierCaptures += checkAmountSoldierCapture(sourcerPoint.Y + (2 * i), sourcerPoint.X + (2 * j), ref io_Destination, ref maximumCaptures, -i, -j);

                            if (amountOfSoldierCaptures > maximumCaptures)
                            {
                                maximumCaptures = amountOfSoldierCaptures;
                                io_Destination.Y = sourcerPoint.Y + (2 * i);
                                io_Destination.X = sourcerPoint.X + (2 * j);
                            }
                        }
                    }
                }
            }

            return amountOfSoldierCaptures;
        }

        public string ComputerOperation(GamePlayer i_ComputerPlayer, out bool o_IsCaptureOption, bool i_StillAbleToCapture, Soldier i_CaptureSoldier, Point i_DestinationPoint)
        {
            Soldier currentSoldier = null;
            List<Soldier> ableToCapture;
            Point destinationPoint = new Point();
            string currentMove;
            o_IsCaptureOption = false;

            if (i_StillAbleToCapture == true)
            {
                currentSoldier = i_CaptureSoldier;
                destinationPoint = i_DestinationPoint;
                o_IsCaptureOption = true;
            }
            else
            {
                ableToCapture = soldiersAbleToCapture(i_ComputerPlayer);

                if (ableToCapture.Count > 0)
                {
                    o_IsCaptureOption = true;
                    currentSoldier = findMostCaptureSoldier(ableToCapture, ref destinationPoint);
                }
                else
                {
                    currentSoldier = findSoldierCanBecomeKing(i_ComputerPlayer, ref destinationPoint);
                    if (currentSoldier == null)
                    {
                        currentSoldier = findSoldierAbleToMove(i_ComputerPlayer, ref destinationPoint);
                    }
                }
            }

            currentMove = string.Format("{0}{1}>{2}{3}", (char)(currentSoldier.X + k_ColConvertor), (char)(currentSoldier.Y + k_RowConvertor), (char)(destinationPoint.X + k_ColConvertor), (char)(destinationPoint.Y + k_RowConvertor));

            return currentMove;
        }

        private Soldier findSoldierCanBecomeKing(GamePlayer i_ComputerPlayer, ref Point io_DestinationPoint)
        {
            Soldier currentSoldier = null;
            Point currentCheckDestinatioPoint = new Point();
            bool isAbleToBeKing = false;

            for (int soldierIndex = 0; soldierIndex < i_ComputerPlayer.PlayersSoldier.Count && isAbleToBeKing == false; ++soldierIndex)
            {
                currentCheckDestinatioPoint.Y = i_ComputerPlayer.PlayersSoldier[soldierIndex].Y + 1;
                for (int i = -1; i < 2 && isAbleToBeKing == false; i += 2)
                {
                    if (i_ComputerPlayer.PlayersSoldier[soldierIndex].SoldierKind == eSoldierKind.MAN &&
                        i_ComputerPlayer.PlayersSoldier[soldierIndex].Y == m_Board.BoardSize - 2)
                    {
                        currentCheckDestinatioPoint.X = i_ComputerPlayer.PlayersSoldier[soldierIndex].X + i;
                        if (m_Board.isSoldierInBoardLimits(currentCheckDestinatioPoint) == true)
                        {
                            if (soldierAbleToMove(i_ComputerPlayer.PlayersSoldier[soldierIndex], currentCheckDestinatioPoint, i_ComputerPlayer.PlayerKind))
                            {
                                io_DestinationPoint = currentCheckDestinatioPoint;
                                currentSoldier = i_ComputerPlayer.PlayersSoldier[soldierIndex];
                                isAbleToBeKing = true;
                            }
                        }
                    }
                }
            }

            return currentSoldier;
        }

        private Soldier findSoldierAbleToMove(GamePlayer i_ComputerPlayer, ref Point io_DestinationPoint)
        {
            Soldier currentSoldier = null;
            Point currentCheckDestinatioPoint = new Point();
            Point directionPoint = new Point();
            bool foundSoldier = false;
            Random randomIndexSoldier = new Random();
            int currentIndexSoldier;

            while (foundSoldier == false)
            {
                currentIndexSoldier = randomIndexSoldier.Next(0, i_ComputerPlayer.PlayersSoldier.Count);

                for (int i = -1; i < 2 && foundSoldier == false; i += 2)
                {
                    directionPoint.Y = i;
                    currentCheckDestinatioPoint.Y = i_ComputerPlayer.PlayersSoldier[currentIndexSoldier].Y + i;

                    for (int j = -1; j < 2 && foundSoldier == false; j += 2)
                    {
                        directionPoint.X = j;
                        currentCheckDestinatioPoint.X = i_ComputerPlayer.PlayersSoldier[currentIndexSoldier].X + j;

                        if (m_Board.isSoldierInBoardLimits(currentCheckDestinatioPoint) == true)
                        {
                            if (soldierAbleToMove(i_ComputerPlayer.PlayersSoldier[currentIndexSoldier], currentCheckDestinatioPoint, i_ComputerPlayer.PlayerKind))
                            {
                                io_DestinationPoint = currentCheckDestinatioPoint;
                                currentSoldier = i_ComputerPlayer.PlayersSoldier[currentIndexSoldier];
                                foundSoldier = true;
                            }
                        }
                    }
                }
            }

            return currentSoldier;
        }
    }
}