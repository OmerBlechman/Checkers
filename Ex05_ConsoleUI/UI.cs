using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using BoardSizeEnum;
using Ex05_GameSettingForm;
using Ex05_CheckerGameForm;

namespace Ex05_UI
{
     public class UI
     {
          private int k_SixOnSixWidth = 350, k_SixOnSixHeight = 420, k_SixOnSixPlayerOneXLocation = 80, k_SixOnSixPlayerTwoXLocation = 30;
          private int k_EightOnEightWidth = 450, k_EightOnEightHeight = 520, k_EightOnEightPlayerOneXLocation = 110, k_EightOnEightPlayerTwoXLocation = 50;
          private int k_TenOnTenWidth = 550, k_TenOnTenHeight = 620, k_TenOnTenPlayerOneXLocation = 140, k_TenOnTenPlayerTwoXLocation = 70;

          private GameSettingsForm m_GameSettingForm;
          private CheckersGameForm m_CheckerGameForm;
          private eBoardSize m_BoardSize = eBoardSize.NOT_INITIAL;

          private void initialCheckersGameForm()
          {
               if (m_BoardSize == eBoardSize.SIX_ON_SIX)
               {
                    m_CheckerGameForm = new CheckersGameForm(
                         (int)m_BoardSize,
                         k_SixOnSixWidth,
                         k_SixOnSixHeight,
                         k_SixOnSixPlayerOneXLocation,
                         k_SixOnSixPlayerTwoXLocation,
                         m_GameSettingForm.PlayerOneName,
                         m_GameSettingForm.PlayerTwoName);
               }
               else if (m_BoardSize == eBoardSize.EIGHT_ON_EIGHT)
               {
                    m_CheckerGameForm = new CheckersGameForm(
                         (int)m_BoardSize,
                         k_EightOnEightWidth,
                         k_EightOnEightHeight,
                         k_EightOnEightPlayerOneXLocation,
                         k_EightOnEightPlayerTwoXLocation,
                         m_GameSettingForm.PlayerOneName,
                         m_GameSettingForm.PlayerTwoName);
               }
               else
               {
                    m_CheckerGameForm = new CheckersGameForm(
                         (int)m_BoardSize,
                         k_TenOnTenWidth,
                         k_TenOnTenHeight,
                         k_TenOnTenPlayerOneXLocation,
                         k_TenOnTenPlayerTwoXLocation,
                         m_GameSettingForm.PlayerOneName,
                         m_GameSettingForm.PlayerTwoName);
               }
          }

          public void Run()
          {
               m_GameSettingForm = new GameSettingsForm();
               m_GameSettingForm.ShowDialog();
               m_BoardSize = m_GameSettingForm.BoardSize;
               initialCheckersGameForm();
               m_CheckerGameForm.ShowDialog();
          }
     }
}