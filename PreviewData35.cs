using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;

namespace Pexo16
{
    class PreviewData35
    {
        Graph graph = Graph.Instance;
        Device35 deviceFromFile = Device35.Instance;
        //Device35 dv35 = Device35.Instance;

        Print_content print_a = new Print_content();
        PrintDialog dlgPrint = new PrintDialog();
        PrintDocument prn_Document = new PrintDocument();
        PrintPreviewDialog prn_PreviewDialog = new PrintPreviewDialog();
        Image prn_Image;
        bool f_Refresh;
        private int prn_NumOfPage;

        private Font printFont;
        private StreamReader streamToPrint;

        internal enum Print_content
        {
            Print_Data_Only,
            Print_Graph_Only,
            Print_Summary_Only,
            Print_General_Only,
            Print_Graph_and_Summary,
            Print_general_information,
            Print_Graph_Only1
        }

        internal enum My_ImgType
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
        }

        public void Print_Preview(Print_content content)
        {

            PrintPreviewDialog dlgPrintPreview = new PrintPreviewDialog();

            //dlgPrintPreview.Width = 3000;
            //dlgPrintPreview.Size = new System.Drawing.Size(2000, 3000);

            ToolStrip ts = (ToolStrip)dlgPrintPreview.Controls[1];
            ToolStripItem printItem = ts.Items["printToolStripButton"];

            ToolStripItem myPrintItem = null;
            myPrintItem = ts.Items.Add("Print", printItem.Image, new EventHandler(MyPrintItemClicked));
            print_a = content;
            myPrintItem.DisplayStyle = ToolStripItemDisplayStyle.Text;
            //Relocate the item to the beginning of the toolstrip
            ts.Items.Insert(0, myPrintItem);

            ToolStripItem mySpaceItem = null;
            mySpaceItem = ts.Items.Add("-");
            mySpaceItem.DisplayStyle = ToolStripItemDisplayStyle.Text;
            ts.Items.Insert(1, mySpaceItem);

            //print pdf (has not Print Setup menu)
            if (mGlobal.PrintPDF == true)
            {
                goto setup;
            }

            ToolStripItem myPageSetupItem = null;
            myPageSetupItem = ts.Items.Add("Print Setup...", printItem.Image, new EventHandler(MyPageSetupItemClicked));
            myPageSetupItem.DisplayStyle = ToolStripItemDisplayStyle.Text;
            //Relocate the item to the beginning of the toolstrip
            ts.Items.Insert(2, myPageSetupItem);

        setup:
            ts.Items.Insert(3, mySpaceItem);


            PreparePrintDocument(content);

        __reload_PreviewDialog:
            //INSERT DATA
            dlgPrintPreview.ClientSize = new Size(Screen.PrimaryScreen.Bounds.Width , Screen.PrimaryScreen.Bounds.Height);
            dlgPrintPreview.MinimumSize = dlgPrintPreview.ClientSize;
            dlgPrintPreview.Location = new System.Drawing.Point(10, 10);
            dlgPrintPreview.UseAntiAlias = true; //smooth fonts.
            dlgPrintPreview.Document = prn_Document;
            prn_PreviewDialog = dlgPrintPreview;
            f_Refresh = false;

            //SHOW DIALOG
            prn_PreviewDialog.ShowDialog(); // Preview and print
            //if (f_Refresh == true)
            //{
            //    goto __reload_PreviewDialog;
            //}
        }

        private void MyPageSetupItemClicked(object sender, EventArgs e)
        {
            try
            {
                dlgPrint.AllowSelection = true;
                dlgPrint.ShowNetwork = true;
                dlgPrint.Document = prn_Document;
                if (dlgPrint.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }
                prn_Document.DefaultPageSettings.PaperSize = dlgPrint.Document.DefaultPageSettings.PaperSize;
                f_Refresh = true;
                prn_PreviewDialog.Close();
            }
            catch (Exception)
            {
            }
        }

        private void MyPrintItemClicked(object sender, EventArgs e)
        {
            try
            {
                PreparePrintDocument(print_a);
                prn_Document.Print();
                prn_PreviewDialog.Close();
            }
            catch (Exception)
            {
                prn_PreviewDialog.Close();
            }
        }

        private void PreparePrintDocument(Print_content content)
        {
            dlgPrint = new PrintDialog();
            PrintDocument print_document = new PrintDocument();


            print_document.DefaultPageSettings.Margins.Top = 20; //mil=inch/100
            print_document.DefaultPageSettings.Margins.Bottom = 20; //mil=inch/100
            print_document.DefaultPageSettings.Margins.Left = 20; //mil=inch/100
            print_document.DefaultPageSettings.Margins.Right = 20; //mil=inch/100
            //print_document.DefaultPageSettings.PaperSize = print_document.PrinterSettings.PaperSizes[14];
            PaperKind a = print_document.DefaultPageSettings.PaperSize.Kind;
            //print_document.DefaultPageSettings.PaperSize.PaperName = "Custom";

            //print_document.DefaultPageSettings.PaperSize.Width = 1000;


            print_document.DefaultPageSettings.Landscape = true;
            prn_NumOfPage = 0;
            ////		Select Case Content

            ////ORIGINAL LINE: Case Print_content.Print_Graph_and_Summary
            if (content == Print_content.Print_Graph_and_Summary)
            {
                print_document.PrintPage += Create_PrintPage_Graph_and_Summary;
                //Case Print_content.Print_general_information
                //    AddHandler print_document.PrintPage, AddressOf Create_PrintPage_information
            }
            prn_Document = print_document;
        }

        public void Create_PrintPage_Graph_and_Summary(object sender, PrintPageEventArgs e)
        {
            Int32 Start_text_Y = 0;
            if (prn_NumOfPage == 0)
            {
                Start_text_Y = Insert_Graph(0, ref e);
                //****** qua trang moi ******
                //if (prn_Document.DefaultPageSettings.Landscape)
                //{
                //    Insert_Footer(ref e);
                //    e.HasMorePages = true;
                //    prn_NumOfPage = 1;
                //    return;
                //}
            }
            if (Start_text_Y == 0)
            {
                Start_text_Y = prn_Document.DefaultPageSettings.Margins.Top;
            }
            Start_text_Y += 10;

            ArrayList data = new ArrayList();
            for (int i = 0; i < deviceFromFile.Channels[0].Data.Length; i++)
            {
                data.Add(deviceFromFile.Channels[0].Data[i]);
            }
            Insert_Summary(ref data, Start_text_Y, ref e);
            Insert_Footer(ref e);
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
            ExImg.ImageType = My_ImgType._png;

            string FileName = "";
            FileName = mGlobal.app_patch(FileName);

            if (System.IO.File.Exists(FileName + "\\~temp._t_") == false)
            {
                System.IO.File.WriteAllText(FileName + "\\~temp._t_", null);
            }

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
            System.IO.FileInfo fi = new System.IO.FileInfo(FileName + "\\~temp._t_");
            fi.Delete();

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

            else if (_img.ImageType == My_ImgType._bmp)
            {
                graph.chart1.SaveImage(FilePath, System.Drawing.Imaging.ImageFormat.Bmp);
            }

            else if (_img.ImageType == My_ImgType._gif)
            {
                graph.chart1.SaveImage(FilePath, System.Drawing.Imaging.ImageFormat.Gif);
            }

            else if (_img.ImageType == My_ImgType._png)
            {
                graph.chart1.SaveImage(FilePath, System.Drawing.Imaging.ImageFormat.Png);
            }

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

        private void Insert_Footer(ref PrintPageEventArgs e)
        {
            Font font_B = new Font("Tahoma", 8F, FontStyle.Bold); //bold
            string str = null;
            //get margin
            Int32 PrintAreaHeight = 0;
            Int32 PrintAreaWidth = 0;
            Int32 marginLeft = 0;
            Int32 marginTop = 0;
            PrintAreaHeight = prn_Document.DefaultPageSettings.PaperSize.Height - prn_Document.DefaultPageSettings.Margins.Top - prn_Document.DefaultPageSettings.Margins.Bottom;
            PrintAreaWidth = prn_Document.DefaultPageSettings.PaperSize.Width - prn_Document.DefaultPageSettings.Margins.Left - prn_Document.DefaultPageSettings.Margins.Right;
            marginLeft = prn_Document.DefaultPageSettings.Margins.Left;
            marginTop = prn_Document.DefaultPageSettings.Margins.Top;
            if (prn_Document.DefaultPageSettings.Landscape)
            {
                Int32 intTemp = 0;
                intTemp = PrintAreaHeight;
                PrintAreaHeight = PrintAreaWidth;
                PrintAreaWidth = intTemp;
                //if the user selects landscape mode, swap the printing area height and width
            }

            //###############
            //FOOTER
            str = "Marathon Products, Inc.";
            e.Graphics.DrawString(str, font_B, Brushes.Black, marginLeft + (PrintAreaWidth - e.Graphics.MeasureString(str, font_B).Width) / 2, marginTop + PrintAreaHeight - font_B.Height * 2);
            str = "www.marathonproducts.com / info@marathonproducts.com";
            e.Graphics.DrawString(str, font_B, Brushes.Black, marginLeft + (PrintAreaWidth - e.Graphics.MeasureString(str, font_B).Width) / 2, marginTop + PrintAreaHeight - font_B.Height);
        }

        public void Insert_Summary(ref ArrayList Data, Int32 StartOfText_Y, ref PrintPageEventArgs e)
        {
            int LineNo = 0;
            Font font = new Font("Tahoma", 8);
            Font font_BU = new Font("Tahoma", 8F, FontStyle.Bold | FontStyle.Underline); //bold and underline
            Font font_B = new Font("Tahoma", 8F, FontStyle.Bold);
            float space = 0F;
            //get margin
            int PrintAreaHeight = 0;
            int PrintAreaWidth = 0;
            int marginLeft = 0;
            int marginTop = 0;
            PrintAreaHeight = prn_Document.DefaultPageSettings.PaperSize.Height - prn_Document.DefaultPageSettings.Margins.Top - prn_Document.DefaultPageSettings.Margins.Bottom;
            PrintAreaWidth = prn_Document.DefaultPageSettings.PaperSize.Width - prn_Document.DefaultPageSettings.Margins.Left - prn_Document.DefaultPageSettings.Margins.Right;
            marginLeft = prn_Document.DefaultPageSettings.Margins.Left;
            marginTop = prn_Document.DefaultPageSettings.Margins.Top;
            if (prn_NumOfPage == 0)
            {
                LineNo = 0;
            }
            if (prn_Document.DefaultPageSettings.Landscape)
            {
                int intTemp = 0;
                intTemp = PrintAreaHeight;
                PrintAreaHeight = PrintAreaWidth;
                PrintAreaWidth = intTemp;
                //if the user selects landscape mode, swap the printing area height and width
                if (prn_NumOfPage == 1)
                {
                    LineNo = 0;
                }
            }
            space = PrintAreaWidth / (deviceFromFile.numOfChannel + 1);

            if(LineNo == deviceFromFile.numOfChannel)
            {
                return;
            }

            switch (LineNo)
            {
                case 1:
                    goto Line_1;
                //case 2:
                //    goto Line_2;
                //case 3:
                //    goto Line_3;
                //case 4:
                //    goto Line_4;
                //case 5:
                //    goto Line_5;
                //case 6:
                //    goto Line_6;
                //case 7:
                //    goto Line_7;
                //case 8:
                //    goto Line_8;
                //case 9:
                //    goto Line_9;
                //case 10:
                //    goto Line_10;
                //case 11:
                //    goto Line_11;
                //case 12:
                //    goto Line_12;
                //case 13:
                //    goto Line_13;
                //case 14:
                //    goto Line_14;
                //case 15:
                //    goto Line_15;
                //case 16:
                //    goto Line_16;
                //case 17:
                //    goto Line_17;
                //case 18:
                //    goto Line_18;
                //case 19:
                //    goto Line_19;
                //case 20:
                //    goto Line_20;
                //case 21:
                //    goto Line_21;
                //case 22:
                //    goto Line_22;
                //case 23:
                //    goto Line_23;
                //case 24:
                //    goto Line_24;
                //case 25:
                //    goto Line_25;
                //case 26:
                //    goto Line_26;
                //case 27:
                //    goto Line_27;
                //case 28:
                //    goto Line_28;
                //case 29:
                //    goto Line_29;
                //case 30:
                //    goto Line_30;
                //case 31:
                //    goto Line_31;
                //case 32:
                //    goto Line_32;
                //case 33:
                //    goto Line_33;
                //case 34:
                //    goto Line_34;
                //case 35:
                //    goto Line_35;
                //case 36:
                //    goto Line_36;
                //case 37:
                //    goto Line_37;
                //case 38:
                //    goto Line_38;
                //case 39:
                //    goto Line_39;
                //case 40:
                //    goto Line_40;
                //case 41:
                //    goto Line_41;
                //case 42:
                //    goto Line_42;
                //case 43:
                //    goto Line_43;
                //case 44:
                //    goto Line_44;
                //case 45:
                //    goto Line_45;
                //case 46:
                //    goto Line_46;
                //case 47:
                //    goto Line_47;
                //case 48:
                //    goto Line_48;
                //case 49:
                //    goto Line_49;
                //case 50:
                //    goto Line_50;
                //case 51:
                //    goto Line_51;
                //case 52:
                //    goto Line_52;
                //case 53:
                //    goto Line_53;
                //case 54:
                //    goto Line_54;
                //case 55:
                //    goto Line_55;
                //case 56:
                //    goto Line_56;
                //case 57:
                //    goto Line_57;
                //case 58:
                //    goto Line_58;
                //case 59:
                //    goto Line_59;
                //case 60:
                //    goto Line_60;
                //case 61:
                //    goto Line_61;
                //case 62:
                //    goto Line_62;
                //case 63:
                    return;
            }

        Line_1:
            e.Graphics.DrawString("Channel:", font_B, Brushes.Black, marginLeft, StartOfText_Y);
            int channel = 1;
            int dem = 0;
            for (int i = 0; i < deviceFromFile.numOfChannel; i++)
            {
                if(deviceFromFile.Channels[i].Unit == 3)
                {
                        switch (dem)
                        {
                            case 0:
                                e.Graphics.DrawString("Channel " + (channel), font_B, Brushes.Black, space * (i + 1), StartOfText_Y);
                                break;

                            case 1:
                                e.Graphics.DrawString("Channel " + (channel), font_B, Brushes.Black, space * (i + 1), StartOfText_Y);
                                break;

                            case 2:
                                e.Graphics.DrawString("Channel " + (channel), font_B, Brushes.Black, space * (i + 1), StartOfText_Y);
                                channel += 1;
                                dem = -1;
                                break;
                        }
                        dem += 1;
                }
                else
                {
                    e.Graphics.DrawString("Channel " + (channel), font_B, Brushes.Black, space * (i + 1), StartOfText_Y);  
                    channel += 1;
                }
            }

            StartOfText_Y += font_B.Height;
            LineNo = 1;
            if (StartOfText_Y > (marginTop + PrintAreaHeight - font_B.Height * 4))
            {
                e.HasMorePages = true;
                prn_NumOfPage += 1;
                return;
            }

        Line_2:
            e.Graphics.DrawString("Unit:", font_B, Brushes.Black, marginLeft, StartOfText_Y);

            int channelu = 1;
            int demu = 0;

            for (int i = 0; i < deviceFromFile.numOfChannel; i++)
            {
                if (deviceFromFile.Channels[i].Unit == 3)
                {
                    switch (demu)
                    {
                        case 0:
                            e.Graphics.DrawString("X", font, Brushes.Black, space * (i + 1), StartOfText_Y);
                            break;

                        case 1:
                            e.Graphics.DrawString("Y", font, Brushes.Black, space * (i + 1), StartOfText_Y);
                            break;

                        case 2:
                            e.Graphics.DrawString("Z", font, Brushes.Black, space * (i + 1), StartOfText_Y);
                            channelu += 1;
                            demu = -1;
                            break;
                    }
                    demu += 1;
                }
                else
                {
                    e.Graphics.DrawString(mGlobal.IntToUnit35(byte.Parse(mGlobal.unitTemp[i].ToString())), font, Brushes.Black, space * (i + 1), StartOfText_Y);
                    channelu += 1;
                }
            }
            //for (int i = 0; i < deviceFromFile.numOfChannel; i++)
            //{
            //    e.Graphics.DrawString(mGlobal.IntToUnit35(byte.Parse(deviceFromFile.Channels[i].Unit.ToString())), font, Brushes.Black, space * (i + 1), StartOfText_Y);
            //}
            StartOfText_Y += font.Height;
            LineNo = 2;
            if (StartOfText_Y > (marginTop + PrintAreaHeight - font.Height * 4))
            {
                //e.HasMorePages = true;
                //prn_NumOfPage += 1;
                return;
            }

        Line_3:
            e.Graphics.DrawString("Highest:", font_B, Brushes.Black, marginLeft, StartOfText_Y);
            for (int i = 0; i < deviceFromFile.numOfChannel; i++)
            {
                if (deviceFromFile.Channels[i].Unit != 0) // Not Use: Unit = 0
                {
                    if (deviceFromFile.Channels[i].Unit == 3)
                    {
                        e.Graphics.DrawString(deviceFromFile.Channels[i].high_suminfo.ToString("0.000"), font, Brushes.Black, space * (i + 1), StartOfText_Y);
                    }
                    else
                    {
                        //e.Graphics.DrawString(graph.high_suminfo[i].ToString("00.0"), font, Brushes.Black, space * (i + 1), StartOfText_Y);
                        e.Graphics.DrawString(deviceFromFile.Channels[i].high_suminfo.ToString("00.0"), font, Brushes.Black, space * (i + 1), StartOfText_Y);
                    }
                }
                else
                {
                    e.Graphics.DrawString("---", font, Brushes.Black, space * (i + 1), StartOfText_Y);
                }

            }
            StartOfText_Y += font.Height;
            LineNo = 3;
            if (StartOfText_Y > (marginTop + PrintAreaHeight - font.Height * 4))
            {
                //e.HasMorePages = true;
                //prn_NumOfPage += 1;
                return;
            }

        Line_4:
            e.Graphics.DrawString("Lowest:", font_B, Brushes.Black, marginLeft, StartOfText_Y);
            for (int i = 0; i < deviceFromFile.numOfChannel; i++)
            {
                if (deviceFromFile.Channels[i].Unit != 0) // Not Use: Unit = 0
                {
                    if (deviceFromFile.Channels[i].Unit == 3)
                    {
                        //e.Graphics.DrawString(graph.low_suminfo[i].ToString("00.0"), font, Brushes.Black, space * (i + 1), StartOfText_Y);
                        e.Graphics.DrawString(deviceFromFile.Channels[i].low_suminfo.ToString("0.000"), font, Brushes.Black, space * (i + 1), StartOfText_Y);
                    }
                    else
                    {
                        e.Graphics.DrawString(deviceFromFile.Channels[i].low_suminfo.ToString("00.0"), font, Brushes.Black, space * (i + 1), StartOfText_Y);
                    }
                }
                else
                {
                    e.Graphics.DrawString("---", font, Brushes.Black, space * (i + 1), StartOfText_Y);
                }
            }
            StartOfText_Y += font.Height;
            LineNo = 4;
            if (StartOfText_Y > (marginTop + PrintAreaHeight - font.Height * 4))
            {
                //e.HasMorePages = true;
                //prn_NumOfPage += 1;
                return;
            }

        Line_5:
            e.Graphics.DrawString("Average:", font_B, Brushes.Black, marginLeft, StartOfText_Y);
            for (int i = 0; i < deviceFromFile.numOfChannel; i++)
            {
                if (deviceFromFile.Channels[i].Unit != 0) // Not Use: Unit = 0
                {
                    if (deviceFromFile.Channels[i].Unit == 3)
                    {
                        e.Graphics.DrawString(deviceFromFile.Channels[i].ave_frm_suminfo.ToString("0.000"), font, Brushes.Black, space * (i + 1), StartOfText_Y);
                    }
                    else
                    {
                        //e.Graphics.DrawString(graph.ave_frm_suminfo[i].ToString("00.0"), font, Brushes.Black, space * (i + 1), StartOfText_Y);
                        e.Graphics.DrawString(deviceFromFile.Channels[i].ave_frm_suminfo.ToString("00.0"), font, Brushes.Black, space * (i + 1), StartOfText_Y);
                    }
                }
                else
                {
                    e.Graphics.DrawString("---", font, Brushes.Black, space * (i + 1), StartOfText_Y);
                }
            }
            StartOfText_Y += font.Height;
            LineNo = 5;
            if (StartOfText_Y > (marginTop + PrintAreaHeight - font.Height * 4))
            {
                //e.HasMorePages = true;
                //prn_NumOfPage += 1;
                return;
            }

        Line_6:
            e.Graphics.DrawString("Max:", font_B, Brushes.Black, marginLeft, StartOfText_Y);
            for (int i = 0; i < deviceFromFile.numOfChannel; i++)
            {
                if (deviceFromFile.Channels[i].Unit != 0) // Not Use: Unit = 0
                {
                    if (deviceFromFile.Channels[i].NoAlarm == true)
                    {
                        e.Graphics.DrawString("No Alarm", font, Brushes.Black, space * (i + 1), StartOfText_Y);
                    }
                    else
                    {
                        decimal tamp = deviceFromFile.Channels[i].AlarmMax / 10;
                        e.Graphics.DrawString(Math.Round((Convert.ToSingle(deviceFromFile.Channels[i].AlarmMax) / 10)).ToString("00.0"), font, Brushes.Black, space * (i + 1), StartOfText_Y);
                    }
                }
                else
                {
                    e.Graphics.DrawString("---", font, Brushes.Black, space * (i + 1), StartOfText_Y);
                }
            }
            StartOfText_Y += font.Height;
            LineNo = 6;
            if (StartOfText_Y > (marginTop + PrintAreaHeight - font.Height * 4))
            {
                //e.HasMorePages = true;
                //prn_NumOfPage += 1;
                return;
            }

        Line_7:
            e.Graphics.DrawString("Min:", font_B, Brushes.Black, marginLeft, StartOfText_Y);
            for (int i = 0; i < deviceFromFile.numOfChannel; i++)
            {
                if (deviceFromFile.Channels[i].Unit != 0) // Not Use: Unit = 0
                {
                    if (deviceFromFile.Channels[i].NoAlarm == true)
                    {
                        e.Graphics.DrawString("No Alarm", font, Brushes.Black, space * (i + 1), StartOfText_Y);
                    }
                    else
                    {
                        decimal tamp = deviceFromFile.Channels[i].AlarmMin / 10;
                        e.Graphics.DrawString(Math.Round((Convert.ToSingle(deviceFromFile.Channels[i].AlarmMin) / 10)).ToString("00.0"), font, Brushes.Black, space * (i + 1), StartOfText_Y);
                    }
                }
                else
                {
                    e.Graphics.DrawString("---", font, Brushes.Black, space * (i + 1), StartOfText_Y);
                }

            }
            StartOfText_Y += font.Height;
            LineNo = 7;
            if (StartOfText_Y > (marginTop + PrintAreaHeight - font.Height * 4))
            {
                //e.HasMorePages = true;
                //prn_NumOfPage += 1;
                return;
            }

            int channela = 1;
            int dema = 0;
            for (int i = 0; i < deviceFromFile.numOfChannel; i++)
            {
                if (deviceFromFile.Channels[i].Unit == 3)
                {
                    switch (dema)
                    {
                        case 0:
                            e.Graphics.DrawString("Alarm Channel" + channela + " X :", font_BU, Brushes.Black, marginLeft, StartOfText_Y);
                            break;

                        case 1:
                            e.Graphics.DrawString("Alarm Channel" + channela + " Y :", font_BU, Brushes.Black, marginLeft, StartOfText_Y);
                            break;

                        case 2:
                            e.Graphics.DrawString("Alarm Channel" + channela + " Z :", font_BU, Brushes.Black, marginLeft, StartOfText_Y);
                            channela += 1;
                            dema = -1;
                            break;
                    }
                    dema += 1;
                }
                else
                {
                    e.Graphics.DrawString("Alarm Channel" + channela + " :", font_BU, Brushes.Black, marginLeft, StartOfText_Y);
                    channela += 1;
                }

                //e.Graphics.DrawString("Alarm Channel"+ i +":", font_BU, Brushes.Black, marginLeft, StartOfText_Y);
                StartOfText_Y += font_BU.Height;
                LineNo = 43;
                if (StartOfText_Y > (marginTop + PrintAreaHeight - font.Height * 4))
                {
                    //e.HasMorePages = true;
                    //prn_NumOfPage += 1;
                    return;
                }

  
                e.Graphics.DrawString("Number of measurements above high alarm limit:", font, Brushes.Black, space, StartOfText_Y);
                if (deviceFromFile.Channels[i].Unit != 0) // Not Use: Unit = 0
                {
                    e.Graphics.DrawString(deviceFromFile.Channels[i].MaxCount.ToString(), font, Brushes.Black, space * 4, StartOfText_Y);
                }
                else
                {
                    e.Graphics.DrawString("Not Use", font, Brushes.Black, space * 4, StartOfText_Y);
                }

                StartOfText_Y += font.Height;
                LineNo = 44;
                if (StartOfText_Y > (marginTop + PrintAreaHeight - font.Height * 4))
                {
                    //e.HasMorePages = true;
                    //prn_NumOfPage += 1;
                    return;
                }

  
                e.Graphics.DrawString("Number of measurements below low alarm limit:", font, Brushes.Black, space, StartOfText_Y);
                if (deviceFromFile.Channels[i].Unit != 0) // Not Use: Unit = 0
                {
                    e.Graphics.DrawString(deviceFromFile.Channels[i].MinCount.ToString(), font, Brushes.Black, space * 4, StartOfText_Y);
                }
                else
                {
                    e.Graphics.DrawString("Not Use", font, Brushes.Black, space * 4, StartOfText_Y);
                }

                StartOfText_Y += font.Height;
                LineNo = 45;
                if (StartOfText_Y > (marginTop + PrintAreaHeight - font.Height * 4))
                {
                    //e.HasMorePages = true;
                    //prn_NumOfPage += 1;
                    return;
                }


                e.Graphics.DrawString("Time over high alarm limit:", font, Brushes.Black, space, StartOfText_Y);
                if (deviceFromFile.Channels[i].Unit != 0) // Not Use: Unit = 0
                {
                    e.Graphics.DrawString(mGlobal.Sec2Day(deviceFromFile.Channels[i].MaxCount * Convert.ToInt32(deviceFromFile.Duration)), font, Brushes.Black, space * 4, StartOfText_Y);
                }
                else
                {
                    e.Graphics.DrawString("Not Use", font, Brushes.Black, space * 4, StartOfText_Y);
                }

                StartOfText_Y += font.Height;
                LineNo = 46;
                if (StartOfText_Y > (marginTop + PrintAreaHeight - font.Height * 4))
                {
                    //e.HasMorePages = true;
                    //prn_NumOfPage += 1;
                    return;
                }


                e.Graphics.DrawString("Time over low alarm limit:", font, Brushes.Black, space, StartOfText_Y);
                if (deviceFromFile.Channels[i].Unit != 0) // Not Use: Unit = 0
                {
                    e.Graphics.DrawString(mGlobal.Sec2Day(deviceFromFile.Channels[i].MinCount * Convert.ToInt32(deviceFromFile.Duration)), font, Brushes.Black, space * 4, StartOfText_Y);
                }
                else
                {
                    e.Graphics.DrawString("Not Use", font, Brushes.Black, space * 4, StartOfText_Y);
                }

                StartOfText_Y += font.Height;
                LineNo = 47;
                if (StartOfText_Y > (marginTop + PrintAreaHeight - font.Height * 4))
                {
                    //e.HasMorePages = true;
                    //prn_NumOfPage += 1;
                    return;
                }
            }

        Line_48:
            e.Graphics.DrawString("Setting Information:", font_BU, Brushes.Black, marginLeft, StartOfText_Y);
            StartOfText_Y += font_B.Height;
            LineNo = 48;
            if (StartOfText_Y > (marginTop + PrintAreaHeight - font.Height * 4))
            {
                //e.HasMorePages = true;
                //prn_NumOfPage += 1;
                return;
            }

        Line_49:
            e.Graphics.DrawString("Duration:", font, Brushes.Black, space, StartOfText_Y);
            int temp = deviceFromFile.Duration;
            e.Graphics.DrawString(temp + " Day", font, Brushes.Black, space * 4, StartOfText_Y);
            StartOfText_Y += font.Height;
            LineNo = 49;
            if (StartOfText_Y > (marginTop + PrintAreaHeight - font.Height * 4))
            {
                //e.HasMorePages = true;
                //prn_NumOfPage += 1;
                return;
            }

        Line_50:
            e.Graphics.DrawString("Delay Start:", font, Brushes.Black, space, StartOfText_Y);
            e.Graphics.DrawString(deviceFromFile.Delay + " Min", font, Brushes.Black, space * 4, StartOfText_Y);
            StartOfText_Y += font.Height;
            LineNo = 50;
            if (StartOfText_Y > (marginTop + PrintAreaHeight - font.Height * 4))
            {
                //e.HasMorePages = true;
                //prn_NumOfPage += 1;
                return;
            }
        Line_51:
            e.Graphics.DrawString("Logger Information:", font_BU, Brushes.Black, marginLeft, StartOfText_Y);
            StartOfText_Y += font_B.Height;
            LineNo = 51;
            if (StartOfText_Y > (marginTop + PrintAreaHeight - font.Height * 4))
            {
                //e.HasMorePages = true;
                //prn_NumOfPage += 1;
                return;
            }

        Line_52:
            e.Graphics.DrawString("Serial Number:", font, Brushes.Black, space, StartOfText_Y);
            e.Graphics.DrawString(deviceFromFile.Serial, font, Brushes.Black, space * 4, StartOfText_Y);
            StartOfText_Y += font.Height;
            LineNo = 52;
            if (StartOfText_Y > (marginTop + PrintAreaHeight - font.Height * 4))
            {
                //e.HasMorePages = true;
                //prn_NumOfPage += 1;
                return;
            }

        Line_53:
            e.Graphics.DrawString("Description:", font, Brushes.Black, space, StartOfText_Y);
            e.Graphics.DrawString(deviceFromFile.Description, font, Brushes.Black, space * 4, StartOfText_Y);
            StartOfText_Y += font.Height;
            LineNo = 53;
            if (StartOfText_Y > (marginTop + PrintAreaHeight - font.Height * 4))
            {
                //e.HasMorePages = true;
                //prn_NumOfPage += 1;
                return;
            }

        Line_54:
            e.Graphics.DrawString("Location:", font, Brushes.Black, space, StartOfText_Y);
            e.Graphics.DrawString(deviceFromFile.Location, font, Brushes.Black, space * 4, StartOfText_Y);
            StartOfText_Y += font.Height;
            LineNo = 54;
            if (StartOfText_Y > (marginTop + PrintAreaHeight - font.Height * 4))
            {
                //e.HasMorePages = true;
                //prn_NumOfPage += 1;
                return;
            }

        Line_55:
            e.Graphics.DrawString("Time Information:", font_BU, Brushes.Black, marginLeft, StartOfText_Y);
            StartOfText_Y += font_B.Height;
            LineNo = 55;
            if (StartOfText_Y > (marginTop + PrintAreaHeight - font.Height * 4))
            {
                //e.HasMorePages = true;
                //prn_NumOfPage += 1;
                return;
            }

        Line_56:
            e.Graphics.DrawString("Start Time:", font, Brushes.Black, space, StartOfText_Y);
            e.Graphics.DrawString(graph.StartTime + " " + graph._timezone.ToString(), font, Brushes.Black, space * 4, StartOfText_Y);
            StartOfText_Y += font.Height;
            LineNo = 56;
            if (StartOfText_Y > (marginTop + PrintAreaHeight - font.Height * 4))
            {
                //e.HasMorePages = true;
                //prn_NumOfPage += 1;
                return;
            }

        Line_57:
            e.Graphics.DrawString("Stop Time:", font, Brushes.Black, space, StartOfText_Y);
            e.Graphics.DrawString(graph.StopTime + " " + graph._timezone.ToString(), font, Brushes.Black, space * 4, StartOfText_Y);
            StartOfText_Y += font.Height;
            LineNo = 57;
            if (StartOfText_Y > (marginTop + PrintAreaHeight - font.Height * 4))
            {
                //e.HasMorePages = true;
                //prn_NumOfPage += 1;
                return;
            }

        Line_58:
            e.Graphics.DrawString("Eclapse Time:", font, Brushes.Black, space, StartOfText_Y);
            e.Graphics.DrawString(graph._eclapsetime, font, Brushes.Black, space * 4, StartOfText_Y);
            StartOfText_Y += font.Height;
            LineNo = 58;
            if (StartOfText_Y > (marginTop + PrintAreaHeight - font.Height * 4))
            {
                //e.HasMorePages = true;
                //prn_NumOfPage += 1;
                return;
            }

        Line_59:
            e.Graphics.DrawString("Measurements:", font_BU, Brushes.Black, marginLeft, StartOfText_Y);
            StartOfText_Y += font_B.Height;
            LineNo = 59;
            if (StartOfText_Y > (marginTop + PrintAreaHeight - font.Height * 4))
            {
                //e.HasMorePages = true;
                //prn_NumOfPage += 1;
                return;
            }

        Line_60:
            e.Graphics.DrawString("Sample Interval:", font, Brushes.Black, space, StartOfText_Y);
            e.Graphics.DrawString(deviceFromFile.Interval.Substring(17, 6), font, Brushes.Black, space * 4, StartOfText_Y);
            StartOfText_Y += font.Height;
            LineNo = 60;
            if (StartOfText_Y > (marginTop + PrintAreaHeight - font.Height * 4))
            {
                //e.HasMorePages = true;
                //prn_NumOfPage += 1;
                return;
            }

        Line_61:
            e.Graphics.DrawString("Number of Measurements:", font, Brushes.Black, space, StartOfText_Y);
            e.Graphics.DrawString(mGlobal.num_measure_suminfo.ToString(), font, Brushes.Black, space * 4, StartOfText_Y);
            StartOfText_Y += font.Height;
            LineNo = 61;
            if (StartOfText_Y > (marginTop + PrintAreaHeight - font.Height * 4))
            {
                //e.HasMorePages = true;
                //prn_NumOfPage += 1;
                return;
            }

        Line_62:
            e.Graphics.DrawString("Comment:", font_BU, Brushes.Black, marginLeft, StartOfText_Y);
            StartOfText_Y += font.Height;
            LineNo = 62;
            if (StartOfText_Y > (marginTop + PrintAreaHeight - font.Height * 4))
            {
                //e.HasMorePages = true;
                //prn_NumOfPage += 1;
                return;
            }

        Line_63:
            e.Graphics.DrawString(deviceFromFile.comment, font, Brushes.Black, marginLeft, StartOfText_Y);
            StartOfText_Y += font.Height;
            LineNo = 63;
            if (StartOfText_Y > (marginTop + PrintAreaHeight - font.Height * 4))
            {
                //e.HasMorePages = true;
                //prn_NumOfPage += 1;
                return;
            }
        }
    }
}
