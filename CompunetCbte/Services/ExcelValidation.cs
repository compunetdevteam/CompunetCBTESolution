using System;
using System.Collections;
using OfficeOpenXml;

namespace CompunetCbte.Services
{
    public class ExcelValidation
    {
        public string ValidateExcel(int noOfRow, ExcelWorksheet workSheet, int noOfRequired)
        {
            int colum = 0;
            var myList = new ArrayList();
            for (int row = 2; row <= noOfRow; row++)
            {
                try
                {
                    for (int i = 1; i <= noOfRequired; i++)
                    {
                        myList.Add(workSheet.Cells[row, i].Value.ToString().Trim());
                        colum = i + 1;
                    }


                }
                catch (Exception e)
                {
                    //Validation = row.ToString();
                    return row.ToString() + " " + colum.ToString();
                }
            }
            return "Success";
        }
    }
}