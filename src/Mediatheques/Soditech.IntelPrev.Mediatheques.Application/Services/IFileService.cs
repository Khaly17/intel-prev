using System.Threading;
using System.Threading.Tasks;
using Sensor6ty.Results;

namespace  Soditech.IntelPrev.Mediatheques.Application.Services;

public interface IFileService
{
    Task<byte[]> GetFileAsync(string path);
    Task<Result> CreateFileAsync(string fileName, byte[] stream, CancellationToken cancellationToken = default);
}