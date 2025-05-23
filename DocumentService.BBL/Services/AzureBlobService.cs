﻿using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Clinic.Domain;
using DocumentService.BBL.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace DocumentService.BBL.Services;
public class AzureBlobService : IAzureBlobService
{
    private readonly string connectionString;
    private readonly string containerName;
    private readonly BlobServiceClient blobServiceClient;
    private readonly BlobContainerClient containerClient;
    private const string AzureConnectionStringSectionName = "AzureBlobStorage";
    private const string AzureContainerNameSectionName = "BlobStorageContainerName";

    public AzureBlobService(IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString(AzureConnectionStringSectionName) ??
            throw new InvalidOperationException(string.Format(NotificationMessages.SectionMissingErrorMessage, AzureConnectionStringSectionName));
        containerName = configuration.GetSection(AzureContainerNameSectionName)?.Value ??
            throw new InvalidOperationException(string.Format(NotificationMessages.SectionMissingErrorMessage, AzureContainerNameSectionName));
        blobServiceClient = new BlobServiceClient(connectionString);
        containerClient = blobServiceClient.GetBlobContainerClient(containerName);
    }

    private async Task<BlobClient> GetBlobClientAsync(string blobName)
    {
        await containerClient.CreateIfNotExistsAsync();
        if (string.IsNullOrEmpty(blobName))
        {
            throw new InvalidOperationException(NotificationMessages.NoBlobNameErrorMessage);
        }
        return containerClient.GetBlobClient(blobName);
    }

    public async Task<string> UploadFileAsync(string localFilePath, string blobName, CancellationToken cancellationToken)
    {
        var blobClient = await GetBlobClientAsync(blobName);
        await blobClient.UploadAsync(localFilePath, true, cancellationToken);
        if (string.IsNullOrEmpty(blobClient.Uri?.ToString()))
        {
            throw new InvalidOperationException(NotificationMessages.NoUrlErrorMessage);
        }
        return blobClient.Uri.ToString();
    }

    public async Task<string> UploadFileAsync(IFormFile file, string blobName, CancellationToken cancellationToken)
    {
        var blobClient = await GetBlobClientAsync(blobName);

        using (var stream = file.OpenReadStream())
        {
            await blobClient.UploadAsync(stream, true, cancellationToken);
        }

        if (string.IsNullOrEmpty(blobClient.Uri?.ToString()))
        {
            throw new InvalidOperationException(NotificationMessages.NoUrlErrorMessage);
        }

        return blobClient.Uri.ToString();
    }

    public async Task<string> UploadFileFromMemoryAsync(byte[] fileBytes, string blobName, CancellationToken cancellationToken)
    {
        var blobClient = await GetBlobClientAsync(blobName);

        using (MemoryStream stream = new MemoryStream(fileBytes))
        {
            await blobClient.UploadAsync(stream, cancellationToken);
        }
        if (string.IsNullOrEmpty(blobClient.Uri?.ToString()))
        {
            throw new InvalidOperationException(NotificationMessages.NoUrlErrorMessage);
        }
        return blobClient.Uri.ToString();
    }

    public async Task<bool> DeleteBlobAsync(string blobName, CancellationToken cancellationToken)
    {
        var blobClient = await GetBlobClientAsync(blobName);
        return await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
    }

    public async Task DownloadFileAsync(string blobName, string downloadFilePath, CancellationToken cancellationToken)
    {
        var blobClient = await GetBlobClientAsync(blobName);
        BlobDownloadResult downloadResult = await blobClient.DownloadContentAsync(cancellationToken);
        await File.WriteAllBytesAsync(Path.Combine(downloadFilePath, blobName), downloadResult.Content.ToArray(), cancellationToken);
    }
}