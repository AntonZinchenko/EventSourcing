using System;

namespace Bank.Api.Controllers
{
    public class TransferRequest
    {
        /// <summary>
        /// Идентификатор счета на который производится зачисление денеждных средств.
        /// </summary>
        public Guid TargetAccountId { get; set; }

        /// <summary>
        /// Сумма перевода.
        /// </summary>
        public decimal Sum { get; set; }
    }
}
