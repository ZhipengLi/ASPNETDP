namespace ASPPatterns.Chap3.WinForm.View
{
    partial class PrdForm
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
            this.ddlCustomerType = new System.Windows.Forms.ComboBox();
            this.lblErrorMessage = new System.Windows.Forms.Label();
            this.dataGridViewProductList = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProductList)).BeginInit();
            this.SuspendLayout();
            // 
            // ddlCustomerType
            // 
            this.ddlCustomerType.FormattingEnabled = true;
            this.ddlCustomerType.Items.AddRange(new object[] {
            "Regular",
            "Trade"});
            this.ddlCustomerType.Location = new System.Drawing.Point(12, 38);
            this.ddlCustomerType.Name = "ddlCustomerType";
            this.ddlCustomerType.Size = new System.Drawing.Size(121, 28);
            this.ddlCustomerType.TabIndex = 0;
            this.ddlCustomerType.SelectedIndexChanged += new System.EventHandler(this.ddlCustomerType_SelectedIndexChanged);
            // 
            // lblErrorMessage
            // 
            this.lblErrorMessage.AutoSize = true;
            this.lblErrorMessage.Location = new System.Drawing.Point(12, 86);
            this.lblErrorMessage.Name = "lblErrorMessage";
            this.lblErrorMessage.Size = new System.Drawing.Size(0, 20);
            this.lblErrorMessage.TabIndex = 1;
            // 
            // dataGridViewProductList
            // 
            this.dataGridViewProductList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewProductList.Location = new System.Drawing.Point(16, 130);
            this.dataGridViewProductList.Name = "dataGridViewProductList";
            this.dataGridViewProductList.RowTemplate.Height = 28;
            this.dataGridViewProductList.Size = new System.Drawing.Size(1160, 236);
            this.dataGridViewProductList.TabIndex = 2;
            // 
            // PrdForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1211, 403);
            this.Controls.Add(this.dataGridViewProductList);
            this.Controls.Add(this.lblErrorMessage);
            this.Controls.Add(this.ddlCustomerType);
            this.Name = "PrdForm";
            this.Text = "ProductList";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProductList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox ddlCustomerType;
        private System.Windows.Forms.Label lblErrorMessage;
        private System.Windows.Forms.DataGridView dataGridViewProductList;
    }
}

