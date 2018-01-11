using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmergencyCompany.Entity;
using EmergencyBaseService;
using EmergencyCompany.Model;
using AutoMapper;
using CommonLib;
using EmergencyEntity.PageQuery;
using System.Data;
using Dapper;
using EmergencyData.MicroOrm.SqlGenerator;

namespace EmergencyCompany.Application
{
    public class DangerousProductService : BaseAppService, IDangerousProductService
    {
        public async Task AddDangerousProduct(EntityDangerousProduct entityDangerous)
        {
            var model = Mapper.Map<EntityDangerousProduct, TableDangerousProduct>(entityDangerous);
            model.Id = Utils.GetNewId();
            var dangerousProductRep = GetRepositoryInstance<TableDangerousProduct>();
            dangerousProductRep.Insert(model);
        }

        public async Task DeleteDangerousProduct(string companyId)
        {
            var dangerousProductRep = GetRepositoryInstance<TableDangerousProduct>();
            var strSql = "DELETE T_DangerousProduct WHERE CompanyId=@companyId";

            var paras = new DynamicParameters(new
            {
                companyId = companyId
            });
            await dangerousProductRep.Connection.ExecuteAsync(strSql, paras);
        }

        public async Task EditDangerousProduct(EntityDangerousProduct entityDangerous)
        {
            var model = Mapper.Map<EntityDangerousProduct, TableDangerousProduct>(entityDangerous);
            var dangerousProductRep = GetRepositoryInstance<TableDangerousProduct>();
            dangerousProductRep.Update<TableDangerousProduct>(
                model, dangerousProduct => new
                {
                    dangerousProduct.RegesterId,
                    dangerousProduct.ProductName,
                    dangerousProduct.AliasName,
                    dangerousProduct.ProductAttributes,
                    dangerousProduct.Manufacturability,
                    dangerousProduct.ProductReserve,
                    dangerousProduct.YearProduct,
                    dangerousProduct.Cas,
                    dangerousProduct.Un,
                    dangerousProduct.IsToxicity,
                    dangerousProduct.Instructions,
                    dangerousProduct.Memo,
                    dangerousProduct.ExpertOpinion,
                    dangerousProduct.ManagementPlan
                });
        }

        public async Task<List<EntityDangerousProduct>> GetDangerousProduct(string companyId)
        {
            var dangerousProductRep = GetRepositoryInstance<TableDangerousProduct>();
            var restult = dangerousProductRep.FindAll(x => x.CompanyId == companyId).ToList();
            var model = Mapper.Map<List<TableDangerousProduct>, List<EntityDangerousProduct>>(restult);
            return model;
        }

        public async Task<EntityDangerousProduct> GetDangerousProductInfo(string id)
        {
            var dangerousProductRep = GetRepositoryInstance<TableDangerousProduct>();
            var restult = dangerousProductRep.Find(x => x.Id == id);
            var model = Mapper.Map<TableDangerousProduct, EntityDangerousProduct>(restult);
            return model;
        }

        public async Task<PageBase<EntityDangerousProduct>> GetPageDangerousInfo(EntityDangerousPageQuery dangerousPageQuery)
        {
            var result = new PageBase<EntityDangerousProduct>
            {
                CurrentPage = dangerousPageQuery.CurrentPage,
                PageSize = dangerousPageQuery.PageSize
            };

            var strSql = new StringBuilder();

            //计算总数
            strSql.Append(@"        
                            SELECT  @totalCount = COUNT(1)
                            FROM    dbo.T_DangerousProduct WITH ( NOLOCK ) ");

            strSql.Append(" where CompanyId =@companyId ");
            if (!string.IsNullOrEmpty(dangerousPageQuery.ProductName))
            {
                strSql.Append(" and  ProductName like '%' + @productName +'%' ;");
            }
            //分页信息
            strSql.Append(@";SELECT * FROM (SELECT  ROW_NUMBER() OVER ( ORDER BY CreateTime DESC ) RowNumber ,
                            Id ,
                            CompanyId ,
                            ProductName ,
                            AliasName ,
                            ProductAttributes ,
                            Manufacturability ,
                            ProductReserve ,
                            YearProduct ,
                            Cas ,
                            Un ,
                            IsToxicity ,
                            Instructions ,
                            Memo ,
                            CreateTime ,
                            Status ,
                            ExpertOpinion ,
                            ManagementPlan ,
                            RegesterId
                    FROM    dbo.T_DangerousProduct WITH ( NOLOCK ) ");

            strSql.Append(" where CompanyId =@companyId ");
            if (!string.IsNullOrEmpty(dangerousPageQuery.ProductName))
            {
                strSql.Append("  and ProductName like '%' + @productName +'%' ");
            }
            strSql.Append(@"
                                   ) AS a
                            WHERE   a.RowNumber > @startIndex
                                    AND a.RowNumber <= @endIndex              
                        ");
            strSql.Append(@" order by a.RowNumber ");

            var paras = new DynamicParameters(new
            {
                companyId = dangerousPageQuery.CompanyId,
                productName = dangerousPageQuery.ProductName,
                startIndex = (dangerousPageQuery.CurrentPage - 1) * dangerousPageQuery.PageSize,
                endIndex = dangerousPageQuery.CurrentPage * dangerousPageQuery.PageSize
            });
            var dangerousProductRep = GetRepositoryInstance<TableDangerousProduct>();

            paras.Add("totalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var sqlQuery = new SqlQuery(strSql.ToString(), paras);

            var listResult = dangerousProductRep.FindAll(sqlQuery).ToList();

            result.Items = Mapper.Map<List<TableDangerousProduct>, List<EntityDangerousProduct>>(listResult);

            result.TotalCounts = paras.Get<int?>("totalCount") ?? 0;
            result.TotalPages = Convert.ToInt32(Math.Ceiling(result.TotalCounts / (dangerousPageQuery.PageSize * 1.0)));

            return result;
        }
    }
}
