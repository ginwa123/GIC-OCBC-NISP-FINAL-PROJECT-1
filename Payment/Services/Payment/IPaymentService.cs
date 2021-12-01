using System.Collections.Generic;
using System.Threading.Tasks;

namespace Payment.Services
{
    public interface IPaymentService
    {
        public Task<Models.Payment> Get(int id);
        public Task<List<Models.Payment>> Get();
        public Task<int> Update(Models.Payment oldPayment, Models.Payment newPayment);
        public Task<int> Delete(Models.Payment payment);
        public Task<int> Create(Models.Payment payment);

    }
}
