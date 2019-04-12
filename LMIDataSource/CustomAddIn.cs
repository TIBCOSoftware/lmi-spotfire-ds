/*
 * Copyright © 2018. TIBCO Software Inc.
 * This file is subject to the license terms contained
 * in the license file that is distributed with this file.
 */
using Spotfire.Dxp.Application.Extension;

namespace LMIDataSource
{
    /// <summary>
    /// </summary>
    public sealed class CustomAddIn : AddIn
    {
        /// <summary>Registers the LMI data source, this will make the data source avaliable for selection
        /// in various user interface. The type identifier for the data source contains the information on the text to show
        /// in those user interfaces.
        /// </summary>
        /// <param name="registrar">The registrar where the data source is registered.</param>
        protected override void RegisterDataSources(DataSourceRegistrar registrar)
        {
            base.RegisterDataSources(registrar);

            registrar.Register<LmiDataSource>(LmiCustomDataSourceIdentifiers.LmiDataSource);
        }

        protected override void RegisterTools(ToolRegistrar registrar)
        {
            base.RegisterTools(registrar);

            CustomMenuGroup menuGroup = new CustomMenuGroup("LogLogic data source");

            registrar.Register(new AddLMIDataSourceTool("Add advanced search data..."), menuGroup);

        }
    }
}
