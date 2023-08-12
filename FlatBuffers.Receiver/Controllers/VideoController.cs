﻿using FlatBuffers.Domain.Services.Abstractions;
using FlatBuffers.Domain.Services.Flatbuffers.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace FlatBuffers.Receiver.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VideoController : ControllerBase
    {
        private readonly ILogger<VideoController> _logger;
        private readonly IVideoService _videoService;
        private readonly IFlatBuffersVideoConverter _videoConverter;

        public VideoController(ILogger<VideoController> logger, IFlatBuffersVideoConverter videoConverter, IVideoService videoService)
        {
            _logger = logger;
            _videoConverter = videoConverter;
            _videoService = videoService;
        }

        [HttpGet]
        public IActionResult GetVideos()
        {
            try
            {
                var vid = _videoService.CreateVideo();
                var byteArr = _videoConverter.Serialize(vid);

                Response.ContentType = "application/octet-stream";
                Response.Headers.Add("Content-Length", byteArr.Length.ToString());

                return Ok(new FileContentResult(byteArr, "application/octet-stream"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return Problem();
            }
        }

        [HttpPost]
        public IActionResult PostVideo([FromBody] byte[] byteArr)
        {
            try
            {
                var test = HttpContext;
                Console.WriteLine("Received");
                var video = _videoConverter.Deserialize(byteArr);
                _logger.LogInformation(video.ToString());

                var videoArr = _videoConverter.Serialize(video);

                return Ok(new ByteArrayContent(videoArr));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return Problem();
            }
        }
    }
}