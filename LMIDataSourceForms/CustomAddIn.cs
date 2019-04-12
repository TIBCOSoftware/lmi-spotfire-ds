/*
 * Copyright © 2018. TIBCO Software Inc.
 * This file is subject to the license terms contained
 * in the license file that is distributed with this file.
 */
 using LMIDataSource;
using Spotfire.Dxp.Application.Extension;
using System.Windows.Forms;

namespace LMIDataSourceForms
{
    /// <summary>
    /// </summary>
    public sealed class CustomAddIn : AddIn
    {

        /// <summary>Register the mapping between the prompt model and the dialog to show.
        /// </summary>
        /// <param name="registrar">The view registrar.</param>
        protected override void RegisterViews(ViewRegistrar registrar)
        {
            base.RegisterViews(registrar);

            // This tells the framework to show the HttpDataSourceDialog when 
            // the system is asked to prompt on the HttpDataSourcePromptModel.
            registrar.Register(typeof(Form), typeof(LmiDataSourcePromptModel), typeof(LmiDataSourceDialog));
        }
    }
}
