/*
 * Copyright © 2018. TIBCO Software Inc.
 * This file is subject to the license terms contained
 * in the license file that is distributed with this file.
 */
 using LMIDataSource;
using Spotfire.Dxp.Application.Extension;
using Spotfire.Dxp.Web.Forms;

namespace LMIDataSourceWeb
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
            //System.Diagnostics.Debugger.Launch();
            base.RegisterViews(registrar);

            // This tells the framework to show the HttpDataSourceDialog when 
            // the system is asked to prompt on the HttpDataSourcePromptModel.
            registrar.Register(typeof(PromptControl), typeof(LmiDataSourcePromptModel), typeof(MyCustomPanelView));
        }
    }
}
