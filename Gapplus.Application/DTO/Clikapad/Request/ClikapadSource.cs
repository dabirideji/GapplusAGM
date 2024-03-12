using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class ClikapadSource
    {
        //private AxCLIKAPAD_ActiveX_v4.AxCLIKAPAD_v4_OCX axCLiKAPAD;
        ///// <summary>
        //private string txtFeedBack;
        //private string txtKpadLo;
        //private string txtKpadHi;
        //private string[] BaseSerial;
        //private List<KeypadResults> keypadresults = new List<KeypadResults>();
   

        //public ClikapadSource()
        //{
        //    //
        //    // Required for Windows Form Designer support
        //    //
        //    InitializeComponent();

        //    //
        //    // TODO: Add any constructor code after InitializeComponent call
        //    //
        //}

        // private void InitializeComponent()
        //{
        //    // axCLiKAPAD
        //    System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ClikapadSource));
        //    this.axCLiKAPAD = new AxCLIKAPAD_ActiveX_v4.AxCLIKAPAD_v4_OCX();
        //    //this.axCLiKAPAD.Enabled = true;
        //    //this.axCLiKAPAD.Name = "axCLiKAPAD";
        //    //this.axCLiKAPAD.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axCLiKAPAD.OcxState")));
        //    this.axCLiKAPAD.VoteReceived += new AxCLIKAPAD_ActiveX_v4.__CLIKAPAD_v4_OCX_VoteReceivedEventHandler(this.axCLiKAPAD_VoteReceived);
        //    this.axCLiKAPAD.ErrorMessage += new AxCLIKAPAD_ActiveX_v4.__CLIKAPAD_v4_OCX_ErrorMessageEventHandler(this.axCLiKAPAD_ErrorMessage);
        //    this.axCLiKAPAD.Feedback += new AxCLIKAPAD_ActiveX_v4.__CLIKAPAD_v4_OCX_FeedbackEventHandler(this.axCLiKAPAD_Feedback);
        //    this.axCLiKAPAD.MultiBaseInfo += new AxCLIKAPAD_ActiveX_v4.__CLIKAPAD_v4_OCX_MultiBaseInfoEventHandler(this.axCLiKAPAD_MultiBaseInfo);
        //}

        //private void cmdListDevices_Click(object sender, System.EventArgs e)
        //{
        //    axCLiKAPAD.ListDevices();
        //}

        //private void axCLiKAPAD_ErrorMessage(object sender, AxCLIKAPAD_ActiveX_v4.__CLIKAPAD_v4_OCX_ErrorMessageEvent e)
        //{
        //    txtFeedBack = TimeNow() + "Error " + e.errorNum + " - " + e.errorMsg + System.Environment.NewLine + txtFeedBack;
        //}

        //private void axCLiKAPAD_Feedback(object sender, AxCLIKAPAD_ActiveX_v4.__CLIKAPAD_v4_OCX_FeedbackEvent e)
        //{

        //    txtFeedBack= TimeNow() + e.sFeedback + System.Environment.NewLine + txtFeedBack;
        //}

        //private void axCLiKAPAD_MultiBaseInfo(object sender, AxCLIKAPAD_ActiveX_v4.__CLIKAPAD_v4_OCX_MultiBaseInfoEvent e)
        //{
        //    txtFeedBack = TimeNow() + e.noOfBases + " Base Station(s) Found - " + e.baseInfo;
        //    if (e.noOfBases > 0)
        //        BaseSerial = e.baseInfo.Split(new char[] { ',' });

        //}

        //private void axCLiKAPAD_VoteReceived(object sender, AxCLIKAPAD_ActiveX_v4.__CLIKAPAD_v4_OCX_VoteReceivedEvent e)
        //{
        //    txtFeedBack = TimeNow() + "Vote Received - " + e.votes + System.Environment.NewLine + txtFeedBack;
        //}

        //private void cmdStartVote_Click(object sender, System.EventArgs e)
        //{
        //    /// start a vote for the first base station
        //    axCLiKAPAD.StartVote(int.Parse(BaseSerial[0]), float.Parse(txtKpadLo), float.Parse(txtKpadHi), 0, 11, 0, "", 0, 0, 1);
        //}

        //private void cmdStop_Click(object sender, System.EventArgs e)
        //{
        //    axCLiKAPAD.EndProcess(int.Parse(BaseSerial[0]));
        //}

        //String TimeNow()
        //{
        //    string theTime = DateTime.Now.ToLongTimeString();
        //    theTime = "[" + theTime + "] - ";
        //    return theTime;
        //}
    }
}