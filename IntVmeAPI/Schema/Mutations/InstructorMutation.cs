using AppAny.HotChocolate.FluentValidation;
using HotChocolate.Authorization;
using HotChocolate.Subscriptions;
using IntVmeAPI.DTOs;
using IntVmeAPI.Services;
using IntVmeAPI.Validators;

namespace IntVmeAPI.Schema.Mutations
{
    [ExtendObjectType(typeof(Mutation))]
    public class InstructorMutation
    {
        [Authorize]
        public async Task<InstructorResult> CreateInstructor(
            [UseFluentValidation, UseValidators<InstructorInputValidator>] InstructorInput instructorInput,
            SchoolDBContext context,
            ITopicEventSender topicEventSender
            )
        {
            InstructorDTO instructorDTO = new InstructorDTO()
            {
                FirstName = instructorInput.FirstName,
                LastName = instructorInput.LastName,
                Salary = instructorInput.Salary,
            };

            context.Add(instructorDTO);
            await context.SaveChangesAsync();

            InstructorResult instructorResult = new InstructorResult()
            {
                Id = instructorDTO.Id,
                FirstName = instructorDTO.FirstName,
                LastName = instructorDTO.LastName,
                Salary = instructorDTO.Salary,
            };

            await topicEventSender.SendAsync(nameof(Subscription.InstructorCreated), instructorResult);
            return instructorResult;
        }

        [Authorize]
        public async Task<InstructorResult> UpdateInstructor(
            Guid id,
            [UseFluentValidation, UseValidators<InstructorInputValidator>] InstructorInput instructorInput,
            SchoolDBContext context)
        {
            InstructorDTO? instructorDTO = await context.Instructors.FindAsync(id);

            if (instructorDTO == null)
            {
                throw new GraphQLException(new Error("Instructor not found", "INSTRUCTOR_NOT_FOUND"));
            }

            instructorDTO.FirstName = instructorInput.FirstName;
            instructorDTO.LastName = instructorInput.LastName;
            instructorDTO.Salary = instructorInput.Salary;

            context.Update(instructorDTO);
            await context.SaveChangesAsync();

            InstructorResult instructorResult = new InstructorResult()
            {
                Id = instructorDTO.Id,
                FirstName = instructorDTO.FirstName,
                LastName = instructorDTO.LastName,
                Salary = instructorDTO.Salary,
            };

            return instructorResult;
        }

        [Authorize(Policy = "IsAdmin")]
        public async Task<bool> DeleteInstructor(
            Guid id,
            SchoolDBContext context)
        {
            InstructorDTO instructorDTO = new InstructorDTO()
            {
                Id = id
            };

            context.Remove(instructorDTO);

            try
            {
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
