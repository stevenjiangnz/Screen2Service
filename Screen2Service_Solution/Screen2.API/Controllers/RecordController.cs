using Newtonsoft.Json;
using Screen2.BLL;
using Screen2.DAL.Interface;
using Screen2.Entity;
using Screen2.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Screen2.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/record")]
    public class RecordController : BaseApiController
    {
        #region Constructor
        public RecordController(IUnitWork unit)
            : base(unit)
        {
        }
        #endregion

        #region Methods
        [HttpGet]
        [Route("getbyzone")]
        public async Task<IHttpActionResult> Get(int? zoneId, int size =50)
        {
            List<Record> slist = null;

            try
            {
                //var currentUser = await GetCurrentUser();

                RecordBLL bll = new RecordBLL(_unit);

                slist = bll.GetByZoneID(zoneId, size).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(slist);
        }


        [HttpGet]
        public HttpResponseMessage Download(int id)
        {
            RecordBLL rbll = new RecordBLL(_unit);
            Record rec = rbll.GetByID(id);
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

            string filePath = rbll.GetRecordFilePath(rec);

            var stream = new FileStream(filePath, FileMode.Open);
            result.Content = new StreamContent(stream);
            
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = Path.GetFileName(rec.Path);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.Add("x-filename", Path.GetFileName(rec.Path));

            result.Content.Headers.ContentLength = stream.Length;

            return result;
        }

        [HttpPut]
        public async Task<IHttpActionResult> Put(Record rec)
        {
            try
            {
                var currentUser = await GetCurrentUser();
                rec.Owner = currentUser.Id;
                rec.CreateDT = DateTime.Now;

                new RecordBLL(_unit).Create(rec);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }

            return Ok(rec);
        }

        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                var currentUser = await GetCurrentUser();
                RecordBLL bll = new RecordBLL(_unit);

                bool isAdmin = await AppUserManager.IsInRoleAsync(currentUser.Id, "Admin");

                if (isAdmin)
                {
                    bll.DeleteRecord(id);
                }
                else
                {
                    var w = bll.GetByID(id);

                    if (w.Owner == currentUser.Id)
                    {
                        bll.DeleteRecord(id);
                    }
                    else
                    {
                        BadRequest("You don't have permission to delete this record.");
                    }

                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_log, ex.ToString());
                return InternalServerError(ex);
            }
            return Ok();
        }



        [HttpPost] // This is from System.Web.Http, and not from System.Web.Mvc
        public async Task<IHttpActionResult> UploadRecord()
        {
            var currentUser = await GetCurrentUser();

            RecordBLL rBll = new RecordBLL(_unit);

            if (!Request.Content.IsMimeMultipartContent())
            {
                this.Request.CreateResponse(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = GetMultipartProvider();
            var result = await Request.Content.ReadAsMultipartAsync(provider);

            // On upload, files are given a generic name like "BodyPart_26d6abe1-3ae1-416a-9429-b35f15e6e5d5"
            // so this is how you can get the original file name
            var originalFileName = GetDeserializedFileName(result.FileData.First());

            // uploadedFileInfo object will give you some additional stuff like file length,
            // creation time, directory name, a few filesystem methods etc..
            var uploadedFileInfo = new FileInfo(result.FileData.First().LocalFileName);

            var inRec = GetFormData(result);

            inRec.Owner = currentUser.Id;
            inRec.CreateDT = DateTime.Now;
            inRec.OriginalFileName = originalFileName;

            // Through the request response you can return an object to the Angular controller
            // You will be able to access this in the .success callback through its data attribute
            // If you want to send something to the .error callback, use the HttpStatusCode.BadRequest instead
            var returnData = rBll.UploadRecord(inRec, uploadedFileInfo);

            return Ok(new { returnData });
        }

        // You could extract these two private methods to a separate utility class since
        // they do not really belong to a controller class but that is up to you
        private MultipartFormDataStreamProvider GetMultipartProvider()
        {
            var uploadFolder = ConfigurationManager.AppSettings["uploadLocalFolder"]; 
            var root = HttpContext.Current.Server.MapPath(uploadFolder);
            Directory.CreateDirectory(root);
            return new MultipartFormDataStreamProvider(root);
        }

        // Extracts Request FormatData as a strongly typed model
        private Record GetFormData(MultipartFormDataStreamProvider result)
        {
            Record rec = new Record();

            var keys = result.FormData.Keys;

            foreach(var k in keys)
            {
                if(k.ToString().ToLower().IndexOf("tradingdate")>0)
                {
                    rec.TradingDate = int.Parse(result.FormData.GetValues(k.ToString()).FirstOrDefault().ToString());
                }

                if (k.ToString().ToLower().IndexOf("title") > 0)
                {
                    rec.Title = result.FormData.GetValues(k.ToString()).FirstOrDefault().ToString();
                }

                if (k.ToString().ToLower().IndexOf("type") > 0)
                {
                    rec.Type = result.FormData.GetValues(k.ToString()).FirstOrDefault().ToString();
                }


                if (k.ToString().ToLower().IndexOf("zoneid") > 0)
                {
                    var zoneIDString = result.FormData.GetValues(k.ToString()).FirstOrDefault().ToString();

                    if(!zoneIDString.Equals("null", StringComparison.CurrentCultureIgnoreCase))
                    {
                        rec.ZoneId = int.Parse(zoneIDString);
                    }
                }

            }

            return rec;
        }

        private string GetDeserializedFileName(MultipartFileData fileData)
        {
            var fileName = GetFileName(fileData);
            return JsonConvert.DeserializeObject(fileName).ToString();
        }

        public string GetFileName(MultipartFileData fileData)
        {
            return fileData.Headers.ContentDisposition.FileName;
        }


        public class UploadDataModel
        {
            public string testString1 { get; set; }
            public string testString2 { get; set; }
        }


        public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
        {
            public CustomMultipartFormDataStreamProvider(string rootPath) : base(rootPath)
            {
            }

            public CustomMultipartFormDataStreamProvider(string rootPath, int bufferSize) : base(rootPath, bufferSize)
            {
            }

            public override string GetLocalFileName(HttpContentHeaders headers)
            {
                //Make the file name URL safe and then use it & is the only disallowed url character allowed in a windows filename
                var name = !string.IsNullOrWhiteSpace(headers.ContentDisposition.FileName)
                  ? headers.ContentDisposition.FileName
                  : "NoName";
                return name.Trim('"').Replace("&", "and");
            }
        }
        #endregion
    }
}