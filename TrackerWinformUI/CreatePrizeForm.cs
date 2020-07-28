using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLibrary.DBConnection;
using TrackerLibrary.Interfaces;
using TrackerLibrary.Models;

namespace TrackerWinformUI
{
    public partial class CreatePrizeForm : Form
    {
        public CreatePrizeForm()
        {
            InitializeComponent();
        }

        private void createTeamButton_Click(object sender, EventArgs e)
        {
            if(ValidateForm())
            {
                PrizeModel model = new PrizeModel(
                    placeNameValue.Text, 
                    placeNumberValue.Text, 
                    prizeAmountValue.Text, 
                    prizePercentageValue.Text);

                foreach (IDataConnection db in GlobalConfig.Connections)
                {
                    db.CreatePrize(model);
                }

                placeNameValue.Text = "";
                placeNumberValue.Text = "";
                prizeAmountValue.Text = "0";
                prizePercentageValue.Text = "0";
            }
            else
            {
                MessageBox.Show("This form has invalid information. Please Check it and try again");
            }
            
        }

        private bool ValidateForm()
        {
            bool result = true;
            int placeNumber = 0;
            bool placeNumberValidNumber = int.TryParse(placeNumberValue.Text, out placeNumber);

            if (placeNumberValidNumber == false || placeNumber < 1)
            {
                result = false;
            }

            if(placeNameValue.Text.Length == 0)
            {
                result = false;
            }

            bool prizeAmountValidValue = decimal.TryParse(prizeAmountValue.Text, out decimal prizeValue);
            if (prizeAmountValidValue == false || prizeValue <= 0)
            {
                result = false;
            }

            bool prizePercentageValidValue = double.TryParse(prizePercentageValue.Text, out double prizePercentage);
            if (prizePercentageValidValue == false || prizePercentage < 0 || prizePercentage > 100)
            {
                result = false;
            }
           


            return result;
        }
    }
}
