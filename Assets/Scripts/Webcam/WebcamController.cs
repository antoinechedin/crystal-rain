using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Aruco;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using UnityEngine;

public class WebcamController : MonoBehaviour
{
    public PointF[] markerWrapPositions;

    PointF[] markerPositions;
    PointF[] boardCorners;
    VideoCapture webcam;
    Dictionary markerDict;
    GridBoard gridBoard;
    DetectorParameters arucoParameters;

    private void Awake()
    {
        boardCorners = new PointF[4];
        markerPositions = new PointF[4];
        markerWrapPositions = new PointF[4];

        markerDict = new Dictionary(Dictionary.PredefinedDictionaryName.Dict4X4_50);
        gridBoard = new GridBoard(4, 4, 80, 30, markerDict);
        arucoParameters = DetectorParameters.GetDefault();
    }

    private void Start()
    {
        Debug.Log("start");
        webcam = new VideoCapture(0);
        //webcam.FlipHorizontal = true;
        webcam.ImageGrabbed += HandleWebcam;
    }

    private void Update()
    {
        if (webcam.IsOpened) webcam.Grab();
    }

    private void HandleWebcam(object sender, EventArgs e)
    {
        Image<Bgr, byte> origin = new Image<Bgr, byte>(webcam.Width, webcam.Height);
        if (webcam.IsOpened) webcam.Retrieve(origin);

        Image<Bgr, byte> markers = origin.Clone();
        Image<Bgr, byte> output = origin.Clone();
        Image<Bgr, byte> transformed = new Image<Bgr, byte>(512, 512);

        // Gather marker
        VectorOfInt ids = new VectorOfInt();
        VectorOfVectorOfPointF corners = new VectorOfVectorOfPointF();
        ArucoInvoke.DetectMarkers(origin, markerDict, corners, ids, arucoParameters);

        PointF[] wrapCorners = new PointF[4];
        wrapCorners[0] = new PointF(0, 0);
        wrapCorners[1] = new PointF(512, 0);
        wrapCorners[2] = new PointF(512, 512);
        wrapCorners[3] = new PointF(0, 512);
        Mat perspectiveMatrix = CvInvoke.GetPerspectiveTransform(boardCorners, wrapCorners);
        CvInvoke.WarpPerspective(origin, transformed, perspectiveMatrix, new Size(512, 512));

        for (int i = 0; i < ids.Size; i++)
        {
            switch (ids[i])
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    boardCorners[ids[i]] = getCentroid(corners[i]);
                    break;
                case 4:
                case 5:
                case 6:
                case 7:
                    markerPositions[ids[i] - 4] = getCentroid(corners[i]);
                    break;
            }
        }
        markerWrapPositions = CvInvoke.PerspectiveTransform(markerPositions, perspectiveMatrix);

        CvInvoke.Line(output, new Point((int)boardCorners[0].X, (int)boardCorners[0].Y), new Point((int)boardCorners[1].X, (int)boardCorners[1].Y), new MCvScalar(0, 0, 255), 2);
        CvInvoke.Line(output, new Point((int)boardCorners[1].X, (int)boardCorners[1].Y), new Point((int)boardCorners[2].X, (int)boardCorners[2].Y), new MCvScalar(0, 0, 255), 2);
        CvInvoke.Line(output, new Point((int)boardCorners[2].X, (int)boardCorners[2].Y), new Point((int)boardCorners[3].X, (int)boardCorners[3].Y), new MCvScalar(0, 0, 255), 2);
        CvInvoke.Line(output, new Point((int)boardCorners[3].X, (int)boardCorners[3].Y), new Point((int)boardCorners[0].X, (int)boardCorners[0].Y), new MCvScalar(0, 0, 255), 2);
        CvInvoke.Line(output, new Point((int)boardCorners[0].X, (int)boardCorners[0].Y), new Point((int)boardCorners[2].X, (int)boardCorners[2].Y), new MCvScalar(0, 0, 255), 2);
        CvInvoke.Line(output, new Point((int)boardCorners[1].X, (int)boardCorners[1].Y), new Point((int)boardCorners[3].X, (int)boardCorners[3].Y), new MCvScalar(0, 0, 255), 2);

        // For graphic debug purpose
        ArucoInvoke.DetectMarkers(transformed, markerDict, corners, ids, arucoParameters);
        if (ids.Size > 0)
        {
            ArucoInvoke.DrawDetectedMarkers(transformed, corners, ids, new MCvScalar(0, 0, 255));
        }

        // CvInvoke.Imshow("Markers view", markers);
        CvInvoke.Imshow("Output view", output);
        CvInvoke.Imshow("Wrap view", transformed);
        // CvInvoke.Imshow("Shape detection view", shapeDetection);
    }

    private static PointF getCentroid(VectorOfPointF points)
    {
        float xMean = 0, yMean = 0;
        for (int i = 0; i < points.Size; i++)
        {
            xMean += points[i].X;
            yMean += points[i].Y;
        }
        xMean /= points.Size;
        yMean /= points.Size;
        return new PointF(xMean, yMean);
    }

    private void OnDestroy()
    {
        webcam.Dispose();
        CvInvoke.DestroyAllWindows();
    }

}
