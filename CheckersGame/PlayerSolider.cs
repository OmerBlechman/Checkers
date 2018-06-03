using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using SoldierKindEnum;
using PlayerSignOnBoardEnum;

namespace PlayerSoldier
{
     public class Soldier
     {
          private Point m_SoldierPoint;
          private eSoldierKind m_SoldierKind = eSoldierKind.MAN;
          private ePlayerSignOnBoard m_PlayerSignOnBoard;

          public ePlayerSignOnBoard PlayerSignOnBoard
          {
               get
               {
                    return m_PlayerSignOnBoard;
               }

               set
               {
                    m_PlayerSignOnBoard = value;
               }
          }

          public int X
          {
               get
               {
                    return m_SoldierPoint.X;
               }

               set
               {
                    m_SoldierPoint.X = value;
               }
          }

          public int Y
          {
               get
               {
                    return m_SoldierPoint.Y;
               }

               set
               {
                    m_SoldierPoint.Y = value;
               }
          }

          public Point SoldierPoint
          {
               get
               {
                    return m_SoldierPoint;
               }
          }

          public eSoldierKind SoldierKind
          {
               get
               {
                    return m_SoldierKind;
               }

               set
               {
                    m_SoldierKind = value;
               }
          }
     }
}
