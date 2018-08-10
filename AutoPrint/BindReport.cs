using BLL;
using CrystalDecisions.CrystalReports.Engine;
using IYASAKAReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoPrint
{
    public class BindReport
    {
        #region 绑定安检
        /// <summary>
        /// 绑定安检报告单
        /// </summary>
        /// <param name="strKey">ID</param>
        /// <param name="strPath">路径</param>
        /// <param name="bSingle">是不是单次打印</param>
        /// <returns>ReportDocument for AJ</returns>
        public ReportDocument BindAJReportEx(string strKey, string strPath, bool bSingle)
        {
            ReportDocument document = new ReportDocument();
            document.Load(strPath);



            RESULT_VEHICLE_INFO_BLL bll = new RESULT_VEHICLE_INFO_BLL();
            var vehicleInfo = bll.GetModelList("ID=" + strKey);

            AJReport ajReport = new AJReport(vehicleInfo[0], bSingle);
            rpt.DsAJ_BGD ds = new rpt.DsAJ_BGD();

            Dictionary<string, Tuple<string, string, string>> dicFialItems;
            ds = (rpt.DsAJ_BGD)ajReport.BindReportResource_Report(ds, out dicFialItems);

            document.SetDataSource(ds);

            return document;
        }

        /// 绑定安检仪器报告单
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="strPath"></param>
        /// <returns></returns>
        public ReportDocument BindAJYQReportEx(string strKey, string strPath, bool bSingle)
        {
            RESULT_VEHICLE_INFO_BLL bll = new RESULT_VEHICLE_INFO_BLL();
            var vehicleInfo = bll.GetModelList("ID=" + strKey);

            ReportDocument document = new ReportDocument();
            document.Load(strPath);

            rpt.DsAJ_YQ ds = new rpt.DsAJ_YQ();
            AJReport ajReport = new AJReport(vehicleInfo[0], bSingle);
            ajReport.PassC = "○";
            ajReport.FailC = "×";
            ajReport.NoAudit = "-";
            ajReport.NoTest = "-";

            ds = (rpt.DsAJ_YQ)ajReport.BindReportSource_YQ(ds);

            document.SetDataSource(ds);
            return document;
        }

        /// <summary>
        /// 绑定安检人工报告单
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="strPath"></param>
        /// <param name="bSingle"></param>
        /// <returns></returns>
        public ReportDocument BindAJRGReportEx(string strKey, string strPath, bool bSingle)
        {
            ReportDocument document = new ReportDocument();
            document.Load(strPath);

            RESULT_VEHICLE_INFO_BLL bll = new RESULT_VEHICLE_INFO_BLL();
            var vehicleInfo = bll.GetModelList("ID=" + strKey);

            AJReport ajReport = new AJReport(vehicleInfo[0], bSingle);
            rpt.DsAJRG ds = new rpt.DsAJRG();
            ds = (rpt.DsAJRG)ajReport.BindReportResource_RG(ds);

            document.SetDataSource(ds);
            return document;
        }
        #endregion
    }
}
