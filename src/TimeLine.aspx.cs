using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Services;
using System.Web.Services;
using System.IO;  

namespace MP2D
{
    public partial class TimeLine : System.Web.UI.Page
    {
        public static string currentUser;
        public static string userInBar;
        protected void Page_Load(object sender, EventArgs e)
        {
            currentUser = Application["UserName"].ToString();
            if( flag )
            {
                Application["UserInBar"] = userInBar;
                flag = false;
            }
            else
            {
                userInBar = Application["UserInBar"].ToString();
            }
            
            Username.Text = currentUser;
            if (currentUser == userInBar)
            {
                AddFriend.Visible = false;
            }
            AddFriend.Visible = true;
            setPendingList();
            setFriendList();
            getStatusList();
        }

        public void getStatusList()
        {
            //string user = Application["UserName"].ToString();
            string user = userInBar;
            string[] status = ADOMethods.Status_Display(user);

            for (int i = 0; i < status.Length - 1; i++)
            {
                int nol = ADOMethods.Likes_Display(user, status[i]);
                setAPost(user, status[i], nol);
            }

        }

        protected void setAPost(string UserName, string Text, int noOfLikes)
        {
            string cText = "";
            for (int i = 0; i < Text.Length; i++)
            {
                if (Text[i] == ' ')
                {
                    cText += ":";
                }
                else
                {
                    cText += Text[i];
                }
            }
            LiteralControl lc = new LiteralControl();
            lc.Text = "<div class=\"apost\">" + UserName + " : <br />" +
                            "" + Text + " <br />" +
                            "<input type=\"button\"id=" + UserName + "_" + cText + " value=\"Like\" runat=\"server\" onclick=\"like_click(this)\" class=\"btns\" />" +
                            "<div id=n::" + UserName + "_" + cText + " >" + noOfLikes + " Likes</div>" +
                            "<input type=\"button\" id=" + UserName + "_" + cText + " value=\"View Comments\" onclick=\"getCmts(this)\">"
                            + "<div id=c::" + UserName + "_" + cText + " >" + "" + "</div>"+
                             "<input type=\"text\"id=bc~" + UserName + "_" + cText + " value=\"\" runat=\"server\"/><br>"+
                             "<input type=\"button\"id=ac~" + UserName + "_" + cText + " value=\"Comment\" runat=\"server\" onclick=\"addNewCmt(this)\" class=\"btns\" />"
                            +"</div>";
            PostArea.Controls.AddAt(0, lc);
        }

        [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
        public string[] GetCompletionList(string prefixText, int count)
        {
            string[] cs = ADOMethods.UsersList(prefixText);

            return cs;
        }

        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {
            Application["UserInBar"] = TextBox1.Text;
            Response.Redirect("TimeLine.aspx");
            userInBar = TextBox1.Text;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

        }



        protected void setFriendList()
        {
            friendList.Controls.Clear();

            string[] friends = ADOMethods.Friend_List(currentUser);
            LiteralControl list = new LiteralControl();
            // <li><a href='~/TimeLine.aspx?uid=name'></a></li>
            string inText = "<ul>";

            for (int i = 0; i < friends.Length - 1; i++)
            {
                inText += "<li><a href='TimeLine.aspx' onclick='getUser(this)'>" + "<b><i><h3>" + friends[i] + "</h3>" +"</a></li>";
            }

            inText += "</ul>";

            list.Text = inText;
            friendList.Controls.Add(list);
        }



        protected void setPendingList()
        {
            //Get Pending User data
            pendingList.Items.Clear();
            string[] pendingFriends = ADOMethods.Friend_List_Pending(currentUser);

            for (int i = 0; i < pendingFriends.Length - 1; i++)
            {
                pendingList.Items.Add(new ListItem(pendingFriends[i]));
            }

            //Drop Down List of pending
            if (pendingFriends.Length == 1)
            {
                Accept.Visible = false;
                Reject.Visible = false;
                pendingList.Visible = false;
            }
        }



        protected void AddFriend_Click(object sender, EventArgs e)
        {
            userInBar = TextBox2.Text;
            ADOMethods.Request_Sent(currentUser, userInBar);
        }

        protected void Accept_Click(object sender, EventArgs e)
        {
            ADOMethods.Request_Accept(currentUser, pendingList.SelectedItem.Text);
            setPendingList();
            setFriendList();
        }

        protected void Reject_Click(object sender, EventArgs e)
        {
            ADOMethods.Request_Reject(currentUser, pendingList.SelectedItem.Text);
            setPendingList();
            setFriendList();
        }

        protected void postButton_ServerClick(object sender, EventArgs e)
        {
            string statusToPost = newStatus.Value.ToString();

            string user = Application["UserName"].ToString();
            ADOMethods.Status_Add(user, statusToPost);
            PostArea.Controls.Clear();
            getStatusList();
            //setAPost(user, statusToPost, 0, 0);

        }

        [WebMethod]
        public static string ProcessLike(string data)
        {
           
            string[] newD = data.Split('_');
            string username = newD[0];
            string[] statusr = newD[1].Split(':');
            string status = statusr[0];
            for (int i = 1; i < statusr.Length; i++)
            {
                status += " " + statusr[ i ];
            }
            ADOMethods.Likes_Increment( username, status);
            int likes = ADOMethods.Likes_Display(username,status);
            return data + "=" + likes;
        }

        [WebMethod]
        public static string ProcessDisLike(string data)
        {

            string[] newD = data.Split('_');
            string username = newD[0];
            string[] statusr = newD[1].Split(':');
            string status = statusr[0];
            for (int i = 1; i < statusr.Length; i++)
            {
                status += " " + statusr[i];
            }
            ADOMethods.Likes_Decrement(username, status);
            int likes = ADOMethods.Likes_Display(username, status);
            return data + "=" + likes;
        }

        [WebMethod]
        public static string[] ProcessCmts(string data)
        {

            string[] newD = data.Split('_');
            string username = newD[0];
            string[] statusr = newD[1].Split(':');
            string status = statusr[0];
            for (int i = 1; i < statusr.Length; i++)
            {
                status += " " + statusr[i];
            }
            string[] cmts= ADOMethods.Status_Comments( status );
            for (int i = 0; i < cmts.Length; i++ )
            {
                int x = ADOMethods.Likes_Comments(cmts[ i ]);
                cmts[i] = x + "$" + cmts[i];
            }
            if (cmts.Length == 1) cmts[0] = "H";
            return cmts;
        }

        [WebMethod]
        public static void AddCmt( string data )
        {
            char[] b = {'%'};
            string[] temp1 = data.Split(b);
            string cmt = temp1[ 0 ];
            data = temp1[ 1 ];

            char[] a = {'~'};
            string[] temp = data.Split( a );
            data = temp[ 1 ];
            string[] newD = data.Split('_');
            string username = newD[0];
            string[] statusr = newD[1].Split(':');
            string status = statusr[0];
            for (int i = 1; i < statusr.Length; i++)
            {
                status += " " + statusr[i];
            }
          
            ADOMethods.Add_Comment(cmt, status);
        }

        [WebMethod]
        public static string cProcessLike(string data)
        {
            char[] a = { '*' };
            string[] dat = data.Split(a);
            string data1 = dat[ 0 ];
            for (int i = 1; i < dat.Length; i++ )
            {
                data1 += " "+ dat[i];
            }
            ADOMethods.Inc_Comment(data1);
            int likes = ADOMethods.Likes_Comments(data1);
            string ret = likes + "$" + data;
            setConsole(ret);
            return ret;
        }

        [WebMethod]
        public static string cProcessDisLike(string data)
        {
            char[] a = { '*' };
            string[] dat = data.Split(a);
            string data1 = dat[0];
            for (int i = 1; i < dat.Length; i++)
            {
                data1 += " " + dat[i];
            }
            ADOMethods.Dec_Comment(data1);
            int likes = ADOMethods.Likes_Comments(data1);
            string ret = likes + "$" + data;
            setConsole(ret);
            return ret;
        }

        [WebMethod]
        public static void setUserBar(string data)
        {
            userInBar = data;
            flag = true;
        }

        public static void setConsole(string data)
        {
            File.WriteAllText("D://date.txt", data + "-----");
        }
        public static bool flag = false;

 
    }
}