# LMI Data Source for TIBCO Spotfire®

## Description
The LMI Data Source for TIBCO Spotfire® software creates a new data source type that connects to an TIBCO LogLogic® Log Management Intelligence (LMI) instance.
There are four ways to use this LogLogic® LMI data source:
- Run a new query (EQL or SQL formatted)
- Retrieve results for an existing query (a tab in the UI)
- Run a new Correlation query (ECL formatted)
- Retrieve results from an existing correlation search (a tab in the UI)

Results are retrieved from LogLogic LMI and stored in Spotfire® memory. Once this is done all the usual Spotfire interactions with the data are possible.

This works with both the analyst client and the web client.

## Pre-requisites
This code has been tested with TIBCO Spotfire® 7.11 and TIBCO Spotfire® X (10.3.0) and
TIBCO LogLogic® Log Management Intelligence 6.2.0 and above

## Package installation
The provided .spk file must be uploaded to the Spotfire Server instance, and deployed in an area.
If you are using the web client, the web player must be updated, as indicated in the web player management screen.
For the analyst client, the update will be pushed at next restart. You need to accept the installation of the non-signed extensions the first time.

## Usage
You need to fill in the hostname for your LogLogic LMI host, and a user/password pair for authentication.
For the analyst client, the certificate checking may pop-up a prompt for you to review and accept.
For the web client, the LogLogic LMI certificate MUST be trusted (i.e., it must be from a well known issuer, and match the hostname).
If needed, it works with Let's Encrypt certificates and  wildcard Let's Encrypt certificates.

For every EQL/SQL query, you must include a filter on sys_eventTime (e.g., for EQL '| sys_eventTime in -1h')
You cannot use bloks.

For ECL, you need to type in the whole content of an ECL blok, and add a line on top that gives the rule name using the syntax:
Rule <rule name>
  
You can add a new LogLogic LMI data source using either:
(+) Other / "From LogLogic LMI Advanced Search" (this only works for TIBCO Spotfire® analyst);
or
Tools / LogLogic data sources / Add advanced search data 

For TIBCO Spotfire® Web, you need to add another (dummy) data source before being able to add an LMI data source using the tools menu.

## Building the package

You need Microsoft Visual Studio 2017 and the TIBCO Spotfire® SDK for the targeted release.
In build.ps1 script, update the following variables according to the locations of the SDK and the sources.

```
$env:MSBUILDPATH="C:\Program Files (x86)\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\"
$env:SPOTFIREPACKAGEBUILDERCONSOLEPATH="<path to>\TIBCO Spotfire SDK\10.1.0\SDK\Package Builder\"
$env:BUILDHOME="<path to>\LMIDataSource"
```

Then launch the build script, the final artifact is LmiDataSource.sdn

