namespace SchoolGeoResources.Api.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolGeoResources.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class StorageController : ControllerBase
{
    private readonly IStorageService _storageService;

    public StorageController(IStorageService storageService)
    {
        _storageService = storageService;
    }

    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadFile(IFormFile file, CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { error = "No file uploaded." });
        }

        using var stream = file.OpenReadStream();
        var url = await _storageService.UploadFileAsync(stream, file.FileName, file.ContentType, cancellationToken);

        return Ok(new { url });
    }
}
