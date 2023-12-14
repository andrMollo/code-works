namespace CodeWorksUI
{
    partial class ExportAssemblyForm
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
            this.titleLabel = new System.Windows.Forms.Label();
            this.exportCheckBox = new System.Windows.Forms.CheckBox();
            this.printCheckBox = new System.Windows.Forms.CheckBox();
            this.quantityGroupBox = new System.Windows.Forms.GroupBox();
            this.assemblyQtyLabel = new System.Windows.Forms.Label();
            this.assemblyQtyTextBox = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.quantityGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Segoe UI", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(158)))), ((int)(((byte)(223)))));
            this.titleLabel.Location = new System.Drawing.Point(12, 9);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(866, 149);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "Export assembly";
            // 
            // exportCheckBox
            // 
            this.exportCheckBox.AutoSize = true;
            this.exportCheckBox.Font = new System.Drawing.Font("Segoe UI", 15.85714F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exportCheckBox.Location = new System.Drawing.Point(37, 215);
            this.exportCheckBox.Name = "exportCheckBox";
            this.exportCheckBox.Size = new System.Drawing.Size(416, 54);
            this.exportCheckBox.TabIndex = 1;
            this.exportCheckBox.Text = "Export all components";
            this.exportCheckBox.UseVisualStyleBackColor = true;
            // 
            // printCheckBox
            // 
            this.printCheckBox.AutoSize = true;
            this.printCheckBox.Font = new System.Drawing.Font("Segoe UI", 15.85714F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.printCheckBox.Location = new System.Drawing.Point(491, 215);
            this.printCheckBox.Name = "printCheckBox";
            this.printCheckBox.Size = new System.Drawing.Size(387, 54);
            this.printCheckBox.TabIndex = 2;
            this.printCheckBox.Text = "Print all components";
            this.printCheckBox.UseVisualStyleBackColor = true;
            // 
            // quantityGroupBox
            // 
            this.quantityGroupBox.Controls.Add(this.assemblyQtyTextBox);
            this.quantityGroupBox.Controls.Add(this.assemblyQtyLabel);
            this.quantityGroupBox.Font = new System.Drawing.Font("Segoe UI", 15.85714F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.quantityGroupBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(158)))), ((int)(((byte)(223)))));
            this.quantityGroupBox.Location = new System.Drawing.Point(37, 309);
            this.quantityGroupBox.Name = "quantityGroupBox";
            this.quantityGroupBox.Size = new System.Drawing.Size(841, 306);
            this.quantityGroupBox.TabIndex = 3;
            this.quantityGroupBox.TabStop = false;
            this.quantityGroupBox.Text = "Component quantity";
            // 
            // assemblyQtyLabel
            // 
            this.assemblyQtyLabel.AutoSize = true;
            this.assemblyQtyLabel.ForeColor = System.Drawing.Color.Black;
            this.assemblyQtyLabel.Location = new System.Drawing.Point(40, 84);
            this.assemblyQtyLabel.Name = "assemblyQtyLabel";
            this.assemblyQtyLabel.Size = new System.Drawing.Size(332, 50);
            this.assemblyQtyLabel.TabIndex = 0;
            this.assemblyQtyLabel.Text = "Assembly quantity:";
            // 
            // assemblyQtyTextBox
            // 
            this.assemblyQtyTextBox.Location = new System.Drawing.Point(400, 81);
            this.assemblyQtyTextBox.Name = "assemblyQtyTextBox";
            this.assemblyQtyTextBox.Size = new System.Drawing.Size(242, 57);
            this.assemblyQtyTextBox.TabIndex = 1;
            // 
            // okButton
            // 
            this.okButton.BackColor = System.Drawing.Color.White;
            this.okButton.Font = new System.Drawing.Font("Segoe UI Semibold", 20.14286F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.okButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(158)))), ((int)(((byte)(223)))));
            this.okButton.Location = new System.Drawing.Point(700, 898);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(178, 94);
            this.okButton.TabIndex = 4;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = false;
            // 
            // cancelButton
            // 
            this.cancelButton.BackColor = System.Drawing.Color.White;
            this.cancelButton.Font = new System.Drawing.Font("Segoe UI Semibold", 20.14286F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelButton.Location = new System.Drawing.Point(12, 898);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(188, 94);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = false;
            // 
            // ExportAssemblyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 38F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(896, 1004);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.quantityGroupBox);
            this.Controls.Add(this.printCheckBox);
            this.Controls.Add(this.exportCheckBox);
            this.Controls.Add(this.titleLabel);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ExportAssemblyForm";
            this.Text = "Export assembly";
            this.quantityGroupBox.ResumeLayout(false);
            this.quantityGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.CheckBox exportCheckBox;
        private System.Windows.Forms.CheckBox printCheckBox;
        private System.Windows.Forms.GroupBox quantityGroupBox;
        private System.Windows.Forms.TextBox assemblyQtyTextBox;
        private System.Windows.Forms.Label assemblyQtyLabel;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
    }
}

