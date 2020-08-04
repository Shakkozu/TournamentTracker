using System;
using System.Windows.Forms;
using TrackerLibrary.DataAccess;
using TrackerLibrary.Interfaces;
using TrackerLibrary.Models;

namespace TrackerWinformUI
{
    public partial class CreatePrizeForm : Form
    {
        /// <summary>
        /// Defines parent that called this form. This variable is used to return created prize
        /// </summary>
        IPrizeRequester callingForm;
        public CreatePrizeForm(IPrizeRequester caller)
        {
            InitializeComponent();

            callingForm = caller;
        }

        private void createPrizeButton_Click(object sender, EventArgs e)
        {
            //If Form is filled with correct data
            if(ValidateForm())
            {
                //Create new PrizeModel based on from values
                PrizeModel model = new PrizeModel(
                    placeNameValue.Text, 
                    placeNumberValue.Text, 
                    prizeAmountValue.Text, 
                    prizePercentageValue.Text);

                //Add new record to the database/textfile
                GlobalConfig.Connection.CreatePrize(model);

                callingForm.PrizeComplete(model);

                this.Close();

                //Reset the form values

                //placeNameValue.Text = "";
                //placeNumberValue.Text = "";
                //prizeAmountValue.Text = "0";
                //prizePercentageValue.Text = "0";
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
