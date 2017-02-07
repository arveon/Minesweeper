namespace Minesweeper
{
	partial class GameScreen
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
			this.SuspendLayout();
			// 
			// GameScreen
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			switch (Control.dif)
			{
				case Constants.Difficulty.Easy:
					this.ClientSize = new System.Drawing.Size(Constants.EASY_SCREEN_WIDTH, Constants.EASY_SCREEN_HEIGHT);
					break;
				case Constants.Difficulty.Medium:
					this.ClientSize = new System.Drawing.Size(Constants.MEDIUM_SCREEN_WIDTH, Constants.MEDIUM_SCREEN_HEIGHT);
					break;
				case Constants.Difficulty.Hard:
					this.ClientSize = new System.Drawing.Size(Constants.HARD_SCREEN_WIDTH, Constants.HARD_SCREEN_HEIGHT);
					break;
			}
			
			this.Name = "GameScreen";
			this.Text = "Minesweeper";
			this.Load += new System.EventHandler(this.GameScreen_Load);
			this.ResumeLayout(false);

		}

		#endregion
	}
}