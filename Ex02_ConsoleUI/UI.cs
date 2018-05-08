using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using PlayerSoldier;
using Player;
using BoardSizeEnum;
using GameOptionEnum;
using CheckersGame;

namespace Ex02_ConsoleUI
{
    public class UI
    {
        private const char k_ColConvertor = 'A', k_RowConvertor = 'a', k_Quit = 'Q';
        private const int k_NumberOfPlayers = 2, k_FirstOption = 0, k_SecondOption = 1, k_RestartMode = 1;
        private GamePlayer[] m_Players = new GamePlayer[k_NumberOfPlayers];
        private int m_Turn = 0;
        private Game m_Engine;

        private void readPlayerName(int i_NamePlayerIndex)
        {
            Console.WriteLine("please enter your name:");
            string input = Console.ReadLine();
            m_Players[i_NamePlayerIndex].PlayerName = input;
            checkLegalityPlayerName(i_NamePlayerIndex);
        }

        private void checkLegalityPlayerName(int i_NamePlayerIndex)
        {
            bool isLegalName = true;
            string currentPlayerName = m_Players[i_NamePlayerIndex].PlayerName;

            do
            {
                isLegalName = true;

                if (currentPlayerName.Length > 20)
                {
                    isLegalName = false;
                }

                for (int i = 0; i < currentPlayerName.Length && isLegalName == true; ++i)
                {
                    if (char.IsLetter(currentPlayerName[i]) == false)
                    {
                        isLegalName = false;
                    }
                }

                if (isLegalName == false)
                {
                    Console.WriteLine("please enter legal name");
                    m_Players[i_NamePlayerIndex].PlayerName = Console.ReadLine();
                    currentPlayerName = m_Players[i_NamePlayerIndex].PlayerName;
                }
            }
            while (isLegalName == false);
        }

        private eBoardSize readBoardSize()
        {
            int userInputBoardSize;
            bool isNumber;

            Console.WriteLine("please enter board size: 6 or 8 or 10");
            isNumber = int.TryParse(Console.ReadLine(), out userInputBoardSize);
            checkLigalityBoardSize(ref userInputBoardSize, isNumber);

            return (eBoardSize)userInputBoardSize;
        }

        private void checkLigalityBoardSize(ref int io_UserInputBoardSize, bool i_IsNumber)
        {
            bool isLegalBoardSize = true;

            do
            {
                isLegalBoardSize = true;

                if (i_IsNumber == false)
                {
                    isLegalBoardSize = false;
                }

                if (io_UserInputBoardSize != (int)eBoardSize.SIX_ON_SIX
                     && io_UserInputBoardSize != (int)eBoardSize.EIGHT_ON_EIGHT
                     && io_UserInputBoardSize != (int)eBoardSize.TEN_ON_TEN)
                {
                    Console.WriteLine("please enter legal board size");
                    i_IsNumber = int.TryParse(Console.ReadLine(), out io_UserInputBoardSize);
                    isLegalBoardSize = false;
                }
            }
            while (isLegalBoardSize == false);
        }

        private int readRestartOption()
        {
            Console.WriteLine("please press 1 to restart game and 0 to quit");
            return getGameOption();
        }

        private void readGameOption()
        {
            int gameOption;

            Console.WriteLine("please press 0 to play against computer and 1 against human");
            gameOption = getGameOption();

            if (gameOption == (int)eGameOptions.COMPUTER)
            {
                m_Players[1].IsComputer = true;
                m_Players[1].PlayerName = "Computer";
            }
        }

        private int getGameOption()
        {
            int gameOption;
            bool isNumber;

            isNumber = int.TryParse(Console.ReadLine(), out gameOption);
            checkLegalityGameOption(isNumber, ref gameOption);

            return gameOption;
        }

        private void checkLegalityGameOption(bool i_isNumber, ref int i_gameOption)
        {
            bool isLegalGameOption = true;

            do
            {
                isLegalGameOption = true;

                if (i_isNumber == false || (i_gameOption != k_FirstOption && i_gameOption != k_SecondOption))
                {
                    isLegalGameOption = false;
                    Console.WriteLine("please enter legal game option");
                    i_isNumber = int.TryParse(Console.ReadLine(), out i_gameOption);
                }
            }
            while (isLegalGameOption == false);
        }

        private void getUserInput(out eBoardSize o_BoardSize)
        {
            readPlayerName(0);
            o_BoardSize = readBoardSize();
            readGameOption();

            if (m_Players[1].IsComputer == false)
            {
                readPlayerName(1);
            }
        }

        private void initialPlayersSoliders(int i_BoardSize)
        {
            Point startPoint = new Point(0, 0);

            for (int i = 0; i < k_NumberOfPlayers; ++i)
            {
                m_Players[i].InitialSoldiers(ref startPoint, i_BoardSize, i);
                startPoint.Y += 2;
                m_Players[i].PlayerKind = i;
                m_Players[i].AmountOfSoldiersValue = 0;
            }
        }

        private void initialPlayersGame()
        {
            for (int i = 0; i < k_NumberOfPlayers; ++i)
            {
                m_Players[i] = new GamePlayer();
            }
        }

        public void Run()
        {
            eBoardSize boardSize;
            int gameMode;

            initialPlayersGame();
            getUserInput(out boardSize);

            do
            {
                m_Turn = 0;
                m_Engine = new Game((int)boardSize);
                initialPlayersSoliders((int)boardSize);
                printFirstTimeBoard();
                playGame();
                gameMode = readRestartOption();
            }
            while (gameMode == k_RestartMode);
        }

        private void printFirstTimeBoard()
        {
            Ex02.ConsoleUtils.Screen.Clear();
            m_Engine.PrintMatrix();
            Console.Write("{0}'s Turn({1}): ", m_Players[m_Turn].PlayerName, m_Players[m_Turn].PlayerSignOnBoard);
        }

        private void playGame()
        {
            bool gameOver = false;
            bool drawMode = false;
            string turnInput = null;
            bool isCaptureOption;
            Point sourcePoint = Point.Empty;
            Point destinationPoint = Point.Empty;
            Soldier currentSoldier = null;
            bool stillAbleToCapture = false;
            bool ableToPlay = false;
            bool quitMode;

            do
            {
                ableToPlay = checkDrawModeAndGameOver(out gameOver, out drawMode);
                if (m_Players[m_Turn].IsComputer == false)
                {
                    turnInput = getUserTurnInputAndCheckQuitMode(out quitMode, currentSoldier, stillAbleToCapture);

                    if (ableToPlay == false || quitMode == true)
                    {
                        break;
                    }

                    HumanMoveValidation(ref turnInput, ref sourcePoint, ref destinationPoint, out isCaptureOption, currentSoldier, stillAbleToCapture);
                }
                else
                {
                    if (ableToPlay == false)
                    {
                        break;
                    }

                    turnInput = m_Engine.ComputerOperation(m_Players[m_Turn], out isCaptureOption, stillAbleToCapture, currentSoldier, destinationPoint);
                    setSourceAndDestinationCordinates(turnInput, ref sourcePoint, ref destinationPoint);
                }

                operation(turnInput, sourcePoint, ref destinationPoint, isCaptureOption, ref stillAbleToCapture, out currentSoldier);
            }
            while (turnInput[0] != k_Quit);

            printResults(turnInput, gameOver, drawMode);
        }

        private void operation(string i_TurnInput, Point i_SourcePoint, ref Point io_DestinationPoint, bool i_IsCaptureOption, ref bool io_StillAbleToCapture, out Soldier o_CurrentSoldier)
        {
            int previousPlayerTurn = m_Turn;

            m_Engine.PlayerOperation(m_Players, i_SourcePoint, io_DestinationPoint, i_IsCaptureOption, m_Turn, out o_CurrentSoldier);
            previousPlayerTurn = m_Turn;
            io_StillAbleToCapture = true;

            if (i_IsCaptureOption == false || m_Engine.IsAbleToCapture(o_CurrentSoldier, m_Turn, ref io_DestinationPoint) == false)
            {
                io_StillAbleToCapture = false;
                m_Turn = (m_Turn + 1) % m_Players.Length;
            }

            printBoardStatus(i_TurnInput, previousPlayerTurn, io_StillAbleToCapture);
        }

        private void printResults(string i_UserTurnInput, bool i_GameOver, bool i_DrawMode)
        {

            if (i_UserTurnInput[0] == k_Quit || i_GameOver == true)
            {
                calculateScore();
                Console.WriteLine("{0} win!", m_Players[(m_Turn + 1) % m_Players.Length].PlayerName);
            }

            if (i_DrawMode == true)
            {
                Console.WriteLine("Draw!");
            }

            printScore();
        }

        private void printScore()
        {
            for (int i = 0; i < k_NumberOfPlayers; ++i)
            {
                Console.WriteLine("{0} have: {1} points", m_Players[i].PlayerName, m_Players[i].Score);
            }
        }

        private void printBoardStatus(string i_PreviousTurnMove, int i_PreviousTurn, bool i_StillAbleToCapture)
        {
            Ex02.ConsoleUtils.Screen.Clear();
            m_Engine.PrintMatrix();

            Console.WriteLine("{0}'s Move was({1}): {2}", m_Players[i_PreviousTurn].PlayerName, m_Players[i_PreviousTurn].PlayerSignOnBoard, i_PreviousTurnMove);

            if (i_StillAbleToCapture == true)
            {
                Console.Write("{0}'s Turn({1}): ", m_Players[i_PreviousTurn].PlayerName, m_Players[i_PreviousTurn].PlayerSignOnBoard);
            }
            else
            {
                Console.Write("{0}'s Turn({1}): ", m_Players[m_Turn].PlayerName, m_Players[m_Turn].PlayerSignOnBoard);
            }
        }

        private void HumanMoveValidation(ref string i_UserTurnInput, ref Point io_SourcePoint, ref Point io_DestinationPoint, out bool o_IsCaptureOption, Soldier i_CaptureSoldier, bool i_StillAbleToCapture)
        {
            bool isLegalMove;

            do
            {
                setSourceAndDestinationCordinates(i_UserTurnInput, ref io_SourcePoint, ref io_DestinationPoint);
                isLegalMove = m_Engine.PlayerValidation(m_Players[m_Turn], io_SourcePoint, io_DestinationPoint, out o_IsCaptureOption);

                if (isLegalMove == false)
                {
                    Console.WriteLine("Error choice, please enter new choice");
                    i_UserTurnInput = Console.ReadLine();
                    checkLegalityUserTurnInput(ref i_UserTurnInput, i_CaptureSoldier, i_StillAbleToCapture);

                    if (i_UserTurnInput[0] == k_Quit)
                    {
                        break;
                    }
                }
            }
            while (isLegalMove == false);
        }

        private void setSourceAndDestinationCordinates(string i_UserTurnInput, ref Point io_SourcePoint, ref Point io_DestinationPoint)
        {
            io_SourcePoint.X = i_UserTurnInput[0] - k_ColConvertor;
            io_SourcePoint.Y = i_UserTurnInput[1] - k_RowConvertor;
            io_DestinationPoint.X = i_UserTurnInput[3] - k_ColConvertor;
            io_DestinationPoint.Y = i_UserTurnInput[4] - k_RowConvertor;
        }

        private string getUserTurnInputAndCheckQuitMode(out bool o_QuitMode, Soldier i_CurrentSoldier, bool i_StillAbleToCapture)
        {
            string userTurnInput;

            userTurnInput = Console.ReadLine();
            checkLegalityUserTurnInput(ref userTurnInput, i_CurrentSoldier, i_StillAbleToCapture);

            if (userTurnInput[0] == k_Quit)
            {
                o_QuitMode = true;
            }
            else
            {
                o_QuitMode = false;
            }

            return userTurnInput;
        }

        private bool getDestinationInput(string i_UserTurnInput)
        {
            int boardSize = m_Engine.BoardSize();
            char maxRangeUpperCaseLetter = (char)(boardSize + k_ColConvertor);
            char maxRangeLowerCaseLetter = (char)(boardSize + k_RowConvertor);
            bool isLegalInput = false;

            if (i_UserTurnInput[3] >= 'A' && i_UserTurnInput[3] < maxRangeUpperCaseLetter && i_UserTurnInput[4] >= 'a' && i_UserTurnInput[4] < maxRangeLowerCaseLetter)
            {
                if (i_UserTurnInput[2] == '>')
                {
                    isLegalInput = true;
                }
            }

            return isLegalInput;
        }

        private bool isLegalUserInputMove(string i_UserTurnInput, Soldier i_CurrentSoldier, bool i_StillAbleToCapture)
        {
            bool isLegalInputMove = false;
            int boardSize = m_Engine.BoardSize();
            char maxRangeUpperCaseLetter = (char)(boardSize + k_ColConvertor);
            char maxRangeLowerCaseLetter = (char)(boardSize + k_RowConvertor);

            if (i_UserTurnInput.Length == 5)
            {
                if (i_StillAbleToCapture == true)
                {
                    if (i_UserTurnInput[0] == (char)(i_CurrentSoldier.X + k_ColConvertor) && i_UserTurnInput[1] == (char)(i_CurrentSoldier.Y + k_RowConvertor))
                    {
                        if (getDestinationInput(i_UserTurnInput) == true)
                        {
                            isLegalInputMove = true;
                        }
                    }
                }
                else if (i_UserTurnInput[0] >= 'A' && i_UserTurnInput[0] < maxRangeUpperCaseLetter && i_UserTurnInput[1] >= 'a' && i_UserTurnInput[1] < maxRangeLowerCaseLetter)
                {
                    if (getDestinationInput(i_UserTurnInput) == true)
                    {
                        isLegalInputMove = true;
                    }
                }
            }

            return isLegalInputMove;
        }

        private void checkLegalityUserTurnInput(ref string io_UserTurnInput, Soldier i_CurrentSoldier, bool i_StillAbleToCapture)
        {
            bool isLegalInput = false;

            do
            {
                if (io_UserTurnInput.Length == 1)
                {
                    isLegalInput = checkQuitMode(io_UserTurnInput);
                }
                else
                {
                    isLegalInput = isLegalUserInputMove(io_UserTurnInput, i_CurrentSoldier, i_StillAbleToCapture);
                }

                if (isLegalInput == false)
                {
                    Console.WriteLine("Illegal input, please enter legal input");
                    io_UserTurnInput = Console.ReadLine();
                }
            }
            while (isLegalInput == false);
        }

        private bool checkQuitMode(string i_UserTurnInput)
        {
            bool isLegalInput = false;

            if (i_UserTurnInput[0] == k_Quit && m_Players[m_Turn].AmountOfSoldiersValue < m_Players[(m_Turn + 1) % k_NumberOfPlayers].AmountOfSoldiersValue)
            {
                isLegalInput = true;
            }

            return isLegalInput;
        }

        private void calculateScore()
        {
            if (m_Players[0].AmountOfSoldiersValue > m_Players[1].AmountOfSoldiersValue)
            {
                m_Players[0].Score = m_Players[0].AmountOfSoldiersValue - m_Players[1].AmountOfSoldiersValue;
            }
            else if (m_Players[0].AmountOfSoldiersValue < m_Players[1].AmountOfSoldiersValue)
            {
                m_Players[1].Score = m_Players[1].AmountOfSoldiersValue - m_Players[0].AmountOfSoldiersValue;
            }
            
        }

        private bool checkDrawModeAndGameOver(out bool o_IsGameOver, out bool o_IsDrawMove)
        {
            bool currentPlayerAbleToMove;
            o_IsGameOver = false;
            o_IsDrawMove = false;

            currentPlayerAbleToMove = m_Engine.PlayerAbleToMove(m_Players[m_Turn]);

            if (currentPlayerAbleToMove == false)
            {
                if (m_Engine.PlayerAbleToMove(m_Players[(m_Turn + 1) % m_Players.Length]) == false)
                {
                    o_IsDrawMove = true;
                }
                else
                {
                    o_IsGameOver = true;
                }
            }

            return o_IsGameOver == false && o_IsDrawMove == false;
        }
    }
}
