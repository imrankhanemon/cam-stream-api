# CamStreamApi
This project is heavily inspired by [this project](https://github.com/ChristophWieske/asp-net-core-live-stream-source) (thanks Chris you're my hero).
<br />
A pathetic attempt to implement a camera live streaming service in ASP.NET core with OpenCv library. No, why's there no documentation on the web about this problem? Like, does no one else have ever tried to implement such a thing in ASP.NET before?
<br />
 Rants aside, note that the hosted service always reads the camera frames in the background regardless of the client requests which is good if you wanna save the footage somewhere. Each retrieved frame is available indivitually and you can do whatever your heart desires to them before streaming to the client.

## Setup
1. Download and install [.NET 6 Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) on your machine.
2. Open your favorite command line app and run the following command in the project folder:
```shell
$ dotnet run
```
3. If everything goes right, a magical console window should appear out of thin air and somewhere in the window there's a line that reads "Now listening on: (some random link)". 
4. Paste the said link in your favorite browser and type "/Cam1Stream" at the end and then and hit enter.
5. Smile at your beautiful face.

