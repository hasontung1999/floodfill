namespace WindowsFormsApp3
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.openGLControl = new SharpGL.OpenGLControl();
            this.DrawLineButton = new System.Windows.Forms.Button();
            this.SetLineColor = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.DrawEllipseButton = new System.Windows.Forms.Button();
            this.DrawRectangleButton = new System.Windows.Forms.Button();
            this.DrawPentagonButton = new System.Windows.Forms.Button();
            this.DrawTimeLabel = new System.Windows.Forms.Label();
            this.DrawTimer = new System.Windows.Forms.Timer(this.components);
            this.DrawPolygonButton = new System.Windows.Forms.Button();
            this.SetLineWidthUI = new System.Windows.Forms.NumericUpDown();
            this.ClearScreen = new System.Windows.Forms.Button();
            this.SetFillColorButton = new System.Windows.Forms.Button();
            this.MoveButton = new System.Windows.Forms.Button();
            this.DrawCircleButton = new System.Windows.Forms.Button();
            this.DrawEquilTriangleButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.openGLControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SetLineWidthUI)).BeginInit();
            this.SuspendLayout();
            // 
            // openGLControl
            // 
            this.openGLControl.BackColor = System.Drawing.SystemColors.Control;
            this.openGLControl.DrawFPS = false;
            this.openGLControl.ForeColor = System.Drawing.SystemColors.WindowText;
            this.openGLControl.Location = new System.Drawing.Point(158, 58);
            this.openGLControl.Name = "openGLControl";
            this.openGLControl.OpenGLVersion = SharpGL.Version.OpenGLVersion.OpenGL2_1;
            this.openGLControl.RenderContextType = SharpGL.RenderContextType.DIBSection;
            this.openGLControl.RenderTrigger = SharpGL.RenderTrigger.TimerBased;
            this.openGLControl.Size = new System.Drawing.Size(591, 380);
            this.openGLControl.TabIndex = 0;
            this.openGLControl.OpenGLInitialized += new System.EventHandler(this.openGLControl_OpenGLInitialized);
            this.openGLControl.OpenGLDraw += new SharpGL.RenderEventHandler(this.openGLControl_OpenGLDraw);
            this.openGLControl.Resized += new System.EventHandler(this.openGLControl_Resized);
            this.openGLControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.openGLControl_MouseDown);
            this.openGLControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.openGLControl_MouseMove);
            this.openGLControl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.openGLControl_MouseUp);
            // 
            // DrawLineButton
            // 
            this.DrawLineButton.Location = new System.Drawing.Point(31, 29);
            this.DrawLineButton.Name = "DrawLineButton";
            this.DrawLineButton.Size = new System.Drawing.Size(75, 23);
            this.DrawLineButton.TabIndex = 1;
            this.DrawLineButton.Text = "Draw Line";
            this.DrawLineButton.UseVisualStyleBackColor = true;
            this.DrawLineButton.Click += new System.EventHandler(this.DrawLineButton_Click);
            // 
            // SetLineColor
            // 
            this.SetLineColor.Location = new System.Drawing.Point(23, 76);
            this.SetLineColor.Name = "SetLineColor";
            this.SetLineColor.Size = new System.Drawing.Size(105, 23);
            this.SetLineColor.TabIndex = 2;
            this.SetLineColor.Text = "Set Line Color";
            this.SetLineColor.UseVisualStyleBackColor = true;
            this.SetLineColor.Click += new System.EventHandler(this.SetLineColor_Click);
            // 
            // DrawEllipseButton
            // 
            this.DrawEllipseButton.Location = new System.Drawing.Point(113, 29);
            this.DrawEllipseButton.Name = "DrawEllipseButton";
            this.DrawEllipseButton.Size = new System.Drawing.Size(75, 23);
            this.DrawEllipseButton.TabIndex = 3;
            this.DrawEllipseButton.Text = "Draw Ellipse";
            this.DrawEllipseButton.UseVisualStyleBackColor = true;
            this.DrawEllipseButton.Click += new System.EventHandler(this.DrawEllipseButton_Click);
            // 
            // DrawRectangleButton
            // 
            this.DrawRectangleButton.Location = new System.Drawing.Point(195, 28);
            this.DrawRectangleButton.Name = "DrawRectangleButton";
            this.DrawRectangleButton.Size = new System.Drawing.Size(97, 23);
            this.DrawRectangleButton.TabIndex = 4;
            this.DrawRectangleButton.Text = "Draw Rectangle";
            this.DrawRectangleButton.UseVisualStyleBackColor = true;
            this.DrawRectangleButton.Click += new System.EventHandler(this.DrawRectangleButton_Click);
            // 
            // DrawPentagonButton
            // 
            this.DrawPentagonButton.Location = new System.Drawing.Point(299, 27);
            this.DrawPentagonButton.Name = "DrawPentagonButton";
            this.DrawPentagonButton.Size = new System.Drawing.Size(96, 23);
            this.DrawPentagonButton.TabIndex = 5;
            this.DrawPentagonButton.Text = "Draw Pentagon";
            this.DrawPentagonButton.UseVisualStyleBackColor = true;
            this.DrawPentagonButton.Click += new System.EventHandler(this.DrawPentagonButton_Click);
            // 
            // DrawTimeLabel
            // 
            this.DrawTimeLabel.AutoSize = true;
            this.DrawTimeLabel.Location = new System.Drawing.Point(668, 31);
            this.DrawTimeLabel.Name = "DrawTimeLabel";
            this.DrawTimeLabel.Size = new System.Drawing.Size(78, 13);
            this.DrawTimeLabel.TabIndex = 6;
            this.DrawTimeLabel.Text = "Draw Time: 0 s";
            // 
            // DrawTimer
            // 
            this.DrawTimer.Tick += new System.EventHandler(this.DrawTimer_Tick);
            // 
            // DrawPolygonButton
            // 
            this.DrawPolygonButton.Location = new System.Drawing.Point(402, 27);
            this.DrawPolygonButton.Name = "DrawPolygonButton";
            this.DrawPolygonButton.Size = new System.Drawing.Size(95, 23);
            this.DrawPolygonButton.TabIndex = 7;
            this.DrawPolygonButton.Text = "Draw Polygon";
            this.DrawPolygonButton.UseVisualStyleBackColor = true;
            this.DrawPolygonButton.Click += new System.EventHandler(this.DrawPolygonButton_Click);
            // 
            // SetLineWidthUI
            // 
            this.SetLineWidthUI.Location = new System.Drawing.Point(23, 134);
            this.SetLineWidthUI.Name = "SetLineWidthUI";
            this.SetLineWidthUI.Size = new System.Drawing.Size(105, 20);
            this.SetLineWidthUI.TabIndex = 8;
            this.SetLineWidthUI.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SetLineWidthUI.ValueChanged += new System.EventHandler(this.SetLineWidthUI_ValueChanged);
            // 
            // ClearScreen
            // 
            this.ClearScreen.Location = new System.Drawing.Point(23, 160);
            this.ClearScreen.Name = "ClearScreen";
            this.ClearScreen.Size = new System.Drawing.Size(105, 23);
            this.ClearScreen.TabIndex = 9;
            this.ClearScreen.Text = "Clear Screen";
            this.ClearScreen.UseVisualStyleBackColor = true;
            this.ClearScreen.Click += new System.EventHandler(this.ClearScreen_Click);
            // 
            // SetFillColorButton
            // 
            this.SetFillColorButton.Location = new System.Drawing.Point(23, 105);
            this.SetFillColorButton.Name = "SetFillColorButton";
            this.SetFillColorButton.Size = new System.Drawing.Size(105, 23);
            this.SetFillColorButton.TabIndex = 10;
            this.SetFillColorButton.Text = "Set Fill Color";
            this.SetFillColorButton.UseVisualStyleBackColor = true;
            this.SetFillColorButton.Click += new System.EventHandler(this.SetFillColorButton_Click);
            // 
            // MoveButton
            // 
            this.MoveButton.Location = new System.Drawing.Point(23, 189);
            this.MoveButton.Name = "MoveButton";
            this.MoveButton.Size = new System.Drawing.Size(105, 23);
            this.MoveButton.TabIndex = 11;
            this.MoveButton.Text = "Move Shape";
            this.MoveButton.UseVisualStyleBackColor = true;
            this.MoveButton.Click += new System.EventHandler(this.MoveButton_Click);
            // 
            // DrawCircleButton
            // 
            this.DrawCircleButton.Location = new System.Drawing.Point(501, 27);
            this.DrawCircleButton.Name = "DrawCircleButton";
            this.DrawCircleButton.Size = new System.Drawing.Size(75, 23);
            this.DrawCircleButton.TabIndex = 12;
            this.DrawCircleButton.Text = "Draw Circle";
            this.DrawCircleButton.UseVisualStyleBackColor = true;
            this.DrawCircleButton.Click += new System.EventHandler(this.DrawCircleButton_Click);
            // 
            // DrawEquilTriangleButton
            // 
            this.DrawEquilTriangleButton.Location = new System.Drawing.Point(582, 27);
            this.DrawEquilTriangleButton.Name = "DrawEquilTriangleButton";
            this.DrawEquilTriangleButton.Size = new System.Drawing.Size(75, 23);
            this.DrawEquilTriangleButton.TabIndex = 13;
            this.DrawEquilTriangleButton.Text = "Draw Equil Triangle";
            this.DrawEquilTriangleButton.UseVisualStyleBackColor = true;
            this.DrawEquilTriangleButton.Click += new System.EventHandler(this.DrawEquilTriangleButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.DrawEquilTriangleButton);
            this.Controls.Add(this.DrawCircleButton);
            this.Controls.Add(this.MoveButton);
            this.Controls.Add(this.SetFillColorButton);
            this.Controls.Add(this.ClearScreen);
            this.Controls.Add(this.SetLineWidthUI);
            this.Controls.Add(this.DrawPolygonButton);
            this.Controls.Add(this.DrawTimeLabel);
            this.Controls.Add(this.DrawPentagonButton);
            this.Controls.Add(this.DrawRectangleButton);
            this.Controls.Add(this.DrawEllipseButton);
            this.Controls.Add(this.SetLineColor);
            this.Controls.Add(this.DrawLineButton);
            this.Controls.Add(this.openGLControl);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.openGLControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SetLineWidthUI)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SharpGL.OpenGLControl openGLControl;
        private System.Windows.Forms.Button DrawLineButton;
        private System.Windows.Forms.Button SetLineColor;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button DrawEllipseButton;
        private System.Windows.Forms.Button DrawRectangleButton;
        private System.Windows.Forms.Button DrawPentagonButton;
        private System.Windows.Forms.Label DrawTimeLabel;
        private System.Windows.Forms.Timer DrawTimer;
        private System.Windows.Forms.Button DrawPolygonButton;
        private System.Windows.Forms.NumericUpDown SetLineWidthUI;
        private System.Windows.Forms.Button ClearScreen;
        private System.Windows.Forms.Button SetFillColorButton;
        private System.Windows.Forms.Button MoveButton;
        private System.Windows.Forms.Button DrawCircleButton;
        private System.Windows.Forms.Button DrawEquilTriangleButton;
    }
}

