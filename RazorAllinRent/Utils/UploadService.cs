namespace RazorAllinRent.Utils
{
    public class UploadService
    {
        private readonly string _uploadsFolder;

        public UploadService(IWebHostEnvironment environment)
        {
            // Define the path where images will be saved
            _uploadsFolder = Path.Combine(environment.ContentRootPath, "..", "SharedImages");

            // Create the directory if it doesn't exist
            if (!Directory.Exists(_uploadsFolder))
            {
                Directory.CreateDirectory(_uploadsFolder);
            }
        }

        // Method to handle file upload
        public async Task<string> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("No file uploaded.");
            }

            // Ensure the file is an image (optional validation)
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new ArgumentException("Invalid file type.");
            }

            // Generate a unique file name
            var uniqueFileName = Guid.NewGuid().ToString() + fileExtension;

            // Create the full path to save the file
            var filePath = Path.Combine(_uploadsFolder, uniqueFileName);

            // Save the file to the disk
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // Return the relative path to access the image
            return uniqueFileName;
        }
    }
}
