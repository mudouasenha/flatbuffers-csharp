using FlatBuffers.Domain.Services.Abstractions;
using FlatBuffers.Domain.Services.Converters.Abstractions;
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

        // test

        [HttpGet]
        public IActionResult GetVideos()
        {
            try
            {
                Response.ContentType = "application/octet-stream";

                var vid = _videoService.CreateVideo();
                var byteArr = _videoConverter.Serialize(vid);

                return Ok(new ByteArrayContent(byteArr));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return Problem();
            }
        }

        [HttpPost]
        public IActionResult PostVideo(byte[] byteArr)
        {
            try
            {
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