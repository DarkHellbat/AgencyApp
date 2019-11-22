﻿using Agency.Models.Filters;
using Agency.Models.Models;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agency.Models.Repository
{
    public class EmployerRepository : Repository<Vacancy, VacancyFilter>
    {
        public EmployerRepository(ISession session) :
                base(session)
        {

        }

        public IList<Vacancy> ShowMyVacancies(long userId, VacancyFilter filter ,FetchOptions options)
        {
            var crit = session.CreateCriteria<Vacancy>();
            crit.Add(Restrictions.Eq("Creator.Id", userId));
            if (options != null)
            {
                SetFetchOptions(crit, options);
            }
            return crit.List<Vacancy>();
            
        }

        public long SaveWProcedure(Vacancy vacancy, long Id)
        {
            var query = session.CreateSQLQuery("exec sp_InsertVacancy :VacancyName, :VacancyDescription, :Starts, :Ends, :User_id, :Status, :Company_id")
                    .SetParameter("VacancyName", vacancy.VacancyName)
                    .SetParameter("VacancyDescription", vacancy.VacancyDescription)
                    .SetParameter("Starts", vacancy.Starts)
                    .SetParameter("Ends", vacancy.Ends)
                    .SetParameter("User_id", Id)
                    .SetParameter("Status", vacancy.Status)
                    .SetParameter("Company_id", vacancy.Company.Id)
                    ;
            var result = query.UniqueResult();

            return long.Parse(result.ToString());
            //здесь нет опыта, потому что нужно передавать множественный параметр. Делается оно через XML, но я пока такое не умею
            //Опыт записывается через апдейт, который идет после
        }

        public override void SetupFilter(ICriteria crit, VacancyFilter filter)
        {
            base.SetupFilter(crit, filter);
            if (filter != null)
            {
                if (filter.Experience != null)
                {
                    List<long> exp = new List<long>();
                    foreach (var e in filter.Experience)
                    {
                        exp.Add(e.Id);
                    }
                    crit.Add(Restrictions.In("Id", exp));
                }
                if (filter.StartDateRange != null)
                {
                    if (filter.StartDateRange.From.HasValue)
                    {
                        crit.Add(Restrictions.Ge("Starts", filter.StartDateRange.From.Value));
                    }
                    if (filter.StartDateRange.To.HasValue)
                    {
                        crit.Add(Restrictions.Le("Starts", filter.StartDateRange.To.Value));
                    }
                }
                if (filter.EndDateRange != null)
                {
                    if (filter.EndDateRange.From.HasValue)
                    {
                        crit.Add(Restrictions.Ge("Ends", filter.EndDateRange.From.Value));
                    }
                    if (filter.StartDateRange.To.HasValue)
                    {
                        crit.Add(Restrictions.Le("Ends", filter.EndDateRange.To.Value));
                    }
                }
                if (filter.Statuses != null)
                {
                    foreach (var s in filter.Statuses)
                    {
                        crit.Add(Restrictions.Eq("Status", filter.Statuses));
                    }
                }
            }
        }

        public IList<Vacancy> FindSuitableVacancy(List<long> experiences)
        {
            var crit = session.CreateCriteria<Vacancy>()
                .CreateAlias("Requirements", "VacancyExperience")
                .Add(Restrictions.In("VacancyExperience.id", experiences));
            return crit.List<Vacancy>();

        }
    }
}
