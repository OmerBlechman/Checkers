namespace Ex05_GameSettingForm
{
     public partial class GameSettingsForm
     {
          /// <summary>
          /// Required designer variable.
          /// </summary>
          private System.ComponentModel.IContainer components = null;

          /// <summary>
          /// Clean up any resources being used.
          /// </summary>
          /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
          protected override void Dispose(bool disposing)
          {
               if (disposing && (components != null))
               {
                    components.Dispose();
               }

               base.Dispose(disposing);
          }

          #region Windows Form Designer generated code

          /// <summary>
          /// Required method for Designer support - do not modify
          /// the contents of this method with the code editor.
          /// </summary>
          private void InitializeComponent()
          {
               this.buttonDone = new System.Windows.Forms.Button();
               this.labelBoardSize = new System.Windows.Forms.Label();
               this.radioButtonSixOnSix = new System.Windows.Forms.RadioButton();
               this.radioButtonEightOnEight = new System.Windows.Forms.RadioButton();
               this.radioButtonTenOnTen = new System.Windows.Forms.RadioButton();
               this.labelPlayers = new System.Windows.Forms.Label();
               this.labelPlayerOne = new System.Windows.Forms.Label();
               this.textBoxPlayerOne = new System.Windows.Forms.TextBox();
               this.checkBoxPlayerTwo = new System.Windows.Forms.CheckBox();
               this.textBoxPlayerTwo = new System.Windows.Forms.TextBox();
               this.SuspendLayout();
               // 
               // buttonDone
               // 
               this.buttonDone.BackColor = System.Drawing.SystemColors.Control;
               this.buttonDone.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
               this.buttonDone.Location = new System.Drawing.Point(133, 140);
               this.buttonDone.Name = "buttonDone";
               this.buttonDone.Size = new System.Drawing.Size(75, 23);
               this.buttonDone.TabIndex = 0;
               this.buttonDone.Text = "Done";
               this.buttonDone.UseVisualStyleBackColor = false;
               this.buttonDone.Click += new System.EventHandler(this.buttonDone_Click);
               // 
               // labelBoardSize
               // 
               this.labelBoardSize.AutoSize = true;
               this.labelBoardSize.Location = new System.Drawing.Point(13, 13);
               this.labelBoardSize.Name = "labelBoardSize";
               this.labelBoardSize.Size = new System.Drawing.Size(61, 13);
               this.labelBoardSize.TabIndex = 1;
               this.labelBoardSize.Text = "Board Size:";
               // 
               // radioButtonSixOnSix
               // 
               this.radioButtonSixOnSix.AutoSize = true;
               this.radioButtonSixOnSix.Location = new System.Drawing.Point(26, 30);
               this.radioButtonSixOnSix.Name = "radioButtonSixOnSix";
               this.radioButtonSixOnSix.Size = new System.Drawing.Size(48, 17);
               this.radioButtonSixOnSix.TabIndex = 2;
               this.radioButtonSixOnSix.TabStop = true;
               this.radioButtonSixOnSix.Text = "6 x 6";
               this.radioButtonSixOnSix.UseVisualStyleBackColor = true;
               this.radioButtonSixOnSix.CheckedChanged += new System.EventHandler(this.radioButtonSixOnSix_CheckedChanged);
               // 
               // radioButtonEightOnEight
               // 
               this.radioButtonEightOnEight.AutoSize = true;
               this.radioButtonEightOnEight.Location = new System.Drawing.Point(81, 30);
               this.radioButtonEightOnEight.Name = "radioButtonEightOnEight";
               this.radioButtonEightOnEight.Size = new System.Drawing.Size(48, 17);
               this.radioButtonEightOnEight.TabIndex = 3;
               this.radioButtonEightOnEight.TabStop = true;
               this.radioButtonEightOnEight.Text = "8 x 8";
               this.radioButtonEightOnEight.UseVisualStyleBackColor = true;
               this.radioButtonEightOnEight.CheckedChanged += new System.EventHandler(this.radioButtonEightOnEight_CheckedChanged);
               // 
               // radioButtonTenOnTen
               // 
               this.radioButtonTenOnTen.AutoSize = true;
               this.radioButtonTenOnTen.Location = new System.Drawing.Point(136, 30);
               this.radioButtonTenOnTen.Name = "radioButtonTenOnTen";
               this.radioButtonTenOnTen.Size = new System.Drawing.Size(60, 17);
               this.radioButtonTenOnTen.TabIndex = 4;
               this.radioButtonTenOnTen.TabStop = true;
               this.radioButtonTenOnTen.Text = "10 x 10";
               this.radioButtonTenOnTen.UseVisualStyleBackColor = true;
               this.radioButtonTenOnTen.CheckedChanged += new System.EventHandler(this.radioButtonTenOnTen_CheckedChanged);
               // 
               // labelPlayers
               // 
               this.labelPlayers.AutoSize = true;
               this.labelPlayers.Location = new System.Drawing.Point(16, 54);
               this.labelPlayers.Name = "labelPlayers";
               this.labelPlayers.Size = new System.Drawing.Size(44, 13);
               this.labelPlayers.TabIndex = 5;
               this.labelPlayers.Text = "Players:";
               // 
               // labelPlayerOne
               // 
               this.labelPlayerOne.AutoSize = true;
               this.labelPlayerOne.Location = new System.Drawing.Point(26, 80);
               this.labelPlayerOne.Name = "labelPlayerOne";
               this.labelPlayerOne.Size = new System.Drawing.Size(45, 13);
               this.labelPlayerOne.TabIndex = 6;
               this.labelPlayerOne.Text = "Player1:";
               // 
               // textBoxPlayerOne
               // 
               this.textBoxPlayerOne.Location = new System.Drawing.Point(108, 77);
               this.textBoxPlayerOne.Name = "textBoxPlayerOne";
               this.textBoxPlayerOne.Size = new System.Drawing.Size(100, 20);
               this.textBoxPlayerOne.TabIndex = 7;
               // 
               // checkBoxPlayerTwo
               // 
               this.checkBoxPlayerTwo.AutoSize = true;
               this.checkBoxPlayerTwo.ForeColor = System.Drawing.SystemColors.ControlText;
               this.checkBoxPlayerTwo.Location = new System.Drawing.Point(29, 110);
               this.checkBoxPlayerTwo.Name = "checkBoxPlayerTwo";
               this.checkBoxPlayerTwo.Size = new System.Drawing.Size(64, 17);
               this.checkBoxPlayerTwo.TabIndex = 8;
               this.checkBoxPlayerTwo.Text = "Player2:";
               this.checkBoxPlayerTwo.UseVisualStyleBackColor = true;
               this.checkBoxPlayerTwo.CheckedChanged += new System.EventHandler(this.checkBox_Click);
               // 
               // textBoxPlayerTwo
               // 
               this.textBoxPlayerTwo.Enabled = false;
               this.textBoxPlayerTwo.Location = new System.Drawing.Point(108, 108);
               this.textBoxPlayerTwo.Name = "textBoxPlayerTwo";
               this.textBoxPlayerTwo.Size = new System.Drawing.Size(100, 20);
               this.textBoxPlayerTwo.TabIndex = 9;
               this.textBoxPlayerTwo.Text = "[Computer]";
               // 
               // GameSettingsForm
               // 
               this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
               this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
               this.ClientSize = new System.Drawing.Size(216, 173);
               this.Controls.Add(this.textBoxPlayerTwo);
               this.Controls.Add(this.checkBoxPlayerTwo);
               this.Controls.Add(this.textBoxPlayerOne);
               this.Controls.Add(this.labelPlayerOne);
               this.Controls.Add(this.labelPlayers);
               this.Controls.Add(this.radioButtonTenOnTen);
               this.Controls.Add(this.radioButtonEightOnEight);
               this.Controls.Add(this.radioButtonSixOnSix);
               this.Controls.Add(this.labelBoardSize);
               this.Controls.Add(this.buttonDone);
               this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
               this.Name = "GameSettingsForm";
               this.ShowInTaskbar = false;
               this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
               this.Text = "Game Settings";
               this.ResumeLayout(false);
               this.PerformLayout();

          }

          #endregion

          private System.Windows.Forms.Button buttonDone;
          private System.Windows.Forms.Label labelBoardSize;
          private System.Windows.Forms.RadioButton radioButtonSixOnSix;
          private System.Windows.Forms.RadioButton radioButtonEightOnEight;
          private System.Windows.Forms.RadioButton radioButtonTenOnTen;
          private System.Windows.Forms.Label labelPlayers;
          private System.Windows.Forms.Label labelPlayerOne;
          private System.Windows.Forms.TextBox textBoxPlayerOne;
          private System.Windows.Forms.CheckBox checkBoxPlayerTwo;
          private System.Windows.Forms.TextBox textBoxPlayerTwo;
     }
}