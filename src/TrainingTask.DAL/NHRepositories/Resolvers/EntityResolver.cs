using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using NHibernate;

namespace TrainingTask.DAL.NHRepositories.Resolvers
{
    internal class EntityResolver<TSource, TDest, TSourceMember, TDestMember>
        : IMemberValueResolver<TSource, TDest, TSourceMember, TDestMember>
    {
        private readonly ISession _session;

        public EntityResolver(ISession session)
        {
            _session = session;
        }

        public TDestMember Resolve(TSource source, TDest destination, TSourceMember sourceMember, TDestMember destMember,
            ResolutionContext context)
        {
            return _session.Get<TDestMember>(sourceMember);
        }
    }
}
