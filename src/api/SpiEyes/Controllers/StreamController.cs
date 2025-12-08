using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SpiEyes.Models;
using SpiEyes.Services;

namespace SpiEyes.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StreamController : ControllerBase
{
    private readonly Config _config;
    private readonly IFFmpegRtspReaderService _fFmpegRtspReaderService;
    
    public StreamController(IOptions<Config> configOptions, IFFmpegRtspReaderService fFmpegRtspReaderService)
    {
        _config = configOptions.Value;
        _fFmpegRtspReaderService = fFmpegRtspReaderService;
    }

    [HttpGet("Stream1")]
    public async Task<IActionResult> Stream()
    {
        try
        {
            Response.Headers.Add("Content-Type", "video/webm");
            Response.Headers.Add("Cache-Control", "no-cache");

            await _fFmpegRtspReaderService.StartAsync();

            return new FileStreamResult(_fFmpegRtspReaderService.GetStream(), "video/webm");
        }
        catch(Exception ex)
        {
            Console.WriteLine($"ERROR: {ex.Message}");
            return new NoContentResult();
        }
    }
}