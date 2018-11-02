using AutoMapper;
using AutoMapper.Configuration;
using Fosol.Schedule.DAL.Interfaces;

namespace Fosol.Schedule.DAL.Maps
{
    public interface IProfileAddMap : IProfileExpression, IProfileConfiguration
    {
        void BindDataSource(IDataSource datasource);
    }
}
