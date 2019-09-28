using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadsheetUtilities;
using System.Text.RegularExpressions;
using System.Xml;


namespace SS
{//
    /// <summary>
    /// An AbstractSpreadsheet object represents the state of a simple spreadsheet.  A 
    /// spreadsheet consists of an infinite number of named cells.
    /// 
    /// A string is a valid cell name if and only if:
    ///   (1) its first character is an underscore or a letter
    ///   (2) its remaining characters (if any) are underscores and/or letters and/or digits
    /// Note that this is the same as the definition of valid variable from the PS3 Formula class.
    /// 
    /// For example, "x", "_", "x2", "y_15", and "___" are all valid cell  names, but
    /// "25", "2x", and "&" are not.  Cell names are case sensitive, so "x" and "X" are
    /// different cell names.
    /// 
    /// A spreadsheet contains a cell corresponding to every possible cell name.  (This
    /// means that a spreadsheet contains an infinite number of cells.)  In addition to 
    /// a name, each cell has a contents and a value.  The distinction is important.
    /// 
    /// The contents of a cell can be (1) a string, (2) a double, or (3) a Formula.  If the
    /// contents is an empty string, we say that the cell is empty.  (By analogy, the contents
    /// of a cell in Excel is what is displayed on the editing line when the cell is selected.)
    /// 
    /// In a new spreadsheet, the contents of every cell is the empty string.
    ///  
    /// The value of a cell can be (1) a string, (2) a double, or (3) a FormulaError.  
    /// (By analogy, the value of an Excel cell is what is displayed in that cell's position
    /// in the grid.)
    /// 
    /// If a cell's contents is a string, its value is that string.
    /// 
    /// If a cell's contents is a double, its value is that double.
    /// 
    /// If a cell's contents is a Formula, its value is either a double or a FormulaError,
    /// as reported by the Evaluate method of the Formula class.  The value of a Formula,
    /// of course, can depend on the values of variables.  The value of a variable is the 
    /// value of the spreadsheet cell it names (if that cell's value is a double) or 
    /// is undefined (otherwise).
    /// 
    /// Spreadsheets are never allowed to contain a combination of Formulas that establish
    /// a circular dependency.  A circular dependency exists when a cell depends on itself.
    /// For example, suppose that A1 contains B1*2, B1 contains C1*2, and C1 contains A1*2.
    /// A1 depends on B1, which depends on C1, which depends on A1.  That's a circular
    /// dependency.
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {
        
        private DependencyGraph DG;
        private Dictionary<string, Cell> Cells;
       
        public Spreadsheet () : base (x => true, x => x, "default")
        {

            //declaring a dependency graph.
            DG = new DependencyGraph();
            //declaring a cell from the cells class
            Cells = new Dictionary<string, Cell>();
            Changed = false;
        }

        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version) : base(x => true, x => x, version)
        {
            //declaring a dependency graph.
            DG = new DependencyGraph(); 
            //declarign a cell from the cells class.
            Cells = new Dictionary<string, Cell>();
            //declaring is Valid and Normalize
            this.IsValid = isValid;
            this.Normalize = normalize;
            this.Version = version;
            Changed = false;
        }

        public Spreadsheet(string path, Func<string, bool> isValid, Func<string, string> normalize, string version) : base(x => true, x => x, version)
        {
            //declaring a dependency graph.
            DG = new DependencyGraph();
            //declarign a cell from the cells class.
            Cells = new Dictionary<string, Cell>();
            //declaring is Valid and Normalize
            this.IsValid = isValid;
            this.Normalize = normalize;
            this.Version = version;
            Changed = false;

            try
            {   //reading a file from the path
                using (XmlReader reader = XmlReader.Create(path))
                {
                    while (reader.Read())
                    {   //if start element is correct.
                        if (reader.IsStartElement())
                        {   //if spreadsheet is read.
                            if (reader.Name == "spreadsheet")
                            {   //set version to reader version.
                                version = reader["version"];
                            }
                            //if cell is read.
                            if (reader.Name == "cell")
                            {
                                //if reader Name is not equal to name.
                                while (!reader.Name.Equals("name"))
                                {
                                    reader.Read();
                                }
                                reader.Read();
                                //storing value in name.
                                string name = reader.Value;
                                while (!reader.Name.Equals("contents"))
                                {

                                    reader.Read();
                                }
                                reader.Read();
                                //set value in content.
                                string content = reader.Value;

                                //rerunning setCentent of cell on name and content.
                                SetContentsOfCell(name, content);
                            }
                        }
                    }
                }
            }
          
            catch (Exception)
            {
                throw new SpreadsheetReadWriteException("error");
            }

            //if version doesnt match throw an exception.
            if (!GetSavedVersion(path).Equals(Version))
            {
                throw new SpreadsheetReadWriteException("the version do not match");
            }
        }
        public IEnumerable<String> Remove(String name)
        {
            if (Cells.ContainsKey(name))
            {
                if (Cells[name] != null)
                {
                    HashSet<string> temp = new HashSet<string>();

                    //replacing all dependents of data with empty dependents
                    DG.ReplaceDependents(name, temp);
                    IEnumerable<string> recalculation = (GetCellsToRecalculate(name));
                    Cell cell = new Cell(name);
                    //setting contents to number.
                    cell.setContent("");
                    //storing cell with data and number in dictionary.
                    Cells[name] = cell;

                    //if set content is sucessfull setting changed to true.
                    this.Changed = true;
                    //looping through and using a calculateValue method to calculate string.
                    foreach (string a in recalculation)
                    {
                        calculateValue(a);
                    }
                    //return the recalculated value.
                    return new HashSet<string>(recalculation);
                }
            }
            return new HashSet<String>();
        }
     
        // ADDED FOR PS5
        /// <summary> spreadsheet has been modified since it was created or saved                  
        /// (whichever happened most recently); false otherwise.
        /// </summary>
        public override bool Changed { get; protected set; }


        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        public override object GetCellContents(string name)
        {
            //creating a new cell with string name as a parameter.
            Cell cell = new Cell(name);
            
            //if name is null we throw Invalidname Exception.
            if(name == null)
            {
                throw new InvalidNameException();
            }
            //if name is not in corent format we throw invalidNameException.
            if(!Regex.IsMatch(name, @"^[a-zA-Z]+\d+$"))
            {                                    
                throw new InvalidNameException();
            }
            //if no exception is thrown we get the cells content and return it.
            string normalized = isCorrect(name);
            if (Cells.ContainsKey(normalized))
            {
              
                    return Cells[normalized].getContent();
                
            }

            //if cell was not present we return a blank string.
            return "";
        }

        // ADDED FOR PS5
        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the value (as opposed to the contents) of the named cell.  The return
        /// value should be either a string, a double, or a SpreadsheetUtilities.FormulaError.
        /// </summary>
        public override object GetCellValue(string name)
        {
            //if name is null of regex doesnt match throw an exception.
            if(name == null || !Regex.IsMatch(name, @"^[a-zA-Z]+\d+$"))
            {
                throw new InvalidNameException();
            }
            //if cells key is name return empty string.
            if (!Cells.ContainsKey(name))
            {
                return "";
            }
            //if content for the given name is formulaError return formulaError.
            if(Cells[name].getContent() is FormulaError)
            {
                return new FormulaError("Incorrect Formula");
            }
            //return value of the given cell name.
            return Cells[name].getValue();
           
        }

        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            //created a list that can be returned with all the nonEmptyCells.
           List<string> list = new List<string>();

            //loops through all the Cells keys.
           foreach(string a in Cells.Keys)
            {
                //if Cells[a] is not a string we add it to the list as it
                //should be either a double or a formula.
                if (!(Cells[a].getContent() is String))
                {
                    list.Add(a);  
                }         
                //if Cell[a] is a string we check if it is not a "" and add to the list.
               else if (((string)Cells[a].getContent() != ""))
                {
                    list.Add(a);
                }
            }
           //return the list with non empty list of cells name.
            return list;
        }

        // ADDED FOR PS5
        /// <summary>
        /// Returns the version information of the spreadsheet saved in the named file.
        /// If there are any problems opening, reading, or closing the file, the method
        /// should throw a SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        public override string GetSavedVersion(string filename)
        {
            //if filename is null and filename is empty string throw exception.
            if (filename == null || filename == ""){
                throw new SpreadsheetReadWriteException("Given file name is invalid");
            }

            string result = "";
            try
            {   //creating a xml file with a filename.
                using (XmlReader read = XmlReader.Create(filename))
                {
                    read.Read();

                    //reading the startElement.
                    if (read.IsStartElement())
                    {
                        //if name reads spreadsheet
                        if (read.Name.Equals("spreadsheet"))

                        { 
                            //get the version and return the version.
                            result = read.GetAttribute("version");
                            read.Close();
                            return result;
                        }
                    }

                }
            }
            catch
            {
                throw new SpreadsheetReadWriteException("Did not exist");
            }
            return result;
              

               
        }

        // ADDED FOR PS5
        /// <summary>
        /// Writes the contents of this spreadsheet to the named file using an XML format.
        /// The XML elements should be structured as follows:
        /// 
        /// <spreadsheet version="version information goes here">
        /// 
        /// <cell>
        /// <name>
        /// cell name goes here
        /// </name>
        /// <contents>
        /// cell contents goes here
        /// </contents>    
        /// </cell>
        /// 
        /// </spreadsheet>
        /// 
        /// There should be one cell element for each non-empty cell in the spreadsheet.  
        /// If the cell contains a string, it should be written as the contents.  
        /// If the cell contains a double d, d.ToString() should be written as the contents.  
        /// If the cell contains a Formula f, f.ToString() with "=" prepended should be written as the contents.
        /// 
        /// If there are any problems opening, writing, or closing the file, the method should throw a
        /// SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        public override void Save(string filename)
        {
            //if filename is null and if filename is empty string throw exception.
            if(filename == null || filename =="")
            {
                throw new SpreadsheetReadWriteException("Given file name is invalid");
            }
            try
            {
                //use of xml settings to get an indent.
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;
                settings.Indent = true;
                settings.IndentChars = ("  ");

                //Xml class used to create XML file
                using (XmlWriter writer = XmlWriter.Create(filename, settings))
                {
                    writer.WriteStartDocument();
                    //writing the title
                    writer.WriteStartElement("spreadsheet");
                    //writing the version.
                    writer.WriteAttributeString("version", Version);
                    //looping through the cell.Values to convert it to XML file
                    foreach (Cell c in Cells.Values)
                    {
                        //writing Xml cell block.
                        writer.WriteStartElement("cell");
                        writer.WriteElementString("name", c.getName().ToString());
                        if (c.getContent() is string)
                        {
                            //if getContent is string write content.
                            writer.WriteElementString("contents", (string)c.getContent());
                        }
                        if (c.getContent() is double)
                        {
                            //if getContnt is double write double to XMl.
                            writer.WriteElementString("contents", c.getContent().ToString());
                        }
                        if (c.getContent() is Formula)
                        {
                            //if getContent is a Formula write formula to Xml.
                            writer.WriteElementString("contents", "=" + c.getContent().ToString());

                        }
                        //ending writer.
                        writer.WriteFullEndElement();

                    }
                    writer.WriteEndElement();
                    writer.WriteEndDocument();

                }
                //setting changed to false.
                this.Changed = false;
            }
            catch
            {
                throw new SpreadsheetReadWriteException("Error thrown");
            }
           
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes number.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        protected override ISet<string> SetCellContents(string name, double number)
        {
            //if name is null we throw Invalidname Exception.
          
                HashSet<string> temp = new HashSet<string>();
                //replacing all dependents of data with empty dependents
                DG.ReplaceDependents(name, temp);
                IEnumerable<string> recalculation = (GetCellsToRecalculate(name));
                //Created a new cell with name. 
                Cell cell = new Cell(name);
                //setting contents to number.
                cell.setContent(number);
                //storing cell with data and number in dictionary.
                Cells[name] = cell;
                //recalculating the name and storing it in result.

                //if set content is sucessfull setting changed to true.
                this.Changed = true;
                //looping through and using a calculateValue method to calculate string.
                foreach (string a in recalculation)
                {
                    calculateValue(a);
                }
                //return the recalculated value.
                return new HashSet<string>(recalculation);
            
     
           
        }

        /// <summary>
        /// If text is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes text.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        protected override ISet<string> SetCellContents(string name, string text)
        {
           
                //empty hashset  
                HashSet<string> temp = new HashSet<string>();

                //replacing all dependents of data with empty dependents
                DG.ReplaceDependents(name, temp);
                IEnumerable<string> recalculation = (GetCellsToRecalculate(name));
                Cell cell = new Cell(name);
                //setting contents to number.
                cell.setContent(text);
                //storing cell with data and number in dictionary.
                Cells[name] = cell;
               
                //if set content is sucessfull setting changed to true.
                this.Changed = true;
                //looping through and using a calculateValue method to calculate string.
                foreach (string a in recalculation)
                {
                    calculateValue(a);
                }
                //return the recalculated value.
                return new HashSet<string>(recalculation);

            
            //when the text is empty string return a empty hashset.
           
        }

        /// <summary>
        /// If the formula parameter is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if changing the contents of the named cell to be the formula would cause a 
        /// circular dependency, throws a CircularException.  (No change is made to the spreadsheet.)
        /// 
        /// Otherwise, the contents of the named cell becomes formula.  The method returns a
        /// Set consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        protected override ISet<string> SetCellContents(string name, Formula formula)
        {
            
          
            //hashset with all the variables for formula hashset
            HashSet<string> temp = new HashSet<string>(formula.GetVariables());
            //replacing all dependents of data with empty dependents
            DG.ReplaceDependents(name, temp);
            IEnumerable<string> recalculation = (GetCellsToRecalculate(name));
            //creating a new cell with string name as a parameter.
            Cell cell = new Cell(name);
            //setting contents to number.
            cell.setContent(formula);
            //storing cell with data and number in dictionary.
            Cells[name] = cell;
            //recalculating the name and storing it in result.
            //if set content is sucessfull setting changed to true.
            this.Changed = true;
            //looping through and using a calculateValue method to calculate string.
            foreach (string a in recalculation)
            {
                calculateValue(a);
            }

            //return the recalculated value.
            return new HashSet<string>(recalculation);

        }

        /// <summary>
        /// Check for the valid name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string isCorrect(string name)
        {
            //normalized name and stored it back to name.
            name = Normalize(name);
            //checking if the regex is match.
            if (Regex.IsMatch(name, @"^[a-zA-Z]+\d+$"))
            {   
                //checking if name is Valid.
                if (IsValid(name))
                {
                    //return name.
                    return name;
                }
            }
            throw new InvalidNameException();
        }
        // ADDED FOR PS5
        /// <summary>
        /// If content is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if content parses as a double, the contents of the named
        /// cell becomes that double.
        /// 
        /// Otherwise, if content begins with the character '=', an attempt is made
        /// to parse the remainder of content into a Formula f using the Formula
        /// constructor.  There are then three possibilities:
        /// 
        ///   (1) If the remainder of content cannot be parsed into a Formula, a 
        ///       SpreadsheetUtilities.FormulaFormatException is thrown.
        ///       
        ///   (2) Otherwise, if changing the contents of the named cell to be f
        ///       would cause a circular dependency, a CircularException is thrown.
        ///       
        ///   (3) Otherwise, the contents of the named cell becomes f.
        /// 
        /// Otherwise, the contents of the named cell becomes content.
        /// 
        /// If an exception is not thrown, the method returns a set consisting of
        /// name plus the names of all other cells whose value depends, directly
        /// or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetContentsOfCell(string name, string content)
        {
            
            ISet<string> result = new HashSet<string>();
            //if content is null throw argumentException.
            if(content == null)
            {
                throw new ArgumentNullException();
            }
            //if name is null and if the name is not valid throw exception.
            if(name == null || !IsValid(name))
            {
                throw new InvalidNameException();
            }
            //if content is empty string return a hashset with the name.
            if(content == "")
            {
                return new HashSet<string>(new string[] { name });
            }
            //storinng the saved name from the isCorrect(name),
            string nameAfterNormalization = isCorrect(name);

            //if the content is a double SetCellContent.
            if(double.TryParse(content, out double contentIfParsed))
            {
                result = SetCellContents(nameAfterNormalization, contentIfParsed);
            }
            //if content begins with =, content is a formula.
            else if (beginsWithEqualsSign(content))
            {   
                //formulaFolowingEquals is formula excluding the preceding equals sign.
                string formulaFollowingEquals = content.Substring(1);

                //if content is formula normalize and validate.
                Formula f = new Formula(formulaFollowingEquals, this.Normalize, this.IsValid);
                result = SetCellContents(nameAfterNormalization, f);
                
                
            }
            else
            {
                //if content is string setCellContent.
              result =  SetCellContents(nameAfterNormalization, content);
            }

         
           return result;
        }

        /// <summary>
        ///caculate the value for cells.
        ///if formula use evaluate to make the values as a formula error or double.
        ///if double make it double
        ///if string make it a string.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>

        public object calculateValue(string name)
        {
            //if name is null of invalid throws exception.
            if(name == null || !IsValid(name))
            {
                throw new InvalidNameException();
            }
            //if the content of the given name is a formula.
            if (Cells[name].getContent() is Formula)
            {
                Formula f = new Formula(Cells[name].getContent().ToString());
                //Evaluate the formula and setValue to result, and use lookup.
                object result = (Cells[name].getContent() as Formula).Evaluate(LookUp);
                Cells[name].setValue(result);
                return result;
            }
            else if(Cells[name].getContent() is double)
            {
                //if content of the given name is a double return content.
                object result =  Cells[name].getContent();
                Cells[name].setValue(result);
                return result;
            }
            else
            {   //if content is not a double or a formula retuen a content of the formula as a string.
                object result = Cells[name].getContent();
                Cells[name].setValue(result);
                return result; 
            }

        }

        
        /// <summary>
        /// checks if the string begins with an equal sign 
        /// </summary>
        /// <param name="content"></param>
        /// <returns>bool</returns>
        private bool beginsWithEqualsSign(string content)
        {
            //takes the first char of the string.
            char a = content.First();
            
            //if first char is equal then return true else false.
            if (a == '=')
            {
                return true;
            }
            else
                return false;
        }

      
        /// <summary>
        /// LooksUp the value if rhe given string formula.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>double</returns>
        private double LookUp(string name)
        {
            //if name is null or empty string throws an exception.
            if(name == null || name == "" )
            {
                throw new ArgumentException();
            }
            //if cells contains key name we check if it is a double.
            if (Cells.ContainsKey(name))
            {
                if(Cells[name].getValue() is double)
                {
                    //store double result and return the value.
                    double result = (double)Cells[name].getValue();
                    return result;
                }
            }
            throw new ArgumentException();  
        }

    

        /// <summary>
        /// If name is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name isn't a valid cell name, throws an InvalidNameException.
        /// 
        /// Otherwise, returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell.  In other words, returns
        /// an enumeration, without duplicates, of the names of all cells that contain
        /// formulas containing name.
        /// 
        /// For example, suppose that
        /// A1 contains 3
        /// B1 contains the formula A1 * A1
        /// C1 contains the formula B1 + A1
        /// D1 contains the formula B1 - C1
        /// The direct dependents of A1 are B1 and C1
        /// </summary>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            HashSet<string> hs = new HashSet<string>();
            //if name is null we throw  Argumentnull Exception.
            if (name == null)
            {
                throw new ArgumentNullException();
            }
            //if name is not in corent format we throw invalidNameException.
            if (!Regex.IsMatch(name, @"^[a-zA-Z]+\d+$"))
            {
                throw new InvalidNameException();
            }

            return DG.GetDependees(name);
              
        }
    }
}
