using OpenCvSharp;

var result = Location(@$"{Environment.CurrentDirectory.Split(@"\bin")[0]}\Assets\", "8.png", 95, 4000, 10000, 400);

static double Location(string path, string name, int threshold, double areaMin, double areaMax, double centerXMin)
{
    using var gray = Cv2.ImRead(path + name, ImreadModes.Grayscale);
    using var binary = new Mat();
    _ = Cv2.Threshold(gray, binary, threshold, 0xFF, ThresholdTypes.Binary);
    Cv2.FindContours(binary, out var contours, out _, RetrievalModes.Tree, ContourApproximationModes.ApproxNone);
    for (var i = 0; i < contours.Length; ++i)
    {
        var moments = Cv2.Moments(contours[i]);
        try
        {
            var centerX = moments.M10 / moments.M00;
            var centerY = moments.M01 / moments.M00;
            var area = Cv2.ContourArea(contours[i]);
            if (area  < areaMin || area > areaMax || centerX < centerXMin)
                continue;
            using var origin = Cv2.ImRead(path + name);
            origin.DrawContours(contours, i, Scalar.GreenYellow, 5);
            Cv2.ImShow("result", origin);
            _ = Cv2.WaitKey();
            return centerX;
        }
        catch
        {
            continue;
        }
    }
    return -1;
}
