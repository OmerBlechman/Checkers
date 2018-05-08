using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;

namespace Board
{
    public class GameBoard
    {
        private const int k_PlayerOneTurn = 0, k_PlayerTwoTurn = 1;
        private const char k_PlayerOneSoldier = 'X', k_PlayerTwoSoldier = 'O', k_PlayerOneKing = 'K', k_PlayerTwoKing = 'U';
        private const char k_FirstUpperCaseLetter = 'A', k_FirstLowerCaseLetter = 'a', k_EmptyPlace = '\0';
        private const string k_Seperator = "=";
        private int m_boardSize;
        private char[,] m_boardMatrix;

        public int BoardSize
        {
            get
            {
                return m_boardSize;
            }
        }

        public void InitialBoard(int i_BoardSize)
        {
            m_boardSize = i_BoardSize;
            initialBoardMatrix();
        }

        private void initialBoardMatrix()
        {
            m_boardMatrix = new char[m_boardSize, m_boardSize];

            for (int i = 0; i < ((m_boardSize / 2) - 1); ++i)
            {
                for (int j = 0; j < m_boardSize; ++j)
                {
                    if ((i + j) % 2 == 1)
                    {
                        m_boardMatrix[i, j] = k_PlayerTwoSoldier;
                    }
                }
            }

            for (int i = (m_boardSize / 2) + 1; i < m_boardSize; ++i)
            {
                for (int j = 0; j < m_boardSize; ++j)
                {
                    if ((i + j) % 2 == 1)
                    {
                        m_boardMatrix[i, j] = k_PlayerOneSoldier;
                    }
                }
            }
        }

        public void PrintMatrix()
        {
            StringBuilder spaces = new StringBuilder();
            char startCharToPrint = k_FirstUpperCaseLetter;

            spaces.Insert(0, k_Seperator, (4 * m_boardSize) + 2);
            Console.Write("  ");

            for (int i = 0; i < m_boardSize; ++i)
            {
                Console.Write(" {0}  ", startCharToPrint++);
            }

            Console.WriteLine();
            startCharToPrint = k_FirstLowerCaseLetter;

            for (int i = 0; i < m_boardSize; ++i)
            {
                Console.Write("{0}{1}{2}|", spaces, Environment.NewLine, startCharToPrint++);

                for (int j = 0; j < m_boardSize; ++j)
                {
                    Console.Write(" {0} |", m_boardMatrix[i, j]);
                }

                Console.Write(Environment.NewLine);
            }

            Console.WriteLine(spaces);
        }

        public bool CheckPlayerSoldier(Point i_SourcePoint, int i_PlayerTurn)
        {
            bool isLegalPlayerSoldier;

            if (i_PlayerTurn == k_PlayerOneTurn)
            {
                checkLegalSoldier(i_SourcePoint, k_PlayerOneSoldier, k_PlayerOneKing, out isLegalPlayerSoldier);
            }
            else
            {
                checkLegalSoldier(i_SourcePoint, k_PlayerTwoSoldier, k_PlayerTwoKing, out isLegalPlayerSoldier);
            }

            return isLegalPlayerSoldier;
        }

        private void checkLegalSoldier(Point i_SourcePoint, char i_PlayerSoldier, char i_PlayerKing, out bool o_IsLegalPlayerSoldier)
        {
            if (m_boardMatrix[i_SourcePoint.Y, i_SourcePoint.X] == i_PlayerSoldier || m_boardMatrix[i_SourcePoint.Y, i_SourcePoint.X] == i_PlayerKing)
            {
                o_IsLegalPlayerSoldier = true;
            }
            else
            {
                o_IsLegalPlayerSoldier = false;
            }
        }

        private bool stepValidation(Point i_SourcePoint, Point i_DestinationPoint)
        {
            Point differencePoint = new Point(i_DestinationPoint.X - i_SourcePoint.X, i_DestinationPoint.Y - i_SourcePoint.Y);
            bool isLegalMove = false;

            if ((Math.Abs(differencePoint.X) == 2 && Math.Abs(differencePoint.Y) == 2) || (Math.Abs(differencePoint.X) == 1 && Math.Abs(differencePoint.Y) == 1))
            {
                isLegalMove = true;
            }
            else
            {
                isLegalMove = false;
            }

            return isLegalMove;
        }

        public bool CheckLegalMove(Point i_SourcePoint, Point i_DestinationPoint, int i_PlayerTurn, Point i_DirectionPoint)
        {
            bool isLegalMove = true;

            if (CheckPlayerSoldier(i_SourcePoint, i_PlayerTurn) == false)
            {
                isLegalMove = false;
            }

            if (isLegalMove == true)
            {
                isLegalMove = stepValidation(i_SourcePoint, i_DestinationPoint);

                if (isLegalMove == true && i_PlayerTurn == k_PlayerOneTurn)
                {
                    isLegalMove = checkManForwardMove(i_SourcePoint, i_SourcePoint.Y, i_DestinationPoint.Y, k_PlayerOneSoldier);

                    if (isLegalMove == true)
                    {
                        isLegalMove = checkSoldierMoveAndCapture(i_SourcePoint, i_DestinationPoint, i_DirectionPoint, k_PlayerTwoSoldier, k_PlayerTwoKing);
                    }
                }
                else if (isLegalMove == true && i_PlayerTurn == k_PlayerTwoTurn)
                {
                    isLegalMove = checkManForwardMove(i_SourcePoint, i_DestinationPoint.Y, i_SourcePoint.Y, k_PlayerTwoSoldier);

                    if (isLegalMove == true)
                    {
                        isLegalMove = checkSoldierMoveAndCapture(i_SourcePoint, i_DestinationPoint, i_DirectionPoint, k_PlayerOneSoldier, k_PlayerOneKing);
                    }
                }
            }

            return isLegalMove;
        }

        public void RemoveSoldierFromBoard(Point i_SourcePoint)
        {
            m_boardMatrix[i_SourcePoint.Y, i_SourcePoint.X] = k_EmptyPlace;
        }

        private bool checkSoldierMoveAndCapture(Point i_SourcePoint, Point i_DestinationPoint, Point i_DirectionPoint, char i_EnemySoldier, char i_EnemyKing)
        {
            bool isLegalMove = true;
            Point twoStepsMovePoint = new Point(2 * i_DirectionPoint.X, 2 * i_DirectionPoint.Y);
            Point enemyPoint = new Point(i_SourcePoint.X + i_DirectionPoint.X, i_SourcePoint.Y + i_DirectionPoint.Y);

            if (m_boardMatrix[i_DestinationPoint.Y, i_DestinationPoint.X] == k_EmptyPlace)
            {
                if (i_DestinationPoint.Y == i_SourcePoint.Y + twoStepsMovePoint.Y && i_DestinationPoint.X == i_SourcePoint.X + twoStepsMovePoint.X)
                {
                    if (m_boardMatrix[enemyPoint.Y, enemyPoint.X] == i_EnemySoldier || m_boardMatrix[enemyPoint.Y, enemyPoint.X] == i_EnemyKing)
                    {
                        isLegalMove = true;
                    }
                    else
                    {
                        isLegalMove = false;
                    }
                }
            }
            else
            {
                isLegalMove = false;
            }

            return isLegalMove;
        }

        private bool checkManForwardMove(Point i_SourcePoint, int i_LowerLineOnBoard, int i_HigherLineOnBoard, char i_PlayerSoldier)
        {
            bool isLegalMove = true;

            if (m_boardMatrix[i_SourcePoint.Y, i_SourcePoint.X] == i_PlayerSoldier)
            {
                if (i_HigherLineOnBoard > i_LowerLineOnBoard)
                {
                    isLegalMove = false;
                }
            }

            return isLegalMove;
        }

        public bool isSoldierInBoardLimits(Point i_SoldierPoisiton)
        {
            bool legalPosition = false;
            if (i_SoldierPoisiton.Y >= 0 && i_SoldierPoisiton.Y < m_boardSize)
            {
                if (i_SoldierPoisiton.X >= 0 && i_SoldierPoisiton.X < m_boardSize)
                {
                    legalPosition = true;
                }
            }

            return legalPosition;
        }

        public bool CheckCapture(Point i_SourcePoint, Point i_DirectionPoint, int i_PlayerTurn)
        {
            bool isLegalBackMove = false;
            bool isEnemyPlace = false;
            bool isLegalDestinationPosition = false;
            Point enemyPosition = new Point(i_SourcePoint.X + i_DirectionPoint.X, i_SourcePoint.Y + i_DirectionPoint.Y);
            Point destinationCapturePoint = new Point(i_SourcePoint.X + (2 * i_DirectionPoint.X), i_SourcePoint.Y + (2 * i_DirectionPoint.Y));

            if (isSoldierInBoardLimits(enemyPosition) == true)
            {
                if (i_PlayerTurn == k_PlayerOneTurn)
                {
                    isLegalBackMove = !(m_boardMatrix[i_SourcePoint.Y, i_SourcePoint.X] == k_PlayerOneSoldier && i_DirectionPoint.Y > 0);
                    isEnemyPlace = checkExistenceEnemyPoint(enemyPosition, k_PlayerTwoSoldier, k_PlayerTwoKing);
                }
                else
                {
                    isLegalBackMove = !(m_boardMatrix[i_SourcePoint.Y, i_SourcePoint.X] == k_PlayerTwoSoldier && i_DirectionPoint.Y < 0);
                    isEnemyPlace = checkExistenceEnemyPoint(enemyPosition, k_PlayerOneSoldier, k_PlayerOneKing);
                }

                if (isLegalBackMove == false)
                {
                    isEnemyPlace = false;
                }

                if (isEnemyPlace == true)
                {
                    isLegalDestinationPosition = checkDestinationPositionAfterCapture(destinationCapturePoint);
                }
            }

            return isEnemyPlace && isLegalDestinationPosition;
        }

        public bool CheckEmptyPoint(Point i_SourcePoint)
        {
            return m_boardMatrix[i_SourcePoint.Y, i_SourcePoint.X] == k_EmptyPlace;
        }

        private bool checkDestinationPositionAfterCapture(Point i_DestinationPoint)
        {
            return i_DestinationPoint.X >= 0 && i_DestinationPoint.Y >= 0 &&
                    i_DestinationPoint.X < m_boardSize && i_DestinationPoint.Y < m_boardSize &&
                    m_boardMatrix[i_DestinationPoint.Y, i_DestinationPoint.X] == k_EmptyPlace;
        }

        private bool checkExistenceEnemyPoint(Point i_SourcePoint, char i_EnemySoldier, char i_EnemyKing)
        {
            bool isEnemyPlace;

            if (m_boardMatrix[i_SourcePoint.Y, i_SourcePoint.X] == i_EnemySoldier || m_boardMatrix[i_SourcePoint.Y, i_SourcePoint.X] == i_EnemyKing)
            {
                isEnemyPlace = true;
            }
            else
            {
                isEnemyPlace = false;
            }

            return isEnemyPlace;
        }

        private void changeSoldierCordinate(Point i_SourcePoint, Point i_DestinationPoint)
        {
            m_boardMatrix[i_DestinationPoint.Y, i_DestinationPoint.X] = m_boardMatrix[i_SourcePoint.Y, i_SourcePoint.X];
            m_boardMatrix[i_SourcePoint.Y, i_SourcePoint.X] = k_EmptyPlace;
        }

        public void UpdateSoldierPosition(Point i_SourcePoint, Point i_DestinationPoint, Point i_DirectionPoint, int i_PlayerTurn, bool i_IsCaptureTurn)
        {
            Point enemyPoint = new Point(i_SourcePoint.X + i_DirectionPoint.X, i_SourcePoint.Y + i_DirectionPoint.Y);

            changeSoldierCordinate(i_SourcePoint, i_DestinationPoint);
            checkAndUpdateSoldierToKing(i_DestinationPoint, i_PlayerTurn);

            if (i_IsCaptureTurn == true)
            {
                m_boardMatrix[enemyPoint.Y, enemyPoint.X] = k_EmptyPlace;
            }
        }

        private void checkAndUpdateSoldierToKing(Point i_DestinationPoint, int i_PlayerTurn)
        {
            if (i_PlayerTurn == k_PlayerOneTurn && i_DestinationPoint.Y == 0)
            {
                m_boardMatrix[i_DestinationPoint.Y, i_DestinationPoint.X] = k_PlayerOneKing;
            }
            else if (i_PlayerTurn == k_PlayerTwoTurn && i_DestinationPoint.Y == m_boardSize - 1)
            {
                m_boardMatrix[i_DestinationPoint.Y, i_DestinationPoint.X] = k_PlayerTwoKing;
            }
        }
    }
}
