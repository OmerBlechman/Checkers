using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using PlayerSoldier;
using PlayerKindEnum;
using PlayerSignOnBoardEnum;

namespace Player
{
     public class GamePlayer
     {
          private List<Soldier> m_Soldiers;
          private int m_AmountOfSoldiersValue = 0;
          private int m_Score = 0;
          private string m_PlayerName;
          private bool m_IsComputer = false;
          private ePlayerKind m_PlayerKind;

          public int Score
          {
               get
               {
                    return m_Score;
               }

               set
               {
                    m_Score = value;
               }
          }

          public int AmountOfSoldiersValue
          {
               get
               {
                    return m_AmountOfSoldiersValue;
               }

               set
               {
                    m_AmountOfSoldiersValue = value;
               }
          }

          public string PlayerName
          {
               get
               {
                    return m_PlayerName;
               }

               set
               {
                    m_PlayerName = value;
               }
          }

          public List<Soldier> PlayersSoldier
          {
               get
               {
                    return m_Soldiers;
               }

               set
               {
                    m_Soldiers = value;
               }
          }

          public int PlayerKind
          {
               get
               {
                    return (int)m_PlayerKind;
               }

               set
               {
                    m_PlayerKind = (ePlayerKind)value;
               }
          }

          public bool IsComputer
          {
               get
               {
                    return m_IsComputer;
               }

               set
               {
                    m_IsComputer = value;
               }
          }

          public Soldier FindCurrentSoldier(Point i_SourcePoint)
          {
               Soldier currentSoldier = null;

               for (int i = 0; i < m_Soldiers.Count; ++i)
               {
                    if (m_Soldiers[i].X == i_SourcePoint.X && m_Soldiers[i].Y == i_SourcePoint.Y)
                    {
                         currentSoldier = m_Soldiers[i];
                    }
               }

               return currentSoldier;
          }

          public void InitialSoldiers(ref Point io_SoldierPoint, int i_BoardSize, int i_PlayerIndex)
          {
               int soldierCapacity = (i_BoardSize / 2) * ((i_BoardSize / 2) - 1);
               m_Soldiers = new List<Soldier>(soldierCapacity);

               for (int i = 0; i < soldierCapacity; ++i)
               {
                    m_Soldiers.Add(new Soldier());
                    m_Soldiers[i].PlayerSignOnBoard = (ePlayerSignOnBoard)i_PlayerIndex;
                    m_Soldiers[i].Y = i_BoardSize - 1 - io_SoldierPoint.Y;
                    m_Soldiers[i].X = io_SoldierPoint.X;
                    m_AmountOfSoldiersValue++;
                    resetRowCordAndColCord(i_BoardSize, ref io_SoldierPoint);
               }
          }

          public string PlayerSignOnBoard(Point i_SoldierLocation)
          {
               string playerSignOnBoard = null;
               Soldier currentSoldier = FindCurrentSoldier(i_SoldierLocation);

               if(currentSoldier != null)
               {
                    playerSignOnBoard = currentSoldier.PlayerSignOnBoard.ToString();
               }

               return playerSignOnBoard;
          }

          private void resetRowCordAndColCord(int i_BoardSize, ref Point io_SoldierPoint)
          {
               io_SoldierPoint.X += 2;

               if (io_SoldierPoint.X == i_BoardSize)
               {
                    io_SoldierPoint.X = 1;
                    io_SoldierPoint.Y++;
               }
               else if (io_SoldierPoint.X == (i_BoardSize + 1))
               {
                    io_SoldierPoint.X = 0;
                    io_SoldierPoint.Y++;
               }
          }
     }
}
