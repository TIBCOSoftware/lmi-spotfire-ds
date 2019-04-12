/*
 * Copyright © 2018. TIBCO Software Inc.
 * This file is subject to the license terms contained
 * in the license file that is distributed with this file.
 */
using LMIDataSource;
using System;
using System.Windows.Forms;

namespace LMIDataSourceForms
{
    public partial class LmiDataSourceDialog : Form
    {
        /// <summary>The prompt model that this dialog uses.
        /// </summary>
        private readonly LmiDataSourcePromptModel promptModel;

        /// <summary>Initializes a new instance of the HttpDataSourceDialog class.
        /// </summary>
        /// <remarks>This constructor is invoked by the prompting framework.</remarks>
        /// <param name="promptModel">The prompting model for the LMI data source.</param>
        public LmiDataSourceDialog(LmiDataSourcePromptModel promptModel)
            : this()
        {
            this.promptModel = promptModel;

            // Initialize the user interface with the settings from the model.
            if (this.promptModel.Host != null)
            {
                this.hostBox.Text = this.promptModel.Host;
            }
            if (promptModel.UserName != null)
            {
                this.userNameBox.Text = promptModel.UserName;
            }
            if ( promptModel.UserPass != null )
            {
                this.userPassBox.Text = promptModel.UserPass;
            }

            if (!promptModel.IsCorrelation)
            {
                if (promptModel.QueryId == null)
                {
                    if (promptModel.Query != null)
                    {
                        this.queryBox.Text = promptModel.Query;
                    }
                    newQueryChoice.Checked = true;
                } else
                {
                    existingResultsChoice.Checked = true;
                }
            } else
            {
                if (promptModel.QueryId == null)
                {
                    if (promptModel.Query != null)
                    {
                        this.queryBox.Text = promptModel.Query;
                    }                 
                    runCorrChoice.Checked = true;
                } else
                {
                    corrResultsChoice.Checked = true;
                }
            }
        }


        private LmiDataSourceDialog()
        {
            InitializeComponent();
        }

        /// <summary>Event handler for when the dialog is first shown. Used to make
        /// sure that the URL combo box initially has focus.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void HttpDataSourceDialog_Shown(object sender, EventArgs e)
        {
            this.hostBox.Focus();
        }

        /// <summary>Event handler for when the OK button is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnOkButton_Click(object sender, EventArgs ea)
        {
            promptModel.Host = hostBox.Text;
            promptModel.UserName = userNameBox.Text;
            promptModel.UserPass = userPassBox.Text;

            if (existingResultsChoice.Checked)
            {
                promptModel.IsCorrelation = false;
                if (resultPicker.SelectedIndex == -1)
                {
                    var result = MessageBox.Show("You must select a query from the list", "Error",
                                               MessageBoxButtons.OK,
                                               MessageBoxIcon.Error);
                    DialogResult = DialogResult.None;
                    return;
                }
                promptModel.QueryId = queryEntries[resultPicker.SelectedIndex].queryId;
            }
            else if (newQueryChoice.Checked)
            {
                promptModel.Query = queryBox.Text;
                promptModel.IsCorrelation = false;
            }
            else if (runCorrChoice.Checked)
            {
                promptModel.Query = queryBox.Text;
                promptModel.IsCorrelation = true;
                promptModel.From = fromPicker.Value;
                promptModel.To = toPicker.Value;
            } else if ( corrResultsChoice.Checked)
            {
                promptModel.IsCorrelation = true;
                if (resultPicker.SelectedIndex == -1)
                {
                    var result = MessageBox.Show("You must select a query from the list", "Error",
                                               MessageBoxButtons.OK,
                                               MessageBoxIcon.Error);
                    DialogResult = DialogResult.None;
                    return;
                }
                promptModel.QueryId = queryEntries[resultPicker.SelectedIndex].queryId;
            }

            if (newQueryChoice.Checked || runCorrChoice.Checked)
            {

                try
                {
                    checkingLabel.Visible = true;
                    checkingLabel.Update();
                    LmiHandler lmiHandler = new LmiHandler(promptModel.Host, promptModel.Query, promptModel.UserName, promptModel.UserPass, null, promptModel.IsCorrelation, promptModel.From, promptModel.To);
                    lmiHandler.checkConnection();
                }
                catch (Exception e)
                {
                    string message = "Exception:" + e.ToString();
                    const string caption = "Error while checking connection to LMI";
                    var result = MessageBox.Show(message, caption,
                                                 MessageBoxButtons.OK,
                                                 MessageBoxIcon.Error);
                    DialogResult = DialogResult.None;
                    return;
                }
                finally
                {
                    checkingLabel.Visible = false;
                }

            }

            // The URL was value, close the dialog and continue.
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private QueryEntry[] queryEntries;

        private void switchToRunQuery()
        {
            queryBox.Visible = true;
            resultPicker.Visible = false;

            toLabel.Visible = false;
            toPicker.Visible = false;
            fromLabel.Visible = false;
            fromPicker.Visible = false;
        }

        private void switchToRunCorr()
        {
            queryBox.Visible = true;
            resultPicker.Visible = false;
            toLabel.Visible = true;
            toPicker.Visible = true;
            fromLabel.Visible = true;
            fromPicker.Visible = true;
            if (promptModel.From == null)
            {
                promptModel.From = DateTime.Now.AddHours(-1);
            }
            if (promptModel.To == null)
            {
                promptModel.To = DateTime.Now;
            }
            fromPicker.Value = promptModel.From;
            toPicker.Value = promptModel.To;
        }

        private void switchtoPickQuery()
        {
            queryBox.Visible = false;
            resultPicker.Visible = true;

            toLabel.Visible = false;
            toPicker.Visible = false;
            fromLabel.Visible = false;
            fromPicker.Visible = false;

            promptModel.Host = hostBox.Text;
            promptModel.UserName = userNameBox.Text;
            promptModel.UserPass = userPassBox.Text;
            resultPicker.Items.Clear();
            resultPicker.Items.Add("Retrieving list of available result sets...");
            resultPicker.Update();

            try
            {
                LmiHandler lmiHandler = new LmiHandler(promptModel.Host, "", promptModel.UserName, promptModel.UserPass, null, false, DateTime.MinValue, DateTime.MinValue);
                queryEntries = lmiHandler.getQueries();
            } catch (Exception e)
            {
                string message ="Exception:" + e.ToString() ;
                const string caption = "Error while retrieving list of results from LMI";
                var result = MessageBox.Show(message, caption,
                                             MessageBoxButtons.OK,
                                             MessageBoxIcon.Error);
                switchToRunQuery();
                return;
            }

            resultPicker.Items.Clear();
            foreach( QueryEntry queryEntry in queryEntries ) {
                int added = resultPicker.Items.Add(queryEntry.query);
                if (queryEntry.queryId.Equals(promptModel.QueryId))
                {
                    resultPicker.SelectedIndex = added;
                }
            }
        }

        private void switchToCorrResults()
        {
            queryBox.Visible = false;
            resultPicker.Visible = true;

            toLabel.Visible = false;
            toPicker.Visible = false;
            fromLabel.Visible = false;
            fromPicker.Visible = false;

            promptModel.Host = hostBox.Text;
            promptModel.UserName = userNameBox.Text;
            promptModel.UserPass = userPassBox.Text;
            resultPicker.Items.Clear();
            resultPicker.Items.Add("Retrieving list of available correlation result sets...");
            resultPicker.Update();

            try
            {
                LmiHandler lmiHandler = new LmiHandler(promptModel.Host, "", promptModel.UserName, promptModel.UserPass, null, true, DateTime.MinValue, DateTime.MinValue);
                queryEntries = lmiHandler.getQueries();
            }
            catch (Exception e)
            {
                string message = "Exception:" + e.ToString();
                const string caption = "Error while retrieving list of correlation results from LMI";
                var result = MessageBox.Show(message, caption,
                                             MessageBoxButtons.OK,
                                             MessageBoxIcon.Error);
                switchToRunQuery();
                return;
            }

            resultPicker.Items.Clear();
            foreach (QueryEntry queryEntry in queryEntries)
            {
                int added = resultPicker.Items.Add(queryEntry.query);
                if (queryEntry.queryId.Equals(promptModel.QueryId))
                {
                    resultPicker.SelectedIndex = added;
                }
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if ( sender == newQueryChoice && newQueryChoice.Checked)
            {
                switchToRunQuery();
            } else if (sender == existingResultsChoice &&  existingResultsChoice.Checked)
            {
                switchtoPickQuery();
            } else if ( sender == runCorrChoice && runCorrChoice.Checked)
            {
                switchToRunCorr();
            } else if ( sender == corrResultsChoice && corrResultsChoice.Checked)
            {
                switchToCorrResults();
            }
        }
    }
}
