/*
 * Copyright © 2018. TIBCO Software Inc.
 * This file is subject to the license terms contained
 * in the license file that is distributed with this file.
 */
using System;

namespace LMIDataSource
{
    public sealed class LmiDataSourcePromptModel
    {

        /// <summary>Initializes a new instance of the HttpDataSourcePromptModel class.
        /// </summary>
        /// <param name="host">The initial value of the URI.</param>
        public LmiDataSourcePromptModel(String host, String query, String userName, String userPass, String queryId, bool isCorrelation, DateTime from, DateTime to)
        {
            Host = host;
            Query = query;
            UserName = userName;
            UserPass = userPass;
            QueryId = queryId;
            IsCorrelation = isCorrelation;
            From = from;
            To = to;
        }

        /// <summary>Gets or sets the URI where the data source should retrieve the data set from.
        /// </summary>
        public string Host
        {
            get; set;
        }

        /// <summary>Gets or sets the URI where the data source should retrieve the data set from.
        /// </summary>
        public string Query
        {
            get; set;
        }

        public string UserName
        {
            get; set;
        }

        public string UserPass
        {
            get; set;
        }

        public string QueryId { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public bool IsCorrelation { get; set; } = false;
    }
}
