using System;
using System.Windows.Forms;
using BoardSizeEnum;

namespace Ex05_ConsoleUI
{
     public partial class GameSettingsForm : Form
     {
          private const string k_Error = "Error", k_IllegalInput = "Illegal Input!!", k_ComputerName = "[Computer]";
          private eBoardSize m_BoardSize = eBoardSize.NOT_INITIAL;
          private bool m_ExitMode = false;
          private bool m_DoneButtonCloseFrom = false;

          public bool ExitMode
          {
               get
               {
                    return m_ExitMode;
               }
          }

          public GameSettingsForm()
          {
               InitializeComponent();
               FormClosing += formClosing_Click;
          }

          private void formClosing_Click(object sender, FormClosingEventArgs e)
          {
               if (m_DoneButtonCloseFrom == false)
               {
                    m_ExitMode = true;
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

          private void radioButtonSixOnSix_Click(object sender, EventArgs e)
          {
               m_BoardSize = eBoardSize.SIX_ON_SIX;
          }

          private void radioButtonEightOnEight_Click(object sender, EventArgs e)
          {
               m_BoardSize = eBoardSize.EIGHT_ON_EIGHT;
          }

          private void radioButtonTenOnTen_Click(object sender, EventArgs e)
          {
               m_BoardSize = eBoardSize.TEN_ON_TEN;
          }

          private void buttonDone_Click(object sender, EventArgs e)
          {
               if (textBoxPlayerOne.Text != string.Empty && textBoxPlayerTwo.Text != string.Empty && m_BoardSize != eBoardSize.NOT_INITIAL)
               {
                    m_DoneButtonCloseFrom = true;
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
