namespace QuitQ_Ecom.DTOs
{
    public class UserAddressDTO
    {
        public int UserAddressId { get; set; }
        public int? UserId { get; set; }
        public string DoorNumber { get; set; }
        public string? ApartmentName { get; set; }
        public string? Landmark { get; set; }
        public string Street { get; set; }
        public int CityId { get; set; }
        public string PostalCode { get; set; }
        public string ContactNumber { get; set; }
        public int? StatusId { get; set; }

       
            public override string ToString()
        {
            string result = $"DoorNumber: {DoorNumber}";

            if (!string.IsNullOrEmpty(ApartmentName))
            {
                result += $" | ApartmentName: {ApartmentName}";
            }

            if (!string.IsNullOrEmpty(Landmark))
            {
                result += $" | Landmark: {Landmark}";
            }

            result += $" | Street: {Street} | CityId: {CityId} | PostalCode: {PostalCode} | ContactNumber: {ContactNumber}";

            return result;
        }
    }
    }

    
