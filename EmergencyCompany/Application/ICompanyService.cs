using EmergencyCompany.Entity;
using EmergencyCompany.Model;
using EmergencyEntity.PageQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmergencyCompany.Application
{
    public interface ICompanyService
    {
        Task<List<TableCompany>> GetAllCompanyInfo();


        Task<List<TableCompany>> GetCompanyInfo(EntityCompanySearch entityCompany);

        Task InsertCompanyInfoSync(EntityCompany entity);

        Task UpdateCompanyInfo(EntityCompany entity);

        Task<TableCompany> GetCompanyInfo(string id);

        Task DeleteCompanyInfo(string id);

        Task<TableCompany> GetCompanyInfoByName(string companyName);


        Task<List<EntityDirectLevel>> GetCountyRiskLevelInfo();

        Task<PageBase<TableCompany>> GetPageCompanyInfo(EntityCompanyPageQuery companyPageQuery);
    }
}
