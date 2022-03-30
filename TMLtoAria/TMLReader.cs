using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using VMS.TPS.Common.Model.API;

namespace TMLtoAria
{
    public class TMLReader
    {
        public static PdfDocument ConvertTMLtoPDF(string filePathTML, PlanViewModel plan)
        {          
            List<string> TMLSheet = ParseTML(filePathTML);

            PdfDocument outputDocument = new PdfDocument();

            int xPos = 0;
            int yPos = 0;
            int margin = 0;
            int fontSize = 30;
            int fontWeight = 300;
            string fontFamily = "Arial";
            XFont font = new XFont(fontFamily, fontSize, GetFontStyle(fontWeight));
            string text = "";
            int lineSpacing = 1;
            int sectionHeight = 300;
            int i = 0;
            string currentLine = string.Empty;
            string pageOrientation = "Portrait";
            PdfPage currentPage = new PdfPage();
            PdfPage firstPage = new PdfPage();

            try
            {
                foreach (string line in TMLSheet)
                {
                    i++;
                    currentLine = line;
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
                        fontSize = Convert.ToInt32(value.Split(';')[0])/3;
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
                            gfx.DrawLine(lineBlack, xPos, yPos, xPos + currentPage.Width, yPos);
                        }
                    }
                    if (line.StartsWith("LineSpacing"))
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
                            xPos = Convert.ToInt32(value.Split(';')[0])/3 + margin/3;
                            yPos = Convert.ToInt32(value.Split(';')[1]);
                        }
                        else // move(0) or move(100)
                        {
                            xPos = Convert.ToInt32(value)/3 + margin/3;
                            if (value == "0")
                            {
                                yPos += 20;
                            }
                        }
                    }
                    if (line.StartsWith("PageBreak"))
                    {
                        var value = line.Split('=')[1];
                        if (value != "0.0")
                        {
                            yPos += 20;
                            continue;
                        }
                        else
                        {
                            yPos = 0;
                        }
                        PdfPage newPage = new PdfPage();
                        if (pageOrientation == "Landscape")
                        {
                            newPage.Orientation = PdfSharp.PageOrientation.Landscape;
                        }
                        else
                        {
                            newPage.Orientation = PdfSharp.PageOrientation.Landscape;
                        }                   
                        outputDocument.Pages.Add(newPage);
                        currentPage = newPage;
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
                        currentPage = firstPage;
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
                            xPos += Convert.ToInt32(value.Split(';')[0])/3 + margin/3;
                            yPos += Convert.ToInt32(value.Split(';')[1]);
                        }
                        else // move(0) or move(100)
                        {
                            xPos += Convert.ToInt32(value)/3 + margin/3;
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
                        if (text.Contains("$"))
                        {
                            var value = text.Split('$')[1].Trim();
                            if (value == "ImageId")
                                text = plan.PlanImageId;
                            if (value == "ImageUserOrigin")
                                text = plan.ImageUserOrigin;
                            if (value == "CourseId")
                                text = plan.CourseId;
                            if (value == "CourseIntent")
                                text = plan.CourseIntent;
                            if (value == "StructureSetId ")
                                text = plan.PlanStructureSetId;
                            if (value == "PatientTreatmentOrientation")
                                text = plan.TreatmentOrientation;
                            if (value == "PlanId")
                                text = plan.PlanId;
                            if (value == "PlanName")
                                text = plan.PlanName;
                            if (value == "PlanIntent")
                                text = plan.PlanIntent;
                            if (value == "PrimaryOncologistName")
                                text = plan.PatientPrimaryOncologist;
                            if (value == "PatientSex")
                                text = plan.PatientSex;
                            if (value == "PatientDateOfBirth")
                                text = plan.PatientBirthdate.ToString();
                            if (value == "PatientId")
                                text = plan.PatientId;
                            if (value.Contains("PatientLastName"))
                                text = plan.PatientName;
                            if (value.Contains("$TargetVolume"))
                                text = plan.TargetVolumeId;
                            if (value.Contains("PrimaryRefPointId"))
                                text = plan.PrimaryReferencePointId;
                        }

                        using (XGraphics gfx = XGraphics.FromPdfPage(currentPage))
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
                MessageBox.Show("Error reading TML file \n " + filePathTML + "\n" + "at line " + (i + 1) + ": \n" + currentLine);
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
