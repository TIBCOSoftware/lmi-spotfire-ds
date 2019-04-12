/*
 * Copyright © 2018. TIBCO Software Inc.
 * This file is subject to the license terms contained
 * in the license file that is distributed with this file.
 */
using System;
using System.Collections.Generic;

namespace LMIDataSource
{
    using Spotfire.Dxp.Application.Extension;
    using Spotfire.Dxp.Data;
    using Spotfire.Dxp.Data.Exceptions;
    using Spotfire.Dxp.Framework.ApplicationModel;
    using Spotfire.Dxp.Framework.Persistence;
    using System.Runtime.Serialization;

    /// <summary>
    /// This class implements a Spotfire CustomDataSource that gets its data from an LMI instance.
    /// There are 4 use cases:
    /// Running a new EQL/SQL query
    /// Using results from an existing EQL/SQL query
    /// Running a new correlation query (ECL)
    /// Using results from an existing ECL query
    /// </summary>
    [Serializable]
    [PersistenceVersion(1, 0)]
    public sealed class LmiDataSource : CustomDataSource
    {
        public String Host { get; set; } = "";

        public String UserName { get; set; } = "";

        public String UserPass { get; set; } = "";

        public String Query { get; set; } = "";

        public String QueryId { get; set; } = null;

        public Boolean IsCorrelation { get; set; } = false;

        public DateTime From { get; set; } = DateTime.Now.AddHours(-1);

        public DateTime To { get; set; } = DateTime.Now;

        public LmiDataSource()
        {
            // empty, default values are assigned by fields definition
        }

        LmiDataSource(SerializationInfo info, StreamingContext context)
           : base(info, context)
        {
            Host = (String)info.GetValue("host", typeof(String));
            Query = (String)info.GetValue("query", typeof(String));
            UserName = (String)info.GetValue("userName", typeof(String));
            UserPass = (String)info.GetValue("userPass", typeof(String));
            QueryId = (String)info.GetValue("queryId", typeof(String));
            IsCorrelation = (Boolean)info.GetValue("isCorrelation", typeof(Boolean));
            From = (DateTime)info.GetValue("from", typeof(DateTime));
            To = (DateTime)info.GetValue("to", typeof(DateTime));
        }

        public override bool IsLinkable
        {
            get
            {
                return true;
            }
        }

        protected override bool SupportsWebPromptingCore()
        {
            return true;
        }

        /// <summary>Serialization handling. This method is called when the data source is saved or cloned.
        /// </summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The serialization context.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("host", Host, typeof(Uri));
            info.AddValue("query", Query, typeof(String));
            info.AddValue("userName", UserName, typeof(String));
            info.AddValue("userPass", UserPass, typeof(String));
            info.AddValue("queryId", QueryId, typeof(String));
            info.AddValue("isCorrelation", IsCorrelation, typeof(Boolean));
            info.AddValue("from", From, typeof(DateTime));
            info.AddValue("to", To, typeof(DateTime));
        }

        /// <summary>Create a new connection and prompt if needed.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="promptMode">The prompt mode.</param>
        /// <returns>A connection to the data source.</returns>
        protected override DataSourceConnection ConnectCore(
            IServiceProvider serviceProvider, DataSourcePromptMode promptMode)
        {
            // Retrieve the prompt service.
            PromptService promptService = GetService<PromptService>(serviceProvider);

            // Check if it allowed to show a UI
            bool isPromptingAllowed = promptService.IsPromptingAllowed;

            // Check if host/username/password works
            bool connectionOk = false;
            if (!Host.Equals("") && !UserName.Equals("") && !UserPass.Equals(""))
            {
                try
                {
                    LmiHandler lmiHandler = new LmiHandler(Host, "", UserName, UserPass, null, false, From, To);
                    lmiHandler.checkConnection();
                    connectionOk = true;
                }
                catch (Exception)
                {
                    // just ignored at this stage
                }
            }
            // Check if we need to prompt
            bool needsPrompting = !connectionOk || QueryId == null;

            if (!isPromptingAllowed && needsPrompting)
            {
                // It is not allowed to show the prompt UI and there is not enough information
                // to retrieve data so an exception needs to be thrown and import will be aborted.
                throw new ImportException("User prompt needed but disallowed by prompt mode or application mode.");
            }

            // Generate an enumerator for the prompt models.
            IEnumerable<object> promptModels = new List<object>();

            // 2. Prompt for Uri if it is allowed to show UI
            if (isPromptingAllowed)
            {
                switch (promptMode)
                {
                    case DataSourcePromptMode.All:

                        // Always prompt
                        promptModels = this.Prompt();
                        break;
                    case DataSourcePromptMode.RequiredOnly:

                        // Prompt only if needed
                        if (needsPrompting)
                        {
                            promptModels = this.Prompt();
                        }
                        break;
                    case DataSourcePromptMode.None:

                        // No prompting.
                        break;
                    default:

                        // No prompting.
                        break;
                }
            }

            // 3. Create and return a connection
            return new LmiDataSourceConnection(this, serviceProvider, promptMode, promptModels);
        }

        /// <summary>Handle prompting for this data source. This method is written using the
        /// yield return construct to be able to handle asynchronous prompting. After the first yeild
        /// return one can assume that the prompting has been successfully performed.
        /// </summary>
        /// <returns>The prompt models.</returns>
        private IEnumerable<object> Prompt()
        {
            LmiDataSourcePromptModel promptModel = createPromptModel();

            // Return the prompt model to the prompting framework.
            yield return promptModel;

            // When we get to this point we know that the prompting has been performed, so we assign
            // the properties to the model.
            updateFromPromptReturn(promptModel);
        }

        public LmiDataSourcePromptModel createPromptModel()
        {
            return new LmiDataSourcePromptModel(Host, Query, UserName, UserPass, QueryId, IsCorrelation, From, To);
        }

        public void updateFromPromptReturn(LmiDataSourcePromptModel promptModel)
        {
            Host = promptModel.Host;
            Query = promptModel.Query;
            UserName = promptModel.UserName;
            UserPass = promptModel.UserPass;
            QueryId = promptModel.QueryId;
            IsCorrelation = promptModel.IsCorrelation;
            From = promptModel.From;
            To = promptModel.To;
        }

        public override string Name
        {
            get
            {
                string desc;
                if (!IsCorrelation)
                {
                    if (QueryId == null)
                    {
                        desc = "run query: " + Query;
                    }
                    else
                    {
                        desc = "ResultSet " + QueryId;
                    }
                }
                else
                {
                    if (QueryId == null)
                    {
                        desc = "run correlqiton query: " + Query;
                    }
                    else
                    {
                        desc = "Correlation ResultSet: " + QueryId;
                    }
                }

                return "LMI Data source (" + desc + ")";
            }
        }
    }
}
