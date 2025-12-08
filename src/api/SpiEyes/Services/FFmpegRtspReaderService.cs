using System.Diagnostics;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SpiEyes.General;
using SpiEyes.Models;

namespace SpiEyes.Services;

public class FFmpegRtspReaderService : IFFmpegRtspReaderService, IDisposable
{
    private readonly Config _config;
    private Process _ffmpegProcess;
    public Stream OutputStream;
    private bool _running;

    private static bool LOG_FRAMES = false;
    
    public FFmpegRtspReaderService(IOptions<Config> configOptions)
    {
        _config = configOptions.Value;
    }

    public async Task StartAsync()
    {
        if (_running) return;

        var startInfo = new ProcessStartInfo
        {
            FileName = "ffmpeg",
            Arguments =
                $"-i \"{_config.Cameras[0].URL}\" -c:v libvpx -b:v 1M -f webm -",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        _ffmpegProcess = new Process { StartInfo = startInfo };
        _ffmpegProcess.Start();
        OutputStream = _ffmpegProcess.StandardOutput.BaseStream;
        _running = true;

        _ = Task.Run(() => ReadErrorStream(_ffmpegProcess.StandardError));
    }

    private void ReadErrorStream(StreamReader error)
    {
        string line;
        while ((line = error.ReadLine()) != null)
        {
            if (line.Contains("configuration:")) continue;
            if (!LOG_FRAMES && line.Contains("frame=")) continue;
            
            Console.WriteLine("[FFmpeg]\t" + line);
        }
    }

    public Stream GetStream() => OutputStream;

    public void Stop()
    {
        _running = false;
        try
        {
            _ffmpegProcess.Kill();
            _ffmpegProcess.WaitForExit(3000);
        }
        catch
        {
            Console.WriteLine("Can't kill FFMpeg process.");
        }
    }

    public void Dispose()
    {
        Stop();
        _ffmpegProcess.Dispose();
    }
}