namespace SynapticsToTouch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.calibrateButton = new System.Windows.Forms.Button();
            this.calibrateLabel = new System.Windows.Forms.Label();
            this.touchLabel = new System.Windows.Forms.Label();
            this.calibrationStatusLabel = new System.Windows.Forms.Label();
            this.adminButton = new System.Windows.Forms.Button();
            this.hideBox = new System.Windows.Forms.CheckBox();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.touchpadView1 = new SynapticsToTouch.TouchpadView();
            this.SuspendLayout();
            // 
            // calibrateButton
            // 
            this.calibrateButton.Location = new System.Drawing.Point(12, 12);
            this.calibrateButton.Name = "calibrateButton";
            this.calibrateButton.Size = new System.Drawing.Size(75, 32);
            this.calibrateButton.TabIndex = 0;
            this.calibrateButton.Text = "Calibrate";
            this.calibrateButton.UseVisualStyleBackColor = true;
            this.calibrateButton.Click += new System.EventHandler(this.calibrateButton_Click);
            // 
            // calibrateLabel
            // 
            this.calibrateLabel.AutoSize = true;
            this.calibrateLabel.Location = new System.Drawing.Point(12, 59);
            this.calibrateLabel.Name = "calibrateLabel";
            this.calibrateLabel.Size = new System.Drawing.Size(77, 13);
            this.calibrateLabel.TabIndex = 1;
            this.calibrateLabel.Text = "Not Calibrated.";
            // 
            // touchLabel
            // 
            this.touchLabel.AutoSize = true;
            this.touchLabel.Location = new System.Drawing.Point(9, 239);
            this.touchLabel.Name = "touchLabel";
            this.touchLabel.Size = new System.Drawing.Size(61, 13);
            this.touchLabel.TabIndex = 2;
            this.touchLabel.Text = "Touch X, Y";
            // 
            // calibrationStatusLabel
            // 
            this.calibrationStatusLabel.AutoSize = true;
            this.calibrationStatusLabel.Location = new System.Drawing.Point(12, 76);
            this.calibrationStatusLabel.Name = "calibrationStatusLabel";
            this.calibrationStatusLabel.Size = new System.Drawing.Size(87, 13);
            this.calibrationStatusLabel.TabIndex = 3;
            this.calibrationStatusLabel.Text = "Calibration status";
            // 
            // adminButton
            // 
            this.adminButton.Image = ((System.Drawing.Image)(resources.GetObject("adminButton.Image")));
            this.adminButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.adminButton.Location = new System.Drawing.Point(93, 12);
            this.adminButton.Name = "adminButton";
            this.adminButton.Size = new System.Drawing.Size(179, 32);
            this.adminButton.TabIndex = 4;
            this.adminButton.Text = "Run as Administrator";
            this.adminButton.UseVisualStyleBackColor = true;
            this.adminButton.Click += new System.EventHandler(this.adminButton_Click);
            // 
            // hideBox
            // 
            this.hideBox.AutoSize = true;
            this.hideBox.Location = new System.Drawing.Point(12, 219);
            this.hideBox.Name = "hideBox";
            this.hideBox.Size = new System.Drawing.Size(215, 17);
            this.hideBox.TabIndex = 5;
            this.hideBox.Text = "Hide to notification area when minimized";
            this.hideBox.UseVisualStyleBackColor = true;
            this.hideBox.CheckedChanged += new System.EventHandler(this.hideBox_CheckedChanged);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Synaptics To Touch";
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // elementHost1
            // 
            this.elementHost1.Location = new System.Drawing.Point(15, 92);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(257, 104);
            this.elementHost1.TabIndex = 6;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = this.touchpadView1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.elementHost1);
            this.Controls.Add(this.hideBox);
            this.Controls.Add(this.adminButton);
            this.Controls.Add(this.calibrationStatusLabel);
            this.Controls.Add(this.touchLabel);
            this.Controls.Add(this.calibrateLabel);
            this.Controls.Add(this.calibrateButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Synaptics To Touch";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button calibrateButton;
        private System.Windows.Forms.Label calibrateLabel;
        private System.Windows.Forms.Label touchLabel;
        private System.Windows.Forms.Label calibrationStatusLabel;
        private System.Windows.Forms.Button adminButton;
        private System.Windows.Forms.CheckBox hideBox;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Integration.ElementHost elementHost1;
        private TouchpadView touchpadView1;
    }
}

