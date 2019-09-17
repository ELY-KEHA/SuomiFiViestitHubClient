using System.ComponentModel.DataAnnotations;
using Keha.SuomiFiViestitHub.Client.Requests;

namespace Keha.SuomiFiViestitHub.Client
{
    /// <summary>
    /// Base64 encoded files to be sent with the message. NOTE: Max size sum of all files is 3MB
    /// </summary>
    public class ViestitMessageFile
    {
        /// <summary>File name shown in message</summary>
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }
        /// <summary>File size in kilobytes</summary>
        [Required]
        public int Size { get; set; }
        /// <summary>Base64 encoded contents of the file</summary>
        [Required(AllowEmptyStrings = false)]
        public string Content { get; set; }
        /// <summary>Format of the file (mime type) e.g. "application/pdf"</summary>
        [Required(AllowEmptyStrings = false)]
        public string ContentType { get; set; }

        internal static RequestFile ToRequestFile(ViestitMessageFile file)
        {
            return new RequestFile
            {
                Size = file.Size,
                Base64Content = file.Content,
                FileMimeType = file.ContentType,
                Name = file.Name
            };
        }
    }
}
