namespace Pexo16
{
    partial class ViewSumInfoAndChart
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param Name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            this.DeviceBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            ((System.ComponentModel.ISupportInitialize)(this.DeviceBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // DeviceBindingSource
            // 
            this.DeviceBindingSource.DataSource = typeof(Pexo16.Device);
            // 
            // reportViewer1
            // 
            this.reportViewer1.LocalReport.EnableExternalImages = true;
            this.reportViewer1.Location = new System.Drawing.Point(12, 12);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(843, 397);
            this.reportViewer1.TabIndex = 0;
            // 
            // ViewSumInfoAndChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(867, 421);
            this.Controls.Add(this.reportViewer1);
            this.Name = "ViewSumInfoAndChart";
            this.Text = "View Sumary Infomation And Chart";
            this.Load += new System.EventHandler(this.frmViewSumInfoAndChart_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DeviceBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource DeviceBindingSource;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
    }
}