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
