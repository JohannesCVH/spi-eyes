namespace SpiEyes.Services;

public interface IFFmpegRtspReaderService
{   
    public Task StartAsync();
    public Stream GetStream();
}
