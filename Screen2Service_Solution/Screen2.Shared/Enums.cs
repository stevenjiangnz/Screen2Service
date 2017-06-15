using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.Shared
{
    public enum StockType
    {
        Index,
        Stock,
        ETF,
        Other
    }

    public enum SettingKey
    {
        HistoryDataYears,
        YahooProfileUrl
    }

    public enum ActionType
    {
        UploadDailyTicker,
        UploadShareInfo,
        UploadEODToAzure,
        UpdateIndicator,
        ProcessIndicator,
        ProcessAlert,
        ProcessDailyScan
    }

    public enum NoteType
    {
        General,
        MonitorForBuy,
        MonitorToSell
    }

    public enum OrderDirection
    {
        Long,
        Short
    }

    public enum OrderType
    {
        Stop,
        Limit
    }

    public enum OrderStatus
    {
        Open,
        Fulfilled,
        Cancelled
    }

    public enum TradeEntityType
    {
        Zone,
        Broker,
        Account,
        Order,
        Transaction
    }

    public enum TradeAction
    {
        Zone,
        Broker,
        Account_Occupy_Fund,
        Order,
        Transaction
    }

    public enum JourneyAction
    {
        AccountCreate,
        AccountStatus,
        FundDeposit,
        FundWithdraw
    }

    public enum IdeaType
    {
        Strategy_Buy,
        Strategy_Sell,
        Lesson_Buy,
        Lesson_Sell
    }

    public enum TransactionType
    {
        InitAccount,
        NewOrder,
        UpdateOrder,
        DeleteOrder,
        ManualProcess,
        FulfilOrder,
        DepositFund,
        WithdrawFund
    }

    public enum OrderSource
    {
        Input,
        Stop,
        Limit,
        Entry,
        Exit
    }
}
