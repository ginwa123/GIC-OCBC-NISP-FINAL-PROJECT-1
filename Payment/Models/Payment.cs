using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Models
{
    public class Payment
    {
        [Key]
        public int PaymentDetailId { get; set; }
        [Required]
        [MaxLength(100)]
        public string CardOwnerName { get; set; }
        [Required]
        [Index(IsUnique = true)]
        public int CardNumber { get; set; }
        [Required]
        public DateTime ExpirationDate { get; set; }
        [Required]
        [MaxLength(100)]
        public string SecurityCode { get; set; }
    }
}
