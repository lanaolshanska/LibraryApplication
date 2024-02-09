using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Library.BusinessLogic
{
	public class FileHelper()
	{
		public string SaveFile(IFormFile? file, string folderPath, string rootPath)
		{
			var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file?.FileName);
			var filePath = Path.Combine(folderPath, fileName);
			var fullPath = Path.Combine(rootPath, filePath);

			using (var fileStream = new FileStream(fullPath, FileMode.Create))
			{
				file?.CopyTo(fileStream);
			}
			return filePath;
		}

		public void DeleteFile(string fileUrl, string rootPath)
		{
			var oldFilePath = Path.Combine(rootPath, fileUrl);
			if (File.Exists(oldFilePath))
			{
				File.Delete(oldFilePath);
			}
		}
	}
}
