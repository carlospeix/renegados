@ECHO OFF
CD bin\Debug
..\..\..\packages\OpenCover.4.5.2316\OpenCover.Console.exe -register:user -target:"..\..\..\packages\NUnit.Runners.2.6.3\tools\nunit-console.exe" -targetargs:"CustomWorkflowsTest.dll" -output:"..\..\CodeCoverageReports\OpenCoverResults.xml" -skipautoprops
..\..\..\packages\ReportGenerator.1.9.1.0\ReportGenerator.exe "-reports:..\..\CodeCoverageReports\OpenCoverResults.xml" -targetdir:..\..\CodeCoverageReports"
CD ..\..
CodeCoverageReports\index.htm
