/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;

namespace FinancialData.Shared
{
    public interface IFinancialDataProvider
    {
        ISample GetPrice(Asset asset);
        ISamplePackage GetHistory(Asset asset, DateTime start, DateTime end, IProvisionContext provisionContext);

        void AsyncGetHistory(Asset asset, DateTime start, DateTime end, IProvisionContext provisionContext,
            Func<ISample, bool> func);
    }
}