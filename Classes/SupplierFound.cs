using MelodiasDelMundo_Client.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MelodiasDelMundo_Client.Classes
{
    public class SupplierFound
    {
        
        public int SupplierId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public SupplierFound(int supplierId, string name, string address, string postalCode, string city, string country, string phone, string email)
        {
            SupplierId = supplierId;
            Name = name;
            Address = address;
            PostalCode = postalCode;
            City = city;
            Country = country;
            Phone = phone;
            Email = email;
        }

        public SupplierFound(SupplierDTO supplierDto)
        {
            SupplierId=supplierDto.SupplierId;
            Name=supplierDto.Name;
            Address=supplierDto.Address;
            PostalCode=supplierDto.PostalCode;
            City=supplierDto.City;
            Country=supplierDto.Country;
            Phone=supplierDto.Phone;
            Email=supplierDto.Email;
        }
    }
}
