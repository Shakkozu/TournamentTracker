using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLibrary.DataAccess;
using TrackerLibrary.Models;

namespace TrackerWinformUI
{
    public partial class CreateTeamForm : Form
    {

        private List<PersonModel> availableTeamMembers = GlobalConfig.Connection.GetPerson_All();
        private List<PersonModel> selectedTeamMembers = new List<PersonModel>();

        /// <summary>
        /// Item that requested creation of this form. 
        /// The idea is to call this form, after click button "Create New Team"
        /// So result will be returned as changing 'callingForm' property
        /// </summary>
        private ITeamRequester callingForm;

        public CreateTeamForm(ITeamRequester caller)
        {
            InitializeComponent();
            callingForm = caller;
            //CreateSampleData();
            WireUpLists();
        }


        private void CreateSampleData()
        {
            availableTeamMembers.Add(new PersonModel { FirstName = "Tim", LastName = "Corey" });
            availableTeamMembers.Add(new PersonModel { FirstName = "Sue", LastName = "Storm" });
            availableTeamMembers.Add(new PersonModel { FirstName = "Joe", LastName = "Doe" });

            selectedTeamMembers.Add(new PersonModel { FirstName = "Jane", LastName = "Smith" });
            selectedTeamMembers.Add(new PersonModel { FirstName = "Bill", LastName = "Jones" });
        }

        private void WireUpLists()
        {
            selectTeamMemberDropDown.DataSource = null;

            selectTeamMemberDropDown.DataSource = availableTeamMembers;
            selectTeamMemberDropDown.DisplayMember = "FullName";


            teamMembersListBox.DataSource = null;

            teamMembersListBox.DataSource = selectedTeamMembers;
            teamMembersListBox.DisplayMember = "FullName";
        }

        private void createMemberButton_Click(object sender, EventArgs e)
        {
            //Take fields from Add New Member section
            //Add new Person Model
            //Save it to database
            if(ValidateForm())
            {
                PersonModel p = new PersonModel();
                p.FirstName = firstNameValue.Text;
                p.LastName = lastNameValue.Text;
                p.CellphoneNumber = cellPhoneValue.Text;
                p.EmailAddress = emailValue.Text;

                GlobalConfig.Connection.CreatePerson(p);

                selectedTeamMembers.Add(p);
                WireUpLists();


                firstNameValue.Text = "";
                lastNameValue.Text = "";
                emailValue.Text = "";
                cellPhoneValue.Text = "";
            }
            else
            {
                MessageBox.Show("You need to fill in all of the fields");
            }

        }

        private bool ValidateForm()
        {
            //TODO - Upgrade form's  validation 
            if (firstNameValue.Text.Length == 0)
            {
                return false;
            }
            if(lastNameValue.Text.Length == 0)
            {
                return false;
            }
            if(cellPhoneValue.Text.Length == 0 )
            {
                return false;
            }
            if (emailValue.Text.Length == 0)
            {
                return false;
            }
            return true;
        }

        private void addMemberButton_Click(object sender, EventArgs e)
        {
            PersonModel person = (PersonModel)selectTeamMemberDropDown.SelectedItem;
            if(person != null)
            { 
            selectedTeamMembers.Add(person);
            availableTeamMembers.Remove(person);

            WireUpLists();
            }
        }

        private void removeSelectedMemberButton_Click(object sender, EventArgs e)
        {
            PersonModel person = (PersonModel)teamMembersListBox.SelectedItem;
            if (person != null)
            {

                selectedTeamMembers.Remove(person);
                availableTeamMembers.Add(person);

                WireUpLists();
            }
        }

        private void createTeamButton_Click(object sender, EventArgs e)
        {
            TeamModel t = new TeamModel();
            
            t.TeamName = teamNameValue.Text;
            t.TeamMembers = selectedTeamMembers;

            //Why not t= GlobalCondig.Connection.CreateTeam(t)? - Because CreateTeam parameter points to address, and it updates
            //parameter within function call
            GlobalConfig.Connection.CreateTeam(t);
           

            callingForm.TeamComplete(t);

            Close();
            // TODO - If we aren't closing this form after creation, reset the form
            
        }
    }
}
