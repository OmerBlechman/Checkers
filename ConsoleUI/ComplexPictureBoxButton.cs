using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Ex05_ComplexPictureBoxButton
{
     public class ComplexPictureBoxButton : PictureBox
     {
          private Point m_LocationOnBoard = new Point();

          public int X
          {
               get
               {
                    return m_LocationOnBoard.X;
               }

               set
               {
                    m_LocationOnBoard.X = value;
               }
          }

          public int Y
          {
               get
               {
                    return m_LocationOnBoard.Y;
               }

               set
               {
                    m_LocationOnBoard.Y = value;
               }
          }
     }
}
