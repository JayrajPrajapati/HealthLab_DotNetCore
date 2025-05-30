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
    public partial class AppDbContext
    {
        private IAppDbContextProcedures _procedures;

        public virtual IAppDbContextProcedures Procedures
        {
            get
            {
                if (_procedures is null) _procedures = new AppDbContextProcedures(this);
                return _procedures;
            }
            set
            {
                _procedures = value;
            }
        }

        public IAppDbContextProcedures GetProcedures()
        {
            return Procedures;
        }

        protected void OnModelCreatingGeneratedProcedures(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryGridListResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<CustomerGridListResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<DirectPayTransactionGridListResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<LayByPlanListResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<MerchantGridListResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<MerchantRequestGridListResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<RewardsGridListResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<ServiceGridListResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<TempLayByTransactionGridListResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<TopCustomerGridListResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<TopMerchantGridListResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<TopPlanHistoryGridListResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<TopTransactionGridListResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<WalletTransactionGridListResult>().HasNoKey().ToView(null);
        }
    }

    public partial class AppDbContextProcedures : IAppDbContextProcedures
    {
        private readonly AppDbContext _context;

        public AppDbContextProcedures(AppDbContext context)
        {
            _context = context;
        }

        public virtual async Task<List<CategoryGridListResult>> CategoryGridListAsync(string SortColumn, string SortOrder, int? PageSize, int? PageIndex, string SearchText, OutputParameter<int?> TotalRecords, OutputParameter<int?> TotalFilteredRecords, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterTotalRecords = new SqlParameter
            {
                ParameterName = "TotalRecords",
                Direction = System.Data.ParameterDirection.InputOutput,
                Value = TotalRecords?._value ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.Int,
            };
            var parameterTotalFilteredRecords = new SqlParameter
            {
                ParameterName = "TotalFilteredRecords",
                Direction = System.Data.ParameterDirection.InputOutput,
                Value = TotalFilteredRecords?._value ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.Int,
            };
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "SortColumn",
                    Size = -1,
                    Value = SortColumn ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "SortOrder",
                    Size = 10,
                    Value = SortOrder ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "PageSize",
                    Value = PageSize ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "PageIndex",
                    Value = PageIndex ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "SearchText",
                    Size = -1,
                    Value = SearchText ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                parameterTotalRecords,
                parameterTotalFilteredRecords,
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<CategoryGridListResult>("EXEC @returnValue = [dbo].[CategoryGridList] @SortColumn, @SortOrder, @PageSize, @PageIndex, @SearchText, @TotalRecords OUTPUT, @TotalFilteredRecords OUTPUT", sqlParameters, cancellationToken);

            TotalRecords.SetValue(parameterTotalRecords.Value);
            TotalFilteredRecords.SetValue(parameterTotalFilteredRecords.Value);
            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<List<CustomerGridListResult>> CustomerGridListAsync(string SortColumn, string SortOrder, int? PageSize, int? PageIndex, string SearchText, OutputParameter<int?> TotalRecords, OutputParameter<int?> TotalFilteredRecords, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterTotalRecords = new SqlParameter
            {
                ParameterName = "TotalRecords",
                Direction = System.Data.ParameterDirection.InputOutput,
                Value = TotalRecords?._value ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.Int,
            };
            var parameterTotalFilteredRecords = new SqlParameter
            {
                ParameterName = "TotalFilteredRecords",
                Direction = System.Data.ParameterDirection.InputOutput,
                Value = TotalFilteredRecords?._value ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.Int,
            };
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "SortColumn",
                    Size = -1,
                    Value = SortColumn ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "SortOrder",
                    Size = 10,
                    Value = SortOrder ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "PageSize",
                    Value = PageSize ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "PageIndex",
                    Value = PageIndex ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "SearchText",
                    Size = -1,
                    Value = SearchText ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                parameterTotalRecords,
                parameterTotalFilteredRecords,
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<CustomerGridListResult>("EXEC @returnValue = [dbo].[CustomerGridList] @SortColumn, @SortOrder, @PageSize, @PageIndex, @SearchText, @TotalRecords OUTPUT, @TotalFilteredRecords OUTPUT", sqlParameters, cancellationToken);

            TotalRecords.SetValue(parameterTotalRecords.Value);
            TotalFilteredRecords.SetValue(parameterTotalFilteredRecords.Value);
            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<List<DirectPayTransactionGridListResult>> DirectPayTransactionGridListAsync(string SortColumn, string SortOrder, int? PageSize, int? PageIndex, string SearchText, OutputParameter<int?> TotalRecords, OutputParameter<int?> TotalFilteredRecords, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterTotalRecords = new SqlParameter
            {
                ParameterName = "TotalRecords",
                Direction = System.Data.ParameterDirection.InputOutput,
                Value = TotalRecords?._value ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.Int,
            };
            var parameterTotalFilteredRecords = new SqlParameter
            {
                ParameterName = "TotalFilteredRecords",
                Direction = System.Data.ParameterDirection.InputOutput,
                Value = TotalFilteredRecords?._value ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.Int,
            };
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "SortColumn",
                    Size = -1,
                    Value = SortColumn ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "SortOrder",
                    Size = 10,
                    Value = SortOrder ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "PageSize",
                    Value = PageSize ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "PageIndex",
                    Value = PageIndex ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "SearchText",
                    Size = -1,
                    Value = SearchText ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                parameterTotalRecords,
                parameterTotalFilteredRecords,
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<DirectPayTransactionGridListResult>("EXEC @returnValue = [dbo].[DirectPayTransactionGridList] @SortColumn, @SortOrder, @PageSize, @PageIndex, @SearchText, @TotalRecords OUTPUT, @TotalFilteredRecords OUTPUT", sqlParameters, cancellationToken);

            TotalRecords.SetValue(parameterTotalRecords.Value);
            TotalFilteredRecords.SetValue(parameterTotalFilteredRecords.Value);
            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<List<LayByPlanListResult>> LayByPlanListAsync(string SortColumn, string SortOrder, int? PageSize, int? PageIndex, string SearchText, long? Status, OutputParameter<int?> TotalRecords, OutputParameter<int?> TotalFilteredRecords, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterTotalRecords = new SqlParameter
            {
                ParameterName = "TotalRecords",
                Direction = System.Data.ParameterDirection.InputOutput,
                Value = TotalRecords?._value ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.Int,
            };
            var parameterTotalFilteredRecords = new SqlParameter
            {
                ParameterName = "TotalFilteredRecords",
                Direction = System.Data.ParameterDirection.InputOutput,
                Value = TotalFilteredRecords?._value ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.Int,
            };
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "SortColumn",
                    Size = -1,
                    Value = SortColumn ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "SortOrder",
                    Size = 10,
                    Value = SortOrder ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "PageSize",
                    Value = PageSize ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "PageIndex",
                    Value = PageIndex ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "SearchText",
                    Size = -1,
                    Value = SearchText ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "Status",
                    Value = Status ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.BigInt,
                },
                parameterTotalRecords,
                parameterTotalFilteredRecords,
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<LayByPlanListResult>("EXEC @returnValue = [dbo].[LayByPlanList] @SortColumn, @SortOrder, @PageSize, @PageIndex, @SearchText, @Status, @TotalRecords OUTPUT, @TotalFilteredRecords OUTPUT", sqlParameters, cancellationToken);

            TotalRecords.SetValue(parameterTotalRecords.Value);
            TotalFilteredRecords.SetValue(parameterTotalFilteredRecords.Value);
            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<List<MerchantGridListResult>> MerchantGridListAsync(string SortColumn, string SortOrder, int? PageSize, int? PageIndex, string SearchText, OutputParameter<int?> TotalRecords, OutputParameter<int?> TotalFilteredRecords, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterTotalRecords = new SqlParameter
            {
                ParameterName = "TotalRecords",
                Direction = System.Data.ParameterDirection.InputOutput,
                Value = TotalRecords?._value ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.Int,
            };
            var parameterTotalFilteredRecords = new SqlParameter
            {
                ParameterName = "TotalFilteredRecords",
                Direction = System.Data.ParameterDirection.InputOutput,
                Value = TotalFilteredRecords?._value ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.Int,
            };
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "SortColumn",
                    Size = -1,
                    Value = SortColumn ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "SortOrder",
                    Size = 10,
                    Value = SortOrder ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "PageSize",
                    Value = PageSize ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "PageIndex",
                    Value = PageIndex ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "SearchText",
                    Size = -1,
                    Value = SearchText ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                parameterTotalRecords,
                parameterTotalFilteredRecords,
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<MerchantGridListResult>("EXEC @returnValue = [dbo].[MerchantGridList] @SortColumn, @SortOrder, @PageSize, @PageIndex, @SearchText, @TotalRecords OUTPUT, @TotalFilteredRecords OUTPUT", sqlParameters, cancellationToken);

            TotalRecords.SetValue(parameterTotalRecords.Value);
            TotalFilteredRecords.SetValue(parameterTotalFilteredRecords.Value);
            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<List<MerchantRequestGridListResult>> MerchantRequestGridListAsync(string SortColumn, string SortOrder, int? PageSize, int? PageIndex, string SearchText, OutputParameter<int?> TotalRecords, OutputParameter<int?> TotalFilteredRecords, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterTotalRecords = new SqlParameter
            {
                ParameterName = "TotalRecords",
                Direction = System.Data.ParameterDirection.InputOutput,
                Value = TotalRecords?._value ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.Int,
            };
            var parameterTotalFilteredRecords = new SqlParameter
            {
                ParameterName = "TotalFilteredRecords",
                Direction = System.Data.ParameterDirection.InputOutput,
                Value = TotalFilteredRecords?._value ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.Int,
            };
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "SortColumn",
                    Size = -1,
                    Value = SortColumn ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "SortOrder",
                    Size = 10,
                    Value = SortOrder ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "PageSize",
                    Value = PageSize ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "PageIndex",
                    Value = PageIndex ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "SearchText",
                    Size = -1,
                    Value = SearchText ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                parameterTotalRecords,
                parameterTotalFilteredRecords,
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<MerchantRequestGridListResult>("EXEC @returnValue = [dbo].[MerchantRequestGridList] @SortColumn, @SortOrder, @PageSize, @PageIndex, @SearchText, @TotalRecords OUTPUT, @TotalFilteredRecords OUTPUT", sqlParameters, cancellationToken);

            TotalRecords.SetValue(parameterTotalRecords.Value);
            TotalFilteredRecords.SetValue(parameterTotalFilteredRecords.Value);
            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<List<RewardsGridListResult>> RewardsGridListAsync(string SortColumn, string SortOrder, int? PageSize, int? PageIndex, string SearchText, OutputParameter<int?> TotalRecords, OutputParameter<int?> TotalFilteredRecords, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterTotalRecords = new SqlParameter
            {
                ParameterName = "TotalRecords",
                Direction = System.Data.ParameterDirection.InputOutput,
                Value = TotalRecords?._value ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.Int,
            };
            var parameterTotalFilteredRecords = new SqlParameter
            {
                ParameterName = "TotalFilteredRecords",
                Direction = System.Data.ParameterDirection.InputOutput,
                Value = TotalFilteredRecords?._value ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.Int,
            };
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "SortColumn",
                    Size = -1,
                    Value = SortColumn ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "SortOrder",
                    Size = 10,
                    Value = SortOrder ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "PageSize",
                    Value = PageSize ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "PageIndex",
                    Value = PageIndex ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "SearchText",
                    Size = -1,
                    Value = SearchText ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                parameterTotalRecords,
                parameterTotalFilteredRecords,
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<RewardsGridListResult>("EXEC @returnValue = [dbo].[RewardsGridList] @SortColumn, @SortOrder, @PageSize, @PageIndex, @SearchText, @TotalRecords OUTPUT, @TotalFilteredRecords OUTPUT", sqlParameters, cancellationToken);

            TotalRecords.SetValue(parameterTotalRecords.Value);
            TotalFilteredRecords.SetValue(parameterTotalFilteredRecords.Value);
            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<List<ServiceGridListResult>> ServiceGridListAsync(string SortColumn, string SortOrder, int? PageSize, int? PageIndex, string SearchText, OutputParameter<int?> TotalRecords, OutputParameter<int?> TotalFilteredRecords, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterTotalRecords = new SqlParameter
            {
                ParameterName = "TotalRecords",
                Direction = System.Data.ParameterDirection.InputOutput,
                Value = TotalRecords?._value ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.Int,
            };
            var parameterTotalFilteredRecords = new SqlParameter
            {
                ParameterName = "TotalFilteredRecords",
                Direction = System.Data.ParameterDirection.InputOutput,
                Value = TotalFilteredRecords?._value ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.Int,
            };
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "SortColumn",
                    Size = -1,
                    Value = SortColumn ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "SortOrder",
                    Size = 10,
                    Value = SortOrder ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "PageSize",
                    Value = PageSize ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "PageIndex",
                    Value = PageIndex ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "SearchText",
                    Size = -1,
                    Value = SearchText ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                parameterTotalRecords,
                parameterTotalFilteredRecords,
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<ServiceGridListResult>("EXEC @returnValue = [dbo].[ServiceGridList] @SortColumn, @SortOrder, @PageSize, @PageIndex, @SearchText, @TotalRecords OUTPUT, @TotalFilteredRecords OUTPUT", sqlParameters, cancellationToken);

            TotalRecords.SetValue(parameterTotalRecords.Value);
            TotalFilteredRecords.SetValue(parameterTotalFilteredRecords.Value);
            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<List<TempLayByTransactionGridListResult>> TempLayByTransactionGridListAsync(string SortColumn, string SortOrder, int? PageSize, int? PageIndex, string SearchText, OutputParameter<int?> TotalRecords, OutputParameter<int?> TotalFilteredRecords, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterTotalRecords = new SqlParameter
            {
                ParameterName = "TotalRecords",
                Direction = System.Data.ParameterDirection.InputOutput,
                Value = TotalRecords?._value ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.Int,
            };
            var parameterTotalFilteredRecords = new SqlParameter
            {
                ParameterName = "TotalFilteredRecords",
                Direction = System.Data.ParameterDirection.InputOutput,
                Value = TotalFilteredRecords?._value ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.Int,
            };
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "SortColumn",
                    Size = -1,
                    Value = SortColumn ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "SortOrder",
                    Size = 10,
                    Value = SortOrder ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "PageSize",
                    Value = PageSize ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "PageIndex",
                    Value = PageIndex ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "SearchText",
                    Size = -1,
                    Value = SearchText ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                parameterTotalRecords,
                parameterTotalFilteredRecords,
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<TempLayByTransactionGridListResult>("EXEC @returnValue = [dbo].[TempLayByTransactionGridList] @SortColumn, @SortOrder, @PageSize, @PageIndex, @SearchText, @TotalRecords OUTPUT, @TotalFilteredRecords OUTPUT", sqlParameters, cancellationToken);

            TotalRecords.SetValue(parameterTotalRecords.Value);
            TotalFilteredRecords.SetValue(parameterTotalFilteredRecords.Value);
            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<List<TopCustomerGridListResult>> TopCustomerGridListAsync(OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<TopCustomerGridListResult>("EXEC @returnValue = [dbo].[TopCustomerGridList]", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<List<TopMerchantGridListResult>> TopMerchantGridListAsync(OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<TopMerchantGridListResult>("EXEC @returnValue = [dbo].[TopMerchantGridList]", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<List<TopPlanHistoryGridListResult>> TopPlanHistoryGridListAsync(OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<TopPlanHistoryGridListResult>("EXEC @returnValue = [dbo].[TopPlanHistoryGridList]", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<List<TopTransactionGridListResult>> TopTransactionGridListAsync(OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<TopTransactionGridListResult>("EXEC @returnValue = [dbo].[TopTransactionGridList]", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<List<WalletTransactionGridListResult>> WalletTransactionGridListAsync(string SortColumn, string SortOrder, int? PageSize, int? PageIndex, string SearchText, OutputParameter<int?> TotalRecords, OutputParameter<int?> TotalFilteredRecords, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterTotalRecords = new SqlParameter
            {
                ParameterName = "TotalRecords",
                Direction = System.Data.ParameterDirection.InputOutput,
                Value = TotalRecords?._value ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.Int,
            };
            var parameterTotalFilteredRecords = new SqlParameter
            {
                ParameterName = "TotalFilteredRecords",
                Direction = System.Data.ParameterDirection.InputOutput,
                Value = TotalFilteredRecords?._value ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.Int,
            };
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "SortColumn",
                    Size = -1,
                    Value = SortColumn ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "SortOrder",
                    Size = 10,
                    Value = SortOrder ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "PageSize",
                    Value = PageSize ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "PageIndex",
                    Value = PageIndex ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "SearchText",
                    Size = -1,
                    Value = SearchText ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                parameterTotalRecords,
                parameterTotalFilteredRecords,
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<WalletTransactionGridListResult>("EXEC @returnValue = [dbo].[WalletTransactionGridList] @SortColumn, @SortOrder, @PageSize, @PageIndex, @SearchText, @TotalRecords OUTPUT, @TotalFilteredRecords OUTPUT", sqlParameters, cancellationToken);

            TotalRecords.SetValue(parameterTotalRecords.Value);
            TotalFilteredRecords.SetValue(parameterTotalFilteredRecords.Value);
            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }
    }
}
