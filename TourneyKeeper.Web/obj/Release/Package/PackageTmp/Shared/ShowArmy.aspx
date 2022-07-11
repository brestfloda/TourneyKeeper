<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowArmy.aspx.cs" Inherits="TourneyKeeper.Web.ShowArmy" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Armylist</title>
</head>
<body>
    <div style="width: auto">
        <form runat="server" style="width: 100%; height: 100%;">
            <p>
                <asp:Literal runat="server" Mode="PassThrough" ID="ArmylistLabel"></asp:Literal>
            </p>
        </form>
    </div>
</body>
</html>
