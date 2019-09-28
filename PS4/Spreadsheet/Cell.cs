using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SpreadsheetUtilities
{
    class Cell
    {
       
        private object contents;
        private string name;
        private object value;

        /// <summary>
        ///Takes in name as a argument.
        ///cell constructor with private contents and name.
        /// </summary>
        /// <param name="name"></param>
        public Cell(string name)
        {     
            this.name = name;
        }

        /// <summary>
        /// Sets a content to the cell if required. Takes object content as
        /// an argument.
        /// </summary>
        /// <param name="content"></param>
        public void setContent(object content)
        {
            contents = content;
        }

        public void setValue(object value)
        {
            this.value = value;
        }

        public object getValue()
        {
            return this.value;
        }
        /// <summary>
        /// getContent return the content for the specific name.
        /// </summary>
        /// <returns>content</returns>
        public object getContent()
        {
            return this.contents;
        }

        /// <summary>
        /// Method setName is used to set name if required. Takes in 
        /// string n as a parameter and sets it.
        /// </summary>
        /// <param name="n"></param>
        public void setName(string n)
        {
            name = n;
        }

        /// <summary>
        /// Method getName return name of the specific cell.
        /// </summary>
        /// <returns>Name</returns>
        public object getName()
        {
            return this.name;
        }

    }
}
