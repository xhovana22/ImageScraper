using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace GoogleImages
{
    class Program
    {
        static void Main(string[] args)
        {
           //prompt the user to enter the search key
            Console.WriteLine("Search: ");
            string searchKey = Console.ReadLine();

            //define the folder where the images will be saved after download
            string saveImages = "C:\\Users\\xhova\\Source\\Repos\\GoogleImages\\GoogleImages\\Images\\";

            //create an scrapper object
            ImageScraper scp = new ImageScraper();

            //get the HTML code
            string html = scp.GetHtmlCode(searchKey);

            //parse
            List<string> urls = scp.GetUrls(html);

            //download each image
            for(int i=0;i<urls.Count;i++)
            {
                byte[] image = scp.GetImage(urls[i]);

                using (var ms = new MemoryStream(image))
                {
                    Image pictureBox = Image.FromStream(ms);
                    pictureBox.Save(saveImages + searchKey +"_" + i + ".png", ImageFormat.Jpeg);
                  
                }
            }
          

           
            

        }

 
    }
}
