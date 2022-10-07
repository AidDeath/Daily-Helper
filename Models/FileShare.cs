using System.IO;
using Daily_Helper.Helpers;

namespace Daily_Helper.Models
{
    /// <summary>
    /// Information about a local share
    /// </summary>
    public class Share : ObservableObject
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Server"></param>
        /// <param name="shi"></param>
        public Share(string server, string netName, string path, ShareType shareType, string remark)
        {
            if (ShareType.Special == shareType && "IPC$" == netName)
            {
                shareType |= ShareType.IPC;
            }

            _server = server;
            _netName = netName;
            _path = path;
            _shareType = shareType;
            _remark = remark;
        }

        #endregion

        #region Notifying properties

        private string _server;
        /// <summary>
        /// The name of the computer that this share belongs to
        /// </summary>
        public string Server
        {
            get => _server;
            set => SetProperty(ref _server, value);
        }

        private string _netName;
        /// <summary>
        /// Share name
        /// </summary>
        public string NetName
        {
            get => _netName;
            set => SetProperty(ref _netName, value);
        }


        private string _path;
        /// <summary>
        /// Local path
        /// </summary>
        public string Path
        {
            get => _path;
            set => SetProperty(ref _path, value);
        }

        private ShareType _shareType;
        /// <summary>
        /// Share type
        /// </summary>
        public ShareType ShareType
        {
            get => _shareType;
            set => SetProperty(ref _shareType, value);
        }

        private string _remark;
        /// <summary>
        /// Comment
        /// </summary>
        public string Remark
        {
            get => _remark;
            set => SetProperty(ref _remark, value);
        }

        private long _freeSpace;
        /// <summary>
        /// Free space on disk, where file share is situated
        /// </summary>
        public long FreeSpace 
        {
            get => _freeSpace; 
            set => SetProperty(ref _freeSpace, value); 
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns true if this is a file system share
        /// </summary>
        public bool IsFileSystem
        {
            get
            {
                // Shared device
                if (0 != (_shareType & ShareType.Device)) return false;
                // IPC share
                if (0 != (_shareType & ShareType.IPC)) return false;
                // Shared printer
                if (0 != (_shareType & ShareType.Printer)) return false;

                // Standard disk share
                if (0 == (_shareType & ShareType.Special)) return true;

                // Special disk share (e.g. C$)
                if (ShareType.Special == _shareType && null != _netName && 0 != _netName.Length)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Get the root of a disk-based share
        /// </summary>
        public DirectoryInfo? Root => IsFileSystem ? new DirectoryInfo(ToString()) : null;


        #endregion


        /// <summary>
        /// Returns the path to this share
        /// </summary>
        /// <returns></returns>
        public override string ToString() => string.Format(@"{0}\{1}", _server, _netName);

        public void RefreshFreeSpace()
        {
            ShareDetector.GetShareFreeSpace(this);
        }

    }
}
