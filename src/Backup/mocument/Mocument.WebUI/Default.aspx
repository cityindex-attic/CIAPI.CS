<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="Mocument.WebUI._Default" %>
<%@ Import Namespace="Mocument.WebUI.Code" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
        <style type="text/css">
        .style1
        {
            font-size: xx-small;
        }
            .style2
            {
                font-size: medium;
            }
    </style>
 
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
        <p class="style2">
            <strong>I will fulfill all of your fantasies .... 
    </strong> 
    </p>
    <p class="style1">
        ... that is, if all you fantasize about is easy recording, playback 
        and documentation of http traffic.
       
    </p>
        <p class="style1">
            &nbsp;</p>
    (this video is a draft, for internal use. I will clean the cottonballs from my mouth and create several short targeted walkthroughs as functionality matures)
    <iframe src="https://cityindex.viewscreencasts.com/embed/3d7049e2c8e04a2ab7b310e3fca3ed00" width="620" height="378"></iframe>
    <br />
        <br />
    Mocument.it is a reverse proxy tool for HTTP text based API service and client 
    developers that uses the concept of &#39;tapes&#39; that can be recorded, played back, 
    analysed and even be leveraged to generate draft documentation.<br />
        <br />
    <br />
    Log in and go to &#39;My Tapes&#39;.<br />
    <br />
    There you will see a list of tapes that you have created and possible recorded 
    to.<br />
    <br />
    If you click &#39;Details&#39; you will be presented with an abridged summary of the 
    HTTP traffic that has been recorded to the tape.<br />
    <br />
    You may export the tape for your own use. 
        <br />
        <br />
        Tapes are exported in the
    <a href="http://www.softwareishard.com/blog/har-12-spec/">HTTP Archive (HAR) 
    v1.2 format</a>.
        <br />
        <br />
        <h3>
            Creating Tapes</h3>
    <br />
    To create a tape, click &quot;Add new tape&quot; button.<br />
    <br />
    Give your 
        tape a name unique to your tape library.<br />
        <br />
        Only alpha-numeric and underscore are allowed in a tape name.<br />
        <br />
        You may optionally provide a description.<br />
        <br />
        If you plan to record to the tape at this time, check the &#39;Open for recording&#39; 
        checkbox.<br />
        <br />
        Your open tape is secured from tampering by limiting the IP address allowed to 
        record.<br />
        <br />
        By default, your current IP is prepopulated.<br />
        <br />
        <h3>
            Recording to your tapes</h3>
        <br />
        Recording to tapes is accomplished by pointing your client code to the reverse 
        proxy port of mocument.it.<br />
        <br />
        <br />
        Assuming that you typically point your client code to an endpoint located at
    
        <br />
        <br />
        <a href="HTTP://myservice.com/awesomeService">HTTP://<strong>myservice.com/awesomeService</strong></a>
        <br />
        <br />
        you would instead point it to
        
        <br />
        <br />
        <a href="HTTP://mocument.it:81/record/[library]/[tapename]/myservice.com/awesomeService">HTTP://mocument.it:81/<strong>record</strong>/[library]/[tapename]/<strong>myservice.com/awesomeService</strong></a><br />
        <br />
        <br />
        The exact url will be displayed by clicking &#39;details&#39; link next to the tape in 
        your library.<br />
        <br />
        At this time, only HTTP traffic is supported. HTTPS support will be forthcoming.<br />
        <br />
        When you have completed recording you may lock the tape by editing the tape and 
        clearing the &#39;Open for recording&#39; checkbox.<br />
        <br />
        <h3>
            Replaying your tapes</h3>
        <br />
        To play your tape to your API client code simply point it to
        <br />
        <br />
        <a href="HTTP://mocument.it:81/record/[library]/[tapename]/myservice.com/awesomeService">HTTP://mocument.it:81/<strong>play</strong>/[library]/[tapename]/<strong>myservice.com/awesomeService</strong></a><br />
        <br />
        <br />
        and issue the same requests as used to record.&nbsp; Requests arriving at the 
        &#39;play&#39; endpoint will be matched against the recorded traffic and the 
        corresponding response will be returned providing deterministic and expectable 
        results.&nbsp; This may be useful for testing, demo applications, client first 
        API development, etc.<br />
        <br />
        <br />
        <br />
        <br />
        <h3>
            Using the Mocument ReverseProxy locally in testing scenarios</h3>
        <br />
        <br />
        TODO<br />
        <br />
        &nbsp;<br />
    <br />
    <br />
    <br />
    </asp:Content>
