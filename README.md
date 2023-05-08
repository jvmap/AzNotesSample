# AzNotesSample
This is a very basic Azure sample application, to be used in conjunction with a bicep workshop.

## Hands on ##

### Baseline ###

First, let's verify that all tools are correctly installed on your system.

1. In a command window, run `az login` and log in to your Azure subscription. You need permission to create a new resource group. If you don't have access to a subscription with sufficient privileges, sign up for an [Azure Free Trial](https://azure.microsoft.com/en-us/free/) subscription.
1. Create a new resource group:
   ```
   az group create -n EonicsBicepHackNight -l westeurope
   ```
1. Download the [Baseline.bicep](https://raw.githubusercontent.com/jvmap/AzNotesSample/main/Baseline.bicep) file (use right click -> Save as...).
1. Deploy the baseline Bicep file:
   ```
   az deployment group create -g EonicsBicepHackNight --template-file Baseline.bicep
   ```
   Note: you may need to enter the full path to the ```Baseline.bicep``` file.
1. When deployment is finished, visit the [Azure portal](https://portal.azure.com). Navigate to the `EonicsBicepHackNight` resource group and verify that it contains two resources:
   * 1 App Service plan resource
   * 1 App Service resource.<br/>
   Click on the App Service resource, and then click ![azbrowse](https://user-images.githubusercontent.com/1012756/235449503-f9ff1bc3-a58e-4af3-96bd-0bde3343d50f.png).
   You should see a functioning website using server-side, in-memory storage.

Optionally, you can verify that the user-entered note is lost after an application restart like this:
```
az webapp restart -g EonicsBicepHackNight -n <name of your webapp resource>
```
Note: it may take about one minute for the web app to restart.

### Challenge 1: Deploy a storage account ###

Can you extend the bicep file to deploy a storage account resource, in addition to the App Service plan and App Service resources?

Use the following info:
* Resource type: `Microsoft.Storage/storageAccounts`
* Kind: `StorageV2`
* SKU: `Standard_LRS`
* (Optional) Disable public access to blobs by setting the relevant property.
  ![blob_public_access](https://user-images.githubusercontent.com/1012756/235450555-a54fba19-3397-4b70-a4cd-167ede6f8bc5.png)

Once you have updated the bicep file, you can update your current deployment:
```
az deployment group create -g EonicsBicepHackNight --template-file Baseline.bicep
```

After the deployment, the resource group `EonicsBicepHackNight` should contain 3 resources:
* 1 App Service plan resource
* 1 App Service resource
* 1 Storage Account resource

In case you're stuck, or to verify, you may have a look at [the solution](https://raw.githubusercontent.com/jvmap/AzNotesSample/main/Exercise1_solution.bicep).

### Challenge 2: Connect web app to storage account ###

You have now successfully deployed both a web app and a storage account, however, the two are not connected. The web app is still using in-memory storage.

The web app is programmed to use a storage account when the property `STORAGE_ACCOUNT_NAME` is configured. Morever, when `STORAGE_ACCOUNT_NAME` is configured, also `STORAGE_ACCOUNT_KEY` must be configured.

The `STORAGE_ACCOUNT_KEY` property should contain one of the two access keys of the storage account.

The challenge is to modify the bicep file, so that the `STORAGE_ACCOUNT_NAME` and `STORAGE_ACCOUNT_KEY` parameters are automatically configured during deployment. For this, you need to modify the `appService` resource. 

Notes: 
* The value of the `appSettings` property is an array of objects. Each object has a `name` and a `value` property. You can use a syntax very similar to JSON.
* If your storage account resource is named `storage`, you can get the list of access keys using `storage.listKeys()`.

Once you have updated the bicep file, you can update your current deployment:
```
az deployment group create -g EonicsBicepHackNight --template-file Baseline.bicep
```

When you visit the sample website after deployment, you should now see that it is using Azure Blob Storage, instead of in-memory storage.

![access_key](https://user-images.githubusercontent.com/1012756/236755617-5b2166cb-0d7c-4daa-9ed1-74d1e545c876.png)

The note you enter is now persisted, even after application restarts.😏

Optionally, you can verify that the user-entered note is indeed persisted after an application restart like this:
```
az webapp restart -g EonicsBicepHackNight -n <name of your webapp resource>
```
Note: it may take about one minute for the web app to restart.

In case you're stuck, or to verify, you may have a look at [the solution](https://raw.githubusercontent.com/jvmap/AzNotesSample/main/Exercise2_solution.bicep).

### Clean up ###
Run the following command to clean up any Azure resources that you created during this workshop.
```
az group delete -n EonicsBicepHackNight
```

You may also wish to log out from the Azure CLI:
```
az logout
```
