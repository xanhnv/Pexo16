namespace Pexo16
{
    partial class ViewInfo
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
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.clsTablesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.Device35BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.ChannelBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.DeviceBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.GraphBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.clsTablesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Device35BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChannelBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DeviceBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GraphBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // reportViewer1
            // 
            this.reportViewer1.AutoSize = true;
            this.reportViewer1.DocumentMapWidth = 150;
            reportDataSource1.Name = "DataSet1";
            reportDataSource1.Value = this.clsTablesBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Pexo16.reportSumInfo35.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(3, 12);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(1080, 600);
            this.reportViewer1.TabIndex = 0;
            this.reportViewer1.ZoomMode = Microsoft.Reporting.WinForms.ZoomMode.FullPage;
            this.reportViewer1.ZoomPercent = 150;
            // 
            // clsTablesBindingSource
            // 
            this.clsTablesBindingSource.DataSource = typeof(Pexo16.clsTables);
            // 
            // Device35BindingSource
            // 
            this.Device35BindingSource.DataSource = typeof(Pexo16.Device35);
            // 
            // bindingSource1
            // 
            this.bindingSource1.AllowNew = true;
            this.bindingSource1.DataSource = typeof(Pexo16.Channel);
            // 
            // ChannelBindingSource
            // 
            this.ChannelBindingSource.DataSource = typeof(Pexo16.Channel);
            // 
            // DeviceBindingSource
            // 
            this.DeviceBindingSource.DataSource = typeof(Pexo16.Device);
            // 
            // GraphBindingSource
            // 
            this.GraphBindingSource.DataSource = typeof(Pexo16.Graph);
            // 
            // ViewInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1087, 616);
            this.Controls.Add(this.reportViewer1);
            this.Name = "ViewInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "View Data";
            this.Load += new System.EventHandler(this.frmViewData_Load);
            this.Resize += new System.EventHandler(this.ViewInfo_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.clsTablesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Device35BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChannelBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DeviceBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GraphBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.BindingSource DeviceBindingSource;
        public System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.BindingSource ChannelBindingSource;
        private System.Windows.Forms.BindingSource Device35BindingSource;
        private System.Windows.Forms.BindingSource clsTablesBindingSource;
        private System.Windows.Forms.BindingSource GraphBindingSource;



    }
}