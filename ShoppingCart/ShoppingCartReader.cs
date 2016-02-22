using System;
using ShoppingCart.IO;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Accord.Math;
using System.Text;
using Accord.Controls;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Drawing.Imaging;

namespace ShoppingCart
{
    public class ShoppingCartReader
    {
        private ICharacterMatching characterClassifier;

        private LineSegmentation lineSegmentation;

        private BlockSegmentation blockSegmentation;

        private Bitmap image;

        public ShoppingCartReader(ICharacterMatching characterClassifier, ICharacterMatching newLineClassifier,
                                   ICharacterMatching blankLineClassifier)
        {
            this.lineSegmentation = new LineSegmentation(newLineClassifier);
            this.blockSegmentation = new BlockSegmentation(blankLineClassifier);
            this.characterClassifier = characterClassifier;
        }

        public string Read(string shoppingCartImageFilename)
        {
            this.image = ImageAdapter.BinarizeImage(shoppingCartImageFilename);
            return this.Read(ImageAdapter.Read(this.image));
        }

        public string Read(IEnumerable<Sample> imageRows)
        {
            var lines = this.lineSegmentation.Segment(imageRows).ToList();
            var readShoppingCart = new List<char>();
            double[,] imageMatrix; 
            new Accord.Imaging.Converters.ImageToMatrix().Convert(this.image, out imageMatrix);

            using (var g = Graphics.FromImage(this.image))
            {
                foreach (var line in lines)
                {
                    var endOfPreviousBlock = 0;
                    var blocksPerLine = this.blockSegmentation.Segment(line).ToList();
                    blocksPerLine = this.blockSegmentation.RemoveEmptyBlocks(blocksPerLine).ToList();
                    blocksPerLine = this.blockSegmentation.MergeNeighboredBlocks(blocksPerLine).ToList();
                    blocksPerLine = this.blockSegmentation.RemoveSkinnyBlocks(blocksPerLine).ToList();

                    foreach (var block in blocksPerLine)
                    {
                        var distance = block.Column - endOfPreviousBlock;
                        readShoppingCart.AddRange(Enumerable.Repeat(' ', (int)Math.Floor(distance / 5.0)));
                        endOfPreviousBlock = block.Column + block.Width;

                        var y_min = Math.Max(0, block.Row - 2);
                        var y_max = Math.Min(block.Row + block.Height + 1, this.image.Height - 1);
                        var x_min = Math.Max(0, block.Column - 2);
                        var x_max = Math.Min(block.Column + block.Width + 1, this.image.Width - 1);
                        
                        g.DrawRectangle(new Pen(Color.Red, 1.0f), new Rectangle(x_min, y_min, x_max - x_min, y_max - y_min));
                                                
                        var characterImageToClassify = imageMatrix.Submatrix(y_min, y_max, x_min, x_max); 
                         Bitmap blockImage;
                         new Accord.Imaging.Converters.MatrixToImage().Convert(characterImageToClassify, out blockImage);
                         //ImageBox.Show (blockImage);
                         MemoryStream stream = new MemoryStream();                         
                         blockImage.Save(stream, ImageFormat.Jpeg);
                         
                                               										
                        if (characterImageToClassify.Length > 0)
                        {                            
                            double prob = 0.0;
                            //char digit = this.characterClassifier.Detect(Sample.From2dMatrix(characterImageToClassify), out prob);
                            char digit = UploadFile("http://localhost:8080/classifier", stream);
                            if (prob < 0.5)
                            {
                                //ImageBox.Show (blockImage);
                            }

                            g.DrawString(new string(digit, 1), new Font("Arial", 12), Brushes.Blue, block.Column, Math.Max(block.Row - 15, 0));

                            readShoppingCart.Add(digit);
                        }
                    }
                    readShoppingCart.Add('\n');
                }
            }

            ImageBox.Show(this.image, PictureBoxSizeMode.Zoom);
            return new string(readShoppingCart.ToArray());
        }

private string HttpPost(string uri, Stream data)
{
            // Set settings
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);    
    request.ContentLength = data.Length;

    request.ContentType = "multipart/form-data";
    request.Method = "POST";
    request.Accept = "application/json";    
    request.Headers.Add("Content-Disposition", "form-data; name=\"imageFile\"; filename=\"image.JPG\"");            

    // Set the parameters
    using (StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.UTF8))
    {
        data.Position = 0;
        byte[] imageData = new byte[data.Length];
        data.Read(imageData, 0, (int)data.Length);
        
        writer.Write(imageData);
    }

    // Get the response
    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
    {
        return reader.ReadToEnd();
    }
}

    private char UploadFile(string uri, MemoryStream data)
    {
       using(var webClient = new WebClient()){                  
    string boundary = "------------------------" + DateTime.Now.Ticks.ToString("x");
    webClient.Headers.Add("Content-Type", "multipart/form-data; boundary=" + boundary);    
    var begin = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"imageFile\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n", boundary, "image.jpg", "image/jpg");    
    var end = string.Format("\r\n--{0}--\r\n", boundary);

    var bytes = new List<byte>();
    bytes.AddRange(webClient.Encoding.GetBytes(begin));
    bytes.AddRange(data.ToArray());
    bytes.AddRange(webClient.Encoding.GetBytes(end));    

    byte[] resp = webClient.UploadData(uri, "POST", bytes.ToArray());
    
    var response = webClient.Encoding.GetString(resp);
    
    var code = int.Parse(response.Replace("[", string.Empty).Replace("]", string.Empty));
    
    return this.MapCharacter(code);
    }
    }
    
    private char MapCharacter(int code){
        var letters = @"abcdefghijklmnopqrstuvwxyz";
        var characters = letters.ToCharArray();
        var upperCaseCharacters = letters.ToUpper().ToCharArray(); 
        int i = 0;
        if(code < 10){
            return char.Parse(code.ToString());
        }
        
        if(code >= 10 && code < 10 + characters.Length){
            i = code - 10;
            return upperCaseCharacters[i];
        }
        
        i = code - 10 - letters.Length;         
        return characters[i];
    }

    }
}

