namespace SignalView
{
	partial class SignalChart
	{
		/// <summary> 
		/// Требуется переменная конструктора.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Освободить все используемые ресурсы.
		/// </summary>
		/// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Код, автоматически созданный конструктором компонентов

		/// <summary> 
		/// Обязательный метод для поддержки конструктора - не изменяйте 
		/// содержимое данного метода при помощи редактора кода.
		/// </summary>
		private void InitializeComponent()
		{
			this.XScrollRightButton = new System.Windows.Forms.Button();
			this.XScrollLeftButton = new System.Windows.Forms.Button();
			this.YScrollDownButton = new System.Windows.Forms.Button();
			this.YScrollUpButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// XScrollRightButton
			// 
			this.XScrollRightButton.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
			this.XScrollRightButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
			this.XScrollRightButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.XScrollRightButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.XScrollRightButton.Location = new System.Drawing.Point(638, 494);
			this.XScrollRightButton.Name = "XScrollRightButton";
			this.XScrollRightButton.Size = new System.Drawing.Size(30, 23);
			this.XScrollRightButton.TabIndex = 3;
			this.XScrollRightButton.UseVisualStyleBackColor = true;
			this.XScrollRightButton.Click += new System.EventHandler(this.XScrollRightButton_Click);
			// 
			// XScrollLeftButton
			// 
			this.XScrollLeftButton.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
			this.XScrollLeftButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
			this.XScrollLeftButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.XScrollLeftButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.XScrollLeftButton.Location = new System.Drawing.Point(487, 494);
			this.XScrollLeftButton.Name = "XScrollLeftButton";
			this.XScrollLeftButton.Size = new System.Drawing.Size(30, 23);
			this.XScrollLeftButton.TabIndex = 2;
			this.XScrollLeftButton.UseVisualStyleBackColor = true;
			this.XScrollLeftButton.Click += new System.EventHandler(this.XScrollLeftButton_Click);
			// 
			// YScrollDownButton
			// 
			this.YScrollDownButton.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
			this.YScrollDownButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
			this.YScrollDownButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.YScrollDownButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.YScrollDownButton.Location = new System.Drawing.Point(402, 431);
			this.YScrollDownButton.Name = "YScrollDownButton";
			this.YScrollDownButton.Size = new System.Drawing.Size(23, 30);
			this.YScrollDownButton.TabIndex = 1;
			this.YScrollDownButton.UseVisualStyleBackColor = true;
			this.YScrollDownButton.Click += new System.EventHandler(this.YScrollDownButton_Click);
			// 
			// YScrollUpButton
			// 
			this.YScrollUpButton.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
			this.YScrollUpButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
			this.YScrollUpButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.YScrollUpButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.YScrollUpButton.Location = new System.Drawing.Point(402, 377);
			this.YScrollUpButton.Name = "YScrollUpButton";
			this.YScrollUpButton.Size = new System.Drawing.Size(23, 30);
			this.YScrollUpButton.TabIndex = 0;
			this.YScrollUpButton.UseVisualStyleBackColor = true;
			this.YScrollUpButton.Click += new System.EventHandler(this.YScrollUpButton_Click);
			// 
			// SignalChart
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.Controls.Add(this.YScrollUpButton);
			this.Controls.Add(this.YScrollDownButton);
			this.Controls.Add(this.XScrollLeftButton);
			this.Controls.Add(this.XScrollRightButton);
			this.Name = "SignalChart";
			this.Size = new System.Drawing.Size(754, 531);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button XScrollRightButton;
		private System.Windows.Forms.Button XScrollLeftButton;
		private System.Windows.Forms.Button YScrollDownButton;
		private System.Windows.Forms.Button YScrollUpButton;

	}
}
