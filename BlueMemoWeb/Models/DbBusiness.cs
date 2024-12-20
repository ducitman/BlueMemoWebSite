using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BlueMemoWeb.Models
{
    public class DbBusiness
    {
        public void SetConnectionString()
        {
            string oraBOSSConnectionString = ConfigurationManager.ConnectionStrings["BOSSDB"].ConnectionString;            
            string oraBBQAConnectionString = ConfigurationManager.ConnectionStrings["BBQADB"].ConnectionString;            
            string sqlLogConnectionString = ConfigurationManager.ConnectionStrings["LOG"].ConnectionString;
            string sqlBlueMemoConnectionString = ConfigurationManager.ConnectionStrings["BLUEMEMO"].ConnectionString;
            string sqlTMAConnectionString = ConfigurationManager.ConnectionStrings["TMA"].ConnectionString;


            DBHelperBOSS.ConnectionString = oraBOSSConnectionString;            
            DBHelperBBQA.ConnectionString = oraBBQAConnectionString;            
            DBHelperLOG.ConnectionString = sqlLogConnectionString;
            DBHelperBlueMemo.ConnectionString = sqlBlueMemoConnectionString;
            DBHelperTMA.ConnectionString = sqlTMAConnectionString;
        }
        public bool CheckAuthorization(string opCode, string programName, string functionName)
        {
            bool result = false;
            try
            {
                string query = @"SELECT Z_USERAUTHORITY.LOGINUSERNAME
                                FROM Z_USERAUTHORITY LEFT JOIN 
                                     Z_PROGRAMFUNCTIONAUTHORITY ON Z_USERAUTHORITY.AUTHORITYGROUP = Z_PROGRAMFUNCTIONAUTHORITY.AUTHORITYGROUP
                                WHERE Z_USERAUTHORITY.LOGINUSERNAME = '" + opCode + "' AND Z_PROGRAMFUNCTIONAUTHORITY.PROGRAMNAME = '" + programName + "' AND Z_PROGRAMFUNCTIONAUTHORITY.FUNCTIONNAME = '" + functionName + @"'";
                return DBHelperBOSS.CheckExist(query);
            }
            catch (Exception ex) 
            {
                
            }
            return result;
        }       
        
        public DataTable GetEFUData(string efu)
        {
            DataTable tbl = new DataTable();
            string query = @"Select * from Z_MATERIALINVENTORY where CARTNUMBER = '" + efu + "'";
            return tbl;
        }
        public string HoldEfuTag(string operatorID, string efu, string reason)
        {
            string holdSummary = "NG";
            string holdResult = "NG";
            string logResult = "NG";
            string bluememoResult = "NG";
            string queryLog = "";
            DataTable dataTbl = new DataTable();

            if (efu.StartsWith("21")|| efu.StartsWith("22")|| efu.StartsWith("23")|| efu.StartsWith("24")|| efu.StartsWith("25"))
            {
                if (!CheckExistBlueMemo(efu, "BBQA"))
                {
                    try
                    {
                        holdResult = SubHoldEfu(efu, "BBQA");
                        bluememoResult = CreateTheBlueMemoTicket(efu, operatorID);
                    }
                    catch (Exception ex)
                    {

                    }
                }                
            }
            else if(efu.StartsWith("A"))
            {
                if (!CheckExistBlueMemo(efu, "SLIT"))
                {
                    try
                    {
                        holdResult = SubHoldEfu(efu, "SLIT");
                    }
                    catch (Exception ex)
                    {

                    }
                }                
            }
            else if (efu.StartsWith("TMARemixing_"))
            {
                if (!CheckExistBlueMemo(efu, "TMARemixing"))
                {
                    try
                    {
                        holdResult = SubHoldEfu(efu, "TMARemixing");
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            else if (efu.StartsWith("TMAExtruding_"))
            {
                if (!CheckExistBlueMemo(efu, "TMAExtruding"))
                {
                    try
                    {
                        holdResult = SubHoldEfu(efu, "TMAExtruding");
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            else
            {
                try
                {
                    holdResult = SubHoldEfu(efu, "BOSS");
                }
                catch(Exception ex)
                {

                }
            }
            return holdSummary;
        }
        public bool CheckExistBlueMemo(string efu, string type) // Closed == 0: Đang xử lý
        {
            bool result = false;

            switch (type)
            {
                case "BBQA":

                    break;
                case "TMARemixing":

                    break;
                case "TMAExtruding":

                    break;
                case "SLIT":

                    break;
                case "BOSS":

                    break;
            }

            return result;
        }
        public string CreateTheBlueMemoTicket(string efu, string operatorID)
        {
            string result = "NG";



            return result;
        }
        public string CreateTheHoldLOG(string efu, string operatorID)
        {
            string logResult = "NG";



            return logResult;
        }
        public string SubHoldEfu(string efu, string database)
        {
            string result = "NG";
            string query = "";
            switch (database)
            {
                case "BOSSDB":
                    try
                    {
                        query = @"Update Z_MATERIALINVENTORY set CARTSTATUS = '3' where '" + efu + "';";
                        DBHelperBOSS.ExecuteQuery(query);
                        result = "OK";
                    }
                    catch(Exception ex)
                    {
                        result = "NG" + "#" + ex.Message;
                    }
                    break;
                case "BBQA":
                    try
                    {
                        query = @"Update STOCKGUMDATA set HOLDFLAG = '1' where BARCODE = '" + efu.Trim() + "';";
                        DBHelperBBQA.ExecuteQuery(query);
                        result = "OK";
                    }
                    catch (Exception ex)
                    {
                        result = "NG" + "#" + ex.Message;
                    }
                    break;
                case "SLIT":
                    try
                    {
                        query = @"Update Z_SLITSHEETDATA set status = '3' where CARTNUMBER = '" + efu.Trim() + "';";
                        DBHelperBOSS.ExecuteQuery(query);
                        result = "OK";
                    }
                    catch (Exception ex)
                    {
                        result = "NG" + "#" + ex.Message;
                    }
                    break;
                case "TMARemixing":
                    try
                    {
                        query = @"Update TMAINVENTORY set CartStatus = '3' where Barcode = '" + efu.Trim() + "';";
                        DBHelperTMA.ExecuteQuery(query);
                        result = "OK";
                    }
                    catch (Exception ex)
                    {
                        result = "NG" + "#" + ex.Message;
                    }
                    break;
                case "TMAExtruding":
                    try
                    {
                        query = @"Update TMA_EXTRUDING set CartStatus = '3' where Barcode = '" + efu.Trim() + "';";
                        DBHelperTMA.ExecuteQuery(query);
                        result = "OK";
                    }
                    catch (Exception ex)
                    {
                        result = "NG" + "#" + ex.Message;
                    }
                    break;
            }

            return result;
        }
        public string StatusOfEfu(string efu)
        {
            string result = "NG";
            string ef = "";
            DataTable _tbl = new DataTable();
            // TH là Mixing 
            if (efu.StartsWith("21")|| efu.StartsWith("22")|| efu.StartsWith("23")|| efu.StartsWith("24")|| efu.StartsWith("25"))
            {
                _tbl = GetMixingData(efu);
                if(_tbl.Rows.Count > 0)
                {
                    result = "OK#";
                    result += _tbl.Rows[0]["REMAIN"].ToString().Trim() + "#";
                    result += _tbl.Rows[0]["STATUS"].ToString().Trim();
                }
            }
            else if (efu.StartsWith("A"))
            {
                _tbl = GetSlitSheetData(efu);
                if (_tbl.Rows.Count > 0)
                {
                    result = "OK#";
                    result += _tbl.Rows[0]["REMAIN"].ToString().Trim() + "#";
                    result += _tbl.Rows[0]["STATUS"].ToString().Trim();
                }
            }
            else if (efu.StartsWith("TMARemixing_"))
            {
                _tbl = GetTMAData(efu);
                if(_tbl.Rows.Count > 0)
                {
                    result = "OK#";
                    result += _tbl.Rows[0]["TMA_Weight"].ToString().Trim() + "#";
                    result += _tbl.Rows[0]["CartStatus"].ToString().Trim();
                }
            }
            //else if (efu.StartsWith("TMAMixing_"))
            //{

            //}
            else // TH là Material
            {
                ef = efu.Substring(3, 10);
                DataTable tbl = GetMaterialData(ef);
                if(tbl.Rows.Count > 0)
                {
                    result = "OK#";
                    result += tbl.Rows[0]["REMAIN"].ToString().Trim() + "#";
                    result += tbl.Rows[0]["CARTSTATUS"].ToString().Trim();
                }
            }

            return result;
        }
        public DataTable GetMixingData(string efu)
        {
            DataTable tbl = new DataTable();
            try
            {
                tbl = DBHelperBBQA.GetData(@"Select * from STOCKGUMDATA where BARCODE = '" + efu.Trim() + "'");
            }
            catch(Exception ex)
            {

            }            
            return tbl;
        }
        public DataTable GetTMAData(string barcode)
        {
            DataTable tbl = new DataTable();
            try
            {
                tbl = DBHelperTMA.GetData(@"Select * from TMAINVENTORY where Barcode = '" + barcode + "'");
            }
            catch(Exception ex)
            {

            }
            return tbl;
        }
        public DataTable GetMaterialData(string efu)
        {
            DataTable tbl = new DataTable();
            try
            {
                tbl = DBHelperBOSS.GetData(@"Select * from Z_MATERIALINVENTORY where CARTNUMBER = '" + efu.Trim() + "'");
            }
            catch (Exception ex)
            {

            }
            return tbl;
        }
        public DataTable GetSlitSheetData(string efu)
        {
            DataTable tbl = new DataTable();
            try
            {
                tbl = DBHelperBOSS.GetData(@"Select * from Z_SLITSHEETDATA where CARTNUMBER = '" + efu.Trim() + "'");
            }
            catch (Exception ex)
            {

            }
            return tbl;
        }
    }
}