using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HaystackFacecrop
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var client = new WebClient())
            {
                var fileData = System.IO.File.ReadAllBytes(args[0]);
                var picture = new Bitmap(args[0]);
                var resData = client.UploadData("https://api.haystack.ai/api/image/analyze?output=json&apikey=**API_KEY_HERE**&model=gender", "POST", fileData);
                var responseText = System.Text.Encoding.UTF8.GetString(resData);
                dynamic response = JsonConvert.DeserializeObject(responseText);
                var i = 0;

                foreach (dynamic person in response.people)
                {
                    int x = person.location.x;
                    int y = person.location.y;
                    int w = person.location.width;
                    int h = person.location.height;

                    var face = new Bitmap(w, h);
                    var cropRect = new Rectangle(x, y, w, h);

                    using(var graphics = Graphics.FromImage(face))
                    {
                        graphics.DrawImage(picture, new Rectangle(0, 0, face.Width, face.Height), cropRect, GraphicsUnit.Pixel);
                        face.Save($"face_{i++}.jpg");
                    }
                }
            }
        }
    }
}

