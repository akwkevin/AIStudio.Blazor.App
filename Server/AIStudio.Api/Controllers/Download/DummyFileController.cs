﻿using AIStudio.Api.Controllers.Download.Mock;
using Microsoft.AspNetCore.Mvc;

namespace AIStudio.Api.Controllers.Test
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class DummyFileController : ControllerBase
    {
        private readonly ILogger<DummyFileController> _logger;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public DummyFileController(ILogger<DummyFileController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Return the ordered bytes array according to the size.
        /// </summary>
        /// <param name="size">Size of the data</param>
        /// <returns>File stream</returns>
        [HttpGet]
        [Route("file/size/{size}")]
        public IActionResult GetFile(int size)
        {
            _logger.LogTrace($"file/size/{size}");
            var data = DummyData.GenerateOrderedBytes(size);
            return File(data, "application/octet-stream", true);
        }

        /// <summary>
        /// Return the file stream with header or not. Filename just used in URL.
        /// </summary>
        /// <param name="fileName">The file name</param>        
        /// <param name="size">Query param of the file size</param>
        /// <returns>File stream</returns>
        [Route("noheader/file/{fileName}")]
        public IActionResult GetFileWithNameNoHeader(string fileName, [FromQuery] int size)
        {
            _logger.LogTrace($"noheader/file/{fileName}?size={size}");
            var data = new MemoryStream(DummyData.GenerateOrderedBytes(size));
            return Ok(data); // return stream without header data
        }

        /// <summary>
        /// Return the file stream with header or not. Filename just used in URL.
        /// </summary>
        /// <param name="fileName">The file name</param>        
        /// <param name="size">Query param of the file size</param>
        /// <returns>File stream</returns>
        [Route("file/{fileName}")]
        public IActionResult GetFileWithName(string fileName, [FromQuery] int size)
        {
            _logger.LogTrace($"file/{fileName}?size={size}");
            byte[] fileData = DummyData.GenerateOrderedBytes(size);
            return File(fileData, "application/octet-stream", true);
        }

        /// <summary>
        /// Return the file stream with header content-length and filename.
        /// </summary>
        /// <param name="fileName">The file name</param>
        /// <param name="size">Size of the File</param>
        /// <returns>File stream</returns>
        [Route("file/{fileName}/size/{size}")]
        public IActionResult GetFileWithContentDisposition(string fileName, int size)
        {
            _logger.LogTrace($"file/{fileName}/size/{size}");
            byte[] fileData = DummyData.GenerateOrderedBytes(size);
            return File(fileData, "application/octet-stream", fileName, true);
        }

        /// <summary>
        /// Return the file stream with header content-length and filename.
        /// </summary>
        /// <param name="fileName">The file name</param>
        /// <param name="size">Size of the File</param>
        /// <returns>File stream</returns>
        [Route("file/{fileName}/size/{size}/norange")]
        public IActionResult GetFileWithNoAcceptRange(string fileName, int size)
        {
            _logger.LogTrace($"file/{fileName}/size/{size}/norange");
            byte[] fileData = DummyData.GenerateOrderedBytes(size);
            return File(fileData, "application/octet-stream", fileName, false);
        }

        /// <summary>
        /// Return the file stream with header or not. Filename just used in URL.
        /// </summary>
        /// <param name="fileName">The file name</param>        
        /// <param name="size">Query param of the file size</param>
        /// <returns>File stream</returns>
        [Route("file/{fileName}/redirect")]
        public IActionResult GetFileWithNameOnRedirectUrl(string fileName, [FromQuery] int size)
        {
            _logger.LogTrace($"file/{fileName}/redirect?size={size}");
            return LocalRedirectPermanent($"/dummyfile/file/{fileName}?size={size}");
        }

        /// <summary>
        /// Return the filled stream according to the size and failure after specific offset.
        /// </summary>
        /// <param name="size">Size of the data</param>
        /// <param name="offset">failure offset</param>
        /// <returns>File stream</returns>
        [HttpGet]
        [Route("file/size/{size}/failure/{offset}")]
        public FileStreamResult GetOverflowedFile(int size, int offset = 0)
        {
            _logger.LogTrace($"file/size/{size}/failure/{offset}");
            return File(new MockMemoryStream(size, offset), "application/octet-stream", true);
        }

        /// <summary>
        /// Return the filled stream according to the size and timeout after specific offset.
        /// </summary>
        /// <param name="size">Size of the data</param>
        /// <param name="offset">timeout offset</param>
        /// <returns>File stream</returns>
        [HttpGet]
        [Route("file/size/{size}/timeout/{offset}")]
        public FileStreamResult GetSlowFile(int size, int offset = 0)
        {
            _logger.LogTrace($"file/size/{size}/timeout/{offset}");
            return File(new MockMemoryStream(size, offset, true), "application/octet-stream", true);
        }
    }
}
