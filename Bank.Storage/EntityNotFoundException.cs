using System;
using System.ComponentModel.DataAnnotations;

namespace BankAccount.Storage
{
    public class EntityNotFoundException: ValidationException
    {
        public EntityNotFoundException(Guid id, string typeName): base($"Can't find entity by Id: {id}")
        {
            TypeName = typeName;
        }

        public string TypeName { get; }
    }
}
