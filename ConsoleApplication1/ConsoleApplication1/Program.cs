using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;

namespace ConsoleApplication1
{
    
    internal class Program
    {
        private static void Main(string[] args)
        {
            string source = @"C:\Program Files (x86)\salesforce.com\DataLoader\Sumitomo ModelYear\data\ModelYearDeleteData\ModelYearDelete.csv";

            DataTable dt = GetDataTabletFromCSVFile(source);

            List<string> invalidIDs = new List<string>();
            foreach (DataRow item in dt.Rows)
            {
                if (!item["Model Year Salesforce ID"].ToString().Trim().StartsWith("a05"))
                {
                    invalidIDs.Add(item["Model Year Salesforce ID"].ToString());
                }
            }

            if (invalidIDs.Count > 0)
            {
                Console.WriteLine("Inlid IDs count: " + invalidIDs.Count);
                foreach (var item in invalidIDs)
                {
                    Console.WriteLine("ID: " + item.ToString());
                }
                Console.ReadLine();
            }
            else
            {
                runBatchFile();
                Console.WriteLine("Process Completed!");
            }
        }

        private static DataTable GetDataTabletFromCSVFile(string csv_file_path)
        {
            DataTable csvData = new DataTable();
            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(csv_file_path))
                {
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    string[] colFields = csvReader.ReadFields();
                    foreach (string column in colFields)
                    {
                        DataColumn datecolumn = new DataColumn(column);
                        datecolumn.AllowDBNull = true;
                        csvData.Columns.Add(datecolumn);
                    }

                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();
                        //Making empty value as null
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            if (fieldData[i] == "")
                            {
                                fieldData[i] = null;
                            }
                        }
                        csvData.Rows.Add(fieldData);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return csvData;
        }

        public static void runBatchFile()
        {
            Process process1 = new Process();
            try
            {
                Process process = new Process();
                ProcessStartInfo info = new ProcessStartInfo
                {
                    FileName = @"C:\Program Files (x86)\salesforce.com\DataLoader\Sumitomo ModelYear\MODEL-YEAR-DELETE.bat",
                    Verb = "runas"
                };
                
                process.StartInfo = info;
                process.Start();
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error");
                Console.WriteLine(exception.Message);
                MessageBox.Show("Batch file is not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
