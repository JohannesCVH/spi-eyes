using System.Diagnostics;
using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SpiEyes.General;
using SpiEyes.Models;

namespace SpiEyes.Services;

public class FFmpegRtspReaderService : IHostedService
{
    private readonly Config _config;
    public List<Camera> Cameras { get; set; }

    private static bool LOG_FRAMES = false;
    
    public FFmpegRtspReaderService(IOptions<Config> configOptions)
    {
        _config = configOptions.Value;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Cameras = new List<Camera>();
        
        var startInfo = new ProcessStartInfo
        {
            FileName = "ffmpeg",
            Arguments =
                // $"-i \"{_config.Cameras[0].URL}\" -c:v h264 -c:a aac -hls_time 4 -hls_segment_filename /home/johannescvh/SpiEyes/Streams/Segments/segment_%d.ts /home/johannescvh/SpiEyes/Streams/output.m3u8",
                $"-hwaccel vaapi -hwaccel_output_format vaapi -i \"{_config.Cameras[0].URL}\" -c:v h264_vaapi -c:a aac -f hls -hls_time 4 -hls_segment_filename /home/johannescvh/SpiEyes/Streams/Segments/segment_%d.ts /home/johannescvh/SpiEyes/Streams/output.m3u8",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        var process = new Process { StartInfo = startInfo };
        process.Start();
        var camera = new Camera();
        camera.Name = _config.Cameras[0].Name;
        camera.FFmpegProcess = process;
        camera.OutputStream = process.StandardOutput.BaseStream;
        Cameras.Add(camera);

        foreach (Camera cam in Cameras)
        {
            _ = Task.Run(() => ReadErrorStream(cam));
        }
    }

    private void ReadErrorStream(Camera cam)
    {
        string line;
        while ((line = cam.FFmpegProcess.StandardError.ReadLine()) != null)
        {
            if (line.Contains("configuration:")) continue;
            if (!LOG_FRAMES && line.Contains("frame=")) continue;
            
            Console.WriteLine($"[Camera: {cam.Name}] [FFmpeg]\t" + line);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        if (Cameras != null)
        {
            foreach(Camera cam in Cameras)
            {
                cam.FFmpegProcess.Kill();
                cam.FFmpegProcess.WaitForExit(3000);
                cam.FFmpegProcess.Dispose();
            }
        }

        return Task.CompletedTask;
    }
}