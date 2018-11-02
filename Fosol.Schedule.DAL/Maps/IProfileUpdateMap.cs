using AutoMapper;
using AutoMapper.Configuration;
using Fosol.Schedule.DAL.Interfaces;

namespace Fosol.Schedule.DAL.Maps
{
    public interface IProfileUpdateMap : IProfileExpression, IProfileConfiguration
    {
        void BindDataSource(IDataSource datasource);
    }
}
