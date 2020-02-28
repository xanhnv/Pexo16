using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace Pexo16
{
    public partial class ViewSumInfoAndChart : Form
    {
        public ViewSumInfoAndChart()
        {
            InitializeComponent();
        }

        Graph graph = Graph.Instance;

        //Content print_a = new Content();
        PrintDialog dlgPrint = new PrintDialog();
        PrintDocument prn_Document = new PrintDocument();
        PrintPreviewDialog prn_PreviewDialog = new PrintPreviewDialog();
        Image prn_Image;
        bool f_Refresh;
        private int prn_NumOfPage;
        string FileName = "";

        public enum Content
        {
            Print_Data_Only,
            Print_Graph_Only,
            Print_Summary_Only,
            Print_General_Only,
            Print_Graph_and_Summary,
            Print_general_information,
            Print_Graph_Only1
        }

        public enum My_ImgType
        {
            _jpg,
            _wmf,
            _bmp,
            _gif,
            _png
        }

        public struct My_ImageProperties
        {
            public My_ImgType ImageType;
            public Int32 Width;
            public Int32 Height;
        };

        private void frmViewSumInfoAndChart_Load(object sender, EventArgs e)
        {
            Device dv = Device.Instance;
            List<Device> dev = new List<Device>();
            dev.Add(dv);

            Microsoft.Reporting.WinForms.ReportDataSource dataset = new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", dev);
            reportViewer1.LocalReport.DataSources.Add(dataset);
            dataset.Value = dev;
            //Print_Preview(Content.Print_Graph_and_Summary);
            //this.reportViewer1.RefreshReport();

            string imagepath = @"file:///"+"C:\\Desert.jpg";  
            //string imagepath = "Desert.jpg";
            reportViewer1.LocalReport.ReportEmbeddedResource = "Pexo16.Report2.rdlc";

            //ReportParameterCollection para = new ReportParameterCollection();

            //para.Add(new ReportParameter("AddImage", imagepath));
            //para.Values.Add(FileName + "\\~temp._t_");
            //para.Values.Add("C:\\Users\\Public\\Pictures\\Sample Pictures\\Desert.jpg");
            //para.Values.Add(imagepath);

            reportViewer1.LocalReport.EnableExternalImages = true;
            ReportParameter[] param = new ReportParameter[1];
            param[0] = new ReportParameter("AddImage", imagepath);
            reportViewer1.LocalReport.SetParameters(param[0]);
            reportViewer1.RefreshReport();
        }


        public void Print_Preview(Content content)
        {
            reportViewer1.PrinterSettings.DefaultPageSettings.Margins.Top = 20; //mil=inch/100
            reportViewer1.PrinterSettings.DefaultPageSettings.Margins.Bottom = 20; //mil=inch/100
            reportViewer1.PrinterSettings.DefaultPageSettings.Margins.Left = 20; //mil=inch/100
            reportViewer1.PrinterSettings.DefaultPageSettings.Margins.Right = 20; //mil=inch/100
            reportViewer1.PrinterSettings.DefaultPageSettings.PaperSize = dlgPrint.PrinterSettings.DefaultPageSettings.PaperSize;
            reportViewer1.PrinterSettings.DefaultPageSettings.Landscape = true;
            prn_NumOfPage = 0;
          
            if (content == Content.Print_Graph_and_Summary)
            {
                //Rectangle rr = new Rectangle(10, 10, 500, 500);
                //Graph.Chart1.SaveImage("Image.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                //Image img = Image.FromFile("Image.bmp");
                //Bitmap bb = new Bitmap(img, new Size(100, 100));
                //reportViewer1.DrawToBitmap(bb, rr);
                ////r1.DExpenseTableAdapter.Fill(this.DealerExpenseDataSet.DExpense);
                //reportViewer1.BackgroundImage = img;
                //reportViewer1.BackgroundImageLayout = ImageLayout.Center;
                //reportViewer1.RefreshReport();
                ////Show();
                //reportViewer1.Show();

                //reportViewer1. += Create_PrintPage_Graph_and_Summary;
            }

   __reload_PreviewDialog:
            //INSERT DATA
            reportViewer1.ClientSize = new Size(Screen.PrimaryScreen.Bounds.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2);
            reportViewer1.MinimumSize = reportViewer1.ClientSize;
            reportViewer1.Location = new System.Drawing.Point(29, 29);
            //reportViewer1.UseAntiAlias = true; //smooth fonts.
            //reportViewer1.LocalReport. = prn_Document;

            f_Refresh = false;

            PageSettings page = new PageSettings();

            //SHOW DIALOG
            //prn_PreviewDialog.ShowDialog(); // Preview and print
            if (f_Refresh == true)
            {
                goto __reload_PreviewDialog;
            }
        }

     

        public void Create_PrintPage_Graph_and_Summary(object sender, PrintPageEventArgs e)
        {
            Int32 Start_text_Y = 0;
            if (prn_NumOfPage == 0)
            {
                Start_text_Y = Insert_Graph(0, ref e);
                if (prn_Document.DefaultPageSettings.Landscape)
                {
                    //Insert_Footer(ref e);
                    e.HasMorePages = true;
                    prn_NumOfPage = 1;
                    return;
                }
            }
            if (Start_text_Y == 0)
            {
                Start_text_Y = prn_Document.DefaultPageSettings.Margins.Top;
            }
            Start_text_Y += 10;
            //Insert_Summary(alarm1, Start_text_Y, e);
            //Insert_Footer(e);
            if (e.HasMorePages == false)
            {
                prn_NumOfPage = 0;
            }

        }

        private int Insert_Graph(int Index, ref PrintPageEventArgs e)
        {
            Int32 tempInsert_Graph = 0;
            My_ImageProperties ExImg;
            ExImg.Width = 1200;
            ExImg.Height = 800;
            ExImg.ImageType = My_ImgType._gif;
            //xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
            
            FileName = mGlobal.app_patch(FileName);

            if (System.IO.File.Exists(FileName + "\\~temp._t_") == false)
            {
                System.IO.File.WriteAllText(FileName + "\\~temp._t_", null);
            }


            //xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx


            prn_Image = ConvertChart_to_Image(Index, FileName + "\\~temp._t_", ExImg);
            // Draw the image centered.
            Int32 hgt = 0;
            Int32 wid = 0;
            Int32 x0 = 0;
            Int32 y0 = 0;
            if (prn_Document.DefaultPageSettings.Landscape)
            {
                wid = prn_Document.DefaultPageSettings.PaperSize.Height - prn_Document.DefaultPageSettings.Margins.Top - prn_Document.DefaultPageSettings.Margins.Bottom;
                hgt = prn_Document.DefaultPageSettings.PaperSize.Width - prn_Document.DefaultPageSettings.Margins.Left - prn_Document.DefaultPageSettings.Margins.Right;
                y0 = prn_Document.DefaultPageSettings.Margins.Left;
                x0 = prn_Document.DefaultPageSettings.Margins.Top;
            }
            else
            {
                hgt = prn_Document.DefaultPageSettings.PaperSize.Height - prn_Document.DefaultPageSettings.Margins.Top - prn_Document.DefaultPageSettings.Margins.Bottom;
                wid = prn_Document.DefaultPageSettings.PaperSize.Width - prn_Document.DefaultPageSettings.Margins.Left - prn_Document.DefaultPageSettings.Margins.Right;
                x0 = prn_Document.DefaultPageSettings.Margins.Left;
                y0 = prn_Document.DefaultPageSettings.Margins.Top;
            }
            float scale_factor = wid / (float)prn_Image.Width;
            e.Graphics.DrawImage(prn_Image, x0, y0, wid, Convert.ToInt32(prn_Image.Height * scale_factor));
            e.HasMorePages = false;
            tempInsert_Graph = y0 + Convert.ToInt32(prn_Image.Height * scale_factor);
            prn_Image.Dispose();
            //System.IO.FileInfo fi = new System.IO.FileInfo(FileName + "\\~temp._t_");
            //fi.Delete();

            return tempInsert_Graph;
        }

        public Image ConvertChart_to_Image(int inDex, string FilePath, My_ImageProperties _img)
        {
            if (_img.ImageType == My_ImgType._wmf)
            {
                graph.chart1.SaveImage(FilePath, System.Drawing.Imaging.ImageFormat.Wmf);
            }

            else if (_img.ImageType == My_ImgType._jpg)
            {
                graph.chart1.SaveImage(FilePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            //ORIGINAL LINE: Case My_ImgType._bmp
            else if (_img.ImageType == My_ImgType._bmp)
            {
                graph.chart1.SaveImage(FilePath, System.Drawing.Imaging.ImageFormat.Bmp);
            }
            //ORIGINAL LINE: Case My_ImgType._gif
            else if (_img.ImageType == My_ImgType._gif)
            {
                graph.chart1.SaveImage(FilePath, System.Drawing.Imaging.ImageFormat.Gif);
            }
            //ORIGINAL LINE: Case My_ImgType._png
            else if (_img.ImageType == My_ImgType._png)
            {
                graph.chart1.SaveImage(FilePath, System.Drawing.Imaging.ImageFormat.Png);
            }
            //ORIGINAL LINE: Case Else
            else
            {
                graph.chart1.SaveImage(FilePath, System.Drawing.Imaging.ImageFormat.Png);
            }
            //reload
            if (FilePath != "")
            {
                return Image.FromFile(FilePath, true);
            }
            else
            {
                MessageBox.Show("Graph has been copied to clipboard.", "Capture", MessageBoxButtons.OK);
                return null;
            }
        }
    }
}
