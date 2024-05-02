<%@ Control Language="C#" AutoEventWireup="true" CodeBehind=".ascx.cs" Inherits="assessment_platform_developer.ViewSwitcher" %>
<div id="viewSwitcher">
    <%: CurrentView %> view | <a href="<%: SwitchUrl %>" data-ajax="false">Switch to <%: AlternateView %></a>
</div>