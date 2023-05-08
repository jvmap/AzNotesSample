# AzNotesSample
This is a very basic Azure sample application, to be used in conjunction with a bicep workshop.

## Hands on ##

To follow the hands-on lab, you don't have to clone this repository. You only need to download and use the files that are explicitly mentioned in the instructions.

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

### Super Challenge: Passwordless authentication

In this final challenge, we dive a little deeper into the Azure platform. Don't worry if you get stuck or need help. You don't need to know all this if you just want to work with Azure Bicep. However, if you're actively developing Azure applications, this is very useful to know.

If you don't want to try this challenge, please skip to [Clean up](#clean-up).

Let's first describe the problem we're going to solve. The sample application so far is very useful, but it still has one drawback: if an administrator decides to regenerate the access key of the storage account for security purposes, the app ceases to work.

You can verify this by running the following command:
```
az storage account keys renew -g EonicsBicepHackNight -n <name of your storage account resource> --key key1
```

After about one minute, you should see that the web application no longer works. This problem can be solved by simply redeploying the application, but it is not an ideal solution. A better solution is to use passwordless authentication, which is the goal of this challenge.

With passwordless authentication, there are no user-managed credentials, so that's one less thing that can break or be compromised.

To use passwordless authentication with the sample app, we need to do the following:

#### Update app configuration ###
* The application setting `STORAGE_ACCOUNT_KEY` should no longer be configured.
* The setting `MANAGED_IDENTITY` needs to be configured with the value `1`. This setting is recognized by the sample application code, and, if found, tells the sample application to try to use passwordless authentication.

#### Enable system-assigned managed identity ####
The `appService` resource must be configured to enable the system-assigned managed identity. This is done by adding the `identity` attribute and setting the correct `type`.
  In case you want to learn more about managed identities in Azure, see: [What are managed identities for Azure resources?](https://learn.microsoft.com/en-us/azure/active-directory/managed-identities-azure-resources/overview)

#### Grant access to the storage acount ####
The `appService` resource must be granted read and write access to the storage account resource. This is achieved by granting the [Storage Blob Data Contributor](https://learn.microsoft.com/en-us/azure/role-based-access-control/built-in-roles#storage-blob-data-contributor) role *to* the `appService` resource *on* the storage account resource. Due to the way ARM works, this requires the use of an additional bicep file, `storage_permission.bicep`.

Download [storage_permission.bicep](https://raw.githubusercontent.com/jvmap/AzNotesSample/main/storage_permission.bicep) and place it next to your other bicep file(s). `storage_permission.bicep` does not need any modifications for this challenge.

To include the bicep `storage_permission.bicep` file, use the following syntax:
```bicep
module storagePermission 'storage_permission.bicep' = {
  name: 'storagePermission'
  params: {
    principalId: <fill in>
    roleDefinitionId: <fill in>
    storageAccountName: <fill in>
  }
}
```
The `principalId` can be defined symbolically in bicep. Hint: start with the symbolic name of your appService resource, e.g. `appService.`.
The `storageAccountName` can also be defined symbolically in bicep in a similar way.
For `roleDefinitionId`, use the hard-coded value `'ba92f5b4-2d11-453d-a403-e96b0029c9fe'`. This is the ID of the built-in role [Storage Blob Data Contributor](https://learn.microsoft.com/en-us/azure/role-based-access-control/built-in-roles#storage-blob-data-contributor) role, which includes read and write access. For a full list of Azure built-in roles, see [Azure built-in roles](https://learn.microsoft.com/en-us/azure/role-based-access-control/built-in-roles).

You are now ready to deploy your resources!
```
az deployment group create -g EonicsBicepHackNight --template-file Baseline.bicep
```

If all went well, the application should look like this:

![managed_identity](https://user-images.githubusercontent.com/1012756/236767461-73bba52e-0879-4877-b4b5-3fb95ef0a3fe.png)

In case you're stuck, or to verify, you can have a look at [the solution](https://raw.githubusercontent.com/jvmap/AzNotesSample/main/AzNotesSample.bicep).

Optionally, you can verify that you can now renew the storage account access keys, and it will not affect your application at all:
```
az storage account keys renew -g EonicsBicepHackNight -n <name of your storage account resource> --key key1
az storage account keys renew -g EonicsBicepHackNight -n <name of your storage account resource> --key key2
```

### Clean up ###
Run the following command to clean up any Azure resources that you created during this workshop.
```
az group delete -n EonicsBicepHackNight
```

You may also wish to log out from the Azure CLI:
```
az logout
```
