/*
 * Copyright © 2018. TIBCO Software Inc.
 * This file is subject to the license terms contained
 * in the license file that is distributed with this file.
 */
namespace LMIDataSource
{
    using Spotfire.Dxp.Application.Extension;
    using Spotfire.Dxp.Data;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// This class implements the methods used by Spotfire to get the data from a DataSource, row by row.
    /// </summary>
    public sealed class LmiDataRowReader : CustomDataRowReader
    {
        private int rowIdx;
        private ResultsDesc lastResults;
        private readonly DataRowReaderColumn[] columns;
        private DataValueCursor[] cursors;
        private LmiHandler lmiHandler;
        private QueryDesc queryDesc;
        private readonly ResultProperties resultProperties;

        private void initQuery()
        {
            queryDesc = lmiHandler.getQueryDesc();
            rowIdx = -1;
            lastResults = null;
        }

        public LmiDataRowReader(LmiHandler lmiHandler)
        {
            this.lmiHandler = lmiHandler;
            initQuery();

            resultProperties = new ResultProperties();
            resultProperties.SetProperty("Host", lmiHandler.Host);
            resultProperties.SetProperty("UserName", lmiHandler.UserName);
            resultProperties.SetProperty("Query", lmiHandler.Query);
            if ( lmiHandler.QueryId != null)
            {
                resultProperties.SetProperty("QueryID", lmiHandler.QueryId);
            }
            if (lmiHandler.IsCorrelation)
            {
                resultProperties.SetProperty("Correlation", "true");
            } else
            {
                resultProperties.SetProperty("Correlation", "false");
            }
            columns = new DataRowReaderColumn[queryDesc.columns.Length];
            cursors = new DataValueCursor[queryDesc.columns.Length];

            int i = 0;
            foreach (ColumnDesc column in queryDesc.columns)
            {
                switch (column.type)
                {
                    case "TIMESTAMP":
                        cursors[i] = DataValueCursor.CreateMutableCursor(DataType.DateTime);
                        columns[i] = new DataRowReaderColumn(column.name, DataType.DateTime, cursors[i]);
                        break;
                    case "INT":
                        cursors[i] = DataValueCursor.CreateMutableCursor(DataType.Integer);
                        columns[i] = new DataRowReaderColumn(column.name, DataType.Integer, cursors[i]);
                        break;
                    case "LONG":
                        cursors[i] = DataValueCursor.CreateMutableCursor(DataType.LongInteger);
                        columns[i] = new DataRowReaderColumn(column.name, DataType.LongInteger, cursors[i]);
                        break;
                    case "DOUBLE":
                        cursors[i] = DataValueCursor.CreateMutableCursor(DataType.Real);
                        columns[i] = new DataRowReaderColumn(column.name, DataType.Real, cursors[i]);
                        break;

                    case "BOOLEAN":
                        cursors[i] = DataValueCursor.CreateMutableCursor(DataType.Boolean);
                        columns[i] = new DataRowReaderColumn(column.name, DataType.Boolean, cursors[i]);
                        break;
                    //case "INET_ADDR":
                    default:
                        cursors[i] = (MutableValueCursor<string>)DataValueCursor.CreateMutableCursor(DataType.String);
                        columns[i] = new DataRowReaderColumn(column.name, DataType.String, cursors[i]);
                        break;
                }
                i++;
            }
        }

        protected override IEnumerable<DataRowReaderColumn> GetColumnsCore()
        {
            return columns;
        }

        protected override ResultProperties GetResultPropertiesCore()
        {
            return resultProperties;
        }

        static DateTime EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);

        protected override bool MoveNextCore()
        {
            if (lastResults == null || rowIdx == (lastResults.rows.Length - 1))
            {
                lastResults = lmiHandler.nextResults();
                rowIdx = -1;
                if (!lastResults.hasMore && lastResults.rows.Length == 0)
                {
                    return false;
                }
            }
            rowIdx++;
            for (int i = 0; i < cursors.Length; i++)
            {
                if (lastResults.rows[rowIdx][i] == null)
                {
                    cursors[i].CurrentDataValue.SetNullValue();
                    continue;
                }
                if (cursors[i] is MutableValueCursor<String>)
                {
                    ((MutableValueCursor<String>)cursors[i]).MutableDataValue.ValidValue = lastResults.rows[rowIdx][i];
                }
                else if (cursors[i] is MutableValueCursor<DateTime>)
                {
                    long dateTimeLong = Convert.ToInt64(lastResults.rows[rowIdx][i]);
                    DateTime dateTime = EPOCH.AddMilliseconds(dateTimeLong).ToLocalTime();
                    ((MutableValueCursor<DateTime>)cursors[i]).MutableDataValue.ValidValue = dateTime;
                }
                else if (cursors[i] is MutableValueCursor<int>)
                {
                    int intValue = Convert.ToInt32(lastResults.rows[rowIdx][i]);
                    ((MutableValueCursor<int>)cursors[i]).MutableDataValue.ValidValue = intValue;
                }
                else if (cursors[i] is MutableValueCursor<long>)
                {
                    long longValue = Convert.ToInt64(lastResults.rows[rowIdx][i]);
                    ((MutableValueCursor<long>)cursors[i]).MutableDataValue.ValidValue = longValue;
                }
                else if (cursors[i] is MutableValueCursor<double>)
                {
                    double doubleValue = Convert.ToDouble(lastResults.rows[rowIdx][i]);
                    ((MutableValueCursor<double>)cursors[i]).MutableDataValue.ValidValue = doubleValue;
                }
                else if (cursors[i] is MutableValueCursor<bool>)
                {
                    bool boolValue = Convert.ToBoolean(lastResults.rows[rowIdx][i]);
                    ((MutableValueCursor<bool>)cursors[i]).MutableDataValue.ValidValue = boolValue;
                }
            }
            return lastResults.hasMore || rowIdx != (lastResults.rows.Length - 1);
        }

        protected override void ResetCore()
        {
            if (rowIdx != -1)
            {
                initQuery();
            }
        }
    }
}
