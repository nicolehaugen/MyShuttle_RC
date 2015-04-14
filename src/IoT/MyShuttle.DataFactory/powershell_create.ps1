

# https://msdn.microsoft.com/en-us/library/azure/dn820234.aspx  
# switch-azuremode AzureResourceManager

New-AzureDataFactoryLinkedService -ResourceGroupName MyShuttle2 -DataFactoryName MyShuttle2 -File StorageLinkedService.json 
New-AzureDataFactoryLinkedService -ResourceGroupName MyShuttle2 -DataFactoryName MyShuttle2 -File AzureSqlLinkedService.json
New-AzureDataFactoryLinkedService -ResourceGroupName MyShuttle2 -DataFactoryName MyShuttle2 -File HDInsightOnDemandClusterLinkedService.json
New-AzureDataFactoryLinkedService -ResourceGroupName MyShuttle2 -DataFactoryName MyShuttle2 -File MyHDInsightClusterLinkedService.json


New-AzureDataFactoryTable  -ResourceGroupName MyShuttle2 -DataFactoryName MyShuttle2 –File "AzureSQLTableOutput.json"
New-AzureDataFactoryTable  -ResourceGroupName MyShuttle2 -DataFactoryName MyShuttle2 –File "HiveInputBlobTable.json"
New-AzureDataFactoryTable  -ResourceGroupName MyShuttle2 -DataFactoryName MyShuttle2 –File "HiveOutputBlobTable.json"
New-AzureDataFactoryTable  -ResourceGroupName MyShuttle2 -DataFactoryName MyShuttle2 –File "HiveAccelerometerAggregateOutputBlobTable.json"

New-AzureDataFactoryPipeline  -ResourceGroupName MyShuttle2 -DataFactoryName MyShuttle2 -File "SecurityBeltWarningsPipeline.json"
New-AzureDataFactoryPipeline  -ResourceGroupName MyShuttle2 -DataFactoryName MyShuttle2 -File "AccelerometerAggregatePipeline.json"

Set-AzureDataFactoryPipelineActivePeriod -ResourceGroupName MyShuttle2 -DataFactoryName MyShuttle2 -StartDateTime 2015-02-12Z –EndDateTime 2015-02-12Z –Name SecurityBeltWarningsPipeline
Set-AzureDataFactoryPipelineActivePeriod -ResourceGroupName MyShuttle2 -DataFactoryName MyShuttle2 -StartDateTime 2015-02-12Z –EndDateTime 2015-02-12Z –Name AccelerometerAggregatePipeline

