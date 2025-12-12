using System.Diagnostics;

namespace SpiEyes.Models;

public class Camera
{
    public string Name { get; set; }
    public Process FFmpegProcess { get; set; }
    public Stream OutputStream { get; set; }
}
