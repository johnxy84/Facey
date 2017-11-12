using Plugin.Media.Abstractions;
using System.IO;

namespace Facey.Helpers
{
    public static class Helper
    {
        public static byte[] GetByte(MediaFile file)

        {
            var imageAsBytes = new byte[] { };

            using (var memoryStream = new MemoryStream())

            {
                file.GetStream().CopyTo(memoryStream);

                imageAsBytes = memoryStream.ToArray();
            }

            return imageAsBytes;
        }
    }
}