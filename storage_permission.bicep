/*
This template grants the role "roleDefinitionId" to security principal "principalId" on storage account "storageAccountName".
*/

@description('The id of the security principal to which the role is granted.')
param principalId string
@description('The definition id of the role that will be granted.')
param roleDefinitionId string
@description('The name of the storage account to which the role is scoped.')
param storageAccountName string

var roleAssignmentName = guid(principalId, roleDefinitionId, storageAccount.id)

resource storageAccount 'Microsoft.Storage/storageAccounts@2022-09-01' existing = {
  name: storageAccountName
}

resource roleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: roleAssignmentName
  scope: storageAccount
  properties: {
    roleDefinitionId: resourceId('Microsoft.Authorization/roleDefinitions', roleDefinitionId)
    principalId: principalId
  }
}
