using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Pexo16
{
    public partial class Progress : Form
    {
        public event ProgressChangedEventHandler ProgressChanged;

        int sum = 0;

        public Progress(int sumTime)
        {
            InitializeComponent();
            sum = sumTime;

            progressBar1.Maximum = sum;
            progressBar1.Minimum = 0;
            //progressBar1.Step = sum/100;
            //progressBar1.Width = sum;
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((e.Cancelled == true))
            {
                this.lblProgress.Text = "Canceled!";
            }

            else if (!(e.Error == null))
            {
                this.lblProgress.Text = ("Error: " + e.Error.Message);
            }

            else
            {
                this.lblProgress.Text = "Done!";
            }
        }


        private void Progress_Load(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy)
            {
                backgroundWorker1.CancelAsync();
            }
            else
            {
                progressBar1.Value = 0;
                progressBar1.Refresh();
                //backgroundWorker1.RunWorkerAsync();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.WorkerSupportsCancellation == true)
            {
                backgroundWorker1.CancelAsync();
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
    //        BackgroundWorker sendingWorker =
    //(BackgroundWorker)sender;//Capture the BackgroundWorker that fired the event
    //        object[] arrObjects =
    //        (object[])e.Argument;//Collect the array of objects the we received from the main thread

    //        int maxValue = (int)arrObjects[0];//Get the numeric value 
    //        //from inside the objects array, don't forget to cast
    //        StringBuilder sb = new StringBuilder();//Declare a new string builder to store the result.

    //        for (int i = 1; i <= maxValue; i++)//Start a for loop
    //        {
    //            if (!sendingWorker.CancellationPending)//At each iteration of the loop, 
    //            //check if there is a cancellation request pending 
    //            {
    //                //sb.Append(string.Format("Counting number: {0}{1}", PerformHeavyOperation(i), Environment.NewLine));//Append the result to the string builder
    //                sendingWorker.ReportProgress(i);//Report our progress to the main thread
    //            }
    //            else
    //            {
    //                e.Cancel = true;//If a cancellation request is pending, assign this flag a value of true
    //                break;// If a cancellation request is pending, break to exit the loop
    //            }
    //        }

    //        e.Result = sb.ToString();// Send our result to the main thread!






            //BackgroundWorker worker = sender as BackgroundWorker;

            for (int i = 1; i <= 100; i++)
            {
           // while (true)
            //{
                if ((backgroundWorker1.CancellationPending == true))
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    System.Threading.Thread.Sleep(100);
                    // Perform a time consuming operation and report progress.
                    backgroundWorker1.ReportProgress(i);
                    if (mGlobal.numProgress == sum)
                    {
                        break;
                    }

                    //System.Threading.Thread.Sleep(100);
                    //worker.ReportProgress(i);
                }
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lblProgress.Text = (e.ProgressPercentage.ToString() + "%");
            progressBar1.Value = e.ProgressPercentage;
            progressBar1.Refresh();
            //progressBar1.PerformStep();


            //var temp = ProgressChanged;
            //if (temp != null)
            //    temp(this, e);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((e.Cancelled == true))
            {
                this.lblProgress.Text = "Canceled!";
            }

            else if (!(e.Error == null))
            {
                this.lblProgress.Text = ("Error: " + e.Error.Message);
            }

            else
            {
                this.lblProgress.Text = "Done!";
            }
        }


    }
}
