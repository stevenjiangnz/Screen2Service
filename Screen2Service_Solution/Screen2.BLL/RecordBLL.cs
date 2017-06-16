using Screen2.BLL.Interface;
using Screen2.DAL.Interface;
using Screen2.Entity;
using Screen2.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.BLL
{
    public class RecordBLL : BaseBLL<Record>, IBaseBLL<Record>
    {
        private string Upload_Path = ConfigurationManager.AppSettings["recordLocalFolder"];

        #region Constructors
        public RecordBLL(IUnitWork unit) : base(unit)
        {
        }
        #endregion

        #region Methods
        public List<Record> GetByZoneID(int? zoneId, int size)
        {
            List<Record> aList = new List<Record>(); 

            if (size > 0)
            {
                aList = _unit.DataContext.Record
                    .Where(p => p.ZoneId == zoneId).Take(size).OrderByDescending(p => p.TradingDate).ToList();
            }
            else
            {
                aList = _unit.DataContext.Record
                    .Where(p => p.ZoneId == zoneId).OrderByDescending(p => p.TradingDate).ToList();

            }

            return aList;
        }


        public void DeleteRecord(int recId)
        {
            Record rec = this.GetByID(recId);

            try
            {
                File.Delete(rec.Path);

            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, "Error delete file. ", ex);
            }

            this.Delete(recId);
        }

        public Record UploadRecord(Record rec, FileInfo localFileInfo)
        {

            rec.Extension = this.GetExtension(rec.OriginalFileName);

            this.Create(rec);

            rec.FileName = GetTargetFileName(rec);

            var recordFolder = getRecordFolder(rec);

            FileHelper.EnsureDirectory(recordFolder);

            rec.Path = Path.Combine(recordFolder, rec.FileName);
            File.Copy(localFileInfo.FullName, rec.Path, true);        // Todo copy file

            File.Delete(localFileInfo.FullName);

            Update(rec);

            return rec;
        }


        private string getRecordFolder(Record rec)
        {
            string recordFolder = string.Empty;


            rec.FileName = GetTargetFileName(rec);

            if (rec.ZoneId.HasValue)
            {
                Zone z = new ZoneBLL(_unit).GetByID(rec.ZoneId.Value);
                recordFolder = Path.Combine(Upload_Path, "Z" + z.Name);
            }
            else
            {
                recordFolder = Path.Combine(Upload_Path, "ZCurrent");
            }

            recordFolder = Path.Combine(recordFolder, rec.Owner);

            return recordFolder;
        }


        public string GetRecordFilePath(Record rec)
        {
            string recordFolder = this.getRecordFolder(rec);

            return Path.Combine(recordFolder, rec.FileName);
        }

        public string GetExtension(string fileName)
        {
            string ext = string.Empty;

            if(fileName.LastIndexOf('.')>0)
            {
                ext = fileName.Substring(fileName.LastIndexOf('.'));
            }

            return ext;
        }

        public string GetTargetFileName(Record rec)
        {
            string fileName = string.Empty;

            fileName = rec.TradingDate.ToString() + "_" + rec.Type + "_" + rec.Id.ToString() + "_" + rec.Title + rec.Extension;

            return fileName;
        }

        #endregion
    }
}
