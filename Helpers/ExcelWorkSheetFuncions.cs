using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;

namespace GAP.Helpers
{
    public class ExcelWorkSheetFuncions
    {
        Application xl;

        WorksheetFunction wsf;
        public ExcelWorkSheetFuncions()
        {
            xl = new Application();
            wsf = xl.WorksheetFunction;
        }

        /// <summary>
        /// Example of a called 'equation' from excel: Log 
        /// </summary>
        /// <param name="number">Number which want the logarithm</param>
        /// <param name="logNumber">Number base to the logarithm</param>
        /// <returns></returns>
        public double GetLog(double number, double logNumber)
        {
            
            return wsf.Log(number, logNumber);
        }

        //All function that could be called must be in an Array list
        //it can be done over commands like others control
        //to manage each equation select as a unique equation



    }
}
