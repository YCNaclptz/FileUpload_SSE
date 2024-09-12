using Newtonsoft.Json;

namespace FileUpload_SSE.Middleware
{
    public class FileUploadProgressMiddleware
    {
        private readonly RequestDelegate _next;

        public FileUploadProgressMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Method == HttpMethods.Post && context.Request.Form.Files.Count > 0)
            {
                var file = context.Request.Form.Files[0];
                var totalBytes = file.Length;
                var uploadedBytes = 0L;

                context.Response.ContentType = "text/event-stream";
                context.Response.Headers["Cache-Control"] = "no-cache";
                context.Response.Headers["Connection"] = "keep-alive";

                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
                Directory.CreateDirectory(uploads);
                var filePath = Path.Combine(uploads, file.FileName);

                using (var fileStream = file.OpenReadStream())
                using (var outputStream = new FileStream(filePath, FileMode.Create))
                {
                    var buffer = new byte[1024];
                    int bytesRead;
                    while ((bytesRead = await fileStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        uploadedBytes += bytesRead;
                        await outputStream.WriteAsync(buffer, 0, bytesRead);

                        // 發送進度更新
                        var progress = (uploadedBytes / (double)totalBytes) * 100;
                        string szResponseJSON = JsonConvert.SerializeObject(new { uploadedBytes, totalBytes, progress = Convert.ToInt32(progress) });
                        await context.Response.WriteAsync($"data: {szResponseJSON}\n\n");
                        await context.Response.Body.FlushAsync();
                    }
                }
                await context.Response.CompleteAsync();
            }
            else
            {
                await _next(context);
            }
        }
    }
}
