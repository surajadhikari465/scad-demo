using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Net;
using System.Threading;
using System.ComponentModel;

namespace Core
{
    /// <summary>
    /// Singleton web client class for downloading files via HTTP GET, with progress indication events
    /// </summary>
    public class WebClient : IDisposable
    {
        public delegate void ProgressIndicatorDelegate(ProgressData progress);
        public event ProgressIndicatorDelegate OnFileDownloadProgress;
        public delegate void ProcessCompleteDelegate();
        public event ProcessCompleteDelegate OnProcessCompleteEvent;

        private static System.Object m_instanceSyncObject = new System.Object();
        private static WebClient m_instance;
        private bool m_downloading = false;

        // Other managed resource this class uses.
        private Component component = new Component();
        // Track whether Dispose has been called.
        private bool disposed = false;

       private WebClient() 
       {
           ServicePointManager.Expect100Continue = true;
           ServicePointManager.DefaultConnectionLimit = 35;
       }

        public static WebClient Instance
        {
            get
            {
                lock (m_instanceSyncObject)
                {
                    if (m_instance == null)
                    {
                        m_instance = new WebClient();
                    }

                    return m_instance;
                }
            }
        }

        private string m_downloadLocation;
        private string DownloadLocationPath
        {
            get { return m_downloadLocation; }
            set { m_downloadLocation = value; }
        }


        private string m_fileName;
        private string FileName
        {
            get { return m_fileName; }
            set { m_fileName = value; }
        }

        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue 
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the 
        // runtime from inside the finalizer and you should not reference 
        // other objects. Only unmanaged resources can be disposed.
        private void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                    component.Dispose();
                }

            }
            disposed = true;
        }
        
        
        /// <summary>
        /// Callback event for when the HTTP request is complete
        /// </summary>
        /// <param name="asyncResult"></param>
        private void ReadCallbackData(IAsyncResult asyncResult)
        {

            try
            {
                byte[] inBuf = new byte[5000];
                int bytesToRead = inBuf.Length;
                int bytesRead = 0;
                int totalSizeBytes = 0;

                HttpWebRequest httpRequest = (HttpWebRequest)asyncResult.AsyncState;

                if (httpRequest != null)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.EndGetResponse(asyncResult);

                    if (httpResponse != null)
                    {
                        Stream httpResponseStream = httpResponse.GetResponseStream();

                        if (httpResponseStream != null)
                        {
                            totalSizeBytes = (int)httpResponse.ContentLength;


                            //delete the old file if it exists in this location.
                            if (File.Exists(string.Format("{0}\\{1}", m_downloadLocation, m_fileName)))
                                File.Delete(string.Format("{0}\\{1}", m_downloadLocation, m_fileName));

                            using (FileStream writeStream = new FileStream(string.Format("{0}\\{1}", m_downloadLocation, m_fileName), FileMode.CreateNew, FileAccess.Write))
                            {

                                while (bytesRead < totalSizeBytes)
                                {
                                    int iRes = httpResponseStream.Read(inBuf, 0, bytesToRead);

                                    if (iRes == 0)
                                        break;

                                    bytesRead += iRes;

                                    if (writeStream != null)
                                    {
                                        writeStream.Write(inBuf, 0, iRes);
                                    }
                                    else
                                    {
                                        throw new Exception("Unable to create file write stream on device");
                                    }


                                    RaiseFileProgressEvent(new ProgressData(bytesRead, totalSizeBytes));
                                }
                            }

                            //complete
                            RaiseFileDownloadCompleteEvent();

                        }
                        else
                        {
                            RaiseError("Response stream was null, unable to process");
                        }
                    }
                    else
                    {
                        RaiseError("Response was null, unable to load file");
                    }
                }
                else
                {
                    RaiseError("Request was null, unagle to download file");
                }
            }
            catch (Exception ex)
            {
                RaiseError(ex.Message);
            }

        }

        private void RaiseError(string pMessage)
        {
            m_downloading = false;
            throw new Exception(pMessage);
        }

        private void RaiseFileDownloadCompleteEvent()
        {
            if (OnProcessCompleteEvent != null)
            {
                foreach (ProcessCompleteDelegate pDel in OnProcessCompleteEvent.GetInvocationList())
                {
                    try
                    {
                        pDel.Invoke();

                        //change state
                        if (m_downloading)
                            m_downloading = false;
                    }
                    catch (Exception ex)
                    {
                        RaiseError(ex.Message);
                    }
                }
            }
        }

        private void RaiseFileProgressEvent(ProgressData pData)
        {
            if (OnFileDownloadProgress != null)
            {
                foreach (ProgressIndicatorDelegate pDel in OnFileDownloadProgress.GetInvocationList())
                {
                    try
                    {
                    
                        pDel.Invoke(pData);
                    }
                       catch (Exception ex)
                    {
                        RaiseError(ex.Message);
                    }
                }
            }
        }   

        /// <summary>
        /// Downloads a file via HTTP GET asyncrounously.
        /// </summary>
        /// <param name="address">the url including the file name to download</param>
        /// <param name="fileName">the name of the file, this can be the same or if you would like the file name to be changed
        /// after download you can put a different name here</param>
        /// <param name="downloadLocation">the physical path in which the file must be downloaded</param>
        public void DownloadFileAsync(Uri address, string fileName, string downloadLocation)
        {

            //putting this in, do not want more than one request active at a time
            //we should really have a list with the file name check and handle the queued list.
            if (!(m_downloading))
            {

                if (address == null)
                    throw new ArgumentNullException("address");
                if (fileName == null)
                    throw new ArgumentNullException("fileName");
                if (downloadLocation == null)
                    throw new ArgumentException("download location is either null or does not exist");

                HttpWebRequest httpRequest = null;

                m_fileName = fileName;

                if (!(Directory.Exists(downloadLocation)))
                    Directory.CreateDirectory(downloadLocation);

                m_downloadLocation = downloadLocation;

                try
                {
                    httpRequest = (HttpWebRequest)WebRequest.Create(address);
                    httpRequest.Method = "GET";
                    httpRequest.BeginGetResponse(new AsyncCallback(ReadCallbackData), httpRequest);
                }

                catch (WebException wex)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

        }
    }

    // 
    // ProgressData 
    // Keeps track of overall operation progress 
    // Used by async operations for client updates, especially to hold state from the upload phase to download. 
    // 
    public class ProgressData
    {

        public int BytesReceived = 0;
        public int TotalBytesToReceive = -1;

        public ProgressData() { }

        public ProgressData(int bytesRec, int bytesTotal)
        {
            BytesReceived = bytesRec;
            TotalBytesToReceive = bytesTotal;
        }

        internal void Reset()
        {
            BytesReceived = 0;
            TotalBytesToReceive = -1;
        }
    }
}
