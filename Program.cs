﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace ExperimentWithTessaract
{
    class Program
    {
        static void Main(string[] args)
        {
            //var testImagePath = "C:\\Users\\jerry\\source\\repos\\ExperimentWithTessaract\\phototest.tif";
            //var testImagePath = "C:\\Users\\jerry\\source\\repos\\ExperimentWithTessaract\\Hyatt_KOP.JPG";
            //var testImagePath = "C:\\Users\\jerry\\source\\repos\\ExperimentWithTessaract\\Delta.png";
            //var testImagePath = "C:\\Users\\jerry\\source\\repos\\ExperimentWithTessaract\\nyc_2.jpg";
            //var testImagePath = "C:\\Users\\jerry\\source\\repos\\ExperimentWithTessaract\\waffleHouse.jpg";
            //var testImagePath = "C:\\Users\\jerry\\source\\repos\\ExperimentWithTessaract\\Walmart.jpg";
            var testImagePath = "C:\\Users\\jerry\\source\\repos\\ExperimentWithTessaract\\Walmart_hand.tif";
            if (args.Length > 0)
            {
                testImagePath = args[0];
            }

            try
            {
                var engine = new TesseractEngine(@"C:\Users\jerry\source\repos\ExperimentWithTessaract\tessdata", "eng", EngineMode.Default);
                var img = Pix.LoadFromFile(testImagePath);
                var page = engine.Process(img);
                var text = page.GetText();
                Console.WriteLine("Mean confidence: {0}", page.GetMeanConfidence());
                Console.WriteLine("Text (GetText): \r\n{0}", text);
                Console.WriteLine("Text (iterator):");
                string[] lines_to_write_to_file = { };
                using (StreamWriter file = new StreamWriter(@"C:\\Users\\jerry\\source\\repos\\ExperimentWithTessaract\\test-results.txt", false))
                {
                    var iter = page.GetIterator();
                    iter.Begin();
                    do
                    {
                        do
                        {
                            do
                            {
                                do
                                {
                                    if (iter.IsAtBeginningOf(PageIteratorLevel.Block))
                                    {
                                        file.WriteLine(" < BLOCK>");
                                        Console.WriteLine("<BLOCK>");
                                    }

                                    file.Write(iter.GetText(PageIteratorLevel.Word));
                                    file.Write(" ");

                                    Console.Write(iter.GetText(PageIteratorLevel.Word));
                                    Console.Write(" ");

                                    if (iter.IsAtFinalOf(PageIteratorLevel.TextLine, PageIteratorLevel.Word))
                                    {
                                        file.WriteLine("");
                                        Console.WriteLine();
                                    }
                                } while (iter.Next(PageIteratorLevel.TextLine, PageIteratorLevel.Word));

                                if (iter.IsAtFinalOf(PageIteratorLevel.Para, PageIteratorLevel.TextLine))
                                {
                                    file.WriteLine("");

                                    Console.WriteLine();
                                }
                            } while (iter.Next(PageIteratorLevel.Para, PageIteratorLevel.TextLine));
                        } while (iter.Next(PageIteratorLevel.Block, PageIteratorLevel.Para));
                    } while (iter.Next(PageIteratorLevel.Block));
                }

            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
                Console.WriteLine("Unexpected Error: " + e.Message);
                Console.WriteLine("Details: ");
                Console.WriteLine(e.ToString());
            }

            Console.Write("Press any key to continue . . . ");
            Console.ReadKey(true);
        }
    }
}
