using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using VMS.TPS.Common.Model.API;

namespace TMLtoAria
{
    public class TMLReader
    {
        public static PdfDocument ConvertTMLtoPDF(string filePathTML)
        {          
            List<string> TMLSheet = ParseTML(filePathTML);

            PdfDocument outputDocument = new PdfDocument();

            int xPos = 0;
            int yPos = 0;
            int margin = 0;
            int fontSize = 30/3;
            int fontWeight = 300;
            string fontFamily = "Arial";
            XFont font = new XFont(fontFamily, fontSize, GetFontStyle(fontWeight));
            string text = "";
            int lineSpacing = 1;
            int sectionHeight = 300;
            int i = 0;
            string pageOrientation = "Portrait";
            PdfPage firstPage = new PdfPage();

            try
            {
                foreach (string line in TMLSheet)
                {
                    i++;
                    if (string.IsNullOrWhiteSpace(line))
                        continue;
                    if (line.StartsWith("#"))
                        continue;
                        
                    if (line.StartsWith("EndIf"))
                    {

                    }
                    if (line.StartsWith("Else"))
                    {

                    }
                    if (line.StartsWith("Font"))
                    {
                        var value = line.Split('=')[1];
                        //fontSize = Convert.ToInt32(value.Split(';')[0])/3;
                        fontWeight = Convert.ToInt32(value.Split(';')[1]);
                        fontFamily = value.Split(';')[2];
                        font = new XFont(fontFamily, fontSize, GetFontStyle(fontWeight));
                    }
                    if (line.StartsWith("Height"))
                    {

                    }
                    if (line.StartsWith("If"))
                    {
                        var value = line.Split('=')[1];
                        if (value == "$PatientId2")
                        {

                        }
                        if (value == "$Comment")
                        {

                        }
                        if (value == "$PatientComment")
                        {

                        }
                        if (value == "$PlanName")
                        {

                        }
                        if (value == "$PlanComment")
                        {

                        }
                        if (value == "$ImageComment")
                        {

                        }
                        if (value == "$StructureSetComment")
                        {

                        }
                        if (value == "$TableShift")
                        {

                        }
                        if (value == "$IsFieldSymmetric")
                        {

                        }
                        if (value == "$FieldGantryRotationDir")
                        {

                        }
                        if (value == "$IsMotorizedWedgeField")
                        {

                        }
                        if (value == "$IsFieldSetup")
                        {

                        }
                        if (value == "$IsFieldIMRT")
                        {

                        }
                        if (value == "$IsMLCPlanStatic")
                        {

                        }
                        if (value == "$FieldBoluses")
                        {

                        }
                        if (value == "$RefPoints")
                        {

                        }
                        if (value == "$Fields")
                        {

                        }
                        if (value == "$FieldDose")
                        {

                        }
                        if (value == "$FieldCalculationError")
                        {

                        }
                        if (value == "$FieldCalculationWarning")
                        {

                        }
                        if (value == "$TreatmentApprovalDate")
                        {

                        }
                        if (value == "$PlanningApprovalDate")
                        {

                        }
                    }
                    if (line.StartsWith("HorizontalLine"))
                    {
                        using (XGraphics gfx = XGraphics.FromPdfPage(firstPage))
                        {
                            XPen lineBlack = new XPen(XColors.Black, 5);
                            gfx.DrawLine(lineBlack, xPos, yPos, xPos + firstPage.Width, yPos);
                        }
                    }
                    if (line.StartsWith("LineSpacing"))
                    {

                    }
                    if (line.StartsWith("LineText"))
                    {

                    }
                    if (line.StartsWith("Loop"))
                    {

                    }
                    if (line.StartsWith("Margin"))
                    {
                        margin = Convert.ToInt32(line.Split('=')[1]);
                    }
                    if (line.StartsWith("Move"))
                    {
                        var value = line.Split('=')[1];
                        if (line.Contains(";"))
                        {
                            xPos = Convert.ToInt32(value.Split(';')[0]) + margin;
                            yPos = Convert.ToInt32(value.Split(';')[1]);
                        }
                        else // move(0) or move(100)
                        {
                            xPos = Convert.ToInt32(value) + margin;
                            if (value == "0")
                            {
                                yPos += 20;
                            }
                        }
                    }
                    if (line.StartsWith("PageBreak"))
                    {
                        if (pageOrientation == "Landscape")
                        {
                            firstPage.Orientation = PdfSharp.PageOrientation.Landscape;
                        }
                        else
                        {
                            firstPage.Orientation = PdfSharp.PageOrientation.Landscape;
                        }
                    }
                    if (line.StartsWith("PaperOrientation"))
                    {
                        if (line.Split('=')[1].Contains("Landscape"))
                        {
                            firstPage.Orientation = PdfSharp.PageOrientation.Landscape;
                            pageOrientation = "Landscape";
                        }
                        else
                        {
                            firstPage.Orientation = PdfSharp.PageOrientation.Landscape;
                            pageOrientation = "Portrait";
                        }
                        outputDocument.Pages.Add(firstPage);
                    }
                    if (line.StartsWith("PaperSize"))
                    {
                        if (line.Split('=')[1].Contains("A4"))
                        {
                            firstPage.Size = PdfSharp.PageSize.A4;
                        }
                        if (line.Split('=')[1].Contains("A3"))
                        {
                            firstPage.Size = PdfSharp.PageSize.A3;
                        }
                        if (line.Split('=')[1].Contains("Legal"))
                        {
                            firstPage.Size = PdfSharp.PageSize.Legal;
                        }
                        if (line.Split('=')[1].Contains("Letter"))
                        {
                            firstPage.Size = PdfSharp.PageSize.Letter;
                        }
                        if (line.Split('=')[1].Contains("Tabloid"))
                        {
                            firstPage.Size = PdfSharp.PageSize.Tabloid;
                        }
                    }
                    if (line.StartsWith("RelativeMove"))
                    {
                        var value = line.Split('=')[1];
                        if (line.Contains(";"))
                        {
                            xPos += Convert.ToInt32(value.Split(';')[0]) + margin;
                            yPos += Convert.ToInt32(value.Split(';')[1]);
                        }
                        else // move(0) or move(100)
                        {
                            xPos += Convert.ToInt32(value) + margin;
                            if (value == "0")
                            {
                                yPos += 20;
                            }
                        }
                        continue;
                    }
                    if (line.StartsWith("RelativeMoveText"))
                    {

                    }

                    if (line.StartsWith("Section"))
                    {

                    }
                    if (line.StartsWith("Text") || line.StartsWith("LineText"))
                    {
                        text = line.Split('=')[1];
                        using (XGraphics gfx = XGraphics.FromPdfPage(firstPage))
                        {
                            gfx.DrawString(text, font, XBrushes.Black,
                                           new XRect(xPos, yPos, 100, 10),
                                           XStringFormats.TopLeft);
                        }
                    }
                    if (line.StartsWith("TextRight"))
                    {

                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error reading TML file \n " + filePathTML + "\n" + "at line" + i);
            }

            return outputDocument;
        }

        public static List<string> ParseTML(string path)
        {
            List<string> parsedData = new List<string>();
            string fields;

            try
            {
                var reader = new StreamReader(File.OpenRead(path));

                while (!reader.EndOfStream)
                {
                    fields = reader.ReadLine();
                    parsedData.Add(fields);
                }

                reader.Close();
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
            }

            return parsedData;
        }

        public static XFontStyle GetFontStyle(int weight)
        {
            if (300 < weight && weight <= 900)
                return XFontStyle.Bold;
            else
                return XFontStyle.Regular;
        }
    }
}
