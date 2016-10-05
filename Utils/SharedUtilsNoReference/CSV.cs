using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SharedUtilsNoReference
{
    public static class CSV
    {
        //rimuovo tutti i NUL presi da database/csv (/x00)
        public static string ReplaceNull(string Raw)
        {
            return Regex.Replace(Raw, "\x00\x00", " ");
        }


        //todo: use table instead of DataSet
        //todo: create a templated list version
        public static string ToCSV_String(DataSet dataset)
        {
            StringBuilder sb = new StringBuilder();

            //Header
            for (int k = 0; k < dataset.Tables[0].Columns.Count - 1; k++)
                sb.Append(dataset.Tables[0].Columns[k].ColumnName.ToLower() + ';'); //se un csv inizia per ID verrà letto come SYLK
            sb.Append(dataset.Tables[0].Columns[dataset.Tables[0].Columns.Count - 1].ColumnName.ToLower()); //ultimo elemento senza ;

            //append new line
            sb.Append("\r\n");
            for (int i = 0; i < dataset.Tables[0].Rows.Count; i++)
            {
                //add separators
                for (int k = 0; k < dataset.Tables[0].Columns.Count; k++)
                {
                    string aggiungi_colonna = dataset.Tables[0].Rows[i].ItemArray[k].ToString();

                    if (k == dataset.Tables[0].Columns.Count - 1) //ultima colonna senza ;
                        sb.Append(aggiungi_colonna);
                    else
                    {
                        sb.Append(aggiungi_colonna + ';');
                    }
                }

                //append new line
                sb.Append("\r\n");
            }

            return sb.ToString();
        }
    }
}
