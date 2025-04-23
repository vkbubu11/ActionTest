using System.IO;
using System.Drawing;

using static System.Net.Mime.MediaTypeNames;

Main();
static void Main()
{
    Console.WriteLine("Processing Started");
    var reader = new StreamReader("C:\\Users\\biote\\Desktop\\Development\\EKOP\\VisualizeEyeTrackingOnSample\\Measurements\\EyeTrackingBasedOnTiles_20241129.txt");
    Bitmap Timeimage = ConvertToBitmap("C:\\Users\\biote\\Desktop\\Development\\EKOP\\VisualizeEyeTrackingOnSample\\50.png");
    Bitmap PrimerImage = ConvertToBitmap("C:\\Users\\biote\\Desktop\\Development\\EKOP\\VisualizeEyeTrackingOnSample\\50.png");

    List<int> XCoordinates = new List<int>();
    List<int> YCoordinates = new List<int>();
    List<int> ZCoordinates = new List<int>();
    List<double> TimeData = new List<double>();
    List<int> PrimerValues = new List<int>();

    int PointLookingTime;
    int TimeSpentWithEyeOnGivenPointInMiliSec = 1000;
    bool visualizeTime = true;
    int NarrowPointAccaptenceDistance = 10;

    int OriginalMedicalSampleWidth = 35584;
    int OriginalMedicalSampleHeight = 35840;

    int UsedTileWidthDuring3DTiling = 1112;
    int UsedTileHeightDuring3DTiling = 1024;



    while (!reader.EndOfStream)
    {
        var line = reader.ReadLine();
        var values = line.Split('\t');

        ZCoordinates.Add((Int32.Parse(values[0])));
        XCoordinates.Add((Int32.Parse(values[1]) * UsedTileWidthDuring3DTiling) / (OriginalMedicalSampleWidth / Timeimage.Width));
        YCoordinates.Add((Int32.Parse(values[2]) * UsedTileHeightDuring3DTiling) / (OriginalMedicalSampleHeight / Timeimage.Height));
        TimeData.Add(Convert.ToDouble(values[3]));
        PrimerValues.Add(Int32.Parse(values[4]));
    }

    for (int i = 0; i < XCoordinates.Count; i++)
    {
        if (TimeData[i] > 0.0)
        {
            for (int j = XCoordinates[i] - NarrowPointAccaptenceDistance; j < XCoordinates[i] + NarrowPointAccaptenceDistance; j++)
            {
                for (int k = YCoordinates[i] - NarrowPointAccaptenceDistance; k < YCoordinates[i] + NarrowPointAccaptenceDistance; k++)
                {

                    if (Math.Sqrt((Math.Pow(j - Convert.ToDouble(XCoordinates[i]), 2) + Math.Pow(k - Convert.ToDouble(YCoordinates[i]), 2))) <= NarrowPointAccaptenceDistance)
                    {
                        if (j >= 0 && k >= 0 && j < Timeimage.Width && k < Timeimage.Height)
                        {
                            Timeimage.SetPixel(j, k, Color.FromArgb(255, 64, 52, 235));
                        }

                    }
                }
            }
        }

    }

    Timeimage.Save("C:\\Users\\biote\\Desktop\\Development\\EKOP\\VisualizeEyeTrackingOnSample\\TimaDataImage.png");
    Console.WriteLine("TimeImage Processing is over");

    for (int i = 0; i < XCoordinates.Count; i++)
    {
        for (int j = XCoordinates[i] - NarrowPointAccaptenceDistance; j < XCoordinates[i] + NarrowPointAccaptenceDistance; j++)
        {
            for (int k = YCoordinates[i] - NarrowPointAccaptenceDistance; k < YCoordinates[i] + NarrowPointAccaptenceDistance; k++)
            {
                if (PrimerValues[i] == 1)
                {
                    if (Math.Sqrt((Math.Pow(j - Convert.ToDouble(XCoordinates[i]), 2) + Math.Pow(k - Convert.ToDouble(YCoordinates[i]), 2))) <= NarrowPointAccaptenceDistance)
                    {
                        if (j >= 0 && k >= 0 && j < PrimerImage.Width && k < PrimerImage.Height)
                        {
                            PrimerImage.SetPixel(j, k, Color.Green);
                        }

                    }
                    
                }
                if (PrimerValues[i] == 2)
                {
                    if (Math.Sqrt((Math.Pow(j - Convert.ToDouble(XCoordinates[i]), 2) + Math.Pow(k - Convert.ToDouble(YCoordinates[i]), 2))) <= NarrowPointAccaptenceDistance)
                    {
                        if (j >= 0 && k >= 0 && j < PrimerImage.Width && k < PrimerImage.Height)
                        {
                            PrimerImage.SetPixel(j, k, Color.Orange);
                        }

                    }
                }
            }
        }

        
    }

    PrimerImage.Save("C:\\Users\\biote\\Desktop\\Development\\EKOP\\VisualizeEyeTrackingOnSample\\PrimerImage.png");
    Console.WriteLine("PrimerImage Processing is over");


    static Bitmap ConvertToBitmap(string fileName)
    {
        Bitmap bitmap;
        using (Stream bmpStream = System.IO.File.Open(fileName, System.IO.FileMode.Open))
        {
            System.Drawing.Image image = System.Drawing.Image.FromStream(bmpStream);

            bitmap = new Bitmap(image);

        }
        return bitmap;
    }
}