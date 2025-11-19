using EmployeesTasksTracker.TasksService.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace EmployeesTasksTracker.TasksService.Core.Models
{
    public class Task
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid Project { get; init; }
        public Guid TasksGroup { get; init; }
        public DateTime Deadline { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Status Status { get; private set; }
        public Priority Priority { get; set; }
        public List<Guid> Performers { get; set; }
        public List<Guid> Observers { get; set; }

        public void ChangeStatus(Status newStatus)
        {
            if (newStatus == null)
            {
                throw new ArgumentNullException(nameof(newStatus), "Given status was null!");
            }

            if (Status == Status.Completed)
            {
                throw new Exception("Could not change status - task already completed!");
            }

            if (Status == newStatus)
            {
                throw new Exception("Task status has not changed, because it were the same as before");
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
                                throw new ArgumentException(exMessage);
                            }

                            Console.WriteLine("Task status has not changed, because it were and are \"Backlog\"");

                            return;
                        }

                    case Status.Current:
                        {
                            if (Status != Status.Backlog)
                            {
                                throw new ArgumentException(exMessage);
                            }
                            break;
                        }

                    case Status.Active:
                        {
                            if (Status != Status.Current && Status != Status.Testing)
                            {
                                throw new ArgumentException(exMessage);
                            }

                            break;
                        }

                    case Status.Testing:
                        {
                            if (Status != Status.Active)
                            {
                                throw new ArgumentException(exMessage);
                            }

                            break;
                        }

                    case Status.Completed:
                        {
                            if (Status != Status.Testing)
                            {
                                throw new ArgumentException(exMessage);
                            }
                            break;
                        }
                    default:
                        {
                            throw new ArgumentException($"Unknown status : {newStatus}!");
                        }
                }
            }

            Status = newStatus;
        }
    }
}
