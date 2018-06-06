using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using PlayerSoldier;
using Player;
using Board;
using SoldierKindEnum;

namespace CheckersGameLogic
{
     public delegate void LogicErrorNotifierDelegate();

     public delegate void LogicOperationNotifierDelegate(Soldier i_CurrentSoldier, Point i_SourcePoint, ref Point io_DestinationPoint, bool i_IsCaptureOption);

     public delegate void LogicValidationNotifierDelegate(Point i_SourcePoint, Point i_DestinationPoint, bool i_IsCaptureOption, bool i_IsLegalMove);

     public class CheckersGame
     {
          public event Action<Soldier> UpdateSoldierToKingNotifier;

          public event LogicOperationNotifierDelegate LogicOperationNotifier;

          public event LogicValidationNotifierDelegate LogicValidationNotifier;

          public event LogicErrorNotifierDelegate LogicErrorNotifier;

          private const int k_computerTurn = 1, k_ManValue = 1, k_KingValue = 4;
          private GameBoard m_Board;

          public CheckersGame(int i_BoardSize)
          {
               m_Board = new GameBoard();
               m_Board.InitialBoard(i_BoardSize);
          }

          public int BoardSize()
          {
               return m_Board.BoardSize;
          }

          public void SetDirection(Point i_DifferencePoint, out Point o_DirectionPoint)
          {
               o_DirectionPoint = new Point();
               if (i_DifferencePoint.X > 0)
               {
                    o_DirectionPoint.X = 1;
               }
               else
               {
                    o_DirectionPoint.X = -1;
               }

               if (i_DifferencePoint.Y > 0)
               {
                    o_DirectionPoint.Y = 1;
               }
               else
               {
                    o_DirectionPoint.Y = -1;
               }
          }

          public void PlayerValidation(GamePlayer i_CurrentPlayer, Point i_SourcePoint, Point i_DestinationPoint)
          {
               bool isCaptureOption = false;
               bool isLegalMove = true;
               Point directionPoint;
               Point differencePoint = new Point(i_DestinationPoint.X - i_SourcePoint.X, i_DestinationPoint.Y - i_SourcePoint.Y);

               isLegalMove = true;
               SetDirection(differencePoint, out directionPoint);

               if (m_Board.CheckLegalMove(i_SourcePoint, i_DestinationPoint, i_CurrentPlayer.PlayerKind, directionPoint) == false)
               {
                    isLegalMove = false;
               }

               isCaptureOption = (checkCaptureOption(i_CurrentPlayer) == true) && (isLegalMove == true);

               if (isCaptureOption == true)
               {
                    if (m_Board.CheckCapture(i_SourcePoint, directionPoint, i_CurrentPlayer.PlayerKind) == false)
                    {
                         isLegalMove = false;
                    }
               }

               if (isCaptureOption == false && Math.Abs(differencePoint.X) == 2 && Math.Abs(differencePoint.Y) == 2)
               {
                    isLegalMove = false;
               }

               if (isLegalMove == false)
               {
                    if (LogicErrorNotifier != null)
                    {
                         LogicErrorNotifier.Invoke();
                    }
               }

               if (LogicValidationNotifier != null)
               {
                    LogicValidationNotifier.Invoke(i_SourcePoint, i_DestinationPoint, isCaptureOption, isLegalMove);
               }
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

          public void PlayerOperation(GamePlayer[] i_PlayersGame, Point i_SourcePoint, ref Point io_DestinationPoint, bool i_IsCapture, int i_PlayerTurn)
          {
               Soldier currentSoldier;
               Point directionPoint;
               Point differencePoint = new Point(io_DestinationPoint.X - i_SourcePoint.X, io_DestinationPoint.Y - i_SourcePoint.Y);
               int enemyTurn = (i_PlayerTurn + 1) % i_PlayersGame.Length;

               SetDirection(differencePoint, out directionPoint);
               currentSoldier = i_PlayersGame[i_PlayerTurn].FindCurrentSoldier(i_SourcePoint);
               currentSoldier.X = io_DestinationPoint.X;
               currentSoldier.Y = io_DestinationPoint.Y;

               if (i_IsCapture == true)
               {
                    removeEnemySoldier(i_PlayersGame[enemyTurn], i_SourcePoint, directionPoint);
               }

               if (checkKingUpdating(currentSoldier.Y) == true && currentSoldier.SoldierKind == eSoldierKind.MAN)
               {
                    i_PlayersGame[i_PlayerTurn].AmountOfSoldiersValue += 3;
                    currentSoldier.SoldierKind = eSoldierKind.KING;
                    if (UpdateSoldierToKingNotifier != null)
                    {
                         UpdateSoldierToKingNotifier.Invoke(currentSoldier);
                    }
               }

               m_Board.UpdateSoldierPosition(i_SourcePoint, io_DestinationPoint, directionPoint, i_PlayerTurn, i_IsCapture);

               if (LogicOperationNotifier != null)
               {
                    LogicOperationNotifier.Invoke(currentSoldier, i_SourcePoint, ref io_DestinationPoint, i_IsCapture);
               }
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
                    i_EnemyPlayer.AmountOfSoldiersValue -= k_ManValue;
               }
               else
               {
                    i_EnemyPlayer.AmountOfSoldiersValue -= k_KingValue;
               }
          }

          public bool PlayerAbleToMove(GamePlayer i_CurrentPlayer, int i_TurnPlayer)
          {
               bool ableToMove = false;
               Point currentDestinationPoint = new Point();
               Point directionPoint = new Point();

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
                                   if (ableToMove == false)
                                   {
                                        directionPoint.X = rowDirection;
                                        directionPoint.Y = colDirection;
                                        ableToMove = m_Board.CheckCapture(i_CurrentPlayer.PlayersSoldier[i].SoldierPoint, directionPoint, i_TurnPlayer);
                                   }
                              }
                         }
                    }
               }

               return ableToMove;
          }

          private bool soldierAbleToMove(Soldier i_CurrentSoldier, Point i_DestinationPoint, int i_PlayerTurn)
          {
               bool isAbleToMove = true;
               Point differencePoint = new Point(i_DestinationPoint.X - i_CurrentSoldier.X, i_DestinationPoint.Y - i_CurrentSoldier.Y);
               Point directionPoint;

               SetDirection(differencePoint, out directionPoint);

               isAbleToMove = m_Board.CheckLegalMove(i_CurrentSoldier.SoldierPoint, i_DestinationPoint, i_PlayerTurn, directionPoint);

               return isAbleToMove;
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

          private int checkAmountSoldierCapture(
               int i_YCordinate,
               int i_XCordinate, 
               ref Point io_Destination,
               ref int io_MaximumCaptures,
               int i_SourceYDirection,
               int i_SourceXDirection)
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
                              if (i_SourceYDirection != i && i_SourceXDirection != j)
                              {
                                   amountOfSoldierCaptures++;
                                   amountOfSoldierCaptures += checkAmountSoldierCapture(sourcerPoint.Y + (2 * i), sourcerPoint.X + (2 * j), ref io_Destination, ref io_MaximumCaptures, -i, -j);

                                   if (amountOfSoldierCaptures > io_MaximumCaptures)
                                   {
                                        io_MaximumCaptures = amountOfSoldierCaptures;
                                        io_Destination.Y = sourcerPoint.Y + (2 * i);
                                        io_Destination.X = sourcerPoint.X + (2 * j);
                                   }
                              }
                         }
                    }
               }

               return amountOfSoldierCaptures;
          }

          public void ComputerOperation(
               GamePlayer i_ComputerPlayer,
               Soldier i_PlayerSolderStillAbleToCapture,
               ref Soldier io_CaptureSoldier,
               ref Point io_DestinationPoint,
               Point i_SourcePoint)
          {
               bool isCaptureOption = false;
               List<Soldier> ableToCapture;

               if (i_PlayerSolderStillAbleToCapture != null)
               {
                    isCaptureOption = true;
               }
               else
               {
                    ableToCapture = soldiersAbleToCapture(i_ComputerPlayer);

                    if (ableToCapture.Count > 0)
                    {
                         isCaptureOption = true;
                         io_CaptureSoldier = findMostCaptureSoldier(ableToCapture, ref io_DestinationPoint);
                    }
                    else
                    {
                         io_CaptureSoldier = findSoldierCanBecomeKing(i_ComputerPlayer, ref io_DestinationPoint);
                         if (io_CaptureSoldier == null)
                         {
                              io_CaptureSoldier = findSoldierAbleToMove(i_ComputerPlayer, ref io_DestinationPoint);
                         }
                    }
               }

               if (LogicOperationNotifier != null)
               {
                    LogicOperationNotifier.Invoke(io_CaptureSoldier, i_SourcePoint, ref io_DestinationPoint, isCaptureOption);
               }
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
               List<Point> destinationOptions = new List<Point>();
               Point directionPoint = new Point();
               bool foundSoldier = false;
               Random randomIndex = new Random();
               int currentIndex;

               while (foundSoldier == false)
               {
                    currentIndex = randomIndex.Next(0, i_ComputerPlayer.PlayersSoldier.Count);

                    for (int i = -1; i < 2; i += 2)
                    {
                         directionPoint.Y = i;
                         currentCheckDestinatioPoint.Y = i_ComputerPlayer.PlayersSoldier[currentIndex].Y + i;

                         for (int j = -1; j < 2; j += 2)
                         {
                              directionPoint.X = j;
                              currentCheckDestinatioPoint.X = i_ComputerPlayer.PlayersSoldier[currentIndex].X + j;

                              if (m_Board.isSoldierInBoardLimits(currentCheckDestinatioPoint) == true)
                              {
                                   if (soldierAbleToMove(i_ComputerPlayer.PlayersSoldier[currentIndex], currentCheckDestinatioPoint, i_ComputerPlayer.PlayerKind))
                                   {
                                        io_DestinationPoint = currentCheckDestinatioPoint;
                                        currentSoldier = i_ComputerPlayer.PlayersSoldier[currentIndex];
                                        foundSoldier = true;
                                        destinationOptions.Add(io_DestinationPoint);
                                   }
                              }
                         }
                    }
               }

               currentIndex = randomIndex.Next(0, destinationOptions.Count);
               io_DestinationPoint = destinationOptions[currentIndex];

               return currentSoldier;
          }
     }
}