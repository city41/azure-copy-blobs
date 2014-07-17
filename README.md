# Copy Azure Storage Blobs

This is a small console application I whipped up to copy blobs across Azure Storage accounts (including across subscriptions). It is based on various blog posts on the web about this topic.

## How to use
1. clone the repo
2. open the solution in Visual Studio
3. Install the NuGet packages (Windows Azure Storage is the only needed package)
4. Rename `Config.cs.example` to `Config.cs`
5. Fill in Config.cs as needed
6. Build and run the app

It will give you a progress report on the copy every 20 seconds.

## How to fill out Config.cs

It's pretty straightforward.

To get your storage account name. Go to the storage tab in Azure Management Portal. The name will be listed in the first column of the table.

On this same page in Azure Management Portal, down at the bottom click "Manage Access Keys" to get the access key you will need.

To find the container name, dig into the account, go to Containers tab, and find the name there. Inside the container you can also find the blob names you will need.

## Common Reasons Why Copies Fail

1. The blob is in use. If you are copying a VM hard drive, you will need to shut the VM down first. This comes back as a 412 error.

2. Authorization is denied. This happens if the SAS time limit is reached. If you are copying large blobs, the time limit will have to be set accordingly. In my experience, a 128GB blob copied within the US West region takes about 15 minutes. This time can vary greatly depending on size and what regions you are copying to/from.

## If a copy fails

You will need to log into the destination account and delete the blob there. It will be listed as a size of zero bytes

