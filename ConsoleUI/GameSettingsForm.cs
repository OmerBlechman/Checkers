using System;
using System.Windows.Forms;
using BoardSizeEnum;

namespace Ex05_GameSettingForm
{
     public partial class GameSettingsForm : Form
     {
          private const string k_Error = "Error", k_IllegalInput = "Illegal Input!!", k_ComputerName = "[Computer]";
          private const string k_DefaultPlayerOneName = "Player 1", k_DefaultPlayerTwoName = "Player 2";
          private eBoardSize m_BoardSize = eBoardSize.NOT_INITIAL;

          public GameSettingsForm()
          {
               InitializeComponent();
               FormClosing += formClosing_Click;
          }

          private void formClosing_Click(object sender, FormClosingEventArgs e)
          {
               if(m_BoardSize == eBoardSize.NOT_INITIAL)
               {
                    m_BoardSize = eBoardSize.SIX_ON_SIX;
               }

               if (textBoxPlayerOne.Text == string.Empty)
               {
                    textBoxPlayerOne.Text = k_DefaultPlayerOneName;
               }

               if (checkBoxPlayerTwo.Checked == true && textBoxPlayerTwo.Text == string.Empty)
               {
                    textBoxPlayerTwo.Text = k_DefaultPlayerTwoName;
               }
          }

          private void checkBox_Click(object sender, EventArgs e)
          {
               if (checkBoxPlayerTwo.Checked == true)
               {
                    textBoxPlayerTwo.Enabled = true;
                    textBoxPlayerTwo.Text = string.Empty;
               }
               else
               {
                    textBoxPlayerTwo.Enabled = false;
                    textBoxPlayerTwo.Text = k_ComputerName;
               }
          }

          private void radioButtonSixOnSix_CheckedChanged(object sender, EventArgs e)
          {
               m_BoardSize = eBoardSize.SIX_ON_SIX;
          }

          private void radioButtonEightOnEight_CheckedChanged(object sender, EventArgs e)
          {
               m_BoardSize = eBoardSize.EIGHT_ON_EIGHT;
          }

          private void radioButtonTenOnTen_CheckedChanged(object sender, EventArgs e)
          {
               m_BoardSize = eBoardSize.TEN_ON_TEN;
          }

          private void buttonDone_Click(object sender, EventArgs e)
          {
               if (textBoxPlayerOne.Text != string.Empty && textBoxPlayerTwo.Text != string.Empty && m_BoardSize != eBoardSize.NOT_INITIAL)
               {
                    Close();
               }
               else
               {
                    MessageBox.Show(k_IllegalInput, k_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
               }
          }

          public string PlayerOneName
          {
               get { return textBoxPlayerOne.Text; }
          }

          public string PlayerTwoName
          {
               get { return textBoxPlayerTwo.Text; }
          }

          public eBoardSize BoardSize
          {
               get { return m_BoardSize; }
          }
     }
}
