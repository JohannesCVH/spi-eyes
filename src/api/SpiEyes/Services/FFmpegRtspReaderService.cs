using System.Diagnostics;
using Microsoft.Extensions.Options;
using SpiEyes.Models;
using SpiEyes.Utility;

namespace SpiEyes.Services;

public class FFmpegRtspReaderService : IHostedService
{
    private readonly Config _config;
    private readonly ISharedDataService _sharedDataService;
    public List<Camera> Cameras { get; set; }

    private static bool LOG_FRAMES = false;
    
    public FFmpegRtspReaderService(IOptions<Config> configOptions, ISharedDataService sharedDataService)
    {
        _config = configOptions.Value;
        _sharedDataService = sharedDataService;
        DirectoryUtils.GenerateStreamFolders(_config.Cameras);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Cameras = new List<Camera>();
        
        // foreach (CameraConfig cam in new CameraConfig[] {_config.Cameras.FirstOrDefault()})
        for (int i = 0; i < _config.Cameras.Length; i++)
        {
            var camPath = PathUtils.CreateStreamsPath(_config.Cameras[i].Name);
            
            var streamStartInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments =
                    // $"-i \"{_config.Cameras[i].URL}\" -c:v h264 -c:a aac -hls_time 4 -hls_segment_filename /home/johannescvh/SpiEyes/Streams/Segments/segment_%d.ts /home/johannescvh/SpiEyes/Streams/output.m3u8",
                    // $"-hwaccel cuda -hwaccel_output_format cuda -i \"{_config.Cameras[i].URL}\" -c:v h264_nvenc -c:a aac -f hls -hls_time 4 -hls_segment_filename {camPath}/Segments/segment_%d.ts {camPath}/output.m3u8",
                    $"-hwaccel d3d11va -hwaccel_output_format d3d11 -i \"{_config.Cameras[i].URL}\" -c:v h264_nvenc -c:a aac -f hls -hls_time 4 -hls_segment_filename {camPath}/Segments/segment_%d.ts {camPath}/output.m3u8",
                    // $"-hwaccel d3d11va -i \"{_config.Cameras[i].URL}\" -c:v libx264 -c:a aac -f hls -hls_time 4 -hls_segment_filename {camPath}/Segments/segment_%d.ts {camPath}/output.m3u8",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            var streamProc = new Process { StartInfo = streamStartInfo };
            streamProc.Start();

            var frameStartInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments =
                    $"-i \"{_config.Cameras[i].URL}\" -vf fps=1 {camPath}/Frames/frame_%04d.jpg",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            var frameProc = new Process { StartInfo = frameStartInfo };
            frameProc.Start();

            var camera = new Camera();
            camera.Name = _config.Cameras[i].Name;
            camera.StreamProc = streamProc;
            camera.FrameProc = frameProc;
            camera.OutputStream = streamProc.StandardOutput.BaseStream;
            Cameras.Add(camera);
        }

        _sharedDataService.Cameras = Cameras;

        foreach (Camera cam in Cameras)
        {
            _ = Task.Run(() => ReadErrorStream(cam));
        }
    }

    private void ReadErrorStream(Camera cam)
    {
        string line;
        while ((line = cam.StreamProc.StandardError.ReadLine()) != null)
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
                cam.StreamProc.Kill();
                cam.StreamProc.WaitForExit(3000);
                cam.StreamProc.Dispose();
            }
        }

        return Task.CompletedTask;
    }
}