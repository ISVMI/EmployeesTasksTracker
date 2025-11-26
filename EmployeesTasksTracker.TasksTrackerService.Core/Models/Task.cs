using EmployeesTasksTracker.TasksTrackerService.Core.Enums;
using Shared.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace EmployeesTasksTracker.TasksTrackerService.Core.Models
{
    public class Task
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid ProjectId { get; set; }
        public virtual Project Project { get; set; }
        public Guid TasksGroupId { get; set; }
        public virtual TasksGroup TasksGroup { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Status Status { get; private set; }
        public Priority Priority { get; set; }

        public void ChangeStatus(Status newStatus)
        {
            if (newStatus == null)
            {
                throw new ArgumentNullException(nameof(newStatus), "Given status was null!");
            }

            if (Status == Status.Completed)
            {
                throw new DomainException("Could not change status - task already completed!");
            }

            if (Status == newStatus)
            {
                throw new DomainException("Task status has not changed, because it were the same as before");
            }

            if (newStatus != Status.Canceled)
            {

                var exMessage = $"Task can not change from {Status} to {newStatus}!";

                switch (newStatus)
                {
                    case Status.Backlog:
                        {
                            if (Status != Status.Backlog)
                            {
                                throw new DomainException(exMessage);
                            }

                            return;
                        }

                    case Status.Current:
                        {
                            if (Status != Status.Backlog)
                            {
                                throw new DomainException(exMessage);
                            }
                            break;
                        }

                    case Status.Active:
                        {
                            if (Status != Status.Current && Status != Status.Testing)
                            {
                                throw new DomainException(exMessage);
                            }

                            break;
                        }

                    case Status.Testing:
                        {
                            if (Status != Status.Active)
                            {
                                throw new DomainException(exMessage);
                            }

                            break;
                        }

                    case Status.Completed:
                        {
                            if (Status != Status.Testing)
                            {
                                throw new DomainException(exMessage);
                            }
                            break;
                        }
                    default:
                        {
                            throw new DomainException($"Unknown status : {newStatus}!");
                        }
                }
            }

            Status = newStatus;
        }
    }
}
