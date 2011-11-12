namespace Strawberry_Patcher
{
    partial class MainFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFrm));
            this.openWowBtn = new System.Windows.Forms.Button();
            this.patchWowBtn = new System.Windows.Forms.Button();
            this.patchStatusBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // openWowBtn
            // 
            this.openWowBtn.Location = new System.Drawing.Point(0, 124);
            this.openWowBtn.Name = "openWowBtn";
            this.openWowBtn.Size = new System.Drawing.Size(75, 23);
            this.openWowBtn.TabIndex = 0;
            this.openWowBtn.Text = "Open";
            this.openWowBtn.UseVisualStyleBackColor = true;
            this.openWowBtn.Click += new System.EventHandler(this.openWowBtn_Click);
            // 
            // patchWowBtn
            // 
            this.patchWowBtn.Enabled = false;
            this.patchWowBtn.Location = new System.Drawing.Point(81, 124);
            this.patchWowBtn.Name = "patchWowBtn";
            this.patchWowBtn.Size = new System.Drawing.Size(75, 23);
            this.patchWowBtn.TabIndex = 2;
            this.patchWowBtn.Text = "Patch";
            this.patchWowBtn.UseVisualStyleBackColor = true;
            this.patchWowBtn.Click += new System.EventHandler(this.patchWowBtn_Click);
            // 
            // patchStatusBox
            // 
            this.patchStatusBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.patchStatusBox.Location = new System.Drawing.Point(0, 0);
            this.patchStatusBox.Name = "patchStatusBox";
            this.patchStatusBox.ReadOnly = true;
            this.patchStatusBox.Size = new System.Drawing.Size(347, 118);
            this.patchStatusBox.TabIndex = 3;
            this.patchStatusBox.Text = "";
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(347, 154);
            this.Controls.Add(this.patchStatusBox);
            this.Controls.Add(this.patchWowBtn);
            this.Controls.Add(this.openWowBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainFrm";
            this.Text = "Strawberry-Pr0jcts WoW Patcher";
            this.Load += new System.EventHandler(this.MainFrm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button openWowBtn;
        private System.Windows.Forms.Button patchWowBtn;
        private System.Windows.Forms.RichTextBox patchStatusBox;
    }
}

