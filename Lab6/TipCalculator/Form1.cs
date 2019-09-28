using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TipCalculator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            

        }

        private void buttonForTip_Click(object sender, EventArgs e)
        {
            string bill = textBoxEnterBill.Text;
            double.TryParse(bill, out double billAmount);


            //calculate the tip
            double tip = billAmount * 0.2;
            textBox2TipsCalculated.Text = "" + tip;



        }

        private string calculatedTip()
        {
            string bill = textBoxEnterBill.Text;
            if (double.TryParse(bill, out double billAmount))
            {
                buttonForTip.Enabled = false;
            }
            else
                buttonForTip.Enabled = true;

            //calculate the tip
            double tip = billAmount * 0.2;
           return "" + tip;
        }

        private void textBox2TipsCalculated_TextChanged(object sender, EventArgs e)
        {
            string tip = calculatedTip();

            textBox2TipsCalculated.Text = tip;
        }
    }
}
