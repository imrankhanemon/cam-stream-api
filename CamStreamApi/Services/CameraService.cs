using System.Drawing;
using System.Net.Mime;
using CamStreamApi.Utilities;
using Emgu.CV;
using Emgu.CV.Structure;

namespace CamStreamApi.Services;

public class CameraService : IHostedService
{
    #region Fields

    private readonly ILogger<CameraService> _logger;
    private readonly OpenCvCam _cam1;
    public event EventHandler<byte[]>? Cam1FrameReady;
    
    #endregion

    #region Ctors
    
    public CameraService(ILogger<CameraService> logger)
    {
        _logger = logger;
        _cam1 = new OpenCvCam(0);

    }
    
    #endregion

    #region Utilities

    private void Cam1OnFrameReady(object? sender, Mat e)
    {
        // _logger.LogDebug("Frame retrieved.");
        var image = e.ToImage<Bgr, Byte>();
        var bytes = image.ToJpegData();
        Cam1FrameReady?.Invoke(this, bytes);
    }

    private static async Task SaveJpegAsync(byte[] bytes, CancellationToken cancellationToken)
    {
        string imageName = Guid.NewGuid() + ".jpg";
        string path = Path.Combine(Environment.CurrentDirectory, imageName);
        await File.WriteAllBytesAsync(path, bytes, cancellationToken);
    }
    
    #endregion

    #region Methods
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _cam1.Start();
        if (_cam1.IsOpened)
        {
            _cam1.FrameReady += Cam1OnFrameReady;
        }
        else
        {
            _logger.LogError("Cannot open camera.");
        }
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _cam1.Stop();
        _cam1.Dispose();
        return Task.CompletedTask;
    }
    
    #endregion
}