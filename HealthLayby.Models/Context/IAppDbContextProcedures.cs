﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using HealthLayby.Models.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace HealthLayby.Models.Context
{
    public partial interface IAppDbContextProcedures
    {
        Task<List<CategoryGridListResult>> CategoryGridListAsync(string SortColumn, string SortOrder, int? PageSize, int? PageIndex, string SearchText, OutputParameter<int?> TotalRecords, OutputParameter<int?> TotalFilteredRecords, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<CustomerGridListResult>> CustomerGridListAsync(string SortColumn, string SortOrder, int? PageSize, int? PageIndex, string SearchText, OutputParameter<int?> TotalRecords, OutputParameter<int?> TotalFilteredRecords, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<DirectPayTransactionGridListResult>> DirectPayTransactionGridListAsync(string SortColumn, string SortOrder, int? PageSize, int? PageIndex, string SearchText, OutputParameter<int?> TotalRecords, OutputParameter<int?> TotalFilteredRecords, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<LayByPlanListResult>> LayByPlanListAsync(string SortColumn, string SortOrder, int? PageSize, int? PageIndex, string SearchText, long? Status, OutputParameter<int?> TotalRecords, OutputParameter<int?> TotalFilteredRecords, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<MerchantGridListResult>> MerchantGridListAsync(string SortColumn, string SortOrder, int? PageSize, int? PageIndex, string SearchText, OutputParameter<int?> TotalRecords, OutputParameter<int?> TotalFilteredRecords, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<MerchantRequestGridListResult>> MerchantRequestGridListAsync(string SortColumn, string SortOrder, int? PageSize, int? PageIndex, string SearchText, OutputParameter<int?> TotalRecords, OutputParameter<int?> TotalFilteredRecords, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<RewardsGridListResult>> RewardsGridListAsync(string SortColumn, string SortOrder, int? PageSize, int? PageIndex, string SearchText, OutputParameter<int?> TotalRecords, OutputParameter<int?> TotalFilteredRecords, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<ServiceGridListResult>> ServiceGridListAsync(string SortColumn, string SortOrder, int? PageSize, int? PageIndex, string SearchText, OutputParameter<int?> TotalRecords, OutputParameter<int?> TotalFilteredRecords, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<TempLayByTransactionGridListResult>> TempLayByTransactionGridListAsync(string SortColumn, string SortOrder, int? PageSize, int? PageIndex, string SearchText, OutputParameter<int?> TotalRecords, OutputParameter<int?> TotalFilteredRecords, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<TopCustomerGridListResult>> TopCustomerGridListAsync(OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<TopMerchantGridListResult>> TopMerchantGridListAsync(OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<TopPlanHistoryGridListResult>> TopPlanHistoryGridListAsync(OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<TopTransactionGridListResult>> TopTransactionGridListAsync(OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<WalletTransactionGridListResult>> WalletTransactionGridListAsync(string SortColumn, string SortOrder, int? PageSize, int? PageIndex, string SearchText, OutputParameter<int?> TotalRecords, OutputParameter<int?> TotalFilteredRecords, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
    }
}
