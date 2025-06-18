using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Sensor6ty.Results;

namespace Soditech.IntelPrev.Mediatheques.Application.Services;

public class FileService: IFileService
{
    private readonly string _fileFolderPath;
    private readonly ILogger<FileService> _logger;

    public FileService(IConfiguration configuration, ILogger<FileService> logger)
    {
        _logger = logger;
        _fileFolderPath = configuration["FileFolderPath"] ?? string.Empty; 

        if (string.IsNullOrEmpty(_fileFolderPath))
        {
            throw new ArgumentNullException(nameof(_fileFolderPath), "Le chemin du dossier des documents n'est pas défini dans la configuration.");
        }
    }

    public async Task<byte[]> GetFileAsync(string fileName)
    {
        var filePath = Path.Combine(_fileFolderPath, fileName);

        if (File.Exists(filePath)) return await File.ReadAllBytesAsync(filePath);

        _logger.LogError("File {file} does not found.", filePath);
        
        //TODO: use result pattern to return error
        throw new FileNotFoundException("Image non trouvée.");
    }
    
    public async Task<Result> CreateFileAsync(string fileName, byte[] stream, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            return Result.Failure(new Error("400", "file name does not null"));
        }

        if (stream.Length == 0)
        {
            return Result.Failure(new Error("400", "stream length is zero"));
        }

        try
        {
            var filePath = Path.Combine(_fileFolderPath, fileName);
            // ensure that directory exists...
            Directory.CreateDirectory(_fileFolderPath);
            await File.WriteAllBytesAsync(filePath, stream, cancellationToken);
            return Result.Success();

        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error has occured while creating file."); 
            return Result.Failure(new Error("500", "Error has occured while creating file."));
        }    
    }
}
