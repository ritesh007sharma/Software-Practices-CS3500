
using System;
using System.Text;
using System.Windows.Forms;


namespace View

{

    public partial class Form1 : Form

    {
        //instance of our game controller
        private GameController.GameController gameController;
        //booleans for the user input controls
        private bool rightKey;
        private bool leftKey;
        private bool thrustKey;
        private bool fireKey;
        //refernce to the drawing panel to add on to the form
        private DrawingPanel.DrawingPanel panel;


        /// <summary>
        /// this constructor for the form, iniiialized all the form components and adds the panel on the form
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            //game controller referce
            gameController = new GameController.GameController();
            gameController.registerNetwork1(formNetwork);
            gameController.RegisterFrame(refreshPanel);
            gameController.RegisterEvent(append);
            gameController.RegisterClose(close);
            //making a panel for the world
            panel = new DrawingPanel.DrawingPanel(gameController.GetWorld());
            panel.Location = new System.Drawing.Point(0, 40);
            //defualting the panel size to the max, will be later adjusted to the user
            panel.Size = new System.Drawing.Size(2000, 2000);
            //adding control inputs to the panel
            this.Controls.Add(panel);
        }

        /// <summary>
        /// this method deals with invalid host names and resets the user input on the panel 
        /// </summary>
        private void formNetwork()
        {

            button1.Enabled = false;
            //displaying a message when there is a wrong server name 
            MessageBox.Show("Invalid host name. Please Enter a valid server name.");
            //we use a method invoker to reset all of the user buttons and text boxes to true 
            MethodInvoker invoke = new MethodInvoker(
               () => {
                   textBox1.Enabled = true;
                   button1.Enabled = true;
                   serverAddress.Enabled = true;
               }
               );
            this.Invoke(invoke);

        }

        /// <summary>
        /// this method resets the user input when the user closes out of the game
        /// </summary>
        private void close()
        {
            //this method invoker resets the buttons for when the user reopens the game 
            MethodInvoker invoke = new MethodInvoker(
                () => {
                    textBox1.Enabled = false;
                    button1.Enabled = false;
                    serverAddress.Enabled = false;
                }
                );
            this.Invoke(invoke);
        }
        /// <summary>
        /// this method auto updates when the drawing panel on the form is refreshed each time 
        /// </summary>
        private void refreshPanel()
        {
            //we call this method invoker to reset the form's validaty to true each time we refresh the form
            try
            {
                MethodInvoker invoker = new MethodInvoker(() => this.Invalidate(true));
                this.Invoke(invoker);
            }
            //if an exception exists when the form is refreshed
            catch (Exception e)
            {

            }
        }


        /// <summary>
        /// this method deals with when the user releases a key on the keyboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            //we set the booleans to false because the user has already finished giving us an input
            if (e.KeyCode == Keys.Right)
            {
                rightKey = false;
            }
            if (e.KeyCode == Keys.Left)
            {
                leftKey = false;
            }
            if (e.KeyCode == Keys.Space)
            {
                fireKey = false;
            }
            if (e.KeyCode == Keys.Up)
            {
                thrustKey = false;
            }

        }
        /// <summary>
        /// this method deals with when the user presses a key on the keyboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //we set the booleans to false because the user first presses a button to send an input to the server
            if (e.KeyCode == Keys.Right)
            {
                rightKey = true;
            }
            if (e.KeyCode == Keys.Left)
            {
                leftKey = true;
            }
            if (e.KeyCode == Keys.Space)
            {
                fireKey = true;
            }
            if (e.KeyCode == Keys.Up)
            {
                thrustKey = true;
            }


        }

        /// <summary>
        /// this method adds on the stringbuilder when there is new data to send to the server
        /// </summary>
        public void append()
        {
            //making a string builder to append to for later
            StringBuilder sb = new StringBuilder();
            //whenever the user gives the form an input, we append the control to the string
            if (rightKey == true)
            {
                sb.Append("R");
            }
            if (leftKey == true)
            {
                sb.Append("L");
            }
            if (thrustKey == true)
            {
                sb.Append("T");
            }
            if (fireKey == true)
            {
                sb.Append("F");
            }

            //we send the data from the string builder to be sent to the server
            gameController.Send("(" + sb.ToString() + ")" + "\n");
        }


        /// <summary>
        /// this method takes the fields from the textboxes that the user inputs to be utilized in the game controller
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //storing the name text box
            gameController.click(textBox1.Text);
            //this stores the server address in our game controller
            gameController.setName(serverAddress.Text);
        }
        /// <summary>
        /// this method handles when the user clicks on the menu strip button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void controlsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //displaying the message box 
            MessageBox.Show("UP: Thruster\nLEFT: Turn Left\nRIGHT: Turn Right\nSpace bar: Shoot");
        }


    }
}





