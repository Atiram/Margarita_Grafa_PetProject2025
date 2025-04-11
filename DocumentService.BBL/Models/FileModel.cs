﻿using DocumentService.DAL.Enums;

namespace DocumentService.BBL.Models;
public class FileModel
{
    public string? Id { get; set; }
    public DocumentType DocumentType { get; set; }
    public required string BlobName { get; set; }
    public string? StorageLocation { get; set; }
    public string? ReferenceItemId { get; set; }
    public DateTime UploadedDate { get; set; }
}
