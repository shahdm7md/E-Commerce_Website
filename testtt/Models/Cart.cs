using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace testtt.Models
{
    public class Cart
    {
        [Key]
        public int Cart_ID { get; set; }

        [ForeignKey("Customer")]
        public string Cus_ID { get; set; }

        public virtual Customer Customer { get; set; }

        // Navigation property for CartItems
        public virtual ICollection<CartItem> CartItems
        {
            get; set;
        }
    }
}
