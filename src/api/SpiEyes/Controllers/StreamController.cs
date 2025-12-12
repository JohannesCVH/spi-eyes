using Microsoft.AspNetCore.Mvc;

namespace SpiEyes.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StreamController : ControllerBase
{   
    public StreamController()
    {
        
    }

    [HttpGet("Stream1")]
    public async Task<IActionResult> Stream()
    {
        try
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "SpiEyes/Streams/output.m3u8");
            return PhysicalFile(path, "application/x-mpegURL");
        }
        catch(Exception ex)
        {
            Console.WriteLine($"ERROR: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message= "An unexpected error occurred." });
        }
    }

    [HttpGet("{segmentId}.ts")]
    public IActionResult GetStreamSegment(string segmentId)
    {
        var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "SpiEyes/Streams/Segments", $"{segmentId}.ts");
        return PhysicalFile(filePath, "video/MP2T");
    }
}