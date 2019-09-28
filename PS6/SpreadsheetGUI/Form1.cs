using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpreadsheetUtilities;
using SS;

namespace SpreadsheetGUI
{

    /// <summary>
    /// This form1 class  handles all the backend development of the spreadsheet. It implements the spreadsheet class to be
    /// utilized and be displayed on the spreadsheet panel application. 
    /// Author: Ajay Bagali && Ritesh Sharma 
    /// </summary>
    public partial class Form1 : Form
    {
        //this member variable keeps track of the previous column that the user selected 
        private int prevCol;
        //this member variable keeps track of the previous column that the user selected 
        private int prevRow;
        //this string array holds the contents of the selected cell that was intended to copy 
        string[] contentsForCopyPaste = new string[1];
        //spreadsheet displayed on the form 
        Spreadsheet formSpreadsheet;

        /// <summary>
        /// This constructor simply makes a new form application
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            spreadsheetPanel1.SelectionChanged += OnSelectionChanged;
            //makes the new form with the default version
            formSpreadsheet = new Spreadsheet(s => Regex.IsMatch(s, @"[A-Z[1-9][0-9]?"), s => s.ToUpper(), "PS6");
        }

        /// <summary>
        /// This constructor is used when the user tries to open a previously saved spreadsheet, by taking in the file path that
        /// the user provides. 
        /// </summary>
        /// <param name="filepath"></param>
        public Form1(string filepath)
        {

            InitializeComponent();

            spreadsheetPanel1.SelectionChanged += OnSelectionChanged;
            //makes the form and loads the elements from the file path provided
            formSpreadsheet = new Spreadsheet(filepath, s => Regex.IsMatch(s, @"[A-Z[1-9][0-9]?"), s => s.ToUpper(), "PS6");

            //iterating through the existing cells in the spreadsheet
            foreach (string cellName in formSpreadsheet.GetNamesOfAllNonemptyCells())
            {
                //contains the cell name existing in the dictionary
                string converted = cellNameToInt(cellName);
                //getting out the first coordinate of the cell name 
                bool parsedFirst = int.TryParse(converted.First().ToString(), out int firstCoordinate);
                //getting out the second coordinates of the cell name 
                bool parsedSecond = int.TryParse(converted.Substring(1), out int secondCoordinate);
                if (parsedFirst && parsedSecond)
                {
                    //set the value one before for the proper coordinates in the display
                    spreadsheetPanel1.SetValue(firstCoordinate - 1, secondCoordinate - 1, formSpreadsheet.GetCellValue(cellName).ToString());
                    //set the value one before for the proper coordinates in the actual spreadsheet backend 
                    formSpreadsheet.SetContentsOfCell(asciiConverter(firstCoordinate - 1, secondCoordinate - 1), formSpreadsheet.GetCellContents(asciiConverter(firstCoordinate - 1, secondCoordinate - 1)).ToString());
                }
            }
            //getting the value of the very first cell using its coordinates 
            spreadsheetPanel1.GetValue(0, 0, out string value);
            //setting the contents text box
            contentsBox.Text = value;
        }

        /// <summary>
        /// this helper method converts the cell name to ints in terms of rows and columns
        /// ex: A3 = > 03
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string cellNameToInt(string name)
        {
            char first = name.First();
            //subtracting the name and adding a number to it 
            int val = first - 'A' + 1;

            //returns the name as a number
            return val + name.Substring(1);
        }

        /// <summary>
        /// this method loads the form application on the display
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            //focuses the cursor on the first cell text box 
            rowColumnBox.Text = asciiConverter(0, 0);
            //focuses the cursor on the first cell 
            this.ActiveControl = contentsBox;
        }
        /// <summary>
        /// this method gets the selected cell whenever the user goes to a different cell
        /// </summary>
        /// <param name="formSpreadsheet"></param>
        private static void OnSelectionChanged(SpreadsheetPanel formSpreadsheet)
        {
            int col;
            int row;
            //gets the column and row number of the currently selected cell 
            formSpreadsheet.GetSelection(out col, out row);

        }

        /// <summary>
        /// This method functions whenever the user changes something in the contents text box 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int col;
            int row;
            //gets the coordinates of the current selected cell 
            spreadsheetPanel1.GetSelection(out col, out row);
            //focuses on the content text box
            contentsBox.Focus();
            //automatically brings the cursor to the end of the text in the text box 
            contentsBox.SelectionStart = contentsBox.Text.Length;

            //sets the value to the cell of whatever was typed in the contents box
            spreadsheetPanel1.SetValue(col, row, contentsBox.Text);
            //storing the contents of the selected cell 
            object contents = formSpreadsheet.GetCellContents(asciiConverter(col, row));

            try
            {
                //if the contents box isnt a formula              
                if (!contentsBox.Text.StartsWith("="))
                {
                    //storing the dependent cells of that current selected cell in an list 
                    ISet<string> list = formSpreadsheet.SetContentsOfCell(asciiConverter(col, row), contentsBox.Text);
                    //if the content box is empty
                    if (contentsBox.Text == "")
                    {
                        //reevaluating the contents of all the cells that were removed   
                        reEvaluateValues((ISet<String>)formSpreadsheet.Remove(asciiConverter(col, row)));
                    }
                    //reevaluate the list again to evaluate all the current cells 
                    reEvaluateValues(list);
                }
                //if it is a formula 
                if (contentsBox.Text.StartsWith("="))
                {
                    //we make its font blue
                    contentsBox.ForeColor = Color.Blue;
                }
                else
                {
                    //any other contents is displayed in black 
                    contentsBox.ForeColor = Color.Black;
                }
            }

            catch (SpreadsheetUtilities.FormulaFormatException)
            {
                //display its contents as "Error" 
                spreadsheetPanel1.SetValue(col, row, "Error");
            }
            //displaying the current cell's value in the value box
            valueBox.Text = formSpreadsheet.GetCellValue(asciiConverter(col, row)).ToString();
            //setting the previous column coordinate to the new one 
            prevCol = col;
            //setting the previous row coordinate to the new one 
            prevRow = row;

        }


        /// <summary>
        /// this method functions when a different cell is selected
        /// </summary>
        /// <param name="sender"></param>
        private void spreadsheetPanel1_SelectionChanged(SpreadsheetPanel sender)
        {
            int col;
            int row;

            //focuses on the contents box 
            this.ActiveControl = contentsBox;

            spreadsheetPanel1.GetSelection(out col, out row);
            //displaying the coorinates box with the spreadsheet coordinates
            rowColumnBox.Text = asciiConverter(col, row);
            //storing the cell's contents
            object contents = formSpreadsheet.GetCellContents(asciiConverter(col, row));
            //evulate the contents of the previous cell
            evaluateFormula(prevCol, prevRow);
            //displays the contents of the cell in the box 
            printingContents(contents);

        }
        /// <summary>
        /// this method prints the cell's contents back to the contents box
        /// </summary>
        /// <param name="contents"></param>
        private void printingContents(object contents)
        {
            int col;
            int row;
            spreadsheetPanel1.GetSelection(out col, out row);
            //storing the contents of that cell
            object content = formSpreadsheet.GetCellContents(asciiConverter(col, row));

            //if the contents is a formula 
            if (content is Formula)
            {
                //prepend the string of the conents with an equal sign
                contentsBox.Text = "=" + contents.ToString();
                //storing the value
                string value = formSpreadsheet.GetCellValue(asciiConverter(col, row)).ToString();
                //setting the value of that cell
                spreadsheetPanel1.SetValue(col, row, value);

            }
            else
            {
                //anything other than a formula, we display it how it is on the contents box
                contentsBox.Text = contents.ToString();
            }

        }

        /// <summary>
        /// this method evaluates the formula stored in the contents of the selected cell
        /// </summary>
        /// <param name="cellColumn"></param>
        /// <param name="cellRow"></param>
        /// <returns></returns>
        private object evaluateFormula(int cellColumn, int cellRow)
        {
            //storing the empty string
            object result = "";
            //getting the value of the previous cell
            bool passed = spreadsheetPanel1.GetValue(prevCol, prevRow, out string prevContent);
            //storing the previous contents
            object previousContent = prevContent;


            ISet<string> list = new HashSet<string>();
            //if the previous contents was a formula
            if (prevContent.StartsWith("="))
            {
                //surround in a try catch to catch different exceptions
                try
                {
                    //getting the dependent cells and putting them in a list 
                    list = formSpreadsheet.SetContentsOfCell(asciiConverter(cellColumn, cellRow), prevContent);
                    //calculating the value of the selected cell
                    result = formSpreadsheet.calculateValue(asciiConverter(cellColumn, cellRow));
                    //evaluating all the cells in the given list 
                    reEvaluateValues(list);


                }
                //if a circular exception is thrown
                catch (CircularException)
                {
                    int col;
                    int row;
                    spreadsheetPanel1.GetSelection(out col, out row);
                    //reset the cells value as an empty cell 
                    spreadsheetPanel1.SetValue(col, row, "");
                    //showing the user the problem 
                    MessageBox.Show("Circular Exception");

                }
                //if a formula format exception is thrown
                catch (SpreadsheetUtilities.FormulaFormatException)
                {
                    int col;
                    int row;
                    spreadsheetPanel1.GetSelection(out col, out row);
                    //reset the cells value as an empty cell 
                    spreadsheetPanel1.SetValue(col, row, "");
                    //showing the user the problem 
                    MessageBox.Show("Invalid Formula!!! Please Enter a valid Formula.");
                }

            }
            //return the result of the evaluated cell
            return result;
        }
        /// <summary>
        /// this method takes in a list of cell names and it reevaluates the cells values
        /// </summary>
        /// <param name="List"></param>
        private void reEvaluateValues(ISet<string> List)
        {
            //iterating through the list
            foreach (string name in List)
            {
                //get the cells name as an integer
                string converted = cellNameToInt(name);
                //getting the first coordinate
                bool parsedFirst = int.TryParse(converted.First().ToString(), out int firstCoordinate);
                //getting the second coordinate 
                bool parsedSecond = int.TryParse(converted.Substring(1), out int secondCoordinate);
                //setting the new value of that selected cell 
                spreadsheetPanel1.SetValue(firstCoordinate - 1, secondCoordinate - 1, formSpreadsheet.GetCellValue(name).ToString());
                //if the value is a formula error
                if (formSpreadsheet.GetCellValue(name) is FormulaError)
                {
                    //display it as a formula error text
                    spreadsheetPanel1.SetValue(firstCoordinate - 1, secondCoordinate - 1, "FormulaError");
                }
            }
        }

        /// <summary>
        /// this method takes in the number coordinates and it converts it into the shown coordinates on the diplay
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        private string asciiConverter(int col, int row)
        {
            //using ascii to determine the values of the coordinates of the selected cells
            string ascii = char.ConvertFromUtf32('A' + col) + (row + 1);

            return ascii;

        }
        /// <summary>
        /// Function for when the user presses enter, when it evaluates the current selected cell 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contentsBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            int col;
            int row;
            //if the user presses the enter button
            if (e.KeyChar == 13)
            {
                //if it is not empty
                if (contentsBox.Text != "")
                {
                    spreadsheetPanel1.GetSelection(out col, out row);
                    //evualtes the cell and updates its value on the display 
                    evaluateFormula(prevCol, prevRow);

                }

            }


        }
        /// <summary>
        /// this method displays the coordinates of the current selected cell 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rowColumnBox_TextChanged(object sender, EventArgs e)
        {
            int col;
            int row;

            //dislays the current selected cell
            spreadsheetPanel1.GetSelection(out col, out row);

        }
        /// <summary>
        /// this handles all the arrow key functionality 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //gets the current selected cell
            spreadsheetPanel1.GetSelection(out int col, out int row);


            //if the user presses down
            if (e.KeyCode == Keys.Down)
            {
                //evaluates the current selected cell
                keysPressEvaluate(col, row + 1);
                e.Handled = true;
            }
            //if the user presses down
            if (e.KeyCode == Keys.Up)
            {
                //evaluates the current selected cell
                keysPressEvaluate(col, row - 1);
                e.Handled = true;
            }
            //if the user presses right
            if (e.KeyCode == Keys.Right)
            {
                //evaluates the current selected cell
                keysPressEvaluate(col + 1, row);
                e.Handled = true;
            }
            //if the user presses left
            if (e.KeyCode == Keys.Left)
            {
                //evaluates the current selected cell
                keysPressEvaluate(col - 1, row);
                e.Handled = true;
            }
            //if the user press control + c
            if (e.KeyCode == Keys.C && e.Modifiers == (Keys.Control))
            {
                //it stories the contents of the current selected cell
                contentsForCopyPaste[0] = contentsBox.Text;

            }
            //if the user presses ctrl + b
            if (e.KeyCode == Keys.B && e.Modifiers == Keys.Control)
            {
                //pastes the stored contents into the new cell 
                contentsBox.Text = contentsForCopyPaste[0];

            }
        }

        /// <summary>
        /// Method that evaluates the formula and sets the cell content when the Key.Down function
        /// is envoked.
        /// </summary>
        /// <param name="colKeys"></param>
        /// <param name="rowKeys"></param>
        private void keysPressEvaluate(int colKeys, int rowKeys)
        {
            //if keys, rows and columns are not less than 0 or more than 99.
            if (colKeys >= 0 && colKeys <= 25 && rowKeys >= 0 && rowKeys <= 98)
            {
                //convert the text into ascii.
                rowColumnBox.Text = asciiConverter(colKeys, rowKeys);
                //evaluate the formula.
                evaluateFormula(prevCol, prevRow);
                //set the selection in spreadsheet panel.
                spreadsheetPanel1.SetSelection(colKeys, rowKeys);

                object contents = formSpreadsheet.GetCellContents(asciiConverter(colKeys, rowKeys));
                //printing everthing in order using printingContents methods.
                printingContents(contents);

            }
        }

        /// <summary>
        /// Method that envokes a new item from the file strip.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //opens up a new Form.
            FormApplication.getAppContext().RunForm(new Form1());

        }

        /// <summary>
        /// Method that enoveked the saving event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //opens up the save dialof
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "sprd files (*.sprt|*.sprd|All files (*.*)|*.*";
            //saves file in .sprd format in XML file.
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                formSpreadsheet.Save(dialog.FileName);
            }
        }

        /// <summary>
        /// Method that opens file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //opens up an open dialog, prompts the user the choose destination, give it a name and open the folder.
            OpenFileDialog openDialog = new OpenFileDialog();
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                FormApplication.getAppContext().RunForm(new Form1(openDialog.FileName));
            }

        }

        /// <summary>
        /// This method envokes the closing event from the file strip.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if the spreadsheet is change we promt the user.
            if (formSpreadsheet.Changed is true)
            {
                //if the response is YEs we take the user the save option.
                if (MessageBox.Show("Do you want to save your work ", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    saveToolStripMenuItem_Click(sender, e);
                }
                Close();
                //if the response is no we close the program.
            }
            else
            {
                Close();
            }
        }

        /// <summary>
        /// Method that envokes the spreadsheet closing event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if changed is true we prompt the user.
            if (formSpreadsheet.Changed is true)
            {
                //if yes we save the file, if not close the program without saving.
                if (MessageBox.Show("Do you want to save your work ", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    saveToolStripMenuItem_Click(sender, e);
                }
            }
        }
        
        /// <summary>
        /// Method that invokes the button click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            int col;
            int row;

            spreadsheetPanel1.GetSelection(out col, out row);
            //if the button is clicked we evaluate the formula.
            evaluateFormula(col, row);

        }

        /// <summary>
        /// Method that invokes the clear method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if changed is true we prompt the user. 
            if (formSpreadsheet.Changed is true)
            {
                if (MessageBox.Show("Are you sure you want to clear? ", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ///if the user answers yer we clear the spreadsheet.
                    spreadsheetPanel1.Clear();
                    formSpreadsheet = new Spreadsheet();
                    contentsBox.Text = "";
                }

            }
        }

        /// <summary>
        /// This function is for the textBox that contains a value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            int col;
            int row;
            spreadsheetPanel1.GetSelection(out col, out row);
          
        }
        /// <summary>
        /// Help menu strip that shows all the features and specification for spreadsheet.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            MessageBox.Show("Features:\n"+"    1.Our spreadsheet allows the user to navigate using the " +
                "arrow keys on their keyboard. Another additonal feature that these arrow keys do is that " +
                "once the user leaves the selected cell, it evaluates the contents of that cell.\n" +
                 "    2.In the File menu of our spreadsheet, we added in an additional option that clears cells" +
                   " of the current spreadsheet. It also asks the user to save any" +
                   "work that existed in that current spreadsheet.\n" +
                  "    3.If the contents of that cell is a formula, we make the color of the formula in blue.\n" +"    4. Our spreadsheet " +
                  "allows copy and pasting contents of the cell without selecting the value. For copy and paste, press Ctrl + c " +
                  "in the cell you want to copy dont worry about selecting the value and Ctrl + B, this will copy and paste the value " +
                  "without selecting the content.\n " +
                  "Special Insructions:\n" + "    1.In our spreadsheet, the user doesn't have to press enter " +
                   "each time for the contents to be stored/calculated. The user can direcectly go to " +
                   "another cell by either selecting a new cell or by using the arrow keys.\n" +
                  "    2.The user can also type the contents of the selected cell in the contents box" +
                  " right underneath the menu bar.\n" +
                  "    3. The user can Copy/Paste using Ctrl + c(copy) and Ctrl + B(paste) without worrying about selecting the content.");
        }
    }
}
