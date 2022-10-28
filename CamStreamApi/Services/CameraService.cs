using CamStreamApi.Utilities;
using Emgu.CV;

namespace CamStreamApi.Services;

public class CameraService : IHostedService
{
    private readonly ILogger<CameraService> _logger;
    public CameraService(ILogger<CameraService> logger)
    {
        _logger = logger;

    }

    private void Cam1OnFrameReady(object? sender, Mat e)
    {
        _logger.LogDebug("Frame retrieved.");
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var cam1 = new OpenCvCam(0);
        if (cam1.IsOpened)
        {
            cam1.FrameReady += Cam1OnFrameReady;
        }
        else
        {
            _logger.LogError("Cannot open camera.");
        }
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}