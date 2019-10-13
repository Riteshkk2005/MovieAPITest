<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MovieInfoClient._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
  
  <h3><asp:Label ID="message" runat="server" Font-Italic="True" Font-Size="Large" ForeColor="#FF3300"></asp:Label></h3>

 <h3>Search Movies By Title</h3>
    <asp:Label ID="Label1" runat="server" Text="Search By Movie Title"></asp:Label>
    <asp:TextBox ID="mtitle" runat="server"></asp:TextBox>
    <asp:Button ID="btnTitle" runat="server" Text="Search" OnClick="btnTitle_Click" />
    <asp:GridView ID="grdMoviesByTitle" runat="server"></asp:GridView>

 <h3>Search Movies By Genres</h3>
    <asp:Label ID="Label2" runat="server" Text="Search By Movie Genres"></asp:Label>
    <asp:TextBox ID="mGenres" runat="server"></asp:TextBox>
    <asp:Button ID="btnGenres" runat="server" Text="Search" OnClick="btnGenres_Click" />
    <asp:GridView ID="grdMoviesByGenres" runat="server"></asp:GridView>

 <h3>Search Movies By Release Year</h3>
    <asp:Label ID="Label3" runat="server" Text="Search By Movie Year"></asp:Label>
    <asp:TextBox ID="mYear" runat="server"></asp:TextBox>
    <asp:Button ID="btnYear" runat="server" Text="Search" OnClick="btnYear_Click"  />
    <asp:GridView ID="grdYear" runat="server"></asp:GridView>

     <h3>Get Top 5 Total user rating Movies List</h3>
    <asp:Button ID="btnTotalMovies" runat="server" Text="Get Top 5 Total User Rating" OnClick="btnTotalMovies_Click" />
    <asp:GridView ID="grdTotalMovies" runat="server"></asp:GridView>

     <h3>Get Top 5 Total user rating Movies List by USER</h3>
       <asp:TextBox ID="txtUserId" runat="server"></asp:TextBox>
    <asp:Button ID="btnUserRating" runat="server" Text="Get Top 5 Total User Rating by User" OnClick="btnUserRating_Click" />
    <asp:GridView ID="grdUserRating" runat="server"></asp:GridView>

    <h3>Rate the Movie (Need User Id, Movie Id and Rate)</h3>
    <asp:Label ID="Label4" runat="server" Text="UserId"></asp:Label>
    <asp:TextBox ID="txtUsrId" runat="server"></asp:TextBox>
    <asp:Label ID="Label5" runat="server" Text="MovieId"></asp:Label>
    <asp:TextBox ID="txtMovieId" runat="server"></asp:TextBox>
    <asp:Label ID="Label6" runat="server" Text="Rate (1 to 5)"></asp:Label>
    <asp:TextBox ID="txtRate" runat="server"></asp:TextBox>
    <asp:Button ID="btnRate" runat="server" Text="Create new Rate" OnClick="btnRate_Click"/>    
     <asp:Button ID="btnUpdateRate" runat="server" Text="Update Rate" OnClick="btnUpdateRate_Click"/>  
  
    

</asp:Content>

