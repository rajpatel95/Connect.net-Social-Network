using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Configuration;
using System.Data;

namespace MP2D
{
    public class ADOMethods
    {
        public static string CS = "data source=.; database=MP2; integrated security = SSPI";
        public static void LoginAdd(string txtUserName, string txtPassword)
        {
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("spLogin", con);//Name of Procedure
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserName", txtUserName);
                cmd.Parameters.AddWithValue("@PWD", txtPassword);

                con.Open();
                cmd.ExecuteNonQuery();

            }
        }

        public static void AddNewUser(string txtFirstName, string txtLastName, string txtEmail, string txtDOB, string txtUserName, string txtPassword, string txtPhone_No, string txtAddress, string txtAge)
        {
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("spSignUp", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@FirstName", txtFirstName);
                cmd.Parameters.AddWithValue("@LastName", txtLastName);
                cmd.Parameters.AddWithValue("@Email", txtEmail);
                cmd.Parameters.AddWithValue("@DOB", txtDOB);
                cmd.Parameters.AddWithValue("@UserName", txtUserName);
                cmd.Parameters.AddWithValue("@PWD", txtPassword);
                cmd.Parameters.AddWithValue("@Phone_No", txtPhone_No);
                cmd.Parameters.AddWithValue("@Add_ress", txtAddress);
                cmd.Parameters.AddWithValue("@Age", txtAge);

                con.Open();
                cmd.ExecuteNonQuery();

            }
        }

        public static string[] UsersList(string prefix)
        {
            string name = "";
            SqlConnection con = new SqlConnection(CS);

            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "select UserName from dbo.SignUp where UserName like '" + prefix + "%'";
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                name += reader["UserName"].ToString() + " ";
            }

            char[] del = { ' ' };
            string[] listOfNames = name.Split(del);

            con.Close();
            return listOfNames;
        }

        public static void Request_Sent(String txtFriend1, String txtFriend2)
        {
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("spRequest_Sent", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("f1", txtFriend1);
                cmd.Parameters.AddWithValue("f2", txtFriend2);


                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public static void Request_Accept(String txtFriend2, String txtFriend1)
        {
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                con.Open();

                cmd.CommandText = "update IsFriendsWith set Status = 'A' where Status = 'P' and f2 like '" + txtFriend2 + "' and f1 like '" + txtFriend1 + "'";
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public static void Request_Reject(String txtfriend2, String txtfriend1)
        {
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                con.Open();

                cmd.CommandText = "Delete from IsFriendsWith where f1 = '" + txtfriend1 + "' and f2 = '" + txtfriend2 + "' and Status = 'P'";

                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public static string[] Friend_List(String txtFriend1)
        {
            using (SqlConnection con = new SqlConnection(CS))
            {
                String name = "";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandText = "Select f2 from IsFriendsWith where f1 = '" + txtFriend1 + "' and Status = 'A'";

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    name += reader["f2"].ToString() + " ";
                }

                con.Close();

                cmd.CommandText = "Select f1 from IsFriendsWith where f2 = '" + txtFriend1 + "' and Status = 'A'";

                con.Open();
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    name += reader["f1"].ToString() + " ";
                }

                char[] del = { ' ' };
                string[] listOfFriends = name.Split(del);
                con.Close();

                return listOfFriends;
            }
        }

        public static string[] Friend_List_Pending(String txtFriend1)
        {
            using (SqlConnection con = new SqlConnection(CS))
            {
                String name = "";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandText = "Select f1 from IsFriendsWith where f2 like '" + txtFriend1 + "' and Status = 'P'";

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    name += reader["f1"].ToString() + " ";
                }
                char[] del = { ' ' };
                string[] listOfFriends = name.Split(del);
                con.Close();

                return listOfFriends;
            }
        }

        public static void Status_Add(String Person, String Status)
        {
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                con.Open();

                // Cid is auto increment
                cmd.CommandText = "Insert into Timeline values('" + Person + "' , '" + Status + "' , 0)";
                cmd.ExecuteNonQuery();
                con.Close();

            }
        }

        public static void Likes_Increment(String Person, String Status) // Person is the person whose status is liked
        {
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                con.Open();

                cmd.CommandText = "Update Timeline SET Likes = Likes + 1 where UserName = '" + Person + "' AND Text_Status= '" + Status + "'";

                cmd.ExecuteNonQuery();
                con.Close();

            }
        }

        public static void Likes_Decrement(String Person, String Status) // Person is the person whose status is disliked
        {
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                con.Open();

                cmd.CommandText = "Update Timeline SET Likes = Likes - 1 where UserName = '" + Person + "' AND Text_Status= '" + Status + "'";

                cmd.ExecuteNonQuery();
                con.Close();

            }
        }

        public static string[] Status_Comments(String Status) // Displays Comments of a Particular Status
        // Return type is String
        // While entering values in table make sure each and every values are UNIQUE
        {
            using (SqlConnection con = new SqlConnection(CS))
            {
                String Comment = "";

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandText = "Select Comment from Timeline t , Comments c where t.Text_Status = '" + Status + "' and t.Cid = c.Cid  ORDER BY c.Date_Comment";

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read() )
                {
                    Comment += reader["Comment"].ToString() + "~";
                }
                char[] del = { '~' };
                string[] listOfComments = Comment.Split(del);

                con.Close();

                return listOfComments;

            }
        }

        public static string[] Status_Display(String UserName) // Displays Statuses of a Particular Person...Sorted by date
        // Return type is String
        {
            using (SqlConnection con = new SqlConnection(CS))
            {
                String Status = "";

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandText = "select Text_Status From Timeline t where t.UserName = '" + UserName + "' ORDER BY Cid ";

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Status += reader["Text_Status"].ToString() + "~";
                }

                char[] del = { '~' };
                string[] stats = Status.Split(del);

                con.Close();
                return stats;

            }
        }

        public static int Likes_Comments(String Comment) // Displays Likes of a Particular Comment
        //Enter different comments in comment table
        {
            using (SqlConnection con = new SqlConnection(CS))
            {


                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                int like = 0;
                cmd.CommandText = "Select No_Of_Likes from Comments where Comment = '" + Comment + "'";

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    like += Int32.Parse(reader["No_Of_Likes"].ToString());
                }
                con.Close();
                return like;
            }
        }

        public static int Likes_Display(String Person, String Status) // Displays Likes of a Particular Status
        {
            using (SqlConnection con = new SqlConnection(CS))
            {


                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                int like = 0;
                cmd.CommandText = "Select Likes from Timeline where UserName = '" + Person + "' and Text_Status = '" + Status + "'";

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    like += Int32.Parse(reader["Likes"].ToString());
                }

                con.Close();
                return like;

            }
        }

        public static void Inc_Comment(String Comment) // Increases counter of Likes of Comments
        // Make sure comments are different
        {
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                con.Open();

                cmd.CommandText = "Update Comments SET No_Of_Likes = No_Of_Likes + 1 where Comment = '" + Comment + "'";
                cmd.ExecuteNonQuery();

                con.Close();

            }
        }

        public static void Dec_Comment(String Comment) // Increases counter of Likes of Comments
        // Make sure comments are different
        {
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                con.Open();

                cmd.CommandText = "Update Comments SET No_Of_Likes = No_Of_Likes - 1 where Comment = '" + Comment + "'";
                cmd.ExecuteNonQuery();

                con.Close();

            }
        }

        public static void Add_Comment(String Comment, String Status)
        {
            using (SqlConnection con = new SqlConnection(CS))
            {
                String S_id = "";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                con.Open();

                cmd.CommandText = "Select Cid from Timeline where Text_Status ='" + Status + "' ";

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    S_id += reader["Cid"].ToString() + " ";
                }
                int id = Int32.Parse(S_id);
                con.Close();
                con.Open();
                TimeLine.setConsole(id + "   " + Comment);
                cmd.CommandText = "Insert into Comments values( " + id + " , '" + Comment + "'  , 0 , getdate())";

                cmd.ExecuteNonQuery();

                con.Close();
            }
        }
        public static int CheckUser(String UserName, String Password)
        {
            using (SqlConnection con = new SqlConnection(CS))
            {
                String read1 = "";
                String read2 = "";

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandText = "Select UserName , PWD from SignUp WHERE UserName = '" + UserName + "' AND PWD = '" + Password + "'";

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    read1 += reader["UserName"].ToString() + " ";
                    read2 += reader["PWD"].ToString() + " ";

                }
                int check = 1;

                if (read1 == "" || read2 == "")
                {
                    check = 0;
                }
                else
                {
                    check = 1;
                }
                return check;
            }
        }

        public static void ChangePassword(String UserName, String OldPassword, String NewPassword)
        {
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "Update SignUp SET PWD = '" + NewPassword + "' WHERE UserName = '" + UserName + "' AND PWD = '" + OldPassword + "'";

                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }
}