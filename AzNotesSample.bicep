@description('The Azure region in which to deploy the webapp.')
param location string = 'westeurope'

resource webApp 'Microsoft.Web/sites@2022-03-01' = {
  location: location
  name: 'AzNotesSample'
  properties: {

  }
}
