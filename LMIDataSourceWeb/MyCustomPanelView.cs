/*
 * Copyright © 2018. TIBCO Software Inc.
 * This file is subject to the license terms contained
 * in the license file that is distributed with this file.
 */
using LMIDataSource;
using Spotfire.Dxp.Web.Forms;
using System;
using System.Text;
using System.Web.UI;

namespace LMIDataSourceWeb
{
    class MyCustomPanelView : CustomWizardPromptControl
    {
        public MyCustomPanelView(LmiDataSourcePromptModel model)
        {
            this.AddPage(new Page1(model));
            this.AddPage(new Page2(model));
            this.AddPage(new Page3(model));
        }

        private class Page1 : CustomWizardPromptControlPage
        {
            #region Constants and Fields

            private readonly LmiDataSourcePromptModel model;

            #endregion

            #region Constructors and Destructors

            public Page1(LmiDataSourcePromptModel model)
                : base("Configure LMI advanced data source (1/3)")
            {
                this.model = model;
            }

            #endregion

            protected override void OnGetContentsCore(HtmlTextWriter writer)
            {
                string valueHost = model.Host == null ? "" : model.Host;
                string valueUser = model.UserName == null ? "" : model.UserName;
                string valuePass = model.UserPass == null ? "" : model.UserPass;

                writer.WriteLine("<fieldset>");
                writer.WriteLine("<legend>Login details:</legend>");
                writer.WriteLine("<div class=\"host\">");
                writer.WriteLine("<label for=\"lmihost\">Host:</label>");
                writer.WriteLine("<input type =\"text\" id=\"lhost\" name=\"lhost\" required size=\"64\" minlength=\"1\" maxlength=\"128\" value=\"" + valueHost + "\"/>");
                writer.WriteLine("<span class=\"validity\"></span>");
                writer.WriteLine("</div>");
                writer.WriteLine("<div class=\"username\">");
                writer.WriteLine("<label for=\"uname\">Username:</label>");
                writer.WriteLine("<input type =\"text\" id=\"uname\" name=\"uname\" required required size=\"64\" minlength=\"1\" maxlength=\"128\" value=\"" + valueUser + "\"/>");
                writer.WriteLine("<span class=\"validity\"></span>");
                writer.WriteLine("</div>");
                writer.WriteLine("<div class=\"password\">");
                writer.WriteLine("<label for=\"uname\">Password:</label>");
                writer.WriteLine("<input type =\"password\" id=\"upass\" name=\"upass\" required required size=\"64\" minlength=\"1\" maxlength=\"128\"value=\"" + valuePass + "\"/>");
                writer.WriteLine("<span class=\"validity\"></span>");
                writer.WriteLine("</div>");
                writer.WriteLine("</fieldset>");
            }

            protected override void OnGetScriptsCore(StringBuilder builder)
            {
                // empty for now
            }

            protected override void OnLeavePageCore(FormData data)
            {
               // empty for now
            }

            protected override bool OnValidatePromptCore(FormData data)
            {
                model.Host = data.GetStringValue("lhost");
                model.UserName = data.GetStringValue("uname");
                model.UserPass = data.GetStringValue("upass");

                try
                {
                    LmiHandler lmiHandler = new LmiHandler(model.Host, "", model.UserName, model.UserPass, null, false, model.From, model.To);
                    lmiHandler.checkConnection();
                }
                catch (Exception e)
                {
                    ErrorMessage = e.Message;
                    return false;
                }
                return true;
            }
        }

        private class Page2 : CustomWizardPromptControlPage
        {
            #region Constants and Fields

            private readonly LmiDataSourcePromptModel model;

            #endregion

            #region Constructors and Destructors

            public Page2(LmiDataSourcePromptModel model)
                : base("Configure LMI advanced data source (2/3)")
            {
                this.model = model;
            }

            #endregion

            protected override void OnGetContentsCore(HtmlTextWriter writer)
            {
                writer.WriteLine("<fieldset>");
                writer.WriteLine("<legend>What to query</legend>");
                writer.WriteLine("<div class=\"queryType\">");
                writer.WriteLine("<input type=\"radio\" id=\"newRegular\" name=\"qtype\" value=\"newRegular\" " + (!model.IsCorrelation && model.QueryId == null ? "checked" : "") + "/>");
                writer.WriteLine("<label for=\"newRegular\">run a new advanced search</label><br>");
                writer.WriteLine("<input type=\"radio\" id=\"oldRegular\" name=\"qtype\" value=\"oldRegular\" " + (!model.IsCorrelation && model.QueryId != null ? "checked" : "") + "/>");
                writer.WriteLine("<label for=\"oldRegular\">use existing advanced search results</label><br>");
                writer.WriteLine("<input type=\"radio\" id=\"newCorrelation\" name=\"qtype\" value=\"newCorrelation\" " + (model.IsCorrelation && model.QueryId == null ? "checked" : "") + "/>");
                writer.WriteLine("<label for=\"newCorrelation\">run a new correlation search</label><br>");
                writer.WriteLine("<input type=\"radio\" id=\"oldCorrelation\" name=\"qtype\" value=\"oldCorrelation\" " + (model.IsCorrelation && model.QueryId != null ? "checked" : "") + "/>");
                writer.WriteLine("<label for=\"oldCorrelation\">use existing correlation results</label><br>");
                writer.WriteLine("</fieldset>");
            }

            protected override void OnGetScriptsCore(StringBuilder builder)
            {
                // empty for now
            }

            protected override void OnLeavePageCore(FormData data)
            {
                // empty for now
            }

            protected override bool OnValidatePromptCore(FormData data)
            {
                int selected = data["qtype"].IndexOf("true");
                switch (selected)
                {
                    case 0:
                        if (model.IsCorrelation)
                        {
                            model.IsCorrelation = false;
                            model.Query = "";
                        }
                        model.QueryId = null;
                        return true;
                    case 1:
                        model.IsCorrelation = false;
                        if (model.QueryId == null)
                        {
                            model.QueryId = "dummy";
                        }
                        return true;
                    case 2:
                        if (!model.IsCorrelation)
                        {
                            model.IsCorrelation = true;
                            model.Query = "";
                            model.From = DateTime.Now;
                            model.To = model.From;
                        }
                        model.QueryId = null;
                        return true;
                    case 3:
                        model.IsCorrelation = true;
                        if (model.QueryId == null)
                        {
                            model.QueryId = "dummy";
                        }
                        return true;
                }
                return false;
            }
        }

        private class Page3 : CustomWizardPromptControlPage
        {
            #region Constants and Fields

            private readonly LmiDataSourcePromptModel model;
            private QueryEntry[] queryEntries;

            #endregion

            #region Constructors and Destructors

            public Page3(LmiDataSourcePromptModel model)
                : base("Configure LMI advanced data source (3/3)")
            {
                this.model = model;
            }

            #endregion

            protected override void OnGetContentsCore(HtmlTextWriter writer)
            {
                writer.WriteLine("<fieldset>");
                writer.WriteLine("<legend>Query details:</legend>");
                string valueQueryId = model.QueryId == null ? "" : model.QueryId;
                if (model.QueryId == null)
                {
                    writer.WriteLine("<div class=\"query\">");
                    writer.WriteLine("<label for=\"tquery\">Query</label>");
                    writer.WriteLine("<textarea id=\"tquery\" name=\"tquery\" rows=\"5\" cols=\"80\">");
                    writer.WriteLine(model.Query);
                    writer.WriteLine("</textarea><br>");
                    writer.WriteLine("</div>");
                    if (model.IsCorrelation)
                    {
                        string fromStr = model.From.ToString("s", System.Globalization.CultureInfo.InvariantCulture);
                        string toStr = model.To.ToString("s", System.Globalization.CultureInfo.InvariantCulture);
                        writer.WriteLine("<div class=\"fromto\">");
                        writer.WriteLine("<label for=\"dtfrom\">From</label>");
                        writer.WriteLine("<input type=\"text\" id=\"dtfrom\" name=\"dtfrom\" value=\""+fromStr+"\">");
                        writer.WriteLine("<label for=\"dtto\">To</label>");
                        writer.WriteLine("<input type=\"text\" id=\"dtto\" name=\"dtto\" value=\""+toStr+"\">");
                        writer.WriteLine("</div>");
                    }
                }
                else
                {
                    try
                    {
                        LmiHandler lmiHandler = new LmiHandler(model.Host, "", model.UserName, model.UserPass, null, model.IsCorrelation, DateTime.MinValue, DateTime.MinValue);
                        if (!model.IsCorrelation)
                        {
                            queryEntries = lmiHandler.getQueries();
                        }
                        else
                        {
                            queryEntries = lmiHandler.getCorrelationQueries();
                        }
                    }
                    catch (Exception e)
                    {
                        string message = "Exception:" + e.ToString();
                        string caption = "Error while retrieving list of results from LMI: " + message;
                        writer.WriteLine("<span>" + caption + "</span>");
                        return;
                    }
                    writer.WriteLine("<div class=\"queryIdSelect\">");
                    writer.WriteLine("<select id=\"queryId\" name=\"queryId\" size=\"10\">");
                    int selected = 0;
                    for (int i = 0; i < queryEntries.Length; i++)
                    {
                        if (queryEntries[i].queryId.Equals(model.QueryId))
                        {
                            selected = i;
                        }
                    }
                    for (int i = 0; i < queryEntries.Length; i++)
                    {
                        string selectedOption = (i == selected ? " selected" : "");
                        writer.WriteLine("<option value=\"" + queryEntries[i].queryId + "\"" + selectedOption + ">" + queryEntries[i].query + "</option>");
                    }
                    writer.WriteLine("</select>");
                    writer.WriteLine("</div>");

                }
                writer.WriteLine("</fieldset>");
            }

            protected override void OnGetScriptsCore(StringBuilder builder)
            {
                // empty for now
            }

            protected override void OnLeavePageCore(FormData data)
            {
                // empty for now  
            }

            protected override bool OnValidatePromptCore(FormData data)
            {
                if (model.QueryId == null)
                {
                    model.Query = data.GetStringValue("tquery").Replace("\r", " ").Replace("\n", " ");
                    if (model.IsCorrelation)
                    {
                        model.From = DateTime.Parse(data.GetStringValue("dtfrom"), null, System.Globalization.DateTimeStyles.RoundtripKind);
                        model.To = DateTime.Parse(data.GetStringValue("dtto"), null, System.Globalization.DateTimeStyles.RoundtripKind);
                        try
                        {
                            LmiHandler lmiHandler = new LmiHandler(model.Host, model.Query, model.UserName, model.UserPass, null, model.IsCorrelation, model.From, model.To);
                            lmiHandler.checkConnection();
                        }
                        catch (Exception e)
                        {
                            ErrorMessage = e.Message;
                            return false;
                        }
                        return true;
                    }
                }
                else
                {
                    model.QueryId = data.GetStringValue("queryId");
                }
                return true;
            }
        }

    }
}
