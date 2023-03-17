using FlatBuffers.Domain.Entities;
using FlatBuffers.Domain.Services.Abstractions;
using FlatBuffers.Receiver.VideoModel;
using Google.FlatBuffers;
using Microsoft.AspNetCore.Mvc;

namespace FlatBuffers.Receiver.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VideoController : ControllerBase
    {
        private readonly ILogger<VideoController> _logger;
        private readonly IVideoService _videoService;
        private readonly ISerializationService<Video, VideoEntity> _serializationService;

        public VideoController(ILogger<VideoController> logger, ISerializationService<Video, VideoEntity> serializationService, IVideoService videoService)
        {
            _logger = logger;
            _serializationService = serializationService;
            _videoService = videoService;
        }

        [HttpGet]
        public IActionResult GetVideos()
        {
            try
            {
                Response.ContentType = "application/octet-stream";

                var vid = _videoService.CreateVideo();
                var str = _serializationService.Serialize(vid);
                var byteArr = str.ToArray(str.Position, str.Length);

                return Ok(new ByteArrayContent(byteArr));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return Problem();
            }
        }

        [HttpPost]
        public IActionResult PostVideo(ByteBuffer buf)
        {
            try
            {
                var video = _serializationService.Deserialize(buf);
                _logger.LogInformation(video.ToString());

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return Problem();
            }
        }
    }
}