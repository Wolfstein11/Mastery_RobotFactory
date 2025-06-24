using FluentResults;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TuFabricaDDD.Application.Common;
using TuFabricaDDD.Contracts.Repositories;
using TuFabricaDDD.Domain.Entities;
using TuFabricaDDD.Domain.Types;
using TuFabricaDDD.Domain.ValueObjects;

namespace TuFabricaDDD.Application.Commands
{
    public class CreateUnitCommandHandler : ICommandHandler<CreateUnitCommand, Guid>
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IUnitOfWork _unitOfWork;

        public CreateUnitCommandHandler(IRepositoryManager repositoryManager, IUnitOfWork unitOfWork)
        {
            _repositoryManager = repositoryManager ?? throw new ArgumentNullException(nameof(repositoryManager));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<Result<Guid>> Handle(CreateUnitCommand request, CancellationToken cancellationToken)
        {
            // Map command to value objects
            var location = new Location(request.AreaName, request.CurrentLocationX, request.CurrentLocationY);
            var accessPoint = new AccessPoint(
                request.NetworkLocationConnectedAccessPointSsid,
                request.NetworkLocationConnectedAccessPointChannel
            );
            var networkLocation = new NetworkLocation(request.NetworkLocationIpAddress, accessPoint);

            // Instantiate the correct robot type based on category
            Robot robot = request.Category switch
            {
                RobotCategory.Humanoid => new Humanoid(
                    Guid.NewGuid(),
                    request.SerialNumber,
                    location,
                    networkLocation,
                    request.HasManipulators ?? false,
                    request.OperatingSystemVersion ?? string.Empty
                ),
                RobotCategory.FixedRoboticArm => new FixedRoboticArm(
                    Guid.NewGuid(),
                    request.SerialNumber,
                    location,
                    networkLocation,
                    request.ArmLengthInMeters ?? 0,
                    request.PayloadCapacityInKg ?? 0,
                    request.MountingPointId ?? string.Empty
                ),
                RobotCategory.LogisticsMobile => new LogisticsMobile(
                    Guid.NewGuid(),
                    request.SerialNumber,
                    location,
                    networkLocation,
                    request.LocomotionType is not null
                        ? Enum.TryParse<LocomotionType>(request.LocomotionType, out var locomotion) ? locomotion : LocomotionType.Unknown
                        : LocomotionType.Unknown,
                    request.NavigationSystemType ?? string.Empty,
                    request.MaxPayloadCapacityInKg ?? 0
                ),
                RobotCategory.CleaningMobile => new CleaningMobile(
                    Guid.NewGuid(),
                    request.SerialNumber,
                    location,
                    networkLocation,
                    request.LocomotionType is not null
                        ? Enum.TryParse<LocomotionType>(request.LocomotionType, out var locomotion) ? locomotion : LocomotionType.Unknown
                        : LocomotionType.Unknown
                ),
                // Add other categories as needed
                _ => null
            };

            if (robot == null)
                return Result.Fail<Guid>("Unsupported robot category.");

            // Optionally set additional properties for specific types
            if (robot is Humanoid humanoid && request.EquippedTools is not null)
            {
                humanoid.EquippedTools.AddRange(request.EquippedTools);
            }
            if (robot is CleaningMobile cleaningMobile && request.EquippedCleaningTools is not null)
            {
                cleaningMobile.EquippedCleaningTools.AddRange(request.EquippedCleaningTools);
            }

            // Add to repository (use the correct repository for the type)
            switch (robot)
            {
                case Humanoid h:
                    await _repositoryManager.Humanoids.AddAsync(h, cancellationToken);
                    break;
                case FixedRoboticArm arm:
                    await _repositoryManager.FixedRoboticArms.AddAsync(arm, cancellationToken);
                    break;
                case LogisticsMobile logistics:
                    await _repositoryManager.LogisticsMobiles.AddAsync(logistics, cancellationToken);
                    break;
                case CleaningMobile cleaning:
                    await _repositoryManager.CleaningMobiles.AddAsync(cleaning, cancellationToken);
                    break;
                default:
                    await _repositoryManager.Robots.AddAsync(robot, cancellationToken);
                    break;
            }

            await _unitOfWork.SaveChangesAsync();

            return Result.Ok(robot.Id);
        }
    }
}