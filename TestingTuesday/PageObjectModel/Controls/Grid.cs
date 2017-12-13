using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace PageObjectModel.Controls
{
    public class Grid : ControlBase
    {

        //Refers to table in the DOM
        List<string> RecordNotFound = new List<string>();

        public Grid(IWebElement webElement) : base(webElement) {
            //List of known Danish statements for record not found.
            RecordNotFound.Add("(Intet resultat)");
            RecordNotFound.Add("(Ingen aftaler fundet)");
            RecordNotFound.Add("(Ingen kontoplaner fundet)"); 
        }

        public override bool IsClickable
        {
            get
            {
                return true;
            }
        }

        
        public void SelectRow(IWebDriver driver,int index) 
        {
            var elementId = _element.GetAttribute("id");
            //((IJavaScriptExecutor)driver).ExecuteScript(String.Format("$('#{0}').data('kendoGrid').select('tr:eq({1})');", elementId,index));            
            var element = _element.FindElement(By.XPath(String.Format(" //*[@id='{0}']//table/tbody/tr[{1}]", elementId, index)));
            Actions action = new Actions(driver);
            action.Click(element);
            action.Perform();
        }

        public void select(IWebDriver driver,int index) 
        {
            var elementId = _element.GetAttribute("id");
            var element=_element.FindElement(By.XPath(String.Format(" //*[@id='{0}']/div[3]/table/tbody/tr[{1}]",elementId,index)));
            Actions action = new Actions(driver);
            action.Click(element);
            action.Perform();
        }
        public void selectRegel(IWebDriver driver, int index)
        {
            var elementId = _element.GetAttribute("id");
            var element = _element.FindElement(By.XPath(String.Format(" //*[@id='{0}']/div[3]/div[1]/table/tbody/tr[{1}]", elementId, index)));
            Actions action = new Actions(driver);
            action.Click(element);
            action.Perform();
        }

        public void selectYdelse(IWebDriver driver, int index)
        {
            var elementId = _element.GetAttribute("id");
            var element = _element.FindElement(By.XPath(String.Format(" //*[@id='{0}']/div[3]/table/tbody/tr[{1}]", elementId, index)));
            Actions action = new Actions(driver);
            action.Click(element);
            action.Perform();
        }

        public void Dblclick(IWebDriver driver, int index)
        {
            var elementId = _element.GetAttribute("id");
            var element= _element.FindElement(By.XPath(String.Format("//*[@id='{0}']//table/tbody/tr[{1}]", elementId, index)));
            Actions action = new Actions(driver);
            action.DoubleClick(element);
            action.Perform();
        }

        public void DblclickforVirtualScrollGrids(IWebDriver driver, int index)
        {
            var elementId = _element.GetAttribute("id");
            var element = _element.FindElement(By.XPath(String.Format("//*[@id='{0}']/div[3]/div[1]/table/tbody/tr[{1}]", elementId, index)));
            Actions action = new Actions(driver);
            action.DoubleClick(element);
            action.Perform();
        }
        public void RegelDblclick(IWebDriver driver, int index)
        {
            var elementId = _element.GetAttribute("id");
            var element = _element.FindElement(By.XPath(String.Format("//*[@id='{0}']/div[3]/table/tbody/tr[{1}]", elementId, index)));
            Actions action = new Actions(driver);
            action.DoubleClick(element);
            action.Perform();
        }

        public IWebElement AddButton 
        {
            get 
            {
                return _element.FindElement(By.ClassName("k-grid-add"));
            }
        }

        public IWebElement EditingRow {
            get 
            {
                return _element.FindElement(By.CssSelector("div.k-grid-content > table > tbody > tr.ng-scope.k-grid-edit-row"));
            }
        }

        public int Rowcount 
        {
            get
            {
                int rowcount = 0;
                // var rows = _element.FindElements(By.CssSelector("div.k-grid-content > table > tbody > tr")).ToList().Count();
                var rows = _element.FindElements(By.XPath(".//table/tbody/tr")).ToList().Count();

                if (rows == 1) // Validating whether the grid hold record/not.
                {
                    // var firstRow = _element.FindElement(By.CssSelector("div.k-grid-content > table > tbody > tr > td"));
                    var firstRow = _element.FindElement(By.XPath(".//table/tbody/tr/td"));
                    if (!RecordNotFound.Contains(firstRow.GetAttribute("textContent"))) rowcount = 1;
                    //if (firstRow.GetAttribute("textContent").IndexOf("(Intet resultat)", StringComparison.OrdinalIgnoreCase) != -1) rowcount = 1;
                    //if (firstRow.GetAttribute("textContent").IndexOf("(Ingen aftaler fundet)", StringComparison.OrdinalIgnoreCase) != -1) rowcount = 1;                    
                }
                else rowcount = rows;

                return rowcount;
            }
        }

        public int SearchRowcount
        {
            get
            {
                return _element.FindElements(By.CssSelector("div.k-grid-content > table > tbody > tr")).ToList().Count();
            }
        }

        public int SearchRowcountforVirtualScrollGrids
        {
            get
            {
                return _element.FindElements(By.CssSelector("div.k-grid-content > div.k-virtual-scrollable-wrap > table > tbody > tr")).ToList().Count();
            }
        }

        public List<IWebElement> Rows
        {
            get
            {
                //return _element.FindElements(By.CssSelector("div.k-grid-content > table > tbody > tr")).ToList();
                return _element.FindElements(By.XPath(".//table/tbody/tr")).ToList();
            }
        }

        public List<IWebElement> Columns
        {
            get
            {
                return _element.FindElements(By.CssSelector("div.k-grid-header > div > table > thead > tr > th")).ToList();
            }
        }

        public List<IWebElement> SearchRows
        {
            get
            {
                return _element.FindElements(By.CssSelector("div.k-grid-content > table > tbody > tr")).ToList();
            }
        }

        public List<IWebElement> SearchRowsforVirtualScrollGrids
        {
            get
            {
                return _element.FindElements(By.CssSelector("div.k-grid-content > div.k-virtual-scrollable-wrap > table > tbody > tr")).ToList();
            }
        }

        public List<IWebElement> RegelRows
        {
            get
            {
                return _element.FindElements(By.CssSelector("div.k-grid-content > div > table > tbody > tr")).ToList();
            }
        }

        public List<IWebElement> YdelseRegelRows
        {
            get
            {
                return _element.FindElements(By.CssSelector("div.k-grid-content > table > tbody > tr")).ToList();
            }
        }

        public int Columncount
        {
            get
            {
                return _element.FindElements(By.CssSelector("div.k-grid-header > div > table > thead > tr > th")).ToList().Count();
            }
        }

        public void AddRow(IWebDriver driver)
        {
            var elementId = _element.GetAttribute("id");
            ((IJavaScriptExecutor)driver).ExecuteScript(String.Format("$('#{0}').data('kendoGrid').addRow();", elementId));
            //((IJavaScriptExecutor)driver).ExecuteScript(String.Format("$('#{0}').data('kendoDropDownList').trigger('change');", elementId));
        }

        public void EditRow(IWebDriver driver,int rowindex)
        {
            var elementId = _element.GetAttribute("id");
            var script = String.Format("$('#{0}').data('kendoGrid').editRow($('#{0} tr:eq({1})'));", elementId, rowindex);
            ((IJavaScriptExecutor)driver).ExecuteScript(script);
        }

        public void EditCell(IWebDriver driver, int columnindex)
        {
            var elementId = _element.GetAttribute("id");
            var script = String.Format("$('#{0}').data('kendoGrid').editCell($('#{0} td:eq({1})'));", elementId, columnindex);
            ((IJavaScriptExecutor)driver).ExecuteScript(script);
        }

        public void EditCell(IWebDriver driver, string columnName)
        {
            var columnindex = GetColumnIndex(columnName);

            var elementId = _element.GetAttribute("id");
            var script = String.Format("$('#{0}').data('kendoGrid').editCell($('#{0} td:eq({1})'));", elementId, columnindex);
            ((IJavaScriptExecutor)driver).ExecuteScript(script);
        }

        public void EditCell(IWebDriver driver, string columnName, string CellValue)
        {
            var columnindex = GetColumnIndex(columnName);

            //Edit Cell
            var elementId = _element.GetAttribute("id");
            var Editscript = String.Format("$('#{0}').data('kendoGrid').editCell($('#{0} td:eq({1})'));", elementId, columnindex);
            ((IJavaScriptExecutor)driver).ExecuteScript(Editscript);

            //Clear Existing value
            var Clearscript = String.Format("$('#{0}').data('kendoGrid').select().find('input').val('').change();", elementId);
            ((IJavaScriptExecutor)driver).ExecuteScript(Clearscript);

            //Update value
            var Updatescript = String.Format("$('#{0}').data('kendoGrid').select().find('input').val('{1}').change();", elementId, CellValue);
            ((IJavaScriptExecutor)driver).ExecuteScript(Updatescript);

            //close cell
            var Closescript = String.Format("$('#{0}').data('kendoGrid').closeCell();", elementId);            
            ((IJavaScriptExecutor)driver).ExecuteScript(Closescript);


        }

        public void CloseCell(IWebDriver driver)
        {
            var elementId = _element.GetAttribute("id");
            var script = String.Format("$('#{0}').data('kendoGrid').closeCell();", elementId);            
            ((IJavaScriptExecutor)driver).ExecuteScript(script);

        }

        public void SaveRow(IWebDriver driver)
        {
            var elementId = _element.GetAttribute("id");
            var script = String.Format("$('#{0}').data('kendoGrid').saveRow();", elementId);
            ((IJavaScriptExecutor)driver).ExecuteScript(script);
        }

        public void CancelRow(IWebDriver driver)
        {
            var elementId = _element.GetAttribute("id");
            ((IJavaScriptExecutor)driver).ExecuteScript(String.Format("$('#{0}').data('kendoGrid').cancelRow();", elementId));
        }

        public void GridDblclick(IWebDriver driver, int index)
        {
            var elementId = _element.GetAttribute("id");
            var element = _element.FindElement(By.XPath(String.Format("//*[@id='{0}']/div[3]/table/tbody/tr[1]", elementId, index)));
            Actions action = new Actions(driver);
            action.DoubleClick(element);
            action.Perform();
        }

        public void selectRegninger(IWebDriver driver, int index)
        {
            var elementId = _element.GetAttribute("id");
            var element = _element.FindElement(By.XPath(String.Format(" //*[@id='{0}']/div[3]/table/tbody/tr[1]", elementId, index)));
            Actions action = new Actions(driver);
            action.Click(element);
            action.Perform();
        }
        public void selectBundter(IWebDriver driver, int index)
        {
            var elementId = _element.GetAttribute("id");
            var element = _element.FindElement(By.XPath(String.Format("//*[@id='{0}']/div[3]/table/tbody/tr[{1}]", elementId, index)));
            Actions action = new Actions(driver);
            action.Click(element);
            action.Perform();
        }

        public void selectReguleringer(IWebDriver driver, int index)
        {
            var elementId = _element.GetAttribute("id");
            var element = _element.FindElement(By.XPath(String.Format("//*[@id='{0}']/div[3]/table/tbody/tr[{1}]", elementId, index)));
            Actions action = new Actions(driver);
            action.Click(element);
            action.Perform();
        }

        public void EditBatchCell(IWebDriver driver, int rowindex, int columnindex)
        {
            var elementId = _element.GetAttribute("id");                   
            var script = string.Format("$('#{0}').data('kendoGrid').editCell($('#{0}').data('kendoGrid').tbody.find('tr').eq({1}).find('td').eq({2}));", elementId, rowindex, columnindex);           
            ((IJavaScriptExecutor)driver).ExecuteScript(script);
        }

        public void clearSelection(IWebDriver driver)
        {
            var elementId = _element.GetAttribute("id");            
            var script = string.Format("$('#{0}').data('kendoGrid').clearSelection();", elementId);            
            ((IJavaScriptExecutor)driver).ExecuteScript(script);
        }

        public void SelfServiceGridDblclick(IWebDriver driver, int index)
        {
            var elementId = _element.GetAttribute("id");
            var element = _element.FindElement(By.XPath(String.Format("//*[@id='{0}']/div[3]/div[1]/table/tbody/tr[1]", elementId, index)));
            Actions action = new Actions(driver);
            action.DoubleClick(element);
            action.Perform();
        }
        public void SelfServiceGridSelect(IWebDriver driver, int index)
        {
            var elementId = _element.GetAttribute("id");
            var element = _element.FindElement(By.XPath(String.Format("//*[@id='{0}']/div[3]/div[1]/table/tbody/tr[1]", elementId, index)));
            Actions action = new Actions(driver);
            action.Click(element);
            action.Perform();
        }

        public void ClickCheckBox(int Row, int Column)
        {
            var index = 0;
            var Columnindex = 0;

            foreach (var row in Rows)
            {
                index += 1;

                if (index == Row)
                {
                    foreach (var col in row.FindElements(By.XPath(".//td[not(@style='display:none')]")))
                    {
                        Columnindex += 1;
                        if (Columnindex == Column)
                        {
                            var _element = new Checkbox(col.FindElement(By.XPath(".//input[@type='checkbox']")));
                            if (!_element.Checked)
                            {
                                _element.Click();
                                break;
                            }
                        }
                    }
                    break;
                }

            }

        }

        public void ClickCheckBox(int Row, string ColumnName)
        {
               
            var index = 0;
            int ColumnCounter = 0;
            int ColumnIndex = GetColumnIndex(ColumnName);

            foreach (var row in Rows)
            {
                index += 1;

                if (index == Row)
                {
                    foreach (var col in row.FindElements(By.XPath(".//td[not(@style='display:none')]")))
                    {
                        ColumnCounter += 1;
                        if (ColumnCounter == ColumnIndex)
                        {
                            var _element = new Checkbox(col.FindElement(By.XPath(".//input[@type='checkbox']")));
                            if (!_element.Checked)
                            {
                                _element.Click();
                                break;
                            }
                        }
                    }
                    break;
                }

            }

        }
        public String GetCellText(int Row, int Column)
        {
            var index = 0;
            var Columnindex = 0;

            string cellText = "";


            foreach (var row in Rows)
            {
                index += 1;

                if (index == Row)
                {
                    foreach (var col in row.FindElements(By.XPath(".//td[not(@style='display:none')]")))
                    {
                        Columnindex += 1;
                        if (Columnindex == Column)
                        {
                            cellText = col.Text;
                            break;
                        }
                    }
                    break;
                }

            }

            return cellText;
        }

        public void ClickCheckBox1(int Row, int Column)
        {
            var index = 0;
            var Columnindex = 0;

            foreach (var row in Rows)
            {
                index += 1;

                if (index == Row)
                {
                    foreach (var col in row.FindElements(By.XPath(".//td[not(@style='display:none')]")))
                    {
                        Columnindex += 1;
                        if (Columnindex == Column)
                        {
                            var _element = new Checkbox(col.FindElement(By.XPath(".//input[@type='checkbox']")));
                            if (!_element.Checked)
                            {
                                _element.Click();
                                break;
                            }
                        }
                    }
                    break;
                }

            }

        }

        public void ClickCheckBox1(int Row, string ColumnName)
        {

            var index = 0;
            int ColumnCounter = 0;
            int ColumnIndex = GetColumnIndex(ColumnName);

            foreach (var row in Rows)
            {
                index += 1;

                if (index == Row)
                {
                    foreach (var col in row.FindElements(By.XPath(".//td[not(@style='display:none')]")))
                    {
                        ColumnCounter += 1;
                        if (ColumnCounter == ColumnIndex)
                        {
                            var _element = new Checkbox(col.FindElement(By.XPath(".//input[@type='checkbox']")));
                            if (!_element.Checked)
                            {
                                _element.Click();
                                break;
                            }
                        }
                    }
                    break;
                }

            }

        }

        public String GetCellText(int Row, string ColumnName)
        {
            var index = 0;
            int ColumnCounter = 0;
            int ColumnIndex = GetColumnIndex(ColumnName);

            string cellText = "";

            

            foreach (var row in Rows)
            {
                index += 1;

                if (index == Row)
                {
                    foreach (var col in row.FindElements(By.XPath(".//td[not(@style='display:none')]")))
                    {
                        ColumnCounter += 1;
                        if (ColumnCounter == ColumnIndex)
                        {
                            cellText = col.Text;
                            break;
                        }
                    }
                    break;
                }

            }

            return cellText;
        }

        //If the cell has a link, this method could click it.
        public void ClickLink(int Row, string ColumnName)
        {
            var index = 0;
            int ColumnCounter = 0;
            int ColumnIndex = GetColumnIndex(ColumnName);            


            foreach (var row in Rows)
            {
                index += 1;

                if (index == Row)
                {
                    foreach (var col in row.FindElements(By.XPath(".//td[not(@style='display:none')]/a")))
                    {
                        ColumnCounter += 1;
                        if (ColumnCounter == ColumnIndex)
                        {                            
                            col.Click();
                            break;
                        }
                    }
                    break;
                }

            }

            
        }
        
        //If the cell has a link, this method could click it.
        public void ClickLink(int Row, int Column)
        {
            var index = 0;
            var Columnindex = 0;            


            foreach (var row in Rows)
            {
                index += 1;

                if (index == Row)
                {
                    foreach (var col in row.FindElements(By.XPath(".//td[not(@style='display:none')]/a")))
                    {
                        Columnindex += 1;
                        if (Columnindex == Column)
                        {
                            col.Click();
                            break;
                        }
                    }
                    break;
                }

            }

            
        }

        public void Selectcheckbox(int Row, int Column, bool check)
        {
            var index = 0;
            var Columnindex = 0;


            foreach (var row in Rows)
            {
                index += 1;

                if (index == Row)
                {
                    //foreach (var col in row.FindElements(By.XPath("//*[@type = 'checkbox' ]")))
                    
                    foreach (var col in row.FindElements(By.XPath(".//td[not(@style='display:none')]")))
                    {
                        Columnindex += 1;
                        if (Columnindex == Column)
                        {
                            var checkbox = new Checkbox(col.FindElement(By.XPath(".//*[@type='checkbox']")));
                            if (check && !checkbox.Selected) checkbox.Click();                                                        
                            break;
                        }                       
                    }
                    break;
                }

            }
        }

        public void Selectcheckbox(int Row, string ColumnName, bool check)
        {
            var index = 0;
            var Columnindex = 0;
            int ColumnCounter = GetColumnIndex(ColumnName);

            foreach (var row in Rows)
            {
                index += 1;

                if (index == Row)
                {
                    //foreach (var col in row.FindElements(By.XPath("//*[@type = 'checkbox' ]")))

                    foreach (var col in row.FindElements(By.XPath(".//td[not(@style='display:none')]")))
                    {
                        Columnindex += 1;
                        if (Columnindex == ColumnCounter)
                        {
                            var checkbox = new Checkbox(col.FindElement(By.XPath(".//*[@type='checkbox']")));
                            if (check && !checkbox.Selected) checkbox.Click();
                            break;
                        }
                    }
                    break;
                }

            }
        }

        //Checks whether the checkbox is selected/not. True = selected, False = unselected.
        public bool IsCellChecked(int Row, string ColumnName)
        {
            var index = 0;
            var Columnindex = 0;
            int ColumnCounter = GetColumnIndex(ColumnName);
            bool IsCellChecked = false;

            foreach (var row in Rows)
            {
                index += 1;

                if (index == Row)
                {
                    //foreach (var col in row.FindElements(By.XPath("//*[@type = 'checkbox' ]")))
                    foreach (var col in row.FindElements(By.XPath(".//td[not(@style='display:none')]")))
                    {
                        Columnindex += 1;
                        if (Columnindex == ColumnCounter)
                        {
                            var checkbox = new Checkbox(col.FindElement(By.XPath(".//*[@type='checkbox']")));
                            IsCellChecked = checkbox.Selected;
                            break;
                        }
                    }
                    break;
                }                
            }
            return IsCellChecked;
        }

        public int GetColumnIndex(String ColumnName)
        {
            int index = 0;

            /* Below code was selecting all columns which include hidden columns. 
             * This causes trouble when other function like GetRowIndexBy searched only displayed columns.
             */
            //var columns = _element.FindElements(By.CssSelector("div.k-grid-header > div > table > thead > tr > th"));

            var columns = _element.FindElements(By.XPath(".//table/thead/tr/th[not(@style='display:none')]"));

            foreach(var column in columns)
            {
                if (column.Text.Equals(ColumnName, StringComparison.OrdinalIgnoreCase))
                {
                    index += 1;                    
                    break;
                }

                index += 1;
            }

            return index;
        }

        public int GetRowIndexBy(String ColumnName, String RowText)
        {
            var index = 0;
            var rowindex = 0;

            int columnIndex = GetColumnIndex(ColumnName);


            foreach (var row in Rows)
            {
                index += 1;
                var columns = row.FindElements(By.XPath(".//td[not(@style='display:none')]"));                

                if (columns[columnIndex-1].Text.Equals(RowText, StringComparison.OrdinalIgnoreCase))
                {
                    rowindex = index;
                    break;
                }

            }

            return rowindex;
        }


        public int DeleteRowIndexBy(String ColumnName, String RowText)
        {
            /*
             * Summary : This method is designed to delete a row.           
             */

            var index = 0;
            var rowindex = 0;

            int columnIndex = GetColumnIndex(ColumnName);


            foreach (var row in Rows)
            {
                index += 1;
                var columns = row.FindElements(By.XPath(".//td[not(@style='display:none')]"));

                if (columns[columnIndex - 1].Text.Equals(RowText, StringComparison.OrdinalIgnoreCase))
                {
                    rowindex = index;
                    var delete = row.FindElement(By.XPath(".//td[not(@style='display:none')]//span[@class='k-icon k-delete']"));
                    delete.Click(); //Delete row
                    break;
                }

            }

            return rowindex;
        }

        public bool DeleteRowIndexBy(int RowIndex)
        {
            /* Summary : This method is designed to delete a row.  Return true if completed, else return false.         
             */
            try
            {            
               var delete = _element.FindElement(By.XPath(".//tr["+RowIndex.ToString()+"]//td[not(@style='display:none')]//span[@class='k-icon k-delete']"));
               delete.Click(); //Delete row
               return true;
            }
            catch (Exception)
            {
               return false;
            }
            
        }

        public int DeleteRolle(String ColumnName, String RowText)
        {
            return DeleteRowIndexBy(ColumnName, RowText);
        }

        public int DeleteDaekningsOmrade(String ColumnName, String RowText)
        {
            return DeleteRowIndexBy(ColumnName, RowText);
        }

        public int DeleteRettighed(String ColumnName, String RowText)
        {
            return DeleteRowIndexBy(ColumnName, RowText);
        }

        //public int DeleteGodkendyderaendringd(String ColumnName, String RowText)
        //{
        //    return DeleteRowIndexBy(ColumnName, RowText);
        //}

        //public int DeleteOpretfaellesskaber(String ColumnName, String RowText)
        //{
        //    return DeleteRowIndexBy(ColumnName, RowText);
        //}

        //public int DeleteRettighed(String ColumnName, String RowText)
        //{
        //    return DeleteRowIndexBy(ColumnName, RowText);
        //}
        public int DeleteMyndighedsansvar(String ColumnName, String RowText)
        {
            return DeleteRowIndexBy(ColumnName, RowText);
        }
        public int DeleteTeam(String ColumnName, String RowText)
        {
            return DeleteRowIndexBy(ColumnName, RowText);
        }

        public void ClickCell(int row, int column)
        {
            var _cell = _element.FindElement(By.XPath(".//table/tbody/tr[not(@style='display:none')][" + row.ToString() + "]//td[not(@style='display:none')][" + column.ToString() + "]"));

            _cell.Click();

        }

        public int GetRowIndexBy(int ColumnIndex, String RowText)
        {
            var index = 0;
            var rowindex = 0;
            
            foreach (var row in Rows)
            {
                index += 1;
                var columns = row.FindElements(By.XPath(".//td[not(@style='display:none')]"));

                if (columns[ColumnIndex-1].Text.Equals(RowText, StringComparison.OrdinalIgnoreCase))
                {
                    rowindex = index;
                    break;
                }

            }

            return rowindex;
        }

        public bool QuickSearch(string columnName, string rowText)
        {
            //Search the grid if the row is present within the column.
            try
            {
                int columnIndex = GetColumnIndex(columnName);
                var row = _element.FindElement(By.XPath(".//table//tbody//tr/td["+columnIndex.ToString()+"]/span[@text()='" + rowText+"']"));
                return true;
            }
            catch(Exception)
            {
                return false;
            }

        }

    }
}
