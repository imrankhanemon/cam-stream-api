using Emgu.CV;
using Microsoft.AspNetCore.Mvc;

namespace CamStreamApi.Utilities;

public class OpenCvCam : IDisposable
{
    #region Fields

    private readonly VideoCapture _videoCapture;

    public event EventHandler<Mat>? FrameReady;
    public bool IsOpened => _videoCapture.IsOpened;

    #endregion

    #region Ctors

    public OpenCvCam(int camIndex)
    {
        _videoCapture = new VideoCapture(camIndex);
        _videoCapture.ImageGrabbed += VideoCaptureOnImageGrabbed;
        _videoCapture.Start();
    }

    #endregion

    #region Utilities

    protected void OnFrameReady(Mat frame) => FrameReady?.Invoke(this, frame);

    private void VideoCaptureOnImageGrabbed(object? sender, EventArgs e)
    {
        var frame = new Mat();
        _videoCapture.Read(frame);
        OnFrameReady(frame);
    }

    #endregion

    #region Methods

    public void Start() => _videoCapture.Start();

    public void Stop() => _videoCapture.Stop();

    public void Dispose()
    {
        _videoCapture.Dispose();
    }

    #endregion
}