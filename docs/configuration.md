# Configuration
Azure DevOps Migration Tools are mainly powered by configuration which allows you to control most aspects of the execution flow.

## Configuration tool
If you run `vstssyncmigrator.exe init` you will be launched into a configuration tool that will generate a default file. Using the `init` command will create a `configuration.yml` file in the
working directory. At run time you can specify the configuration file to use.

**Note:** Azure DevOps Migration Tools do not ship with internal default configuration and will not function without one.

To create your config file just type `vstssyncmigrator init` in the directory that you unziped the tools and a minimal `configuration.json` configuration
file will be created. Modify this as you need.

Note that the generated file show all the possible options, you configuration file will probably only need a subset of those shown.

## Global configuration
The global configuration created by the `init` command look like this:

```json
{
	"TelemetryEnableTrace": true,
	"Source": {
		"Collection": "https://sdd2016.visualstudio.com/",
		"Name": "DemoProjs"
	},
	"Target": {
		"Collection": "https://sdd2016.visualstudio.com/",
		"Name": "DemoProjt"
	},
	"ReflectedWorkItemIDFieldName": "TfsMigrationTool.ReflectedWorkItemId",
	"FieldMaps": [{
			"ObjectType": "VstsSyncMigrator.Engine.Configuration.FieldMap.MultiValueConditionalMapConfig",
			"WorkItemTypeName": "*",
			"sourceFieldsAndValues": {
				"Field1": "Value1",
				"Field2": "Value2"
			},
			"targetFieldsAndValues": {
				"Field1": "Value1",
				"Field2": "Value2"
			}
		}, {
			"ObjectType": "VstsSyncMigrator.Engine.Configuration.FieldMap.FieldBlankMapConfig",
			"WorkItemTypeName": "*",
			"targetField": "TfsMigrationTool.ReflectedWorkItemId"
		}, {
			"ObjectType": "VstsSyncMigrator.Engine.Configuration.FieldMap.FieldValueMapConfig",
			"WorkItemTypeName": "*",
			"sourceField": "System.State",
			"targetField": "System.State",
			"valueMapping": {
				"Approved": "New",
				"New": "New",
				"Committed": "Active",
				"In Progress": "Active",
				"To Do": "New",
				"Done": "Closed"
			}
		}, {
			"ObjectType": "VstsSyncMigrator.Engine.Configuration.FieldMap.FieldtoFieldMapConfig",
			"WorkItemTypeName": "*",
			"sourceField": "Microsoft.VSTS.Common.BacklogPriority",
			"targetField": "Microsoft.VSTS.Common.StackRank"
		}, {
			"ObjectType": "VstsSyncMigrator.Engine.Configuration.FieldMap.FieldtoFieldMultiMapConfig",
			"WorkItemTypeName": "*",
            "SourceToTargetMappings" = {
                "SourceField1": "TargetField1",
                "SourceField2": "TargetField2"
            }
		}, {
			"ObjectType": "VstsSyncMigrator.Engine.Configuration.FieldMap.FieldtoTagMapConfig",
			"WorkItemTypeName": "*",
			"sourceField": "System.State",
			"formatExpression": "ScrumState:{0}"
		}, {
			"ObjectType": "VstsSyncMigrator.Engine.Configuration.FieldMap.FieldMergeMapConfig",
			"WorkItemTypeName": "*",
			"sourceField1": "System.Description",
			"sourceField2": "Microsoft.VSTS.Common.AcceptanceCriteria",
			"targetField": "System.Description",
			"formatExpression": "{0} <br/><br/><h3>Acceptance Criteria</h3>{1}",
			"doneMatch": "##DONE##"
		}, {
			"ObjectType": "VstsSyncMigrator.Engine.Configuration.FieldMap.RegexFieldMapConfig",
			"WorkItemTypeName": "*",
			"sourceField": "COMPANY.PRODUCT.Release",
			"targetField": "COMPANY.DEVISION.MinorReleaseVersion",
			"pattern": "PRODUCT \\d{4}.(\\d{1})",
			"replacement": "$1"
		}, {
			"ObjectType": "VstsSyncMigrator.Engine.Configuration.FieldMap.FieldValuetoTagMapConfig",
			"WorkItemTypeName": "*",
			"sourceField": "Microsoft.VSTS.CMMI.Blocked",
			"pattern": "Yes",
			"formatExpression": "{0}"
		}, {
			"ObjectType": "VstsSyncMigrator.Engine.Configuration.FieldMap.TreeToTagMapConfig",
			"WorkItemTypeName": "*",
			"toSkip": 3,
			"timeTravel": 1
		}
	],
	"WorkItemTypeDefinition": {
		"Bug": "Bug",
		"Product Backlog Item": "Product Backlog Item"
	},
	"Processors": [{
			"ObjectType": "VstsSyncMigrator.Engine.Configuration.Processing.WorkItemMigrationConfig",
			"Enabled": false,
			"PrefixProjectToNodes": true,
			"UpdateCreatedDate": true,
			"UpdateCreatedBy": true,
			"UpdateSourceReflectedId": true,
			"QueryBit": "AND [TfsMigrationTool.ReflectedWorkItemId] = '' AND  [Microsoft.VSTS.Common.ClosedDate] = '' AND [System.WorkItemType] IN ('Shared Steps', 'Shared Parameter', 'Test Case', 'Requirement', 'Task', 'User Story', 'Bug')"
		},{		
			"ObjectType": "VstsSyncMigrator.Engine.Configuration.Processing.WorkItemRevisionReplayMigrationConfig",
			"Enabled": false,
			"PrefixProjectToNodes": false,
			"UpdateCreatedDate": true,
			"UpdateCreatedBy": true,
			"UpdateSourceReflectedId": false,
			"QueryBit": "AND [TfsMigrationTool.ReflectedWorkItemId] = '' AND [System.Tags] Contains 'Xyz'"
	    },{
			"ObjectType": "VstsSyncMigrator.Engine.Configuration.Processing.WorkItemUpdateConfig",
			"WhatIf": false,
			"Enabled": false,
			"QueryBit": "AND [TfsMigrationTool.ReflectedWorkItemId] = '' AND  [Microsoft.VSTS.Common.ClosedDate] = '' AND [System.WorkItemType] IN ('Shared Steps', 'Shared Parameter', 'Test Case', 'Requirement', 'Task', 'User Story', 'Bug')"
		}, {
			"ObjectType": "VstsSyncMigrator.Engine.Configuration.Processing.NodeStructuresMigrationConfig",
			"Enabled": false,
			"BasePaths": ["Product\\Area\\Path1", "Product\\Area\\Path2"]
			"PrefixProjectToNodes": false
		}, {
			"ObjectType": "VstsSyncMigrator.Engine.Configuration.Processing.LinkMigrationConfig",
			"Enabled": false,
			"QueryBit": "AND ([System.ExternalLinkCount] > 0 OR [System.RelatedLinkCount] > 0)"
		}, {
			"ObjectType": "VstsSyncMigrator.Engine.Configuration.Processing.WorkItemPostProcessingConfig",
			"Enabled": false,
			"QueryBit": "AND [TfsMigrationTool.ReflectedWorkItemId] = '' ",
			"WorkItemIDs": [1, 2, 3]
		}, {
			"ObjectType": "VstsSyncMigrator.Engine.Configuration.Processing.WorkItemDeleteConfig",
			"Enabled": false
		}, {
			"ObjectType": "VstsSyncMigrator.Engine.Configuration.Processing.AttachementExportMigrationConfig",
			"Enabled": false,
			"QueryBit": "AND [System.AttachedFileCount] > 0"
		}, {
			"ObjectType": "VstsSyncMigrator.Engine.Configuration.Processing.AttachementImportMigrationConfig",
			"Enabled": false
		}, {
			"ObjectType": "VstsSyncMigrator.Engine.Configuration.Processing.TestVeriablesMigrationConfig",
			"Enabled": false
		}, {
			"ObjectType": "VstsSyncMigrator.Engine.Configuration.Processing.TestConfigurationsMigrationConfig",
			"Enabled": false
		}, {
			"ObjectType": "VstsSyncMigrator.Engine.Configuration.Processing.TestPlansAndSuitsMigrationConfig",
			"Enabled": false,
			"PrefixProjectToNodes": true,
			"OnlyElementsWithTag" : "tag"
		}, {
			"ObjectType": "VstsSyncMigrator.Engine.Configuration.Processing.TestRunsMigrationConfig",
			"Enabled": false,
			"Status": "Experimental"
		}, {
			"ObjectType": "VstsSyncMigrator.Engine.Configuration.Processing.ImportProfilePictureConfig",
			"Enabled": false
		}, {
			"ObjectType": "VstsSyncMigrator.Engine.Configuration.Processing.ExportProfilePictureFromADConfig",
			"Enabled": false,
			"Domain": null,
			"Username": null,
			"Password": null,
			"PictureEmpIDFormat": null
		}, {
			"ObjectType": "VstsSyncMigrator.Engine.Configuration.Processing.FixGitCommitLinksConfig",
			"TargetRepository" : "TargetRepositoryNameIfNotTheSameAsOnSource",
			"Enabled": false
		}, {
			"ObjectType": "VstsSyncMigrator.Engine.Configuration.Processing.HtmlFieldEmbeddedImageMigrationConfig",
			"Enabled": false,
			"QueryBit": " AND [System.WorkItemType] IN ('Shared Steps', 'Shared Parameter', 'Test Case', 'Requirement', 'Task', 'User Story', 'Bug')",
			"FromAnyCollection": false,
			"AlternateCredentialsUsername": null,
			"AlternateCredentialsPassword": null,
			"UseDefaultCredentials": false,
			"Ignore404Errors": true,
			"DeleteTemporaryImageFiles": true
		}
	]
}

```

And the description of the available options are:

### TelemetryEnableTrace
Allows you to submit trace to Application Insights to allow the devellpment team to diagnose any issues that may be found. If you are submitting a support ticket then please include the Session GUID found in your log file for that run. This will help us find the problem.

**note:** All exceptions that you encounter will surface inside of Visual Studio as the developers are working on the source. This will make sure that they tackle issues as they arise.

### Source & Target
Both the `Source` and `Target` entries hold the collection URL and the Team Project name that you are connecting to. The `Source` is where the tool will read the data to migrate. The `Target` is where the tool will write the data.

### ReflectedWorkItemIDFieldName

This is the field that will be used to store the state for the migration . See [Server Configuration](server-configuration.md)  

### NodeStructuresMigrationConfig
You can specify BasePaths for Areas/Iterations to migrate. The area/iteration has to start with that string to be eligible for migration.
E.g. BasePath = "Product\\Area\\Path1"

With existing areas:
"Product\\Area\\Path1\\TestArea"
"SomeOtherProduct\\Area\\Path1\TestArea"
"Product\\OtherArea\\Path1\\TestArea"

only the first one matches the BasePath "Product\\Area\\Path1" and would be migrated, the other ones are ignored.