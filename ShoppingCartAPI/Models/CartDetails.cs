using ShoppingCartAPI.Models.Dto;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCartAPI.Models
{
    public class CartDetails
    {
        [Key]
        public int CartDetailsId { get; set; }
        public int CartHeaderId { get; set; }
        [ForeignKey("CartHeaderId")]
        public CartHeader cartHeader { get; set; }
        public int ProductId { get; set; }

        [NotMapped]
        public ProductDto Product { get; set; }
        public int Count { get; set; }
    }
}
