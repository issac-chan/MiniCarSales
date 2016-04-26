using MiniCarSales.Models;
using MiniCarSales.Repository;
using MiniCarSales.Utility;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Http;

namespace MiniCarSales.Controllers
{
    //[Authorize]
    public class DataServiceController : ApiController
    {
        public DataServiceController()
        {
        }        

        [AllowAnonymous]
        [Route("authenticate")]
        [HttpPost]
        public HttpResponseMessage Login([FromBody]UserLogin userLogin)
        {
            var userRepository = new UserRepository(new FileConnection(CommonFunction.GetDatabaseFilePath()));

            var isValid = userRepository.ValidateUser(userLogin.UserName, userLogin.Password);

            if (isValid) SetPrincipal(userLogin.UserName);

            return Request.CreateResponse(HttpStatusCode.OK, new { status = isValid });
        }

        [HttpPost]
        public HttpResponseMessage Logout()
        {
            //Simulated logout
            return Request.CreateResponse(HttpStatusCode.OK, new { status = true });
        }


        [HttpGet]
        public HttpResponseMessage Makes()
        {
            var repository = new ManufactureRepository(new FileConnection(CommonFunction.GetDatabaseFilePath()));

            return Request.CreateResponse(HttpStatusCode.OK, repository.ListOfMake());
        }

        [HttpGet]
        public HttpResponseMessage Models(int makeId)
        {
            var repository = new ManufactureRepository(new FileConnection(CommonFunction.GetDatabaseFilePath()));

            return Request.CreateResponse(HttpStatusCode.OK, repository.ListOfModel(makeId));
        }

        [HttpGet]
        public HttpResponseMessage Years()
        {
            var repository = new ManufactureRepository(new FileConnection(CommonFunction.GetDatabaseFilePath()));

            return Request.CreateResponse(HttpStatusCode.OK, repository.ListOfYear());
        }

        [HttpGet]
        public HttpResponseMessage Vehicle(int Id)
        {
            var vehicleRepository = new VehicleRepository(new FileConnection(CommonFunction.GetDatabaseFilePath()));

            var lstVehicles = vehicleRepository.SearchVehicles(null,null,null, Id);

            return Request.CreateResponse(HttpStatusCode.OK, lstVehicles.FirstOrDefault());
        }

        [HttpGet]
        public HttpResponseMessage Vehicles(int top, int skip,int? makeId = null, int? modelId = null, int? yearId = null, int? vehicleId = null)
        {
            var vehicleRepository = new VehicleRepository(new FileConnection(CommonFunction.GetDatabaseFilePath()));
            var manufactureRepository = new ManufactureRepository(new FileConnection(CommonFunction.GetDatabaseFilePath()));

            var lstVehicles = vehicleRepository.SearchVehicles(makeId, modelId, yearId, vehicleId);

            if (lstVehicles == null || lstVehicles.Count == 0) return null;

            HttpContext.Current.Response.Headers.Add("X-InlineCount", lstVehicles.Count.ToString());

            var lstMake = manufactureRepository.ListOfMake();
            var lstModel = manufactureRepository.ListOfModel(null);
            var lstYear = manufactureRepository.ListOfYear();

            var filtedVehicles = lstVehicles.Skip(skip).Take(top);
            
            var vehicleViewModels = (from v in filtedVehicles
                                     select new VehicleViewModel
                                     {
                                         VehicleId = v.VehicleId,
                                         MakeId = v.MakeId,
                                         MakeName = lstMake.FirstOrDefault(x => x.MakeId == v.MakeId).MakeName,
                                         ModelId = v.ModelId,
                                         ModelName = lstModel.FirstOrDefault(x => x.ModelId == v.ModelId).ModelName,
                                         YearId = v.YearId,
                                         YearName = lstYear.FirstOrDefault(x => x.YearId == v.YearId).YearName,
                                         Price = v.Price,
                                         photo = string.IsNullOrEmpty(v.photo)?string.Empty:Path.Combine(CommonFunction.GetUploadPath(),v.photo),
                                         Phone = v.Phone,
                                         ContactEmail = v.ContactEmail,
                                         ContactName = v.ContactName,
                                         ABN = v.ABN,
                                         Description = v.Description,
                                         CreatedBy = v.CreatedBy,
                                         WhenCreated = v.WhenCreated,
                                         WhenChanged = v.WhenChanged,
                                         ChangedBy = v.ChangedBy
                                     }).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, vehicleViewModels);
        }

        [HttpPost]
        public HttpResponseMessage PostEnquiry([FromBody]Enquiry enquiry)
        {
            //send email to customer and then save enquiry detail

            var repository = new EnquiryRepository(new FileConnection(CommonFunction.GetDatabaseFilePath()));

            enquiry.WhenCreated = DateTime.Now;

            return Request.CreateResponse(HttpStatusCode.OK, repository.AddEnquiry(enquiry));
        }

        [HttpPost]
        public HttpResponseMessage UploadFile()
        {
            var uploadedfileName = string.Empty;

            var httpFiles = HttpContext.Current.Request.Files;

            if(httpFiles.Count == 0)
                return Request.CreateResponse(HttpStatusCode.OK, uploadedfileName);

            var httpPostFile = httpFiles[0];

            if(httpPostFile.ContentLength == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, uploadedfileName);
            }

            uploadedfileName = DateTime.Now.ToString("yyyyMMddhhmmss") + Path.GetExtension(httpPostFile.FileName);

            httpPostFile.SaveAs(Path.Combine(CommonFunction.GetUploadLocation(), uploadedfileName));

            return Request.CreateResponse(HttpStatusCode.OK, uploadedfileName);
        }

        [HttpPost]
        public HttpResponseMessage PostVehicle([FromBody]Vehicle vehicle)
        {
            var repository = new VehicleRepository(new FileConnection(CommonFunction.GetDatabaseFilePath()));
            var vehicleId = 0;

            if (vehicle.VehicleId > 0 && repository.UpdateVehicle(vehicle)) {
                vehicleId = vehicle.VehicleId;
                vehicle.WhenChanged = DateTime.Now;
            }
            else
            {
                vehicle.WhenCreated = DateTime.Now;
                vehicleId = repository.AddVehicle(vehicle);
            }

            return Request.CreateResponse(HttpStatusCode.OK, vehicleId);
        }

        public HttpResponseMessage DeleteVehicle(int id)
        {
            var repository = new VehicleRepository(new FileConnection(CommonFunction.GetDatabaseFilePath()));
            var enquiryRepository = new EnquiryRepository(new FileConnection(CommonFunction.GetDatabaseFilePath()));

            enquiryRepository.DeleteEnquiry(id);

            return Request.CreateResponse(HttpStatusCode.OK, repository.DeleteVehicle(id));
        }

        [HttpGet]
        public HttpResponseMessage Enquiries()
        {
            var repository = new EnquiryRepository(new FileConnection(CommonFunction.GetDatabaseFilePath()));

            

            var lstEnquiries = repository.GetEnquires(null, null);

            if(lstEnquiries != null && lstEnquiries.Count >0)
            {
                lstEnquiries = lstEnquiries.OrderByDescending(x => x.WhenCreated).ToList();
            }

            var lstEnquiryViewModels = (from e in lstEnquiries
                                       select new EnquiryViewModel
                                       {
                                           EnquiryId = e.EnquiryId,
                                           Name = e.Name,
                                           Email = e.Email,
                                           Phone = e.Phone,
                                           Comment = e.Comment,
                                           VehicleId = e.VehicleId,
                                           VehicleName = GetVehicleName(e.VehicleId),
                                           WhenCreated = e.WhenCreated,
                                           WhenRead = e.WhenRead
                                       }).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, lstEnquiryViewModels);
        }

        private void SetPrincipal(string name)
        {
            var identity = new GenericIdentity(name);

            var principal = new GenericPrincipal(identity, null);

            Thread.CurrentPrincipal = principal;

            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
        }

        private string GetVehicleName(int vehicleId)
        {
            var repository = new VehicleRepository(new FileConnection(CommonFunction.GetDatabaseFilePath()));
            var manufactureRepository = new ManufactureRepository(new FileConnection(CommonFunction.GetDatabaseFilePath()));
            var lstMake = manufactureRepository.ListOfMake();
            var lstModel = manufactureRepository.ListOfModel(null);
            var lstYear = manufactureRepository.ListOfYear();


            var vehicle = repository.SearchVehicles(null, null, null, vehicleId).FirstOrDefault();

            if (vehicle == null) return string.Empty;

            return lstYear.FirstOrDefault(x => x.YearId == vehicle.YearId).YearName + " " +
                lstMake.FirstOrDefault(x => x.MakeId == vehicle.MakeId).MakeName + " " +
                lstModel.FirstOrDefault(x => x.ModelId == vehicle.ModelId).ModelName;
        }
    }
}