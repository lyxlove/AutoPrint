using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;
using BLL;
using Model;
using System.Threading;
using DBUtility;
using System.Threading.Tasks;

namespace AutoPrint
{
    public partial class Form1 : Form
    {
        private BindReport bindReport = null;
        Thread t = null;
        
        public Form1()
        {
            InitializeComponent();
            btnStop.Enabled = false;
            LoadPrinter();
            bindReport = new BindReport();

        }

        public void LoadPrinter()
        {
            string tt = string.Empty;
            PrintDocument prtdoc = new PrintDocument();
            List<string> list = new List<string>();
            foreach (String str in PrinterSettings.InstalledPrinters)
            {
                combPrinter.Items.Add(str);
            }
         
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(combPrinter.Text))
            {
                MessageBox.Show("请选择打印机");
                return;
            }
            StartPrint();
            btnPrint.Enabled = false;
            btnStop.Enabled = true;
            combPrinter.Enabled = false;
        }

        private void StartPrint()
        {
            t = new Thread(new ParameterizedThreadStart(PrintReport));
            t.IsBackground = true;
            t.Start(combPrinter.Text);
            WriteLog("开始自动打印");
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                if (t != null)
                    t.Suspend();
                t = null;
            }
            catch (Exception ex)
            {

            }
            btnPrint.Enabled = true;
            btnStop.Enabled = false;
            combPrinter.Enabled = true;
            WriteLog("停止自动打印");
        }

        private void PrintReport(object obj)
        {
            try
            {
                while (true)
                {
                    string strPrinterName = (string)obj;
                    if (!string.IsNullOrEmpty(strPrinterName))
                    {
                        string strSearchSql = "SELECT Top 1 A.JCLSH,A.Z_PD,A.JYLBDH, A.ID AS PrintId  FROM RESULT_VEHICLE_INFO A WHERE A.ID NOT IN (SELECT DISPATCH_ID FROM PRINT_RECORD) ORDER BY A.ID DESC";
                        DataTable dtSearch = new DataTable();
                        DbHelper.GetTable(strSearchSql, ref dtSearch);
                        PrinterDataEntity dataEnity = FillTools.FillEntity<PrinterDataEntity>(dtSearch);
                        if (dataEnity != null && dataEnity.PrintId > 0)
                        {
                            WriteLog("查询到代打印数据:" + dataEnity.JCLSH);
                            string strInsertSql = string.Format("INSERT INTO PRINT_RECORD (DISPATCH_ID, JCLSH) VALUES({0},'{1}')", dataEnity.PrintId, dataEnity.JCLSH);
                            if (DbHelper.ExecuteSql(strInsertSql))
                            {
                                if ( dataEnity.JYLBDH == "01,"  && dataEnity.Z_PD == 1)
                                {
                                    WriteLog(string.Format("{0} 打印【报告单】", dataEnity.JCLSH));
                                    PrintBGD(dataEnity.PrintId, strPrinterName);
                                    WriteLog(string.Format("{0} 打印【仪器】", dataEnity.JCLSH));
                                    PrintYQ(dataEnity.PrintId, strPrinterName);
                                }
                                else if (dataEnity.JYLBDH == "02," && dataEnity.Z_PD == 1)
                                {
                                    WriteLog(string.Format("{0} 打印【报告单】", dataEnity.JCLSH));
                                    PrintBGD(dataEnity.PrintId, strPrinterName);
                                    WriteLog(string.Format("{0} 打印【仪器】", dataEnity.JCLSH));
                                    PrintYQ(dataEnity.PrintId, strPrinterName);
                                }
                                else if (dataEnity.JYLBDH == "02," && dataEnity.Z_PD == 2)
                                {
                                    WriteLog(string.Format("{0} 打印【仪器】", dataEnity.JCLSH));
                                    PrintYQ(dataEnity.PrintId, strPrinterName);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message,"Error");
            }
            finally
            {
                
            }
            
        }

        private void PrintBGD(int id, string strPrinterName)
        {
            ReportDocument rd = null;
            try
            {
                rd = bindReport.BindAJReportEx(id.ToString(), "rpt/CrystalReportAJ.rpt", false);
                rd.PrintOptions.PrinterName = strPrinterName;
                rd.PrintToPrinter(1, false, 1, 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (rd != null)
                    rd.Dispose();
            }
        }

        private void WriteLog(string strMsg,string strMsgType = "Normal",string strJCLSH = "")
        {
            try
            {
                string strSql = string.Format("INSERT INTO AutoPrint_Log (Msg,MsgType,Operate_Date,JCLSH,HostName,ThreadId)" +
                   "VALUES ('{0}','{1}','{2}','{3}','{4}','{5}')", strMsg, strMsgType, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), strJCLSH, System.Net.Dns.GetHostName(), string.Empty);
                DbHelper.ExecuteSql(strSql);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PrintYQ(int id, string strPrinterName)
        {
            ReportDocument rd = null;
            try
            {
                rd = bindReport.BindAJYQReportEx(id.ToString(), "rpt/CrystalReportAJYQ.rpt", false);
                rd.PrintOptions.PrinterName = strPrinterName;
                rd.PrintToPrinter(1, false, 1, 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (rd != null)
                    rd.Dispose();
            }
        }

        private void PrintRG(int id, string strPrinterName)
        {
            ReportDocument rd = null;
            try
            {
                rd = bindReport.BindAJRGReportEx(id.ToString(), "rpt/CrystalReportAJRG_KMYD.rpt", false);
                rd.PrintOptions.PrinterName = strPrinterName;
                rd.PrintToPrinter(1, false, 1, 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (rd != null)
                    rd.Dispose();
            }
        }


    }
}
