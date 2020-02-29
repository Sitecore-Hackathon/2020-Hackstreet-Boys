using Hackathon.Foundation.Teams.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hackathon.Foundation.Teams
{
	public partial class TestMe : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}


        protected void CreateTeamButton_Click(object sender, EventArgs e)
        {
            TeamsRepository repo = new TeamsRepository();
            repo.CreateHackathonTeam(teamName.Text, "Awesome Team!", "SzymonAE");
        }

        protected void btnJoin_Click(object sender, EventArgs e)
        {
            TeamsRepository repo = new TeamsRepository();
            repo.JoinHackathonTeam("Test", "test", "aa@aa.com", txtGituser.Text, txtTeamName.Text);
        }
    }
}