// Copyright (C) 2006 Cognex Corporation
//
// Subject to Cognex Corporation's terms and conditions and license agreement,
// you are authorized to use and modify this source code in any way you find
// useful, provided the Software and/or the modified Software is used solely
// in conjunction with a Cognex Machine Vision System.  Furthermore you
// acknowledge and agree that Cognex has no warranty, obligations or liability
// for your use of the Software.
// ***************************************************************************
// This sample program is designed to illustrate certain VisionPro features or 
// techniques in the simplest way possible. It is not intended as the framework 
// for a complete application. In particular, the sample program may not provide
// proper error handling, event handling, cleanup, repeatability, and other 
// mechanisms that a commercial quality application requires.

// This sample demonstrates how to load a persisted QuickBuild application
// and access the results provided in the user
// result queue. The sample uses "mySavedQB.vpp", which consists of a single
// Job that executes a Blob tool with default parameters using images from
// a file.
//
// The provided .vpp file is configured to use "VPRO_ROOT/Images/pmSample.idb" as the
// source of images.  
//
// This application uses a timer to determine when results should be updated
// on the user interface.
//
// To use: If necessary, reconfigure acquisition using QuickBuild.  Then run
// this sample code.  The number of blobs will be displayed in the count text
// box and the Blob tool input image will be displayed in the image display
// control.

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Text;
using Cognex.VisionPro;
using Cognex.VisionPro.QuickBuild;

namespace SimpleApp
{
  /// <summary>
  /// Summary description for Form1.
  /// </summary>
  public class Form1 : System.Windows.Forms.Form
  {
    internal System.Windows.Forms.Label Label1;
    internal System.Windows.Forms.TextBox myCountText;
    internal System.Windows.Forms.TextBox SampleTextBox;
    CogJobManager myJobManager;
    CogJob myJob;
    CogJobIndependent myIndependentJob;
    private System.Windows.Forms.Timer timer1;
    private System.Windows.Forms.CheckBox RunContCheckBox;
    private System.Windows.Forms.Button RunOnceButton;
    private CogRecordDisplay cogRecordDisplay1;
    private System.ComponentModel.IContainer components;

    public Form1()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
      InitializeJobManager();
      this.Closing += new CancelEventHandler(Form1_Closing);

      // This is the timer event
      timer1.Tick += new EventHandler(timer1_Tick);
    }

    private void InitializeJobManager()
    {
      SampleTextBox.Text =
            "This sample demonstrates how to load a persisted QuickBuild application and access " +
            "the results provided in the posted items queue (a.k.a. the user result queue)." +
            Environment.NewLine + Environment.NewLine +
            @"The sample uses ""mySavedQB.vpp"", which consists of a single Job that " +
            "executes a Blob tool with default parameters using images from a file.  " +
            @"The provided .vpp file is configured to use ""VPRO_ROOT\images\pmSample.idb"" as the " +
            "source of images." +
            Environment.NewLine + Environment.NewLine +
            "To use: Click the Run button or the Run Continuous button.  " +
            "The number of blobs will be displayed in the count text box " +
            @"and the Blob tool input image will be displayed in the image display control.";

      // Depersist the QuickBuild session
      myJobManager = (CogJobManager)CogSerializer.LoadObjectFromFile(
          Environment.GetEnvironmentVariable("VPRO_ROOT") + "\\Samples\\Programming\\QuickBuild\\mySavedQB.vpp");
      myJob = myJobManager.Job(0);
      myIndependentJob = myJob.OwnedIndependent;

      // Flush queues
      myJobManager.UserQueueFlush();
      myJobManager.FailureQueueFlush();
      myJob.ImageQueueFlush();
      myIndependentJob.RealTimeQueueFlush();

      // Start the timer.
      timer1.Start();
    }

    // This method handles the tick event from the timer.  When the timer "ticks", 
    // an image is taken from the Job Manager User Queue and is displayed on the GUI.  
    // In this sample, the blob count, which is placed on the Job Real-Time Queue,
    // is also displayed on the GUI.
    private void timer1_Tick(object sender, EventArgs e)
    {
      UpdateGUI();
    }

    // This method grabs the blob count from the 
    // Job Manager User Queue and displays it on the GUI.
    private void UpdateGUI()
    {
      Cognex.VisionPro.ICogRecord tmpRecord;
      Cognex.VisionPro.ICogRecord topRecord = myJobManager.UserResult();

      // check to be sure results are available
      if (topRecord == null) return;

      // Assume that the required "count" record is present, and go get it.
      tmpRecord = topRecord.SubRecords[@"Tools.Item[""CogBlobTool1""].CogBlobTool.Results.GetBlobs().Count"];
      if (tmpRecord != null)
      {
        int count = (int)tmpRecord.Content;
        myCountText.Text = count.ToString();

        // Assume that the required "image" record is present, and go get it.
        tmpRecord = topRecord.SubRecords["ShowLastRunRecordForUserQueue"];
        if (tmpRecord != null)
        {
          tmpRecord = tmpRecord.SubRecords["LastRun"];
          if (tmpRecord != null)
          {
            tmpRecord = tmpRecord.SubRecords["Image Source.OutputImage"];
            if (tmpRecord != null)
            {
              cogRecordDisplay1.Record = tmpRecord;
              cogRecordDisplay1.Fit(true);
            }
          }
        }
      }
    }

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        if (components != null)
        {
          components.Dispose();
        }
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
      this.Label1 = new System.Windows.Forms.Label();
      this.myCountText = new System.Windows.Forms.TextBox();
      this.SampleTextBox = new System.Windows.Forms.TextBox();
      this.timer1 = new System.Windows.Forms.Timer(this.components);
      this.RunContCheckBox = new System.Windows.Forms.CheckBox();
      this.RunOnceButton = new System.Windows.Forms.Button();
      this.cogRecordDisplay1 = new Cognex.VisionPro.CogRecordDisplay();
      ((System.ComponentModel.ISupportInitialize)(this.cogRecordDisplay1)).BeginInit();
      this.SuspendLayout();
      // 
      // Label1
      // 
      this.Label1.Location = new System.Drawing.Point(24, 128);
      this.Label1.Name = "Label1";
      this.Label1.Size = new System.Drawing.Size(40, 16);
      this.Label1.TabIndex = 4;
      this.Label1.Text = "Count:";
      // 
      // myCountText
      // 
      this.myCountText.Location = new System.Drawing.Point(40, 152);
      this.myCountText.Name = "myCountText";
      this.myCountText.ReadOnly = true;
      this.myCountText.Size = new System.Drawing.Size(64, 20);
      this.myCountText.TabIndex = 3;
      // 
      // SampleTextBox
      // 
      this.SampleTextBox.Location = new System.Drawing.Point(432, 16);
      this.SampleTextBox.Multiline = true;
      this.SampleTextBox.Name = "SampleTextBox";
      this.SampleTextBox.ReadOnly = true;
      this.SampleTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.SampleTextBox.Size = new System.Drawing.Size(288, 184);
      this.SampleTextBox.TabIndex = 5;
      // 
      // timer1
      // 
      this.timer1.Interval = 50;
      // 
      // RunContCheckBox
      // 
      this.RunContCheckBox.Appearance = System.Windows.Forms.Appearance.Button;
      this.RunContCheckBox.Location = new System.Drawing.Point(16, 56);
      this.RunContCheckBox.Name = "RunContCheckBox";
      this.RunContCheckBox.Size = new System.Drawing.Size(96, 32);
      this.RunContCheckBox.TabIndex = 2;
      this.RunContCheckBox.Text = "Run Continuous";
      this.RunContCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.RunContCheckBox.CheckedChanged += new System.EventHandler(this.RunContCheckBox_CheckedChanged);
      // 
      // RunOnceButton
      // 
      this.RunOnceButton.Location = new System.Drawing.Point(16, 16);
      this.RunOnceButton.Name = "RunOnceButton";
      this.RunOnceButton.Size = new System.Drawing.Size(96, 32);
      this.RunOnceButton.TabIndex = 1;
      this.RunOnceButton.Text = "Run Once";
      this.RunOnceButton.Click += new System.EventHandler(this.RunOnceButton_Click);
      // 
      // cogRecordDisplay1
      // 
      this.cogRecordDisplay1.Location = new System.Drawing.Point(118, 20);
      this.cogRecordDisplay1.Name = "cogRecordDisplay1";
      this.cogRecordDisplay1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogRecordDisplay1.OcxState")));
      this.cogRecordDisplay1.Size = new System.Drawing.Size(308, 195);
      this.cogRecordDisplay1.TabIndex = 6;
      // 
      // Form1
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(768, 238);
      this.Controls.Add(this.cogRecordDisplay1);
      this.Controls.Add(this.RunContCheckBox);
      this.Controls.Add(this.RunOnceButton);
      this.Controls.Add(this.SampleTextBox);
      this.Controls.Add(this.myCountText);
      this.Controls.Add(this.Label1);
      this.Name = "Form1";
      this.Text = "QuickBuild Sample Application";
      ((System.ComponentModel.ISupportInitialize)(this.cogRecordDisplay1)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }
    #endregion

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      Application.Run(new Form1());
    }

    private void Form1_Closing(object sender, CancelEventArgs e)
    {
      timer1.Stop();
      cogRecordDisplay1.Dispose();
      // Be sure to shudown the CogJobManager!!
      myJobManager.Shutdown();
    }

    private void RunOnceButton_Click(object sender, System.EventArgs e)
    {
      try
      {
        myJobManager.Run();
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
      }
    }

    private void RunContCheckBox_CheckedChanged(object sender, System.EventArgs e)
    {
      if (RunContCheckBox.Checked)
      {
        try
        {
          myJobManager.RunContinuous();
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message);
        }
        RunOnceButton.Enabled = false;
      }
      else
      {
        try
        {
          myJobManager.Stop();
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message);
        }
        RunOnceButton.Enabled = true;
      }
    }
  }
}
