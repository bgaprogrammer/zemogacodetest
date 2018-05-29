using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using JR.FinancialManager.Core.Models;
using JR.FinancialManager.Core.Models.Dto;

namespace JR.FinancialManager.Core.Services
{
    public class MappingsProfile : Profile
    {
        public MappingsProfile()
        {
            CreateMap<Customer, CustomerDto>().ReverseMap();
            CreateMap<TransactionType, TransactionTypeDto>().ReverseMap();
            CreateMap<TransactionDetail, TransactionDetailDto>().ReverseMap();
            CreateMap<Transaction, TransactionDto>().ReverseMap();
        }
    }
}