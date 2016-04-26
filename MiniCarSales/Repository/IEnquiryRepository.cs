using MiniCarSales.Models;
using System.Collections.Generic;

namespace MiniCarSales.Repository
{
    public interface IEnquiryRepository
    {
        bool AddEnquiry(Enquiry enquiry);

        bool UpdateEnquiry(Enquiry enquiry);

        int GetNumberOfNewEnquires(int? vehicleId);

        bool DeleteEnquiry(int vehicleId);

        List<Enquiry> GetEnquires(int? enquiryId, int? vehicleId);
    }
}
