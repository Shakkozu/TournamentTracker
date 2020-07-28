using System;
using System.Windows.Forms;
using TrackerLibrary.DataAccess;
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

                GlobalConfig.Connection.CreatePrize(model);
               

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
            bool placeNumberValidNumber = int.TryParse(placeNumberValue.Text, out int placeNumber);

            if (placeNumberValidNumber == false || placeNumber < 1)
            {
                result = false;
            }

            if(placeNameValue.Text.Length == 0)
            {
                result = false;
            }

            
            bool prizeAmountValidValue = decimal.TryParse(prizeAmountValue.Text, out decimal prizeValue);
            bool prizePercentageValidValue = double.TryParse(prizePercentageValue.Text, out double prizePercentage);

            if(prizeAmountValidValue == false && prizePercentageValidValue == false)
            {
                result = false;
            }
           
            if (prizeValue <= 0 && (prizePercentage < 0 || prizePercentage > 100))
            {
                result = false;
            }
           


            return result;
        }
    }
}
