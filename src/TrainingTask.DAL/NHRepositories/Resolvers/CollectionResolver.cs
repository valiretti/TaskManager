using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using NHibernate;
using NHibernate.Criterion;

namespace TrainingTask.DAL.NHRepositories.Resolvers
{
    internal class CollectionResolver<TSource, TDest, TSourceMember, TDestMember>
        : IMemberValueResolver<TSource, TDest, IEnumerable<TSourceMember>, ISet<TDestMember>>
    {
        private readonly ISession _session;

        public CollectionResolver(ISession session)
        {
            _session = session;
        }


        public ISet<TDestMember> Resolve(TSource source, TDest destination, IEnumerable<TSourceMember> sourceMember, ISet<TDestMember> destMember,
            ResolutionContext context)
        {
            if (sourceMember == null)
                return new HashSet<TDestMember>();

            var ids = sourceMember.Cast<object>().ToArray();
            var result = _session.CreateCriteria(typeof(TDestMember))
                .Add(new InExpression("Id", ids))
                .List<TDestMember>()
                .ToHashSet();
            return result;
        }
    }
}
