/*
 * Copyright © 2018. TIBCO Software Inc.
 * This file is subject to the license terms contained
 * in the license file that is distributed with this file.
 */
using Spotfire.Dxp.Application;
using Spotfire.Dxp.Application.Extension;
using System;
using System.Collections.Generic;

namespace LMIDataSource
{
    class AddLMIDataSourceTool : CustomTool<AnalysisApplication>
    {

        public AddLMIDataSourceTool(string menuText) : base(menuText)
        {
        }

        protected override IEnumerable<object> ExecuteAndPromptCore(AnalysisApplication context)
        {
            LMIDataSource.LmiDataSource lMIDataSource = new LmiDataSource();

            LmiDataSourcePromptModel promptModel = lMIDataSource.createPromptModel();
            yield return promptModel;

            lMIDataSource.updateFromPromptReturn(promptModel);
           
            if (context.Document == null)
            {
                context.Open(lMIDataSource);
            }
            else
            {
                context.Document.Data.Tables.Add("dataTableName", lMIDataSource);
            }
           
        }

        protected override void ExecuteCore(AnalysisApplication context)
        {
            LMIDataSource.LmiDataSource lMIDataSource = new LmiDataSource();
            if (context.Document == null)
            {
                context.Open(lMIDataSource);
            }
            else
            {
                context.Document.Data.Tables.Add("dataTableName", lMIDataSource);
            }
        }

        protected override bool GetSupportsPromptingCore()
        {
            return true;
        }

        protected override bool IsEnabledCore(AnalysisApplication context)
        {
            return true;
        }

        protected override bool IsVisibleCore(AnalysisApplication context)
        {
            return true;
        }
    }
}
