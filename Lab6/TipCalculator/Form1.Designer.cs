namespace TipCalculator
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
            this.textBoxEnterBill = new System.Windows.Forms.TextBox();
            this.textBox2TipsCalculated = new System.Windows.Forms.TextBox();
            this.buttonForTip = new System.Windows.Forms.Button();
            this.labelBill = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBoxEnterBill
            // 
            this.textBoxEnterBill.Location = new System.Drawing.Point(275, 66);
            this.textBoxEnterBill.Name = "textBoxEnterBill";
            this.textBoxEnterBill.Size = new System.Drawing.Size(230, 31);
            this.textBoxEnterBill.TabIndex = 0;
            this.textBoxEnterBill.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // textBox2TipsCalculated
            // 
            this.textBox2TipsCalculated.Location = new System.Drawing.Point(275, 179);
            this.textBox2TipsCalculated.Name = "textBox2TipsCalculated";
            this.textBox2TipsCalculated.Size = new System.Drawing.Size(230, 31);
            this.textBox2TipsCalculated.TabIndex = 1;
            this.textBox2TipsCalculated.TextChanged += new System.EventHandler(this.textBox2TipsCalculated_TextChanged);
            // 
            // buttonForTip
            // 
            this.buttonForTip.Location = new System.Drawing.Point(62, 179);
            this.buttonForTip.Name = "buttonForTip";
            this.buttonForTip.Size = new System.Drawing.Size(153, 42);
            this.buttonForTip.TabIndex = 2;
            this.buttonForTip.Text = "buttonTipCal";
            this.buttonForTip.UseVisualStyleBackColor = true;
            this.buttonForTip.Click += new System.EventHandler(this.buttonForTip_Click);
            // 
            // labelBill
            // 
            this.labelBill.AutoSize = true;
            this.labelBill.Location = new System.Drawing.Point(124, 66);
            this.labelBill.Name = "labelBill";
            this.labelBill.Size = new System.Drawing.Size(98, 25);
            this.labelBill.TabIndex = 3;
            this.labelBill.Text = "Enter Bill";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.labelBill);
            this.Controls.Add(this.buttonForTip);
            this.Controls.Add(this.textBox2TipsCalculated);
            this.Controls.Add(this.textBoxEnterBill);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxEnterBill;
        private System.Windows.Forms.TextBox textBox2TipsCalculated;
        private System.Windows.Forms.Button buttonForTip;
        private System.Windows.Forms.Label labelBill;
    }
}

