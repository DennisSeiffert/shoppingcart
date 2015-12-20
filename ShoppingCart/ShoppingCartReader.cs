﻿using System;
using ShoppingCart.IO;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Accord.Math;
using System.Threading.Tasks;
using System.Text;
using Accord.Controls;
using System.Windows.Forms;
using System.Drawing.Text;
using Accord.Imaging.Filters;
using Accord.Imaging;
using AForge;
using AForge.Imaging;
using AForge.Math;
using System.Numerics;

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
                        // ImageBox.Show (blockImage);
                                               										
                        if (characterImageToClassify.Length > 0)
                        {                            
                            double prob = 0.0;
                            char digit = this.characterClassifier.Detect(Sample.From2dMatrix(characterImageToClassify), out prob);
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


    }
}

