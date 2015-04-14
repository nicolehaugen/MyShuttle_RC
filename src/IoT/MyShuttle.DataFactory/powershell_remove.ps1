

# https://msdn.microsoft.com/en-us/library/azure/dn820234.aspx


Remove-AzureDataFactoryPipeline -ResourceGroupName MyShuttle2 -DataFactoryName MyShuttle2 -Name SecurityBeltWarningsPipeline -force
Remove-AzureDataFactoryPipeline -ResourceGroupName MyShuttle2 -DataFactoryName MyShuttle2 -Name AccelerometerAggregatePipeline -force

Remove-AzureDataFactoryTable -ResourceGroupName MyShuttle2 -DataFactoryName MyShuttle2 -Name AzureSQLTableOutput -force
Remove-AzureDataFactoryTable -ResourceGroupName MyShuttle2 -DataFactoryName MyShuttle2 -Name HiveInputBlobTable -force
Remove-AzureDataFactoryTable -ResourceGroupName MyShuttle2 -DataFactoryName MyShuttle2 -Name HiveOutputBlobTable -force
Remove-AzureDataFactoryTable -ResourceGroupName MyShuttle2 -DataFactoryName MyShuttle2 -Name HiveAccelerometerAggregateBlobTable -force


Remove-AzureDataFactoryLinkedService -ResourceGroupName MyShuttle2 -DataFactoryName MyShuttle2 -Name HDInsightOnDemandCluster -force
Remove-AzureDataFactoryLinkedService -ResourceGroupName MyShuttle2 -DataFactoryName MyShuttle2 -Name MyHDInsightCluster -force
Remove-AzureDataFactoryLinkedService -ResourceGroupName MyShuttle2 -DataFactoryName MyShuttle2 -Name AzureStorage  -force
Remove-AzureDataFactoryLinkedService -ResourceGroupName MyShuttle2 -DataFactoryName MyShuttle2 -Name AzureSQLDatabase  -force





