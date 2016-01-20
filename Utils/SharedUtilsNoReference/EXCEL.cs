using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedUtilsNoReference
{
    public static class EXCEL
    {
        //var excel_dt = LoadWorksheetInDataTable(ConfigurationManager.AppSettings["XLS_NomeSheet"] + "$");
        //
        //List<Change> changes = new List<Change>();
        //foreach (var row in excel_dt.Rows)
        //{
        //    var riga = (DataRow)row;
        //    if (changes.Where(p => p.Old == riga[0].ToString()).Count() == 0)
        //        changes.Add(new Change() { Old = riga[0].ToString(), New = riga[1].ToString() });
        //}
        //
        //var Txt_Old = File.ReadAllText(TXT_File);
        //
        //foreach (var cambiamento in changes)
        //{
        //
        //    if (!string.IsNullOrWhiteSpace(cambiamento.Old) && !string.IsNullOrWhiteSpace(cambiamento.New))
        //        Txt_Old = Txt_Old.Replace(cambiamento.Old, cambiamento.New.Replace(@"'", @"\'"));
        //}
        //
        //File.WriteAllText(TXT_File_New, Txt_Old);

        //public class Change
        //{
        //    public string Old { get; set; }
        //    public string New { get; set; }
        //}



        public static DataTable LoadWorksheetInDataTable(string sheetName,
                                                         string FileXlsPath)
        {
            string XLS_conn = string.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=Excel 8.0", FileXlsPath);

            DataTable sheetData = new DataTable();
            using (var conn = new OleDbConnection(XLS_conn))
            {
                conn.Open();

                // retrieve the data using data adapter
                OleDbDataAdapter sheetAdapter = new OleDbDataAdapter("select * from [" + sheetName + "]", conn);
                sheetAdapter.Fill(sheetData);
            }
            return sheetData;
        }

        public static string GetSheetName(int Pos,
                                          string FileXlsPath)
        {
            string XLS_conn = string.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=Excel 8.0", FileXlsPath);

            using (var oconn = new OleDbConnection(XLS_conn))
            {
                oconn.Open();
                //myCommand.Connection = oconn;
                DataTable dbSchema = oconn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (dbSchema == null || dbSchema.Rows.Count < 1)
                {
                    throw new Exception("Error: Could not determine the name of the first worksheet.");
                }
                string firstSheetName = dbSchema.Rows[1]["TABLE_NAME"].ToString();
                return firstSheetName;
            }
        }
    }
}
