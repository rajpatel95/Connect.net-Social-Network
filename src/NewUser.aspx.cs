using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MP2D
{
    public partial class NewUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void SignUp_Click(object sender, EventArgs e)
        {
            string username = UserName.Text;
            string password = Password.Text;
            string confirmPassword = ConfirmPassword.Text;

            if( String.Compare( password, confirmPassword ) == 0 )
            {
                ADOMethods.LoginAdd(username, password);
                Response.Redirect("Index.aspx");    
            }
            else
            {
                FailureText.Text = "Password and Confirm Password doesn't match.";
            }

        }
       
    }
}