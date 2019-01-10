﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using NHibernate;
using NHibernate.Criterion;
using TrainingTask.Common.Models;
using TrainingTask.DAL.Entities;

namespace TrainingTask.DAL.NHRepositories.Resolvers
{
    internal class ProjectResolver : IMemberValueResolver<CreateProject, ProjectNh, IEnumerable<CreateTask>, ISet<TaskNh>>
    {
        private readonly ISession _session;

        public ProjectResolver(ISession session)
        {
            _session = session;
        }
        
        public ISet<TaskNh> Resolve(CreateProject source, ProjectNh destination, IEnumerable<CreateTask> sourceMember, ISet<TaskNh> destMember,
              ResolutionContext context)
        {
            if (destination.Id != 0)
            {
                var tasksForDelete = destMember.Where(m => source.Tasks.All(t => t.Id != m.Id)).ToArray();
                foreach (var taskNh in tasksForDelete)
                {
                    taskNh.Project = null;
                    _session.Delete(taskNh);
                }
            }

            if (sourceMember == null)
                return new HashSet<TaskNh>();

            var resultTasks = new List<TaskNh>();
            foreach (var t in sourceMember)
            {
                if (t.Id != 0)
                {
                    var task = _session.Get<TaskNh>(t.Id);
                    context.Mapper.Map(t, task, typeof(CreateTask), typeof(TaskNh));
                    task.Project = destination;
                    resultTasks.Add(task);
                }
                else
                {
                    TaskNh task = new TaskNh();
                    context.Mapper.Map(t, task, typeof(CreateTask), typeof(TaskNh));
                    task.Project = destination;
                    resultTasks.Add(task);
                }
            }

            var result = resultTasks.ToHashSet();
            return result;
        }
    }
}
