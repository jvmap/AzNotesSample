param webAppName string = resourceGroup().name
param sku string = 'F1' // The SKU of App Service Plan
param location string = resourceGroup().location // Location for all resources
param repositoryUrl string = 'https://github.com/jvmap/AzNotesSample.git'
param branch string = 'main'

// Many names in Azure need to be globally unique.
// Therefore, we produce a random suffix based on the globally unique parent resource group id.
var suffix = uniqueString(resourceGroup().id)
var webAppUniqueName = '${webAppName}-${suffix}'
var appServicePlanName = toLower('plan${suffix}')

resource appServicePlan 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: appServicePlanName
  location: location
  properties: {
    reserved: false
  }
  sku: {
    name: sku
  }
  kind: 'windows'
}

resource appService 'Microsoft.Web/sites@2022-03-01' = {
  name: webAppUniqueName
  location: location
  properties: {
    serverFarmId: appServicePlan.id
    siteConfig: {
      appSettings: [
      ]
    }
  }
}

resource srcControls 'Microsoft.Web/sites/sourcecontrols@2021-01-01' = {
  name: 'web'
  parent: appService
  properties: {
    repoUrl: repositoryUrl
    branch: branch
    isManualIntegration: true
  }
}
