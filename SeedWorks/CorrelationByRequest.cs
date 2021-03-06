﻿using MediatR;
using System;

namespace SeedWorks
{
    public class CorrelationByRequest<T> : IRequest<T>
    {
        public CorrelationByRequest(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        /// <summary>
        /// Маркер корреляции.
        /// </summary>
        public Guid CorrelationId { get; }
    }
}
