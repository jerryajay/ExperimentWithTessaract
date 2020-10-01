1.	Create a default console application. Make sure that it is .net framework rather than a console app.
2.	Then, inside the Main function, add the code (you would have to change the path according to the username of your system.):

var testImagePath = "C:\\Users\\jerry\\source\\repos\\ExperimentWithTessaract\\phototest.tif";
            if (args.Length > 0)
            {
                testImagePath = args[0];
            }

            try
            {
                using (var engine = new TesseractEngine(@"C:\Users\jerry\source\repos\ExperimentWithTessaract\tessdata", "eng", EngineMode.Default))
                {
                    using (var img = Pix.LoadFromFile(testImagePath))
                    {
                        using (var page = engine.Process(img))
                        {
                            var text = page.GetText();
                            Console.WriteLine("Mean confidence: {0}", page.GetMeanConfidence());

                            Console.WriteLine("Text (GetText): \r\n{0}", text);
                            Console.WriteLine("Text (iterator):");
                            using (var iter = page.GetIterator())
                            {
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
                                                    Console.WriteLine("<BLOCK>");
                                                }

                                                Console.Write(iter.GetText(PageIteratorLevel.Word));
                                                Console.Write(" ");

                                                if (iter.IsAtFinalOf(PageIteratorLevel.TextLine, PageIteratorLevel.Word))
                                                {
                                                    Console.WriteLine();
                                                }
                                            } while (iter.Next(PageIteratorLevel.TextLine, PageIteratorLevel.Word));

                                            if (iter.IsAtFinalOf(PageIteratorLevel.Para, PageIteratorLevel.TextLine))
                                            {
                                                Console.WriteLine();
                                            }
                                        } while (iter.Next(PageIteratorLevel.Para, PageIteratorLevel.TextLine));
                                    } while (iter.Next(PageIteratorLevel.Block, PageIteratorLevel.Para));
                                } while (iter.Next(PageIteratorLevel.Block));
                            }
                        }
                    }
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



3. Now, copy a .tif/.png/.jpg file from a sample collection into the Project structure. The phototest.tif file is obtainable from a subdirectory of: https://github.com/charlesw/tesseract-samples . 
4. Make sure you have Tesseract version 3.0.0 installed via the command : Install-Package Tesseract -Version 3.0.0 put inside the Package Manager Console.
5. Download tessdata: Now, download tessdata 3.04.00 from https://github.com/tesseract-ocr/tessdata/releases/tag/3.04.00 and place the extracted folder inside your project. Rename it to tessdata (rather than tessdata-3.04.00) 
6. Set the environment variable TESSDATA_PREFIX as the path the parent folder where .\tessdata folder resides. In my project setup, the path points to C:\Users\jerry\source\reposExperimentWithTessaract .

After the 6 steps, things should hopefully work fine. 



Note: Make sure the version matches, I’m using Tesseract version 3.0.0 and the downloaded tessdata is 3.04.00. If there is a mismatch in version numbers, the code will not work. 

Happy coding!


References:
1. Starting point: https://github.com/charlesw/tesseract/ . Follow this with some modifications according to the points stated above.
2. Sample source code that is put in the project is available from : https://github.com/charlesw/tesseract-samples
3. For the error: https://github.com/charlesw/tesseract/wiki/Error-1