using System.Text;
using CamStreamApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CamStreamApi.Controllers;

public class CamStreamController : ControllerBase
{
    private readonly CameraService _cameraService;

    public CamStreamController(CameraService cameraService)
    {
        _cameraService = cameraService;
    }


    [HttpGet, Route("[action]")]
    public async Task StreamCam1(CancellationToken cancellationToken)
    {
        Response.StatusCode = 206;
        Response.ContentType = "multipart/x-mixed-replace; boundary=frame";
        Response.Headers.Add("Connection", "Keep-Alive");

        _cameraService.Cam1FrameReady += CameraServiceOnCam1FrameReady;

        await Response.StartAsync(cancellationToken);
        await new TaskCompletionSource().Task;
    }

    private async void CameraServiceOnCam1FrameReady(object? sender, byte[] e)
        => await WriteToResponseAsync(e);

    private void WriteToResponse(byte[] data)
    {
        byte[] header = GetHeader(data.Length);
        Response.Body.Write(header);
        Response.Body.Write(data);
        Response.Body.Write(_footer);
        Response.Body.Flush();
    }

    private async Task WriteToResponseAsync(byte[] data)
    {
        byte[] header = GetHeader(data.Length);
        await Response.Body.WriteAsync(header);
        await Response.Body.WriteAsync(data);
        await Response.Body.WriteAsync(_footer);
        await Response.Body.FlushAsync();
    }

    private static byte[] GetHeader(int length)
    {
        string header = new StringBuilder()
            .AppendLine("--frame")
            .AppendLine("Content-Type:image/jpeg")
            .Append("Content-Length:")
            .AppendLine(length.ToString())
            .AppendLine()
            .ToString();

        byte[] bytes = Encoding.ASCII.GetBytes(header);
        return bytes;
    }

    private readonly byte[] _footer = Encoding.ASCII.GetBytes(Environment.NewLine);
}