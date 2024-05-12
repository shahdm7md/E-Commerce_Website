using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace testtt.Models
{
	public class Address
	{
		public int AddressId { get; set; }
		[Required(ErrorMessage = "Street is required.")]
		public string Street { get; set; }
		public string Apartement { get; set; }
		[Required(ErrorMessage = "Country is required.")]
		public string Country { get; set; }

		// مفتاح خارجي للعلاقة مع الـ Customer
		public int CustomerId { get; set; }
		public Customer Customer { get; set; }
	}
}
