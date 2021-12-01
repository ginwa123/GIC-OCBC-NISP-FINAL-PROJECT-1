using Microsoft.EntityFrameworkCore;
using Payment.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly PaymentDbContext context;

        public PaymentService(PaymentDbContext context)
        {
            this.context = context;
        }
        public async Task<int> Create(Models.Payment payment)
        {
            await context.Payments.AddAsync(payment);
            await context.SaveChangesAsync();
            return payment.PaymentDetailId;
        }

        public async Task<int> Delete(Models.Payment payment)
        {
            context.Payments.Remove(payment);
            await context.SaveChangesAsync();
            return payment.PaymentDetailId;

        }


        public async Task<Models.Payment> Get(int id)
        {
            var payment = await context.Payments
                .FirstOrDefaultAsync(entitiy => entitiy.PaymentDetailId == id);
            return payment;
        }

        public async Task<List<Models.Payment>> Get()
        {
            var payments = await context.Payments.ToListAsync();
            return payments;
        }

        public async Task<int> Update(Models.Payment oldPayment, Models.Payment payment)
        {
            context.Entry(oldPayment).CurrentValues.SetValues(payment);
            await context.SaveChangesAsync();
            return oldPayment.PaymentDetailId;
        }


    }
}
