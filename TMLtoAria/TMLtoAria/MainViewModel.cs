using Microsoft.Win32;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using VMS.OIS.ARIALocal.WebServices.Document.Contracts;
using VMS.TPS.Common.Model.API;
using Microsoft.Toolkit.Mvvm;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace TMLtoAria
{
    public class MainViewModel : ObservableObject
    {
        private string _patientId;
        public string PatientId 
        {
            get => _patientId;
            set => SetProperty(ref _patientId, value);
        }
        private string _dateOfService;
        public string DateOfService
        {
            get => _dateOfService;
            set => SetProperty(ref _dateOfService, value);
        }
        private string _dateEntered;
        public string DateEntered
        {
            get => _dateEntered;
            set => SetProperty(ref _dateEntered, value);
        }
        private byte[] _binaryContent;
        public byte[] BinaryContent
        {
            get => _binaryContent;
            set => SetProperty(ref _binaryContent, value);
        }
        private string _directory;
        public string Directory
        {
            get => _directory;
            set => SetProperty(ref _directory, value);
        }
        private User _appUser;
        public User AppUser
        {
            get => _appUser;
            set => SetProperty(ref _appUser, value);
        }
        private DocumentUser _authoredByUser;
        public DocumentUser AuthoredByUser
        {
            get => _authoredByUser;
            set => SetProperty(ref _authoredByUser, value);
        }
        private DocumentUser _supervisedByUser;
        public DocumentUser SupervisedByUser
        {
            get => _supervisedByUser;
            set => SetProperty(ref _supervisedByUser, value);
        }
        private DocumentUser _enteredByUser;
        public DocumentUser EnteredByUser
        {
            get => _enteredByUser;
            set => SetProperty(ref _enteredByUser, value);
        }
        private DocumentType _documentType;
        public DocumentType DocumentType
        {
            get => _documentType;
            set => SetProperty(ref _documentType, value);
        }
        private string _templateName;
        public string TemplateName
        {
            get => _templateName;
            set => SetProperty(ref _templateName, value);
        }
        private FileFormat _fileFormat;
        public FileFormat FileFormat
        {
            get => _fileFormat;
            set => SetProperty(ref _fileFormat, value);
        }
        private ObservableCollection<FileViewModel> _files;
        public ObservableCollection<FileViewModel> Files
        {
            get => _files;
            set => SetProperty(ref _files, value);
        }
        public IEnumerable<FileViewModel> SelectedFiles
        {
            get { return Files.Where(x => x.IsSelected); }
        }
        public DocSettings DocSettings { get; set; }
        public ICommand GetFilesCommand => new RelayCommand(GetFiles);
        public ICommand ChangeDirectoryCommand => new RelayCommand(ChangeDirectory);
        public ICommand UploadToAriaCommand => new RelayCommand(UploadToAria);
        public MainViewModel(User user, Patient patient, ExternalPlanSetup plan, DocSettings docSet)
        {
            PatientId = patient.Id;
            AppUser = user;
            Directory = docSet.ImportDir;
            DocSettings = docSet;
            DateOfService = $"/Date({Math.Floor((DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds)})/";
            DateEntered = $"/Date({Math.Floor((DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds)})/";
            TemplateName = plan.Id;
            AuthoredByUser = new DocumentUser
            {
                SingleUserId = user.Id
            };
            SupervisedByUser = new DocumentUser
            {
                SingleUserId = user.Id
            };
            EnteredByUser = new DocumentUser
            {
                SingleUserId = user.Id
            };
            FileFormat = FileFormat.PDF;
            
            DocumentType = new DocumentType
            {
                DocumentTypeDescription = "Treatment Plan"
            };
        }
        public void GetFiles()
        {
            Files = new ObservableCollection<FileViewModel>();
            
            DirectoryInfo dir = new DirectoryInfo(Directory);
            FileInfo[] fileInfos = dir.GetFiles("*.tml");
            foreach (var file in fileInfos)
            {
                Files.Add( new FileViewModel
                {
                    FileName = file.Name,
                    FullPath = file.FullName,
                    CreationTime = file.CreationTime,
                    FileNameWithCreationTime = file.Name + " - " + file.CreationTime
                });
            }
            DateOfService = $"/Date({Math.Floor((DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds)})/";
        }

        public void ChangeDirectory()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.SelectedPath = Directory;
            var result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                Directory = folderBrowserDialog.SelectedPath;
                GetFiles();
            }
        }
        public void UploadToAria()
        {
            //Saving to PDF folder for now
            PdfDocument outputDocument = TMLReader.ConvertTMLtoPDF(SelectedFiles.FirstOrDefault().FullPath);
            var outputDirectory = Directory + "\\PDFs\\test.pdf";        
            outputDocument.Save(outputDirectory);

            // Todo:  Send PDF to browser

            //Send to Aria
            //MemoryStream stream = new MemoryStream();
            //outputDocument.Save(stream, false);
            //BinaryContent = stream.ToArray();
            //CustomInsertDocumentsParameter.PostDocumentData(PatientId, AppUser,
            //    BinaryContent, TemplateName, DocumentType, DocSettings);
        }
    }
}
