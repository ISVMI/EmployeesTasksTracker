using EmployeesTasksTracker.TasksTrackerService.Core.Enums;
using Shared.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EmployeesTasksTracker.TasksTrackerService.Core.Models
{
    public class Task
    {
        [Key]
        [Column("Id")]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Column("Name")]
        public string Name { get; set; }
        [Column("Description")]
        public string Description { get; set; }
        public Guid ProjectId { get; set; }
        public virtual Project Project { get; set; }
        public Guid TasksGroupId { get; set; }
        public virtual TasksGroup TasksGroup { get; set; }
        [Column("Deadline")]
        public DateTime Deadline { get; set; }
        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [JsonInclude]
        [Column("Status")]
        public Status Status { get; private set; }
        [Column("Priority")]
        public Priority Priority { get; set; }
        public ICollection<TaskEmployee> TaskEmployees { get; set; } = new HashSet<TaskEmployee>();

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
