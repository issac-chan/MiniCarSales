using System;
using System.Collections.Generic;
using MiniCarSales.Models;
using System.Linq;

namespace MiniCarSales.Repository
{
    public class EnquiryRepository : BaseRepository, IEnquiryRepository
    {
        public EnquiryRepository(FileConnection fileConnection)
        {
            Connection = fileConnection;
        }

        public bool AddEnquiry(Enquiry enquiry)
        {
            var lstEnquiries = FileRepository<List<Enquiry>>.ReadDataFromFile(TableType.enquiry, Connection.FilePath);

            if(lstEnquiries == null || lstEnquiries.Count==0)
            {
                lstEnquiries = new List<Enquiry>();
                enquiry.EnquiryId = 1;
            }
            else
            {
                enquiry.EnquiryId = lstEnquiries.OrderByDescending(x => x.EnquiryId).First().EnquiryId++;
            }

            lstEnquiries.Add(enquiry);

            return FileRepository<List<Enquiry>>.WriteDataToFile(TableType.enquiry, Connection.FilePath, lstEnquiries);
        }

        public int GetNumberOfNewEnquires(int? vehicleId)
        {
            var lstEnquiries = FileRepository<List<Enquiry>>.ReadDataFromFile(TableType.enquiry, Connection.FilePath);

            return lstEnquiries.Where(x => x.WhenRead.HasValue == false).Count();
        }

        public bool UpdateEnquiry(Enquiry enquiry)
        {
            var lstEnquiries = FileRepository<List<Enquiry>>.ReadDataFromFile(TableType.enquiry, Connection.FilePath);

            if (lstEnquiries == null || lstEnquiries.Count() == 0)
                return false;

            var index = lstEnquiries.FindIndex(x => x.EnquiryId == enquiry.EnquiryId);

            if (index < 0) return false;

            lstEnquiries[index] = enquiry;

            return FileRepository<List<Enquiry>>.WriteDataToFile(TableType.enquiry, Connection.FilePath, lstEnquiries);
        }

        public List<Enquiry> GetEnquires(int? enquiryId, int? vehicleId)
        {
            var lstEnquiries = FileRepository<List<Enquiry>>.ReadDataFromFile(TableType.enquiry, Connection.FilePath);

            return (from enquiry in lstEnquiries
                    where (!enquiryId.HasValue || enquiry.EnquiryId == enquiryId.Value)
                    && (!vehicleId.HasValue || enquiry.VehicleId == vehicleId.Value)
                    select enquiry).ToList();
        }       
        
        public bool DeleteEnquiry(int vehicleId)
        {
            var lstEnquiries = FileRepository<List<Enquiry>>.ReadDataFromFile(TableType.enquiry, Connection.FilePath);

            var enquiry = lstEnquiries.FirstOrDefault(x => x.VehicleId == vehicleId);

            if (enquiry == null)
                return false;

            lstEnquiries.Remove(enquiry);

            return FileRepository<List<Enquiry>>.WriteDataToFile(TableType.vehicle, Connection.FilePath, lstEnquiries);
        }
    }
}