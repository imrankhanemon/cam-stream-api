using Emgu.CV;
using Microsoft.AspNetCore.Mvc;

namespace CamStreamApi.Utilities;

public class OpenCvCam
{

    private readonly VideoCapture _videoCapture;

    public event EventHandler<Mat>? FrameReady;
    public bool IsOpened => _videoCapture.IsOpened;

    protected void OnFrameReady(Mat frame) => FrameReady?.Invoke(this, frame);

    public OpenCvCam(int camIndex)
    {
        _videoCapture = new VideoCapture(camIndex);
        _videoCapture.ImageGrabbed += VideoCaptureOnImageGrabbed;
        _videoCapture.Start();
    }

    private void VideoCaptureOnImageGrabbed(object? sender, EventArgs e)
    {
        var frame = new Mat();
        _videoCapture.Read(frame);
        OnFrameReady(frame);
    }
}