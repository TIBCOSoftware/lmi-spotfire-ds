/*
 * Copyright © 2018. TIBCO Software Inc.
 * This file is subject to the license terms contained
 * in the license file that is distributed with this file.
 */
namespace LMIDataSource
{
     using Spotfire.Dxp.Application.Extension;

    /// <summary>Type identifiers for custom data sources. The type identifiers are both used
    /// to uniquely identify a data source and to provide texts that are shown in user interfaces 
    /// where one can select data sources.
    /// </summary>
    public sealed class LmiCustomDataSourceIdentifiers : CustomTypeIdentifiers
    {
        #region Constants and Fields

        /// <summary>Type identifier for the <see cref="LmiDataSouce"/>.
        /// </summary>
        public static readonly CustomTypeIdentifier LmiDataSource =
            CreateTypeIdentifier(
                "LmiDataSource",
                Properties.Resources.LmiDataSource_DisplayName,
                Properties.Resources.LmiDataSource_Description);

        #endregion
    }
}
