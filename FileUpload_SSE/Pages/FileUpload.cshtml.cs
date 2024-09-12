using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FileUpload_SSE.Pages
{
    [BindProperties]
    public class FileUploadModel : PageModel
    {
        private IWebHostEnvironment _environment;
        private HttpContext _context;
        [BindProperty]
        public IFormFile Upload { get; set; }
        public FileUploadModel(IWebHostEnvironment environment)
        {
            _environment = environment;
            _context = HttpContext;
        }
        public void OnGet()
        {
            //if (_context != null)
            //{
            //    if (_context.Request.Headers["Accept"].ToString() == "text/event-stream")
            //    {
            //        _context.Response.ContentType = "text/event-stream";
            //        _context.Response.Headers["Cache-Control"] = "no-cache";
            //        _context.Response.Headers["Connection"] = "keep-alive";
            //    }
            //}


        }

        public async Task OnPostAsync()
        {
            //var dir = Path.Combine(_environment.ContentRootPath, "uploads");
            //var file = Path.Combine(dir, Upload.FileName);
            //Directory.CreateDirectory(dir);
            //using (var fileStream = new FileStream(file, FileMode.Create))
            //{
            //    await Upload.CopyToAsync(fileStream);
            //}
        }
    }
}
