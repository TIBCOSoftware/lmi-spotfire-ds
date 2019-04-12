/*
 * Copyright © 2018. TIBCO Software Inc.
 * This file is subject to the license terms contained
 * in the license file that is distributed with this file.
 */
namespace LMIDataSource
{
    using Spotfire.Dxp.Data;
    using System;
    using System.Collections.Generic;

    internal sealed class LmiDataSourceConnection : DataSourceConnection
    {
        /// <summary>The data source that created this connection.
        /// </summary>
        private readonly LmiDataSource dataSource;

        /// <summary>The prompt mode, needed since we use another data source inside
        /// this one.
        /// </summary>
        private readonly DataSourcePromptMode promptMode;

        /// <summary>The enumerator over the prompt models.
        /// </summary>
        private readonly IEnumerable<object> promptModels;

        private LmiHandler lmiHandler;
    

        /// <summary>Initializes a new instance of the HttpDataSourceConnection class.
        /// </summary>
        /// <param name="dataSource">The data source that created this connection.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="promptMode">The prompt mode.</param>
        /// <param name="promptModels">The enumerator over the prompt models.</param>
        public LmiDataSourceConnection(
            LmiDataSource dataSource,
            IServiceProvider serviceProvider,
            DataSourcePromptMode promptMode,
            IEnumerable<object> promptModels)
            : base(dataSource, serviceProvider)
        {
            this.promptModels = promptModels;
            this.dataSource = dataSource;
            this.promptMode = promptMode;
        }

        /// <summary>Disposes external resources used by the connection.
        /// this implementation will dispose of the query if necessary
        /// </summary>
        /// <param name="disposing">Should we dispose.</param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (lmiHandler != null)
                    {
                        lmiHandler.closeHandler();
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>This method should return a reader that actually return data.
        /// This implementation prepares a LmiHandler for further queries
        /// </summary>
        /// <returns>A reader which returns the rows from the data.</returns>
        protected override DataRowReader ExecuteQueryCore2()
        {
            // Check we have already connected to the data source.
            if (lmiHandler == null)
            {
                lmiHandler = new LmiHandler(dataSource.Host,dataSource.Query, dataSource.UserName, dataSource.UserPass, 
                    dataSource.QueryId, dataSource.IsCorrelation, dataSource.From, dataSource.To);
            }

            // Retrieve the reader from the inner text file data source connection.
            return new LmiDataRowReader(lmiHandler);
        }

        /// <summary>Return the prompt models to the framework which will perform the prompting.
        /// </summary>
        /// <remarks>This method will be called directly after the connection object has been created
        /// in the Connect method on the data source. In the implementation of the LmiDataSource this
        /// will cause the data source to be modified after the prompting has been done.</remarks>
        /// <returns>The prompt models enumerator.</returns>
        protected override IEnumerable<object> GetPromptModelsCore()
        {
            return this.promptModels;
        }
    }
}
