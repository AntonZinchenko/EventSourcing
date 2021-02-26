using System;

namespace Transfer.Contracts.Types
{
    public class TransferView
    {
        public Guid Id { get; set; }

        public Guid SourceAccountId { get; set; }

        public Guid TargetAccountId { get; set; }

        public decimal Sum { get; set; }

        public string State { get; set; }
    }
}
