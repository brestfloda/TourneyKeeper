<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TKImport.aspx.cs" Inherits="TourneyKeeper.Web.TKImport" MasterPageFile="TKSingles.master" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="BodyContent">
    <script type="text/javascript">
        $("#importInput").fileinput();
    </script>
    <div class="row-fluid">
        <div class="col-md-12 padding-0">
            <asp:HiddenField ID="HiddenField1" runat="server" />
            <div class="row">
                <div class="col-md-12">
                    <asp:TextBox ID="outputTextBox" TextMode="MultiLine" runat="server" Width="100%" Height="200"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="col-md-3">
                    <div class="fileinput fileinput-new" data-provides="fileinput">
                        <span class="btn btn-primary btn-file"><span>Choose file</span><asp:FileUpload ID="importInput" runat="server" onchange="javascript: form.submit();" accept="application/xml"/></span>
                        <span class="fileinput-filename"></span><span class="fileinput-new">No file chosen</span>
                    </div>
                    <asp:Label runat="server" ForeColor="Red" ID="warningsLabel"></asp:Label>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
