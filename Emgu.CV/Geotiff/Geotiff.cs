﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Geodetic;
using Emgu.CV.Structure;
using Emgu.CV;
using System.Drawing;

namespace Emgu.CV.Geotiff
{
   /// <summary>
   /// A class that can be used for writing geotiff
   /// </summary>
   public class Geotiff
   {
      #region PInvoke
      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private extern static void geotiffWriteImage(
         [MarshalAs(CvInvoke._stringMarshalType)]
         string fileSpec,
         IntPtr image,
         ref GeodeticCoordinate coor,
         ref MCvPoint3D64f pixelResolution);
      #endregion

      /// <summary>
      /// Save the image as a geotiff file
      /// </summary>
      /// <param name="fileName">The file name to be saved</param>
      /// <param name="image">The image to be written to the geotiff</param>
      /// <param name="originCoordinate">The coordinate of the origin. To be specific, this is the coordinate of the upper left corner of the pixel in the origin</param>
      /// <param name="pixelResolution">The resolution of the pixel in meters</param>
      public static void Save(String fileName, Image<Gray, Byte> image, GeodeticCoordinate originCoordinate, MCvPoint2D64f pixelResolution)
      {
         Size size = image.Size;
         GeodeticCoordinate lowerRight = TransformationWGS84.NED2Geodetic(
            new MCvPoint3D64f(pixelResolution.x * size.Height, pixelResolution.y * size.Width, 0.0), 
            originCoordinate);
         //MCvPoint3D64f res = new MCvPoint3D64f(pixelResolution.x, pixelResolution.y, 0.0);

         MCvPoint3D64f res = new MCvPoint3D64f(
            (lowerRight.Longitude - originCoordinate.Longitude) * (180.0 / Math.PI) / size.Width, 
            (lowerRight.Latitude - originCoordinate.Latitude) * (180.0 / Math.PI) / size.Height, 
            0.0);
         geotiffWriteImage(fileName, image.Ptr, ref originCoordinate, ref res);
      }

      
      /// <summary>
      /// Save the image as a geotiff file
      /// </summary>
      /// <param name="fileName">The file name to be saved</param>
      /// <param name="image">The image to be written to the geotiff</param>
      /// <param name="originCoordinate">The coordinate of the origin. To be specific, this is the coordinate of the upper left corner of the pixel in the origin</param>
      /// <param name="pixelResolution">The resolution of the pixel in meters</param>
      public static void Save(String fileName, Image<Bgra, Byte> image, GeodeticCoordinate originCoordinate, MCvPoint2D64f pixelResolution)
      {
         GeodeticCoordinate lowerRight = TransformationWGS84.NED2Geodetic(new MCvPoint3D64f(pixelResolution.x * image.Height, pixelResolution.y * image.Width, 0.0), originCoordinate);
         //MCvPoint3D64f res = new MCvPoint3D64f(pixelResolution.x, pixelResolution.y, 0.0);
         MCvPoint3D64f res = new MCvPoint3D64f(
            (lowerRight.Longitude - originCoordinate.Longitude) * (180.0 / Math.PI) / image.Width,
            (lowerRight.Latitude - originCoordinate.Latitude) * (180.0 / Math.PI) / image.Height,
            0.0);

         //swap the B and R channel since geotiff assume RGBA for 4 channels image of depth Byte
         using (Image<Bgra, Byte> clone = image.Clone())
         using (Image<Gray, Byte> b = image[0])
         using (Image<Gray, Byte> r = image[2])
         {
            clone[2] = b;
            clone[0] = r;
            geotiffWriteImage(fileName, clone, ref originCoordinate, ref res);
         }
      }

      /// <summary>
      /// Save the image as a geotiff file
      /// </summary>
      /// <param name="fileName">The file name to be saved</param>
      /// <param name="image">The image to be written to the geotiff</param>
      /// <param name="originCoordinate">The coordinate of the origin. To be specific, this is the coordinate of the upper left corner of the pixel in the origin</param>
      /// <param name="pixelResolution">The resolution of the pixel in meters</param>
      public static void Save(String fileName, Image<Bgr, Byte> image, GeodeticCoordinate originCoordinate, MCvPoint2D64f pixelResolution)
      {
         GeodeticCoordinate lowerRight = TransformationWGS84.NED2Geodetic(new MCvPoint3D64f(pixelResolution.x * image.Height, pixelResolution.y * image.Width, 0.0), originCoordinate);
         //MCvPoint3D64f res = new MCvPoint3D64f(pixelResolution.x, pixelResolution.y, 0.0);
         MCvPoint3D64f res = new MCvPoint3D64f(
            (lowerRight.Longitude - originCoordinate.Longitude) * (180.0 / Math.PI) / image.Width,
            (lowerRight.Latitude - originCoordinate.Latitude) * (180.0 / Math.PI) / image.Height,
            0.0);

         //swap the B and R channel since geotiff assume RGB for 3 channels image of depth Byte
         using (Image<Bgr, Byte> clone = image.Clone())
         using (Image<Gray, Byte> b = image[0])
         using (Image<Gray, Byte> r = image[2])
         {
            clone[2] = b;
            clone[0] = r;
            geotiffWriteImage(fileName, clone, ref originCoordinate, ref res);
         }
      }
   }
}