
namespace Tetris
{
    partial class mainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.view_field = new System.Windows.Forms.PictureBox();
            this.next_1 = new System.Windows.Forms.PictureBox();
            this.next_2 = new System.Windows.Forms.PictureBox();
            this.next_3 = new System.Windows.Forms.PictureBox();
            this.next_4 = new System.Windows.Forms.PictureBox();
            this.next_5 = new System.Windows.Forms.PictureBox();
            this.freeFall = new System.Windows.Forms.Timer(this.components);
            this.NEXTLabel = new System.Windows.Forms.Label();
            this.gameOverText = new System.Windows.Forms.Label();
            this.view_hold = new System.Windows.Forms.PictureBox();
            this.HOLDLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.view_field)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.next_1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.next_2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.next_3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.next_4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.next_5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.view_hold)).BeginInit();
            this.SuspendLayout();
            // 
            // view_field
            // 
            this.view_field.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.view_field.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.view_field.Location = new System.Drawing.Point(288, 45);
            this.view_field.Name = "view_field";
            this.view_field.Size = new System.Drawing.Size(200, 400);
            this.view_field.TabIndex = 0;
            this.view_field.TabStop = false;
            // 
            // next_1
            // 
            this.next_1.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.next_1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.next_1.Location = new System.Drawing.Point(494, 45);
            this.next_1.Name = "next_1";
            this.next_1.Size = new System.Drawing.Size(50, 50);
            this.next_1.TabIndex = 1;
            this.next_1.TabStop = false;
            // 
            // next_2
            // 
            this.next_2.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.next_2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.next_2.Location = new System.Drawing.Point(494, 101);
            this.next_2.Name = "next_2";
            this.next_2.Size = new System.Drawing.Size(50, 50);
            this.next_2.TabIndex = 2;
            this.next_2.TabStop = false;
            // 
            // next_3
            // 
            this.next_3.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.next_3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.next_3.Location = new System.Drawing.Point(494, 157);
            this.next_3.Name = "next_3";
            this.next_3.Size = new System.Drawing.Size(50, 50);
            this.next_3.TabIndex = 3;
            this.next_3.TabStop = false;
            // 
            // next_4
            // 
            this.next_4.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.next_4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.next_4.Location = new System.Drawing.Point(494, 213);
            this.next_4.Name = "next_4";
            this.next_4.Size = new System.Drawing.Size(50, 50);
            this.next_4.TabIndex = 4;
            this.next_4.TabStop = false;
            // 
            // next_5
            // 
            this.next_5.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.next_5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.next_5.Location = new System.Drawing.Point(494, 269);
            this.next_5.Name = "next_5";
            this.next_5.Size = new System.Drawing.Size(50, 50);
            this.next_5.TabIndex = 5;
            this.next_5.TabStop = false;
            // 
            // freeFall
            // 
            this.freeFall.Interval = 750;
            this.freeFall.Tick += new System.EventHandler(this.freeFall_Tick);
            // 
            // NEXTLabel
            // 
            this.NEXTLabel.AutoSize = true;
            this.NEXTLabel.Font = new System.Drawing.Font("Sitka Display", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.NEXTLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.NEXTLabel.Location = new System.Drawing.Point(485, 9);
            this.NEXTLabel.Name = "NEXTLabel";
            this.NEXTLabel.Size = new System.Drawing.Size(73, 35);
            this.NEXTLabel.TabIndex = 6;
            this.NEXTLabel.Text = "NEXT";
            // 
            // gameOverText
            // 
            this.gameOverText.AutoSize = true;
            this.gameOverText.Font = new System.Drawing.Font("Yu Gothic UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.gameOverText.ForeColor = System.Drawing.Color.Red;
            this.gameOverText.Location = new System.Drawing.Point(279, 116);
            this.gameOverText.Name = "gameOverText";
            this.gameOverText.Size = new System.Drawing.Size(209, 45);
            this.gameOverText.TabIndex = 7;
            this.gameOverText.Text = "GAME  OVER";
            this.gameOverText.Visible = false;
            // 
            // view_hold
            // 
            this.view_hold.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.view_hold.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.view_hold.Location = new System.Drawing.Point(221, 45);
            this.view_hold.Name = "view_hold";
            this.view_hold.Size = new System.Drawing.Size(50, 50);
            this.view_hold.TabIndex = 9;
            this.view_hold.TabStop = false;
            // 
            // HOLDLabel
            // 
            this.HOLDLabel.AutoSize = true;
            this.HOLDLabel.Font = new System.Drawing.Font("Sitka Display", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.HOLDLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.HOLDLabel.Location = new System.Drawing.Point(208, 9);
            this.HOLDLabel.Name = "HOLDLabel";
            this.HOLDLabel.Size = new System.Drawing.Size(77, 35);
            this.HOLDLabel.TabIndex = 10;
            this.HOLDLabel.Text = "HOLD";
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(851, 495);
            this.Controls.Add(this.HOLDLabel);
            this.Controls.Add(this.view_hold);
            this.Controls.Add(this.gameOverText);
            this.Controls.Add(this.NEXTLabel);
            this.Controls.Add(this.next_5);
            this.Controls.Add(this.next_4);
            this.Controls.Add(this.next_3);
            this.Controls.Add(this.next_2);
            this.Controls.Add(this.next_1);
            this.Controls.Add(this.view_field);
            this.Name = "mainForm";
            this.Text = "mainForm";
            this.Load += new System.EventHandler(this.mainForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.mainForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.view_field)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.next_1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.next_2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.next_3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.next_4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.next_5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.view_hold)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox view_field;
        private System.Windows.Forms.PictureBox next_1;
        private System.Windows.Forms.PictureBox next_2;
        private System.Windows.Forms.PictureBox next_3;
        private System.Windows.Forms.PictureBox next_4;
        private System.Windows.Forms.PictureBox next_5;
        private System.Windows.Forms.Timer freeFall;
        private System.Windows.Forms.Label NEXTLabel;
        private System.Windows.Forms.Label gameOverText;
        private System.Windows.Forms.PictureBox view_hold;
        private System.Windows.Forms.Label HOLDLabel;
    }
}

