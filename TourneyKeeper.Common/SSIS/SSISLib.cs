using Microsoft.SqlServer.Management.IntegrationServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TourneyKeeperCommon.SSIS
{
    public static class SSISLib
    {
        public static Tuple<Operation.ServerOperationStatus, string> RunSSIS(string targetConnectionString)
        {
            SqlConnection ssisConnection = new SqlConnection(targetConnectionString);
            IntegrationServices ssisServer = new IntegrationServices(ssisConnection);
            ProjectInfo testProject = ssisServer.Catalogs["SSISDB"].Folders["TK"].Projects["DataImporter"];
            PackageInfo testPackage = testProject.Packages["CopyData.dtsx"];

            Collection<PackageInfo.ExecutionValueParameterSet> executionValueParameterSet = new Collection<PackageInfo.ExecutionValueParameterSet>();
            executionValueParameterSet.Add(new PackageInfo.ExecutionValueParameterSet { ParameterName = "SYNCHRONIZED", ParameterValue = 1, ObjectType = 50 });

            var executionIdentifier = testPackage.Execute(false, null, executionValueParameterSet);
            ExecutionOperation executionOperation = ssisServer.Catalogs["SSISDB"].Executions[executionIdentifier];

            while (!executionOperation.Completed)
            {
                Thread.Sleep(1000);
                executionOperation.Refresh();
            }

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Messages:");

            foreach (var msg in executionOperation.Messages)
            {
                sb.AppendLine($"{msg.Message}");
            }

            switch (executionOperation.Status)
            {
                case Operation.ServerOperationStatus.Success:
                    sb.Append("SQL Server Agent job, RunSISSPackage, finished successfully.");
                    break;
                default:
                    sb.Append($"SQL Server Agent job, RunSISSPackage, failure. Error status: {executionOperation.Status.ToString()}");
                    break;
            }

            return new Tuple<Operation.ServerOperationStatus, string>(executionOperation.Status, sb.ToString());
        }
    }
}