/*
 * Copyright © 2018. TIBCO Software Inc.
 * This file is subject to the license terms contained
 * in the license file that is distributed with this file.
 */
using Spotfire.Dxp.Framework.HttpClient;
using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace LMIDataSource
{
    [DataContract]
    public sealed class ColumnDesc
    {
        [DataMember]
        public string type;

        [DataMember]
        public string name;
    }

    [DataContract]
    public sealed class QueryDesc
    {
        [DataMember]
        public string queryId;

        [DataMember]
        public ColumnDesc[] columns;
    }

    [DataContract]
    public sealed class CorrQueryDesc
    {
        [DataMember]
        public string id;

        [DataMember]
        public ColumnDesc[] columnDescriptors;
    }


    [DataContract]
    public sealed class CorrResultsDesc
    {
        [DataMember]
        public int from;

        [DataMember]
        public int count;

        [DataMember]
        public int totalCount;

        [DataMember]
        public string[][] results;
    }

    [DataContract]
    public sealed class ResultsDesc
    {
        [DataMember]
        public int offset;

        [DataMember]
        public string[][] rows;

        [DataMember]
        public Boolean hasMore;
    }

    [DataContract]
    public sealed class QueryPost
    {
        [DataMember]
        public string query;

        [DataMember]
        public Boolean cached = false;

        [DataMember]
        public int timeToLive = 600;
    }

    [DataContract]
    public sealed class QueryEntry
    {
        [DataMember]
        public string queryId;

        [DataMember]
        public string query;

        [DataMember]
        public double progress;
    }

    [DataContract]
    public sealed class CorrResultState
    {
        [DataMember]
        public string resultId;

        [DataMember]
        public string query;
    }

    [DataContract]
    public sealed class CorrQueryEntry
    {
        [DataMember]
        public string instanceId;

        [DataMember]
        public string ecl;

        [DataMember]
        public CorrResultState[] resultStates;

    }

    [DataContract]
    public sealed class CorrInstances
    {
        [DataMember]
        public CorrQueryEntry[] instanceStates;
    }

    [DataContract]
    public sealed class CorrelationQueryPost
    {
        [DataMember]
        public string instanceType = "replay";

        [DataMember]
        public string ecl;

        [DataMember]
        public string startDate;

        [DataMember]
        public string endDate;
    }

    [DataContract]
    public sealed class ValidateCorrelationRule
    {
        [DataMember]
        public string rule;
    }

    /// <summary>
    /// This class implements all REST API calls neededby the SpotFire extension Data Srouce to interact with an LMI instance.
    ///
    /// </summary>
    public class LmiHandler
    {
        public string Host { get; }
        public string UserName { get; }
        readonly string userPass;
        public string Query { get; }
        public string QueryId { get; }
        public bool IsCorrelation { get; }
        QueryDesc queryDesc;
        int offset = 0;
        public DateTime From { get; }
        public DateTime To { get; }

        public LmiHandler(string host, string query, string userName, string userPass, string queryId,
            bool isCorrelation, DateTime from, DateTime to)
        {
            Host = host;
            Query = query;
            UserName = userName;
            this.userPass = userPass;
            QueryId = queryId;
            IsCorrelation = isCorrelation;
            From = from;
            To = to;
        }

        private WebRequest createWebRequest(string path)
        {
            int port = IsCorrelation ? 9682 : 9681;
            string address = string.Format("https://{0}:{1}{2}", Host, port, path);

            WebRequest webRequest = WebRequestFactory.CreateSpotfireWebRequest(new Uri(address));
            webRequest.Credentials = new NetworkCredential(UserName, userPass);
            return webRequest;
        }

        internal void validateCorrelationRule()
        {
            string path = "/api/v1/validate/correlation/rule";
            WebRequest webRequest = createWebRequest(path);
            webRequest.Method = "put";
            ValidateCorrelationRule put = new ValidateCorrelationRule();
            put.rule = Query;

            DataContractJsonSerializer qser = new DataContractJsonSerializer(put.GetType());
            qser.WriteObject(webRequest.GetRequestStream(), put);
            webRequest.ContentType = "application/json";

            try
            {
                WebResponse myWebResponse = webRequest.GetResponse();
                StreamReader reader = new StreamReader(myWebResponse.GetResponseStream());
                string text = reader.ReadToEnd();
            }
            catch (WebException we)
            {
                StreamReader r = new StreamReader(we.Response.GetResponseStream());
                string t = r.ReadToEnd();

                throw new IOException(String.Format("Error: {0}\n\n{1}", we.Status, t));

            }

        }

        public void checkConnection()
        {
            if (IsCorrelation)
            {
                validateCorrelationRule();
                return;
            }
            string path = "/api/v1/configuration";
            WebRequest webRequest = createWebRequest(path);
            webRequest.Method = "GET";
            WebResponse myWebResponse = webRequest.GetResponse();

            StreamReader reader = new StreamReader(myWebResponse.GetResponseStream());
            string text = reader.ReadToEnd();
        }

        public QueryEntry[] getCorrelationQueries()
        {
            WebRequest webRequest = createWebRequest("/api/v1/activeInstances?type=replay");
            webRequest.Method = "GET";
            WebResponse myWebResponse = webRequest.GetResponse();

            StreamReader reader = new StreamReader(myWebResponse.GetResponseStream());
            string reply = reader.ReadToEnd();
            CorrInstances corrInstances = new CorrInstances();
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(reply));
            DataContractJsonSerializer ser = new DataContractJsonSerializer(corrInstances.GetType());
            corrInstances = ser.ReadObject(ms) as CorrInstances;
            ms.Close();

            ArrayList queryEnries = new ArrayList();

            foreach (CorrQueryEntry corrQueryentry in corrInstances.instanceStates)
            {
                if (corrQueryentry.resultStates.Length == 0)
                    continue;
                if (!corrQueryentry.resultStates[0].resultId.Equals("0"))
                    continue;
                QueryEntry queryEntry = new QueryEntry();
                queryEntry.queryId = corrQueryentry.instanceId;
                queryEntry.query = corrQueryentry.ecl;
                queryEnries.Add(queryEntry);
            }

            return queryEnries.ToArray(typeof(QueryEntry)) as QueryEntry[];
        }

        public QueryEntry[] getQueries()
        {
            if (IsCorrelation)
            {
                return getCorrelationQueries();
            }
            WebRequest webRequest = createWebRequest("/api/v2/queries");
            webRequest.Method = "GET";
            WebResponse myWebResponse = webRequest.GetResponse();

            StreamReader reader = new StreamReader(myWebResponse.GetResponseStream());
            string reply = reader.ReadToEnd();
            QueryEntry[] queries = new QueryEntry[0];
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(reply));
            DataContractJsonSerializer ser = new DataContractJsonSerializer(queries.GetType());
            queries = ser.ReadObject(ms) as QueryEntry[];
            ms.Close();

            return queries;
        }

        public QueryDesc getCorrelationQueryDesc()
        {
            if (queryDesc != null)
            {
                return queryDesc;
            }

            WebRequest webRequest;

            if (QueryId == null)
            {
                webRequest = createWebRequest("/api/v1/instances");
                webRequest.Method = "POST";
                CorrelationQueryPost queryPost = new CorrelationQueryPost();
                string queryToUse = Query;
                if (!queryToUse.TrimStart().ToUpper().StartsWith("RULE"))
                {
                    queryToUse = "RULE Rule1 " + queryToUse;
                }
                queryPost.ecl = String.Format("BEGIN CORRELATION RULESET\n{0}\nEND CORRELATION RULESET", queryToUse);
                queryPost.startDate = From.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                queryPost.endDate = To.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");

                DataContractJsonSerializer qser = new DataContractJsonSerializer(queryPost.GetType());
                qser.WriteObject(webRequest.GetRequestStream(), queryPost);
                webRequest.ContentType = "application/json";
            }
            else
            {
                String path = String.Format("/api/v1/instance/{0}/result/{1}/columns", QueryId, "0");
                webRequest = createWebRequest(path);
                webRequest.Method = "GET";
            }

            WebResponse myWebResponse = webRequest.GetResponse();
            StreamReader reader = new StreamReader(myWebResponse.GetResponseStream());
            string reply = reader.ReadToEnd();
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(reply));
            CorrQueryDesc corrQueryDesc = new CorrQueryDesc();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(corrQueryDesc.GetType());
            corrQueryDesc = ser.ReadObject(ms) as CorrQueryDesc;
            ms.Close();
            queryDesc = new QueryDesc();
            queryDesc.queryId = corrQueryDesc.id;
            queryDesc.columns = corrQueryDesc.columnDescriptors;

            return queryDesc;
        }


        public QueryDesc getQueryDesc()
        {
            if (queryDesc != null)
            {
                return queryDesc;
            }

            if (IsCorrelation)
            {

                return getCorrelationQueryDesc();

            }
            WebRequest webRequest;

            if (QueryId == null)
            {
                webRequest = createWebRequest("/api/v2/query");
                webRequest.Method = "POST";
                QueryPost queryPost = new QueryPost();
                queryPost.query = Query;
                DataContractJsonSerializer qser = new DataContractJsonSerializer(queryPost.GetType());
                qser.WriteObject(webRequest.GetRequestStream(), queryPost);
                webRequest.ContentType = "application/json";
            }
            else
            {
                String path = String.Format("/api/v2/query/" + QueryId + "/details");
                webRequest = createWebRequest(path);
                webRequest.Method = "GET";

            }
            WebResponse myWebResponse;
            try
            {
                myWebResponse = webRequest.GetResponse();
            }
            catch (WebException we)
            {
                myWebResponse = we.Response;
                string replyExcep = "unspecified error";
                if (myWebResponse != null && myWebResponse.GetResponseStream() != null)
                {
                    StreamReader readerExcep = new StreamReader(myWebResponse.GetResponseStream());
                    replyExcep = readerExcep.ReadToEnd();
                }

                throw new Exception("Error while executing query: " + replyExcep);
            }
            StreamReader reader = new StreamReader(myWebResponse.GetResponseStream());
            string reply = reader.ReadToEnd();
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(reply));
            queryDesc = new QueryDesc();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(queryDesc.GetType());
            queryDesc = ser.ReadObject(ms) as QueryDesc;
            ms.Close();

            return queryDesc;
        }

        public ResultsDesc nextCorrelationResults()
        {
            ResultsDesc resultsDesc;
            do
            {
                String path = String.Format("/api/v1/instance/{0}/correlationevents?from={1}&size=5000&type=MESSAGES", queryDesc.queryId, offset);
                WebRequest webRequest = createWebRequest(path);
                webRequest.Method = "GET";
                WebResponse myWebResponse = webRequest.GetResponse();
                StreamReader reader = new StreamReader(myWebResponse.GetResponseStream());
                string reply = reader.ReadToEnd();
                CorrResultsDesc corrResultsDesc = new CorrResultsDesc();
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(reply));
                DataContractJsonSerializer ser = new DataContractJsonSerializer(corrResultsDesc.GetType());
                corrResultsDesc = ser.ReadObject(ms) as CorrResultsDesc;
                resultsDesc = new ResultsDesc();
                offset += corrResultsDesc.count;
                resultsDesc.offset = corrResultsDesc.from;
                resultsDesc.hasMore = corrResultsDesc.from + corrResultsDesc.count != corrResultsDesc.totalCount;
                resultsDesc.rows = corrResultsDesc.results;
                ms.Close();
            } while (resultsDesc.hasMore && resultsDesc.rows.Length == 0);

            return resultsDesc;
        }

        public ResultsDesc nextResults()
        {
            if (IsCorrelation)
            {
                return nextCorrelationResults();
            }
            ResultsDesc resultsDesc;
            do
            {
                String path = String.Format("/api/v2/query/{0}/results?size=5000", queryDesc.queryId);
                WebRequest webRequest = createWebRequest(path);
                webRequest.Method = "GET";
                WebResponse myWebResponse = webRequest.GetResponse();
                StreamReader reader = new StreamReader(myWebResponse.GetResponseStream());
                string reply = reader.ReadToEnd();
                resultsDesc = new ResultsDesc();
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(reply));
                DataContractJsonSerializer ser = new DataContractJsonSerializer(resultsDesc.GetType());
                resultsDesc = ser.ReadObject(ms) as ResultsDesc;
                ms.Close();
            } while (resultsDesc.hasMore && resultsDesc.rows.Length == 0);

            return resultsDesc;
        }

        public void closeHandler()
        {
            if (queryDesc == null || QueryId != null)
            {
                // do not delete if no query ran or queryId was given as input
                return;
            }
            string path;
            if (!IsCorrelation)
            {
                path = String.Format("/api/v2/query/{0}", queryDesc.queryId);
            }
            else
            {
                path = String.Format("/api/v1/instance/{0}", queryDesc.queryId);
            }
            WebRequest webRequest = createWebRequest(path);
            webRequest.Method = "DELETE";
            webRequest.GetResponse();
        }
    }
}
