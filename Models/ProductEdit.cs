using System.ComponentModel.DataAnnotations;

namespace StockManagementSystem.Models
{
    public class ProductEdit
    {
        public short PId { get; set; }

        public string PName { get; set; } = null!;

        public int PPrice { get; set; }

        public string Pimage { get; set; } = null!;

        public int SaleQuantity { get; set; }

        public int PurchaseOuantity { get; set; }

        public int Stock { get; set; }

        public short? CategoryId { get; set; }

        public short? SupplierId { get; set; }

        public string EncID { get; set; } = string.Empty;

        [DataType(DataType.Upload)]
        public IFormFile? ProductFile { get; set; }
    }
}
