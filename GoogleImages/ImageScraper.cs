using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace GoogleImages
{
 public class ImageScraper
    {
        // this function retrieves the HTML code from a Google image search
        public string GetHtmlCode(string SearchKey)
        {
            string url = "https://www.google.com/search?q=" + SearchKey  + "&tbm=isch";
            string imageHtml = "";

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";

            var response = (HttpWebResponse)request.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                if (dataStream == null)
                    return "";
                using (var sr = new StreamReader(dataStream))
                {
                    imageHtml = sr.ReadToEnd();
                }
            }
            return imageHtml;
        }

        ////this func parses out the img tags located underneath the images_table
        ////and stores the URL's of the images in a list:
        public List<string> GetUrls(string html)
        {
            //specify the number of images to be downloaded
            int imgToDownload = 3;

            //create the list of string that will hold the urls
            var urls = new List<string>();

            int ndx = html.IndexOf("\"ou\"", StringComparison.Ordinal);

            while (urls.Count < imgToDownload && ndx >= 0)
            {
                ndx = html.IndexOf("\"", ndx + 4, StringComparison.Ordinal);
                ndx++;
                int ndx2 = html.IndexOf("\"", ndx, StringComparison.Ordinal);
                string url = html.Substring(ndx, ndx2 - ndx);
                urls.Add(url);
                ndx = html.IndexOf("\"ou\"", ndx2, StringComparison.Ordinal);
            }



            //string search = @",""ou"":""(.*?)"",";
            //MatchCollection matches = Regex.Matches(html, search);

            //foreach (Match match in matches)
            //{
            //    urls.Add(match.Groups[1].Value);
            //}

            return urls;
            
        }
        //we take each URL and have it download the image bytes into a byte array:
        public byte[] GetImage(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            var response = (HttpWebResponse)request.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                if (dataStream == null)
                    return null;
                using (var sr = new BinaryReader(dataStream))
                {
                    byte[] bytes = sr.ReadBytes(100000);

                    return bytes;
                }
            }
            
        }
    }
}
