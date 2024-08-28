[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![License][license-shield]][license-url]
[![LinkedIn][linkedin-shield]][linkedin-url]

<!-- PROJECT LOGO -->
<br />
<div align="center">
  <a href="https://github.com/Azmekk/Magic-Number-Analyzer">
    <img src="images/file-black-icon.png" alt="Logo" width="80" height="80">
  </a>

  <h3 align="center">Magic Number Analyzer</h3>

  <p align="center">
    <a href="https://github.com/Azmekk/Magic-Number-Analyzer/issues">Report Bug</a>
    ·
    <a href="https://github.com/Azmekk/Magic-Number-Analyzer/issues">Request Feature</a>
  </p>
</div>



<!-- ABOUT THE PROJECT -->
## About the project

Hey! I decided to create a library designed to simplify file type detection. By utilizing magic numbers to identify file types within your C# projects you can safely determine file types,
rather than relying on file extensions which can be changed. 

By using this you instead check against a database of known byte sequences to get the mime type of specific files. The library is designed to work with filestreams, memorystreams and a byte array for ease of use. And you can even add your own custom byte arrays.

Whether you're working on file upload validation, data parsing, or simply need to know the nature of a file, you can always reliably check using this library.



### Built With

* [![DotNet][.Net]][.Net-url]
* [![C#][CSharp]][CSharp-url]



<!-- GETTING STARTED -->
## Getting Started

Simply pull the [Nuget](https://www.nuget.org/packages/Martin.FileTools.MagicNumberAnalyzer) package from within Visual Studio and use the static `MagicNumberAnalyzer.GetFileMimeType()` method.

```cs
using Martin.FileTools;

//This can also be a MemoryStream or a byte[]
using FileStream fileStream = File.OpenRead("Your-cool-file");

string fileMimeType = MagicNumberAnalyzer.GetFileMimeType(fileStream);
```

## Default filetypes

The following filetypes are registered by default:

```
.7z: application/x-7z-compressed
.avi: video/x-msvideo
.flv: video/x-flv
.m4v: video/x-m4v
.mp4: video/mp4
.svg: image/svg+xml
.pdf: application/pdf
.mp3: audio/mpeg
.wav: audio/wav
.gif: image/gif
.webm: video/webm
.webp: image/webp
.mov: video/quicktime
.png: image/png
.gz: application/gzip
.bmp: image/bmp
.ico: image/x-icon
.jpg: image/jpeg
.zip: application/zip
.rar: application/vnd.rar
.exe: application/x-msdownload
```

<!-- USAGE EXAMPLES -->
## Custom Magic Numbers

The library provides a comprehensive interface for adding custom magic numbers. You can include unique magic numbers to increase the file identification capabilities in case the default types are not enough.

### Magic Number Integration Example:

**Note: This only needs to be done once due to the static nature of the class and it's properties. A good place to do this would be your startup file or a method that is called once.**

Suppose we have a specific magic number: `0x24 0x27 xx xx 0xF1 0xA1 0x41 xx 0xC3`. To include it into the library, follow these steps:

1. Instantiate a new [`MagicNumber`](https://github.com/Azmekk/Magic-Number-Analyzer/blob/master/src/Martin.FileTools/Types/MagicNumber.cs) class.

2. We know 3 byte sequences `0x24 0x27` at offset 0, `0xF1 0xA1 0x41` at offset 4 and `0xC3` at offset 8.

3. Add a new instance of [`KnownByteSequence`](https://github.com/Azmekk/Magic-Number-Analyzer/blob/master/src/Martin.FileTools/Types/KnownByteSequence.cs) for each known sequence we have.

3. Utilize the [`AddCustomMagicNumber`](https://github.com/Azmekk/Magic-Number-Analyzer/blob/48627d35e6c66bfe2b7d393e1d14ea060867f6ee/src/Martin.FileTools/MagicNumberAnalyzer.cs#L230) method available in [`MagicNumberAnalyzer`](https://github.com/Azmekk/Magic-Number-Analyzer/blob/master/src/Martin.FileTools/MagicNumberAnalyzer.cs) to register the custom magic number.

### Code Example:

```csharp
using Martin.FileTools;
using Martin.FileTools.Types;

//Make sure you register this only once either during startup or a method that gets called once.
MagicNumberAnalyzer.AddCustomMagicNumber(
    new("Your custom mime type", [
        new([0x24, 0x27], 0),
        new([0xF1, 0xA1, 0x41], 4),
        new([0xC3], 8)
    ])
);
```

Note that custom types are checked first so you could effectively override the registered types. That is so it doesn't interfere with your own custom magic number functionality.



<!-- CONTRIBUTING -->
## Suggestions or Feature Requests

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also open an issue with the tag "suggestion".
Don't forget to give the project a star! Thanks again!



<!-- LICENSE -->
## License

Distributed under the MIT License. See `LICENSE.txt` for more information.


<!-- CONTACT -->
## Contact

Martin Yordanov - [Linkedin](https://www.linkedin.com/in/martin-y/)

Project Link: [https://github.com/Azmekk/Magic-Number-Analyzer](https://github.com/Azmekk/Magic-Number-Analyzer)



<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-shield]: https://img.shields.io/github/contributors/Azmekk/Magic-Number-Analyzer.svg?style=for-the-badge
[contributors-url]: https://github.com/Azmekk/Magic-Number-Analyzer/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/Azmekk/Magic-Number-Analyzer.svg?style=for-the-badge
[forks-url]: https://github.com/Azmekk/Magic-Number-Analyzer/network/members
[stars-shield]: https://img.shields.io/github/stars/Azmekk/Magic-Number-Analyzer.svg?style=for-the-badge
[stars-url]: https://github.com/Azmekk/Magic-Number-Analyzer/stargazers
[issues-shield]: https://img.shields.io/github/issues/Azmekk/Magic-Number-Analyzer.svg?style=for-the-badge
[issues-url]: https://github.com/Azmekk/Magic-Number-Analyzer/issues
[license-shield]: https://img.shields.io/github/license/Azmekk/Magic-Number-Analyzer.svg?style=for-the-badge
[license-url]: https://github.com/Azmekk/Magic-Number-Analyzer/blob/master/LICENSE.txt
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://linkedin.com/in/Martin-Y
[.Net]: https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white
[.Net-url]: https://dotnet.microsoft.com/
[CSharp]: https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=whit
[CSharp-Url]: https://learn.microsoft.com/en-us/dotnet/csharp/
