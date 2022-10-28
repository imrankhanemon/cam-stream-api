using System.Text;
using CamStreamApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CamStreamApi.Controllers;

public class CamStreamController : ControllerBase
{
    #region Fields

    private readonly CameraService _cameraService;

    private static readonly byte[] Footer = Encoding.ASCII.GetBytes(Environment.NewLine);

    private static readonly string HeaderTemplate = new StringBuilder()
        .AppendLine("--frame")
        .AppendLine("Content-Type:image/jpeg")
        .AppendLine("Content-Length:{0}")
        .AppendLine()
        .ToString();

    #endregion

    #region Ctors

    public CamStreamController(CameraService cameraService)
    {
        _cameraService = cameraService;
    }

    #endregion

    #region Utilities

    [NonAction]
    private async void CameraServiceOnCam1FrameReady(object? sender, byte[] e)
        => await WriteToResponseAsync(e);

    [NonAction]
    private void WriteToResponse(byte[] data)
    {
        byte[] header = GetHeader(data.Length);
        Response.Body.Write(header);
        Response.Body.Write(data);
        Response.Body.Write(Footer);
        Response.Body.Flush();
    }

    [NonAction]
    private async Task WriteToResponseAsync(byte[] data)
    {
        byte[] header = GetHeader(data.Length);
        await Response.Body.WriteAsync(header);
        await Response.Body.WriteAsync(data);
        await Response.Body.WriteAsync(Footer);
        await Response.Body.FlushAsync();
    }

    [NonAction]
    private static byte[] GetHeader(int length)
    {
        string header = string.Format(HeaderTemplate, length.ToString());
        byte[] bytes = Encoding.ASCII.GetBytes(header);
        return bytes;
    }

    #endregion

    #region Methods

    // [HttpGet, Route("[action]")]
    // public async Task<IActionResult> StreamCam1(CancellationToken cancellationToken)
    // {
    //     const string contentType = "application/octet-stream";
    //     return File(_cameraService.Cam1Stream, contentType, enableRangeProcessing: true);
    // }

    [HttpGet, Route("[action]")]
    public async Task Cam1Stream()
    {
        Response.ContentType = "multipart/x-mixed-replace; boundary=frame";
        Response.Headers.Add("Connection", "Keep-Alive");

        _cameraService.Cam1FrameReady += CameraServiceOnCam1FrameReady;

        await Response.StartAsync();
        await new TaskCompletionSource().Task;
    }

    #endregion
}