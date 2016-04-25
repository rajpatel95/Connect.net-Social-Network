<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TimeLine.aspx.cs" Inherits="MP2D.TimeLine" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="jquery-ui.css" />
    <style>
        #tabs
        {
            background-color:black;
            color:red;
        }
        .btns
        {
            background-color: #6699FF; 
            color: #FFFFFF;
        }
        .linkButton 
        { 
            background: none;
            border: none;
            color: #0066ff;
            text-decoration: underline;
            cursor: pointer; 
        }
        .apost
        {
            background-color:rgb(175, 228, 213);
            border-radius:6px;
            padding:17px;
            margin:7px;
            width:100%;
            align-self:center;
        }
        .cmt
        {
            background-color:rgb(196, 241, 224);
            border-radius:6px;
            padding:10px;
            margin-left:0px;
            margin-right:7px;
            margin-top:7px;
            margin-bottom:7px;
            width:90%;
            align-self:center;
        }
    </style>
    <script src="jquery-2.1.4.js"></script>
    <script src="jquery-ui.min.js"></script>
    <script>
        $(function ()
        {
            $("#tabs").tabs();
        }
        );
        function like_click(ele)
        {
            var x = ele.id;
            var value = ele.value;
            if (value.localeCompare("Like") == 0)
            {
                ele.value = "Dislike";
                PageMethods.ProcessLike(x, OnSucess);
            }
            else
            {
                ele.value = "Like";
                PageMethods.ProcessDisLike(x, OnSucess);
            }
            
        }
        var cid;
        var cid1;
        function CallServerMethod()
        {
            PageMethods.GetMessage("Hi", OnSucess);
        }
        function OnSucess(result)
        {
            var temp = result.split("=");

            temp[0] = "n::" + temp[0];
            document.getElementById(temp[0]).innerHTML = temp[1] + " Likes";
        }
        function getCmts(ele)
        {
            var x = ele.id;
            cid = x;
            PageMethods.ProcessCmts(x, AddComments);
        }
        function getCmtsID()
        {
            
            PageMethods.ProcessCmts(cid, AddComments);
            
        }
        function AddComments( result )
        {
            if (result == "H") return;
            //alert(result);
            document.getElementById("c::" + cid).innerHTML = setACmt(result[0]);
            for( var i = 1; i < result.length-1; i++ )
            {
                document.getElementById("c::" + cid).innerHTML += setACmt(result[i]);
            }
        }
        var cl;
        function setACmt( x )
        {
            var temp = x.split("$");
            var cmt = temp[1];
            var likes = temp[0];
            var cmtid = cmt.split(' ').join('*');
            var retStr = '<div class="cmt">' + cmt + '<br><input type="button" onclick="likeCmtClick( this )" id=' + cmtid + '_b value="Like"class="btns">' + '<div id=' + cmtid + '_l>' + likes + ' Likes</div>' + '</div>'
            return  retStr;
        }

        function likeCmtClick( ele )
        {
            var x = ele.id;
            var temp = x.split("_");
            var cmt = temp[0];
            var value = ele.value;
            if (value.localeCompare("Like") == 0)
            {
                ele.value = "Dislike";
                PageMethods.cProcessLike(cmt, afterLike);
            }
            else
            {
                ele.value = "Like";
                PageMethods.cProcessDisLike(cmt, afterLike);
            }
        }

        function afterLike( result )
        {
            var temp = result.split("$");
            var likes = temp[0];
            var cmt = temp[1];

            document.getElementById(cmt + "_l").innerHTML = likes + " Likes";
        }

        function addNewCmt(ele)
        {
            var user = '<%=Application["UserName"] %>';
            var x = ele.id;
            cid1 = x;
            var cm = document.getElementById( "bc~" + x.substring(3)).value;
            PageMethods.AddCmt( user + " : " + cm+"%"+x,refresh);
        }
        function refresh( result )
        {
            getCmtsID();
        }

        function getUser(ele)
        {
            PageMethods.setUserBar(ele.innerHTML, suc);
        }
        function suc( result )
        {

        }
        
    </script>
    <style>
        
    </style>
</head>
<body style="background-color: rgb(202, 213, 223);">
    <form id="form1" runat="server">
         
        &nbsp;&nbsp;&nbsp;
         
        <div style="padding: 4px; width: 100%; height: 20pt; background-color: rgb(104, 190, 247); float: right; font-family: 'Courier New', Courier, monospace; font-size: 25px; text-align: left; color: #FFFFFF; position: fixed; top: 0px; left: 0px;">

            <div style="float: left; width: 31%;">
                &nbsp;&nbsp;Connect.NET
            </div>
            <div style="float: left; width: 54%; margin-left: 3px; height: 52px;">
                <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        Search :<asp:TextBox ID="TextBox1" runat="server" OnTextChanged="TextBox1_TextChanged"></asp:TextBox>
                         
                          &nbsp; &nbsp;
                            <%--<a href="">Edit Profile</a>--%>
                    </ContentTemplate>
                     
                </asp:UpdatePanel>
           
            </div>
            <div style="float: right; font-size: 20px;">
                
                <asp:Label ID="Username" runat="server" Text="Label"></asp:Label>&nbsp;
                <a href="Index.aspx">Logout</a>
            </div>
            &nbsp;
        </div>


        <input type ="text" runat ="server" /><br />
       
        <div style="margin: 7px;border-radius:4px; background-color: rgb(245, 245, 245); float: left; width: 30%; height: 90%; position: fixed; top: 43px; left: 10px;">
            <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
            
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:TextBox ID="TextBox2" runat="server" AutoPostBack="True" OnTextChanged="AddFriend_Click"></asp:TextBox>
                    <asp:Button ID="AddFriend" runat="server" Text="Send Request" OnClick="AddFriend_Click" CssClass="btns" />
                    <br />
                    <br />
                    Friend Requests Pending :<br />
                    <asp:DropDownList ID="pendingList" runat="server"></asp:DropDownList>
                    &nbsp;<asp:Button ID="Accept" runat="server" Text="Accept" OnClick="Accept_Click" CssClass="btns" />
                    &nbsp;<asp:Button ID="Reject" runat="server" Text="Reject" OnClick="Reject_Click" CssClass="btns" />
                    <br />
                    <br />
                    List Of Friends :<br />
                    <asp:PlaceHolder ID="friendList" runat="server"></asp:PlaceHolder>

                </ContentTemplate>
            </asp:UpdatePanel>
            
        </div>
        <div id="tabs" style="padding:0px;margin:7px; background-color: rgb(245, 245, 245); float: left; height: 89%; top: 43px; left: 485px; position: absolute; width: 65.5%;">
            <ul style="background-color: rgb(104, 190, 247);color: rgb(170, 186, 204);padding:0px;">
                <li><a href="#tabs-1">Timeline</a></li>
               <%-- <li><a href="#tabs-2">Chat</a></li>--%>
            </ul>
            <div id="tabs-1">
            
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <div>
                            <textarea id="newStatus" rows="3" cols="82" runat="server"  ></textarea>
                            <br />
                            <div id="belowPost" ></div>
                            <br />
                            <input type="button" runat="server" id="postButton" onserverclick="postButton_ServerClick" value="Post Status" style="background-color: #6699FF; color: #FFFFFF" />
                            <br />
                            &nbsp;<hr />
                            <div style="overflow:scroll;width:100%;height:360px;">
                                <asp:PlaceHolder ID="PostArea" runat="server"></asp:PlaceHolder>
                            </div>
                        </div>
                    </ContentTemplate>  
                </asp:UpdatePanel>
            </div>
            <%--<div id="tabs-2">
                Chat
            </div>--%>
        </div>
    </form>
</body>
</html>
