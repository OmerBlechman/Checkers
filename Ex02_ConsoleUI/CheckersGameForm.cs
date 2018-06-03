using System;
using System.Drawing;
using System.Windows.Forms;
using CheckersGame;
using Ex05_ComplexButton;
using Player;
using PlayerSignOnBoardEnum;
using PlayerSoldier;

namespace Ex05_ConsoleUI
{
     public class CheckersGameForm : Form
     {
          private const int k_NumberOfPlayers = 2, k_PlayerLabelHeight = 20, k_SquareLeftMargin = 20, k_SquareSize = 50, k_InitialFirstPlayerYCordinate = 2, k_PlayerLabelYCordinate = 30;
          private const string k_CheckersName = "Checkers", k_ComputerName = "[Computer]", k_PlayerQuit = "Quit! Computer Won!", k_AnotherRound = "Another Round?";
          private const string k_CantQuit = "You Cant Quit", k_Draw = "Draw!", k_Win = "Win!", k_IllegalMove = "Illegal Move", k_Error = "Error", k_IllegalSquareChoice = "Illegal Square Choice";
          private readonly Label[] r_PlayersScore = new Label[2];
          private readonly int r_BoardSize;
          private readonly GamePlayer[] r_Players = new GamePlayer[k_NumberOfPlayers];
          private readonly ComplexButton[,] r_BoardSquares;
          private ComplexButton m_SourceClickedButton;
          private Soldier m_PlayerSolderStillAbleToCapture;
          private bool m_EndGame = false;
          private int m_Turn = 0;
          private Game m_Engine;
          private bool m_PlayerQuit = true;

          public CheckersGameForm(int i_BoardSize, int i_Width, int i_Height, int i_PlayerOneXLocation, int i_PlayerTwoXLocation, string i_PlayerOneName, string i_PlayerTwoName)
          {
               Text = k_CheckersName;
               r_BoardSize = i_BoardSize;
               r_BoardSquares = new ComplexButton[i_BoardSize, i_BoardSize];
               initialGame(i_PlayerOneName, i_PlayerTwoName);
               setCheckerFormResolutionAuxiliary(i_Width, i_Height, i_PlayerOneXLocation, i_PlayerTwoXLocation);
               initialCheckersLogicalLayer();
               setClickEventGameButtons();
               FormClosing += formClosing_Click;
          }

          private void initialCheckerGameForm()
          {
               initialCheckerGameFormControls();
               initialFirstLineInBoard();
               initialOtherLinesInBoard();
               setSoldierOnBoard();
          }

          private void initialCheckerGameFormControls()
          {
               r_PlayersScore[0] = new Label();
               r_PlayersScore[0].Text = string.Format("{0}: {1}", r_Players[0].PlayerName, r_Players[0].Score);
               r_PlayersScore[0].Height = k_PlayerLabelHeight;
               r_PlayersScore[1] = new Label();
               r_PlayersScore[1].Text = string.Format("{0}: {1}", r_Players[1].PlayerName, r_Players[1].Score);
               r_PlayersScore[1].Height = k_PlayerLabelHeight;
               Controls.Add(r_PlayersScore[0]);
               Controls.Add(r_PlayersScore[1]);
          }

          private void initialPlayersSoliders()
          {
               Point startPoint = new Point(0, 0);

               for (int i = 0; i < k_NumberOfPlayers; ++i)
               {
                    r_Players[i].InitialSoldiers(ref startPoint, r_BoardSize, i);
                    startPoint.Y += k_InitialFirstPlayerYCordinate;
                    r_Players[i].PlayerKind = i;
               }
          }

          private void initialPlayersGame()
          {
               for (int i = 0; i < k_NumberOfPlayers; ++i)
               {
                    r_Players[i] = new GamePlayer();
               }
          }

          private void setCheckerFormResolutionAuxiliary(int i_Width, int i_Height, int i_PlayerOneXLocation, int i_PlayerTwoXLocation)
          {
               StartPosition = FormStartPosition.CenterScreen;
               FormBorderStyle = FormBorderStyle.FixedToolWindow;
               Width = i_Width;
               Height = i_Height;
               r_PlayersScore[0].Location = new Point(i_PlayerOneXLocation, k_PlayerLabelYCordinate);
               r_PlayersScore[1].Location = new Point(r_PlayersScore[0].Right + i_PlayerTwoXLocation, k_PlayerLabelYCordinate);
          }

          private void restartGame()
          {
               m_Turn = 0;
               m_SourceClickedButton = null;
               m_PlayerSolderStillAbleToCapture = null;
               initialPlayersSoliders();
               cleanBoard();
               setSoldierOnBoard();
               initialCheckersLogicalLayer();
          }

          private void initialGame(string i_PlayerOneName, string i_PlayerTwoName)
          {
               initialPlayersGame();
               r_Players[0].PlayerName = i_PlayerOneName;
               if (i_PlayerTwoName == k_ComputerName)
               {
                    r_Players[1].PlayerName = i_PlayerTwoName.Substring(1, k_ComputerName.Length - 2);
                    r_Players[1].IsComputer = true;
               }
               else
               {
                    r_Players[1].PlayerName = i_PlayerTwoName;
               }

               initialPlayersSoliders();
               initialCheckerGameForm();
          }

          private void cleanBoard()
          {
               foreach (ComplexButton gameButton in r_BoardSquares)
               {
                    gameButton.Text = string.Empty;
               }
          }

          private void initialFirstLineInBoard()
          {
               for (int i = 0; i < r_BoardSize; ++i)
               {
                    r_BoardSquares[0, i] = new ComplexButton();
                    r_BoardSquares[0, i].Width = k_SquareSize;
                    r_BoardSquares[0, i].Height = k_SquareSize;
                    if (i % 2 == 0)
                    {
                         r_BoardSquares[0, i].BackColor = Color.Gray;
                         r_BoardSquares[0, i].Enabled = false;
                    }
                    else
                    {
                         r_BoardSquares[0, i].BackColor = Color.White;
                    }

                    if (i == 0)
                    {
                         r_BoardSquares[0, i].Location = new Point(k_SquareLeftMargin, r_PlayersScore[0].Bottom + k_PlayerLabelYCordinate);
                    }
                    else
                    {
                         r_BoardSquares[0, i].Location = new Point(r_BoardSquares[0, i - 1].Right, r_PlayersScore[0].Bottom + k_PlayerLabelYCordinate);
                    }

                    r_BoardSquares[0, i].X = i;
                    r_BoardSquares[0, i].Y = 0;
                    Controls.Add(r_BoardSquares[0, i]);
               }
          }

          private void initialOtherLinesInBoard()
          {
               for (int i = 1; i < r_BoardSize; ++i)
               {
                    for (int j = 0; j < r_BoardSize; ++j)
                    {
                         r_BoardSquares[i, j] = new ComplexButton();
                         r_BoardSquares[i, j].Width = k_SquareSize;
                         r_BoardSquares[i, j].Height = k_SquareSize;
                         if ((i + j) % 2 == 0)
                         {
                              r_BoardSquares[i, j].BackColor = Color.Gray;
                              r_BoardSquares[i, j].Enabled = false;
                         }
                         else
                         {
                              r_BoardSquares[i, j].BackColor = Color.White;
                         }

                         if (j == 0)
                         {
                              r_BoardSquares[i, j].Location = new Point(k_SquareLeftMargin, r_BoardSquares[i - 1, j].Bottom);
                         }
                         else
                         {
                              r_BoardSquares[i, j].Location = new Point(r_BoardSquares[i, j - 1].Right, r_BoardSquares[i - 1, j].Bottom);
                         }

                         r_BoardSquares[i, j].X = j;
                         r_BoardSquares[i, j].Y = i;
                         Controls.Add(r_BoardSquares[i, j]);
                    }
               }
          }

          private void setSoldierOnBoard()
          {
               for (int i = 0; i < ((r_BoardSize / 2) - 1); ++i)
               {
                    for (int j = 0; j < r_BoardSize; ++j)
                    {
                         if ((i + j) % 2 == 1)
                         {
                              r_BoardSquares[i, j].Text = Enum.GetName(typeof(ePlayerSignOnBoard), ePlayerSignOnBoard.O);
                         }
                    }
               }

               for (int i = (r_BoardSize / 2) + 1; i < r_BoardSize; ++i)
               {
                    for (int j = 0; j < r_BoardSize; ++j)
                    {
                         if ((i + j) % 2 == 1)
                         {
                              r_BoardSquares[i, j].Text = Enum.GetName(typeof(ePlayerSignOnBoard), ePlayerSignOnBoard.X);
                         }
                    }
               }
          }

          private void setClickEventGameButtons()
          {
               for (int i = 0; i < r_BoardSize; ++i)
               {
                    for (int j = 0; j < r_BoardSize; ++j)
                    {
                         if ((i + j) % 2 == 1)
                         {
                              r_BoardSquares[i, j].Click += new EventHandler(complexButton_Click);
                         }
                    }
               }
          }

          private void initialCheckersLogicalLayer()
          {
               m_Engine = new Game(r_BoardSize);
               m_Engine.m_UpdateSoldierToKingNotifier += new Action<Soldier>(updateManToKing);
               m_Engine.m_LogicErrorNotifier += logicErrorListener;
          }

          private void logicErrorListener()
          {
               MessageBox.Show(string.Format(k_IllegalMove, Environment.NewLine), k_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
          }

          private bool isPlayerSoldier(ComplexButton i_CurrentButton)
          {
               bool isLegal = false;

               if (i_CurrentButton.Text == r_Players[m_Turn].PlayerSignOnBoard(new Point(i_CurrentButton.X, i_CurrentButton.Y)))
               {
                    isLegal = true;
               }

               return isLegal;
          }

          private bool checkRecapture(ComplexButton i_CurrentButton)
          {
               return m_PlayerSolderStillAbleToCapture != null &&
                    i_CurrentButton.X == m_PlayerSolderStillAbleToCapture.X &&
                    i_CurrentButton.Y == m_PlayerSolderStillAbleToCapture.Y;
          }

          private void complexButton_Click(object sender, EventArgs e)
          {
               ComplexButton currentButton = sender as ComplexButton;

               if (m_SourceClickedButton == null)
               {
                    if (checkRecapture(currentButton) == true || (m_PlayerSolderStillAbleToCapture == null && isPlayerSoldier(currentButton) == true))
                    {
                         currentButton.BackColor = Color.DodgerBlue;
                         m_SourceClickedButton = currentButton;
                    }
                    else
                    {
                         MessageBox.Show(string.Format(k_IllegalSquareChoice, Environment.NewLine), k_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
               }
               else if (currentButton.BackColor == Color.DodgerBlue)
               {
                    currentButton.BackColor = Color.White;
                    m_SourceClickedButton = null;
               }
               else if (m_SourceClickedButton != null)
               {
                    gameMove(currentButton);
               }
          }

          private void gameMove(ComplexButton i_CurrentButton)
          {
               Point sourcePoint = new Point(m_SourceClickedButton.X, m_SourceClickedButton.Y);
               Point destinationPoint = new Point(i_CurrentButton.X, i_CurrentButton.Y);
               bool isCaptureOption, isLegalMove;

               m_SourceClickedButton.BackColor = Color.White;
               isLegalMove = m_Engine.PlayerValidation(r_Players[m_Turn], sourcePoint, destinationPoint, out isCaptureOption); //think on event operation invoke

               if (isLegalMove == true)
               {
                    if (r_Players[m_Turn].IsComputer == false)
                    {
                         operation(sourcePoint, ref destinationPoint, isCaptureOption);
                         i_CurrentButton.Text = m_SourceClickedButton.Text;
                         m_SourceClickedButton.Text = string.Empty;
                         m_SourceClickedButton = null;
                         checkEndGame();
                    }

                    if (r_Players[m_Turn].IsComputer == true)
                    {
                         Soldier currentSoldier = null;
                         computerOperation(currentSoldier, ref destinationPoint, sourcePoint);
                    }
               }

               m_SourceClickedButton = null;
          }

          private void computerOperation(Soldier i_CurrentSoldier, ref Point io_DestinationPoint, Point i_SourcePoint)
          {
               bool isCaptureOption;
               Point destinationPointBeforeChange;

               if (m_EndGame == false)
               {
                    m_Engine.ComputerOperation(r_Players[m_Turn], out isCaptureOption, m_PlayerSolderStillAbleToCapture, ref i_CurrentSoldier, ref io_DestinationPoint);
                    destinationPointBeforeChange = io_DestinationPoint;
                    i_SourcePoint.X = i_CurrentSoldier.X;
                    i_SourcePoint.Y = i_CurrentSoldier.Y;
                    m_SourceClickedButton = r_BoardSquares[i_SourcePoint.Y, i_SourcePoint.X];
                    operation(i_SourcePoint, ref io_DestinationPoint, isCaptureOption);
                    updateComputerButtons(i_SourcePoint, destinationPointBeforeChange);
                    checkEndGame();
                    if (m_PlayerSolderStillAbleToCapture != null)
                    {
                         computerOperation(i_CurrentSoldier, ref io_DestinationPoint, i_SourcePoint);
                    }
               }
          }

          private void updateComputerButtons(Point i_SourcePoint, Point i_DestinationPoint)
          {
               r_BoardSquares[i_DestinationPoint.Y, i_DestinationPoint.X].Text = r_BoardSquares[i_SourcePoint.Y, i_SourcePoint.X].Text;
               r_BoardSquares[i_SourcePoint.Y, i_SourcePoint.X].Text = string.Empty;
          }

          private void updateManToKing(Soldier i_CurrentSoldier)
          {
               if (i_CurrentSoldier.PlayerSignOnBoard == ePlayerSignOnBoard.X)
               {
                    m_SourceClickedButton.Text = Enum.GetName(typeof(ePlayerSignOnBoard), ePlayerSignOnBoard.K);
                    i_CurrentSoldier.PlayerSignOnBoard = ePlayerSignOnBoard.K;
               }
               else if (i_CurrentSoldier.PlayerSignOnBoard == ePlayerSignOnBoard.O)
               {
                    m_SourceClickedButton.Text = Enum.GetName(typeof(ePlayerSignOnBoard), ePlayerSignOnBoard.U);
                    i_CurrentSoldier.PlayerSignOnBoard = ePlayerSignOnBoard.U;
               }
          }

          private void deleteEnemySoldier(Point i_SourcePoint, Point i_DestinationPoint)
          {
               Point differencePoint = new Point(i_DestinationPoint.X - i_SourcePoint.X, i_DestinationPoint.Y - i_SourcePoint.Y);
               Point directionPoint;

               m_Engine.SetDirection(differencePoint, out directionPoint);
               r_BoardSquares[i_SourcePoint.Y + directionPoint.Y, i_SourcePoint.X + directionPoint.X].Text = string.Empty;
          }

          private void operation(Point i_SourcePoint, ref Point io_DestinationPoint, bool i_IsCaptureOption)
          {
               Soldier currentSoldier;

               m_Engine.PlayerOperation(r_Players, i_SourcePoint, io_DestinationPoint, i_IsCaptureOption, m_Turn, out currentSoldier);
               m_PlayerSolderStillAbleToCapture = currentSoldier;
               if (i_IsCaptureOption == true)
               {
                    deleteEnemySoldier(i_SourcePoint, io_DestinationPoint);
               }

               if (i_IsCaptureOption == false || m_Engine.IsAbleToCapture(currentSoldier, m_Turn, ref io_DestinationPoint) == false)
               {
                    m_PlayerSolderStillAbleToCapture = null;
                    m_Turn = (m_Turn + 1) % r_Players.Length;
               }
          }

          private void calculateScore()
          {
               if (r_Players[0].AmountOfSoldiersValue > r_Players[1].AmountOfSoldiersValue)
               {
                    r_Players[0].Score += r_Players[0].AmountOfSoldiersValue - r_Players[1].AmountOfSoldiersValue;
                    r_PlayersScore[0].Text = string.Format("{0}: {1}", r_Players[0].PlayerName, r_Players[0].Score);
               }
               else if (r_Players[0].AmountOfSoldiersValue < r_Players[1].AmountOfSoldiersValue)
               {
                    r_Players[1].Score += r_Players[1].AmountOfSoldiersValue - r_Players[0].AmountOfSoldiersValue;
                    r_PlayersScore[1].Text = string.Format("{0}: {1}", r_Players[1].PlayerName, r_Players[1].Score);
               }
          }

          private void formClosing_Click(object sender, FormClosingEventArgs e)
          {
               MessageBoxButtons buttons;
               DialogResult result;

               if (m_PlayerQuit == true)
               {
                    buttons = MessageBoxButtons.YesNo;
                    if (r_Players[m_Turn].AmountOfSoldiersValue < r_Players[(m_Turn + 1) % k_NumberOfPlayers].AmountOfSoldiersValue)
                    {
                         result = MessageBox.Show(string.Format("{0} {1}{2}{3}", r_Players[0].PlayerName, k_PlayerQuit, Environment.NewLine, k_AnotherRound), k_CheckersName, buttons, MessageBoxIcon.Question);
                         if (result == DialogResult.Yes)
                         {
                              e.Cancel = true;
                              restartGame();
                         }
                         else
                         {
                              e.Cancel = false;
                         }
                    }
                    else
                    {
                         MessageBox.Show(string.Format("{0}{1}", k_CantQuit, Environment.NewLine), k_CheckersName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                         e.Cancel = true;
                    }
               }
          }

          private void checkEndGame()
          {
               bool currentPlayerAbleToMove;
               MessageBoxButtons buttons;
               DialogResult result;

               buttons = MessageBoxButtons.YesNo;
               currentPlayerAbleToMove = m_Engine.PlayerAbleToMove(r_Players[m_Turn], m_Turn);
               
               if (currentPlayerAbleToMove == false)
               {
                    if (m_Engine.PlayerAbleToMove(r_Players[(m_Turn + 1) % r_Players.Length], m_Turn) == false)
                    {
                         result = MessageBox.Show(string.Format("{0} {1}{2}", k_Draw, Environment.NewLine, k_AnotherRound), k_CheckersName, buttons, MessageBoxIcon.Question);
                    }
                    else
                    {
                         result = MessageBox.Show(
                              string.Format("{0} {1}{2}{3}", r_Players[(m_Turn + 1) % r_Players.Length].PlayerName, k_Win, Environment.NewLine, k_AnotherRound),
                              k_CheckersName,
                              buttons,
                              MessageBoxIcon.Question);
                    }

                    calculateScore();

                    if (result == DialogResult.Yes)
                    {
                         restartGame();
                    }
                    else
                    {
                         m_EndGame = true;
                         m_PlayerQuit = false;
                         Close();
                    }
               }
          }
     }
}
