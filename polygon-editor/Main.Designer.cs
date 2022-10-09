namespace polygon_editor
{
    partial class Main
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
            this.Canvas = new System.Windows.Forms.PictureBox();
            this.tableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.rightPanel = new System.Windows.Forms.Panel();
            this.algoGroupBox = new System.Windows.Forms.GroupBox();
            this.radioButtonBresenhamAlgo = new System.Windows.Forms.RadioButton();
            this.radioButtonSystemAlgo = new System.Windows.Forms.RadioButton();
            this.modeGroupBox = new System.Windows.Forms.GroupBox();
            this.radioButtonEditing = new System.Windows.Forms.RadioButton();
            this.radioButtonAdding = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.Canvas)).BeginInit();
            this.tableLayout.SuspendLayout();
            this.rightPanel.SuspendLayout();
            this.algoGroupBox.SuspendLayout();
            this.modeGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // Canvas
            // 
            this.Canvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Canvas.Location = new System.Drawing.Point(3, 3);
            this.Canvas.Name = "Canvas";
            this.Canvas.Size = new System.Drawing.Size(779, 697);
            this.Canvas.TabIndex = 0;
            this.Canvas.TabStop = false;
            this.Canvas.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseDoubleClick);
            this.Canvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseDown);
            this.Canvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseMove);
            this.Canvas.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseUp);
            // 
            // tableLayout
            // 
            this.tableLayout.ColumnCount = 2;
            this.tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayout.Controls.Add(this.Canvas, 0, 0);
            this.tableLayout.Controls.Add(this.rightPanel, 1, 0);
            this.tableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayout.Location = new System.Drawing.Point(0, 0);
            this.tableLayout.Name = "tableLayout";
            this.tableLayout.RowCount = 1;
            this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayout.Size = new System.Drawing.Size(982, 703);
            this.tableLayout.TabIndex = 1;
            // 
            // rightPanel
            // 
            this.rightPanel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.rightPanel.Controls.Add(this.algoGroupBox);
            this.rightPanel.Controls.Add(this.modeGroupBox);
            this.rightPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rightPanel.Location = new System.Drawing.Point(788, 3);
            this.rightPanel.Name = "rightPanel";
            this.rightPanel.Size = new System.Drawing.Size(191, 697);
            this.rightPanel.TabIndex = 1;
            // 
            // algoGroupBox
            // 
            this.algoGroupBox.Controls.Add(this.radioButtonBresenhamAlgo);
            this.algoGroupBox.Controls.Add(this.radioButtonSystemAlgo);
            this.algoGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.algoGroupBox.Location = new System.Drawing.Point(0, 100);
            this.algoGroupBox.Name = "algoGroupBox";
            this.algoGroupBox.Size = new System.Drawing.Size(191, 100);
            this.algoGroupBox.TabIndex = 1;
            this.algoGroupBox.TabStop = false;
            this.algoGroupBox.Text = "Drawing algorithm";
            // 
            // radioButtonBresenhamAlgo
            // 
            this.radioButtonBresenhamAlgo.AutoSize = true;
            this.radioButtonBresenhamAlgo.Location = new System.Drawing.Point(11, 61);
            this.radioButtonBresenhamAlgo.Name = "radioButtonBresenhamAlgo";
            this.radioButtonBresenhamAlgo.Size = new System.Drawing.Size(172, 24);
            this.radioButtonBresenhamAlgo.TabIndex = 1;
            this.radioButtonBresenhamAlgo.Text = "Bresenham algorithm";
            this.radioButtonBresenhamAlgo.UseVisualStyleBackColor = true;
            // 
            // radioButtonSystemAlgo
            // 
            this.radioButtonSystemAlgo.AutoSize = true;
            this.radioButtonSystemAlgo.Checked = true;
            this.radioButtonSystemAlgo.Location = new System.Drawing.Point(11, 31);
            this.radioButtonSystemAlgo.Name = "radioButtonSystemAlgo";
            this.radioButtonSystemAlgo.Size = new System.Drawing.Size(146, 24);
            this.radioButtonSystemAlgo.TabIndex = 0;
            this.radioButtonSystemAlgo.TabStop = true;
            this.radioButtonSystemAlgo.Text = "System algorithm";
            this.radioButtonSystemAlgo.UseVisualStyleBackColor = true;
            // 
            // modeGroupBox
            // 
            this.modeGroupBox.Controls.Add(this.radioButtonEditing);
            this.modeGroupBox.Controls.Add(this.radioButtonAdding);
            this.modeGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.modeGroupBox.Location = new System.Drawing.Point(0, 0);
            this.modeGroupBox.Name = "modeGroupBox";
            this.modeGroupBox.Size = new System.Drawing.Size(191, 100);
            this.modeGroupBox.TabIndex = 0;
            this.modeGroupBox.TabStop = false;
            this.modeGroupBox.Text = "Mode";
            // 
            // radioButtonEditing
            // 
            this.radioButtonEditing.AutoSize = true;
            this.radioButtonEditing.Location = new System.Drawing.Point(6, 56);
            this.radioButtonEditing.Name = "radioButtonEditing";
            this.radioButtonEditing.Size = new System.Drawing.Size(115, 24);
            this.radioButtonEditing.TabIndex = 1;
            this.radioButtonEditing.TabStop = true;
            this.radioButtonEditing.Text = "Edit polygon";
            this.radioButtonEditing.UseVisualStyleBackColor = true;
            this.radioButtonEditing.CheckedChanged += new System.EventHandler(this.radioButtonEditing_CheckedChanged);
            // 
            // radioButtonAdding
            // 
            this.radioButtonAdding.AutoSize = true;
            this.radioButtonAdding.Checked = true;
            this.radioButtonAdding.Location = new System.Drawing.Point(6, 26);
            this.radioButtonAdding.Name = "radioButtonAdding";
            this.radioButtonAdding.Size = new System.Drawing.Size(117, 24);
            this.radioButtonAdding.TabIndex = 0;
            this.radioButtonAdding.TabStop = true;
            this.radioButtonAdding.Text = "Add polygon";
            this.radioButtonAdding.UseVisualStyleBackColor = true;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(982, 703);
            this.Controls.Add(this.tableLayout);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(1000, 750);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Polygon Editor";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Main_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.Canvas)).EndInit();
            this.tableLayout.ResumeLayout(false);
            this.rightPanel.ResumeLayout(false);
            this.algoGroupBox.ResumeLayout(false);
            this.algoGroupBox.PerformLayout();
            this.modeGroupBox.ResumeLayout(false);
            this.modeGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox Canvas;
        private System.Windows.Forms.TableLayoutPanel tableLayout;
        private System.Windows.Forms.Panel rightPanel;
        private System.Windows.Forms.GroupBox modeGroupBox;
        private System.Windows.Forms.RadioButton radioButtonAdding;
        private System.Windows.Forms.RadioButton radioButtonEditing;
        private System.Windows.Forms.GroupBox algoGroupBox;
        private System.Windows.Forms.RadioButton radioButtonBresenhamAlgo;
        private System.Windows.Forms.RadioButton radioButtonSystemAlgo;
    }
}
